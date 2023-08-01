using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Minever.LowLevel.Core;
using Minever.LowLevel.Core.IO;
using Minever.LowLevel.Java.Core.Extensions;
using Minever.LowLevel.Java.Core.Packets.Serialization;
using System.Net.Sockets;

namespace Minever.LowLevel.Java.Core;

public sealed class JavaProtocolClient : IProtocolClient
{
    private readonly ILogger<JavaProtocolClient> _logger;
    private readonly TcpClient _tcpClient = new();
    private readonly object _lock = new();
    private bool _isDisposed = false;

    private MinecraftWriter? _writer;
    private Task? _listeningTask;
    private readonly CancellationTokenSource _listeningTaskCts = new();
    private event Action<object>? _packetReceived;
    
    public IJavaProtocol Protocol { get; }
    IProtocol IPacketTransceiver.Protocol => Protocol;

    // todo: Disconnected -> None ???
    // todo: Disconnected -> null ???
    public JavaConnectionState ConnectionState { get; private set; } = JavaConnectionState.Disconnected;
    public bool IsConnected => _tcpClient.Connected;

    public event Action<object>? PacketReceived
    {
        add => _packetReceived += value;
        remove => _packetReceived -= value;
    }
    public event Action<Exception?>? Disconnected; // todo: replace with custom delegate ???

    public JavaProtocolClient(IJavaProtocol protocol, ILogger<JavaProtocolClient>? logger = null)
    {
        Protocol = protocol ?? throw new ArgumentNullException(nameof(protocol));
        _logger  = logger ?? NullLogger<JavaProtocolClient>.Instance;
    }

    public async ValueTask ConnectAsync(string host, int port = 25565, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(host);

        /*if (_isDisposed)
            throw new ObjectDisposedException(GetType().FullName);*/

        _logger.LogInformation($"Connecting to {host}:{port}.");
        await _tcpClient.ConnectAsync(host, port, cancellationToken);
        _logger.LogInformation($"Connected to {host}:{port}.");

        ConnectionState = JavaConnectionState.Handshake;
        _writer         = new(_tcpClient.GetStream());
        _listeningTask  = Task.Run(StartListening, _listeningTaskCts.Token);
    }

    public void SendPacket(object packet)
    {
        ArgumentNullException.ThrowIfNull(packet);

        /*if (_isDisposed)
            throw new ObjectDisposedException(GetType().FullName);*/

        var context   = new JavaPacketContext(ConnectionState, PacketDirection.ToServer);
        var nextState = Protocol.GetNextConnectionState(packet, context);

        if (nextState != ConnectionState)
        {
            lock (_lock) // todo: is it needed ???
            {
                SendPacketInternal(packet);
                ConnectionState = nextState;
            }
        }
        else
        {
            SendPacketInternal(packet);
        }
    }

    public Action<object> OnPacket<TPacket>(Action<TPacket> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        Action<object> actualHandler = packet =>
        {
            if (packet.GetType() == typeof(TPacket))
            {
                handler((TPacket)packet);
            }
        };

        PacketReceived += actualHandler;

        return actualHandler;
    }

    public void OnceOnPacket<TPacket>(Action<TPacket> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        Action<object> actualHandler = null!;

        actualHandler = OnPacket<TPacket>(packet =>
        {
            PacketReceived -= actualHandler;
            handler(packet);
        });
    }

    public async Task<TPacket> WaitForPacketAsync<TPacket>(CancellationToken cancellationToken = default)
    {
        var taskCompletionSource = new TaskCompletionSource<TPacket>();
        cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());

        OnceOnPacket<TPacket>(packet => taskCompletionSource.TrySetResult(packet));
        
