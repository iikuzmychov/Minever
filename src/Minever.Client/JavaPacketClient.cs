using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Minever.Networking;
using Minever.Networking.Exceptions;
using Minever.Networking.IO;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;
using Minever.Networking.Serialization;
using System.Net.Sockets;

namespace Minever.Client;

public sealed class JavaPacketClient : IPacketClient
{
    private readonly ILogger<JavaPacketClient> _logger;
    private readonly TcpClient _tcpClient = new();

    private readonly CancellationTokenSource _listenCancellationSource = new();
    private Task? _listenTask;
    private MinecraftReader? _reader;
    private MinecraftWriter? _writer;

    private bool _isDisposed = false;
    private readonly object _lock = new();

    public event Action<object>? PacketReceived;
    public event Action? Disconnected;

    public JavaProtocol Protocol { get; }
    public ConnectionState ConnectionState { get; private set; } = ConnectionState.Handshake;
    public bool IsConnected => _tcpClient.Connected;

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
        _reader          = new MinecraftReader(stream);

        while (true)
        {
            if (_listenCancellationSource!.IsCancellationRequested)
                break;

            if (stream.DataAvailable)
            {
                lock (_lock)
                {
                    int packetLength;

                    try
                    {
                        packetLength = _reader.Read7BitEncodedInt();
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
                        packet = PacketSerializer.Deserialize(_reader, packetLength, context, Protocol);

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
    }

    public async ValueTask ConnectAsync(string host, ushort port = 25565, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(host);

        if (_isDisposed)
            throw new ObjectDisposedException(GetType().FullName);

        await _tcpClient.ConnectAsync(host, port, cancellationToken);
        _logger.LogInformation("Connection established.");

        _writer     = new(_tcpClient.GetStream());
        _listenTask = Task.Run(ListenStream, _listenCancellationSource.Token);
    }

    public async Task DisconnectAsync()
    {
        if (_isDisposed)
            return;
        
        _isDisposed = true;

        _listenCancellationSource.Cancel();
        await _listenTask!;

        _reader?.Dispose();
        _tcpClient.Close();

        if (_writer is not null)
            await _writer.DisposeAsync();

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

        if (_isDisposed)
            throw new ObjectDisposedException(GetType().FullName);

        var context  = new PacketContext(PacketDirection.ClientToServer, ConnectionState);
        var packetId = Protocol.GetPacketId(packetData.GetType(), context);
        var packet   = new MinecraftPacket<object>(packetId, packetData);

        lock (_lock)
        {
            PacketSerializer.Serialize(packet, _writer!);
            _logger.LogDebug($"Packet {packet.Data.GetType().Name} (0x{packetId:X2}, {context.ConnectionState} state) was sended.");

            ConnectionState = Protocol.GetNewState(packet.Data, context);
        }
    }

    public async Task<TData> WaitPacketAsync<TData>(CancellationToken cancellationToken = default)
        where TData : notnull
    {
        if (_isDisposed)
            throw new ObjectDisposedException(GetType().FullName);

        TaskCompletionSource<TData> taskCompletionSource;

        lock (_lock)
        {
            taskCompletionSource = new TaskCompletionSource<TData>();

            OnceOnPacket<TData>(data => taskCompletionSource.SetResult(data));
            cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());
        }

        return await taskCompletionSource.Task;
    }

    public async Task<TResponseData> SendRequestAsync<TResponseData>(
        object requestPacketData, CancellationToken cancellationToken = default)
        where TResponseData : notnull
    {
        ArgumentNullException.ThrowIfNull(requestPacketData);

        if (_isDisposed)
            throw new ObjectDisposedException(GetType().FullName);

        TaskCompletionSource<TResponseData> taskCompletionSource;

        lock (_lock)
        {
            taskCompletionSource = new TaskCompletionSource<TResponseData>();

            cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());
            SendPacket(requestPacketData);
            OnceOnPacket<TResponseData>(data => taskCompletionSource.SetResult(data));
        }

        var responsePacket = await taskCompletionSource.Task;

        return responsePacket;
    }
}