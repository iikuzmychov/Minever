using Minever.Networking;
using Minever.Networking.Exceptions;
using Minever.Networking.IO;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Sockets;
using Microsoft.Extensions.Options;

namespace Minever.Client;

public sealed class JavaPacketClient : IPacketClient
{
    private readonly ILogger<JavaPacketClient> _logger;
    private readonly TcpClient _tcpClient = new();

    private readonly CancellationTokenSource _listenCancellationSource = new();
    private Task? _listenTask;
    private MinecraftWriter? _writer;

    private volatile int _pauseRequestsCount = 0;

    public event Action<object>? PacketReceived;
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

            if (stream.DataAvailable && _pauseRequestsCount == 0)
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
                    Task.Run(() => PacketReceived?.Invoke(packet.Data));
                    ConnectionState = Protocol.GetNewState(packet.Data, context);
                }
            }
        }
    }

    public async ValueTask ConnectAsync(string host, ushort port = 25565, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(host);

        await _tcpClient.ConnectAsync(host, port, cancellationToken);
        _logger.LogInformation("Connection established.");

        _writer     = new(_tcpClient.GetStream());
        _listenTask = Task.Run(ListenStream, _listenCancellationSource.Token);
    }

    public async Task DisconnectAsync()
    {
        if (!IsConnected)
            return;

        _listenCancellationSource.Cancel();
        await _listenTask!;
        _tcpClient.Close();
        _logger.LogInformation("Disconnected.");
        Disconnected?.Invoke();
    }

    async ValueTask IAsyncDisposable.DisposeAsync() => await DisconnectAsync();

    void IDisposable.Dispose() => DisconnectAsync().GetAwaiter().GetResult();

    public Action<object> OnPacket<TData>(Action<TData> action)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(action);

        Action<object> handler = data =>
        {
            if (data is TData tData)
                action(tData);
        };

        PacketReceived += handler;

        return handler;
    }

    public void OnceOnPacket<TData>(Action<TData> action)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(action);

        Action<object> handler = null!;

        handler = OnPacket<TData>(data =>
        {
            PacketReceived -= handler;
            action(data);
        });
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

    public async Task<TData> WaitPacketAsync<TData>(CancellationToken cancellationToken = default)
        where TData : notnull
    {
        _pauseRequestsCount++;

        var taskCompletionSource = new TaskCompletionSource<TData>();

        OnceOnPacket<TData>(data => taskCompletionSource.SetResult(data));
        cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());

        _pauseRequestsCount--;

        return await taskCompletionSource.Task;
    }

    public async Task<TResponseData> SendRequestAsync<TResponseData>(
        object requestPacketData, CancellationToken cancellationToken = default)
        where TResponseData : notnull
    {
        ArgumentNullException.ThrowIfNull(requestPacketData);

        _pauseRequestsCount++;

        var taskCompletionSource = new TaskCompletionSource<TResponseData>();

        cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());
        SendPacket(requestPacketData);
        OnceOnPacket<TResponseData>(data => taskCompletionSource.SetResult(data));

        _pauseRequestsCount--;

        var responsePacket = await taskCompletionSource.Task;

        return responsePacket;
    }
}