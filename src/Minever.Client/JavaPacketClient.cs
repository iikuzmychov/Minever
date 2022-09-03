using Minever.Networking;
using Minever.Networking.Exceptions;
using Minever.Networking.IO;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using Microsoft.Extensions.Logging.Abstractions;

namespace Minever.Client;

public delegate void PacketReceivedHandler<TData>(MinecraftPacket<TData> packet, DateTime dateTime, PacketContext context)
    where TData : notnull;

public sealed class JavaPacketClient : IDisposable, IAsyncDisposable
{
    private readonly TcpClient _tcpClient = new();
    private bool _isListeningPaused = true;
    private CancellationTokenSource? _listenCancellationSource;
    private Task? _listenTask;
    private MinecraftWriter? _writer;
    private ILogger<JavaPacketClient> _logger;

    public event PacketReceivedHandler<object>? PacketReceived;
    public event Action? Disconnected;

    public JavaProtocol Protocol { get; }
    public bool IsConnected => _tcpClient.Connected;
    public ConnectionState ConnectionState { get; private set; } = ConnectionState.Handshake;

    public JavaPacketClient(JavaProtocol protocol, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(protocol);

        Protocol = protocol;
        _logger  = loggerFactory.CreateLogger<JavaPacketClient>();
    }

    public JavaPacketClient(JavaProtocol protocol) : this(protocol, NullLoggerFactory.Instance) { }

    private void ListenStream()
    {
        using var stream = _tcpClient.GetStream();
        using var reader = new MinecraftReader(stream);
        
        while (true)
        {
            if (_listenCancellationSource!.IsCancellationRequested)
                break;

            if (!_isListeningPaused && stream.DataAvailable)
            {
                int packetLength;

                try
                {
                    packetLength = reader.Read7BitEncodedInt();
                }
                catch (Exception exception)
                {
                    _logger.LogCritical(exception, $"Error while reading packet length.");
                    Task.Run(DisconnectAsync);
                    
                    return;
                }

                var context = new PacketContext(PacketDirection.ServerToClient, ConnectionState);
                var packet  = (MinecraftPacket<object>?)null;

                try
                {
                    packet = reader.ReadPacket(packetLength, context, Protocol);

                    _logger.LogDebug($"Packet {packet.Data.GetType().Name} was received (0x{packet.Id:X2}, {context.ConnectionState} state).");
                }
                catch (NotSupportedPacketException exception)
                {
                    _logger.LogWarning(exception.Message);
                }
                catch (PacketDeserializationException exception)
                {
                    _logger.LogWarning(exception.Message);
                }
                catch (Exception exception)
                {
                    _logger.LogCritical(exception, $"Error while reading packet.");
                    Task.Run(DisconnectAsync);

                    return;
                }

                if (packet is not null)
                {
                    // TODO: replace this with adding the packet to a packets queue and invoking PacketReceived in the right order in new loop-method
                    Task.Run(() => PacketReceived?.Invoke(packet, DateTime.Now, context));
                    ConnectionState = Protocol.GetNewState(packet.Data, context);
                }
            }
        }
    }

    public async Task ConnectAsync(string serverAddress, ushort serverPort)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        await _tcpClient.ConnectAsync(serverAddress, serverPort);
        _logger.LogInformation("Connection established.");

        _writer                   = new(_tcpClient.GetStream());
        _listenCancellationSource = new();
        _isListeningPaused        = false;
        _listenTask               = Task.Run(ListenStream, _listenCancellationSource.Token);
    }

    public async Task DisconnectAsync()
    {
        if (!IsConnected)
            return;
                
        _isListeningPaused = true;

        if (PacketReceived is not null)
        {
            foreach (var handler in PacketReceived.GetInvocationList())
                PacketReceived -= (handler as PacketReceivedHandler<object>);
        }

        _listenCancellationSource?.Cancel();
            
        if (_listenTask is not null)
            await _listenTask;

        _tcpClient.Close();
        _logger.LogInformation("Disconnected.");
        Disconnected?.Invoke();
    }

    public void Disconnect() => DisconnectAsync().GetAwaiter().GetResult();

    async ValueTask IAsyncDisposable.DisposeAsync() => await DisconnectAsync();

    void IDisposable.Dispose() => Disconnect();

    public PacketReceivedHandler<object> OnPacket<TData>(PacketReceivedHandler<TData> action)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(action);

        PacketReceivedHandler<object> handler = (packet, dateTime, context) =>
        {
            if (packet.Data is TData)
                action((MinecraftPacket<TData>)packet, dateTime, context);
        };

        PacketReceived += handler;

        return handler;
    }

    public PacketReceivedHandler<object> OnPacket<TData>(Action<TData> action)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(action);

        return OnPacket<TData>((packet, _, _) => action(packet.Data));
    }

    public PacketReceivedHandler<object> OnPacket<TData>(Action<TData, DateTime> action)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(action);

        return OnPacket<TData>((packet, receivedDateTime, _) => action(packet.Data, receivedDateTime));
    }

    public void OnceOnPacket<TData>(PacketReceivedHandler<TData> action)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(action);

        PacketReceivedHandler<object>? handler = null;

        handler = OnPacket<TData>((packet, dateTime, context) =>
        {
            PacketReceived -= handler;
            action(packet, dateTime, context);
        });
    }

    public void OnceOnPacket<TData>(Action<TData> action)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(action);

        OnceOnPacket<TData>((packet, _, _) => action(packet.Data));
    }

    public void OnceOnPacket<TData>(Action<TData, DateTime> action)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(action);

        OnceOnPacket<TData>((packet, receivedDateTime, _) => action(packet.Data, receivedDateTime));
    }

    public void SendPacket(object packetData)
    {
        ArgumentNullException.ThrowIfNull(packetData);

        var context  = new PacketContext(PacketDirection.ClientToServer, ConnectionState);
        var packetId = Protocol.GetPacketId(packetData.GetType(), context);
        var packet   = new MinecraftPacket<object>(packetId, packetData);

        _writer!.WritePacket(packet);
        _logger.LogDebug($"Packet {packet.Data.GetType().Name} (0x{packetId:X2}, {context.ConnectionState} state) was sended.");

        ConnectionState = Protocol.GetNewState(packet.Data, context);
    }

    public async Task<ReceivedPacketInfo<TResponseData>> WaitPacketAsync<TResponseData>()
        where TResponseData : notnull
    {
        var taskCompletionSource = new TaskCompletionSource<ReceivedPacketInfo<TResponseData>>();

        OnceOnPacket<TResponseData>((packet, receivedDateTime, context) =>
            taskCompletionSource.SetResult(new(packet, receivedDateTime, context)));

        return await taskCompletionSource.Task;
    }

    public async Task<ReceivedPacketInfo<TResponseData>> SendRequestAsync<TResponseData>(object requestPacketData)
        where TResponseData : notnull
    {
        ArgumentNullException.ThrowIfNull(requestPacketData);

        var taskCompletionSource = new TaskCompletionSource<ReceivedPacketInfo<TResponseData>>();

        _isListeningPaused = true;

        SendPacket(requestPacketData);

        OnceOnPacket<TResponseData>((packet, receivedDateTime, context) =>
            taskCompletionSource.SetResult(new(packet, receivedDateTime, context)));
        
        _isListeningPaused = false;

        var responsePacket = await taskCompletionSource.Task;

        return responsePacket;
    }
}