        return await taskCompletionSource.Task;
    }

    public async Task<TResponsePacket> GetPacketAsync<TResponsePacket>(object requestPacket, CancellationToken cancellationToken = default)
    {
        Protocol.ThrowIfPacketIsNotSupported(requestPacket, new JavaPacketContext(ConnectionState, PacketDirection.ToServer));

        Task<TResponsePacket> responsePacketTask;

        lock (_lock)
        {
            SendPacketInternal(requestPacket);
            responsePacketTask = WaitForPacketAsync<TResponsePacket>(cancellationToken);
        }

        return await responsePacketTask;
    }

    // todo: refactor/remove
    public Task<Action<object>> OnPacketAsync<TPacket>(Func<TPacket, Task> asyncHandler)
    {
        ArgumentNullException.ThrowIfNull(asyncHandler);

        Action<object> internalHandler = null!;
        var tcs = new TaskCompletionSource<Action<object>>();
        
        internalHandler = async packet =>
        {
            if (packet.GetType() == typeof(TPacket))
            {
                await asyncHandler((TPacket)packet);
                tcs.SetResult(internalHandler);
            }
        };

        PacketReceived += internalHandler;

        return tcs.Task;
    }

    public async ValueTask DisconnectAsync() => await DisconnectAsync(null);

    void IDisposable.Dispose() => DisconnectAsync().GetAwaiter().GetResult();

    async ValueTask IAsyncDisposable.DisposeAsync() => await DisconnectAsync();
    
    private async ValueTask DisconnectAsync(Exception? exception)
    {
        lock (_lock)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
        }

        if (_listeningTask is not null)
        {
            _listeningTaskCts.Cancel();
            await _listeningTask;
        }

        _tcpClient.Close();

        if (_writer is not null)
        {
            await _writer.DisposeAsync();
        }

        ConnectionState = JavaConnectionState.Disconnected;
        
        _logger.LogInformation("Disconnected.");
        Disconnected?.Invoke(exception);
    }

    private void StartListening()
    {
        using var stream = _tcpClient.GetStream();
        using var reader = new MinecraftReader(stream);

        while (true)
        {
            if (_listeningTaskCts.Token.IsCancellationRequested)
            {
                return;
            }

            if (stream.DataAvailable)
            {
                // todo: do we need this ???
                lock (_lock)
                {
                    int packetLength;

                    try
                    {
                        packetLength = reader.ReadVarInt();
                    }
                    catch (Exception exception)
                    {
                        _logger.LogCritical(exception, $"Error while reading packet length.");
                        Task.Run(() => DisconnectAsync(exception));

                        return;
                    }

                    var packetBytes = reader.ReadBytes(packetLength);
                    var context     = new JavaPacketContext(ConnectionState, PacketDirection.FromServer);
                    
                    object packet;

                    try
                    {
                        packet = JavaPacketSerializer.Deserialize(packetBytes, Protocol, context);

                        //_logger.LogDebug($"Packet {packet.Data.GetType().Name} was received (0x{packet.Id:X2}, {context.ConnectionState} state).");
                        _logger.LogDebug($"[{ConnectionState.ToString4()}] Packet {packet.GetType().Name} was received.");
                    }
                    catch (NotSupportedException exception) // todo: change to NotSupportedPacketException
                    {
                        _logger.LogWarning(exception.Message);
                        return;
                    }
                    //catch (PacketDeserializationException exception)
                    //{
                    //    _logger.LogWarning(exception.Message);
                    //}
                    catch (Exception exception)
                    {
                        _logger.LogCritical(exception, $"Error while reading packet.");
                        Task.Run(() => DisconnectAsync(exception));

                        return;
                    }

                    Task.Run(() => _packetReceived?.Invoke(packet));
                    ConnectionState = Protocol.GetNextConnectionState(packet, context);
                }
            }
        }
    }

    // todo: rename to SendPacketPrivate/SendPacketCore ???
    private void SendPacketInternal(object packet)
    {
        JavaPacketSerializer.Serialize(_writer!, packet, Protocol, new JavaPacketContext(ConnectionState, PacketDirection.ToServer));
        //_logger.LogDebug($"Packet {packet.GetType().Name} (0x{packetId:X2}, {ConnectionState} state) sended");
        _logger.LogDebug($"[{ConnectionState.ToString4()}] Packet {packet.GetType().Name} was sent.");
    }
}