using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Minever.Core;
using Minever.Core.IO;
using Minever.Java.Core.Packets.Serialization;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Principal;

namespace Minever.Java.Core;

public sealed class JavaProtocolClient : IProtocolClient
{
    private readonly ILogger<JavaProtocolClient> _logger;
    private readonly TcpClient _tcpClient = new();
    private readonly object _lock = new();
    private bool _isDisposed = false;

    private MinecraftWriter? _writer;
    private Task? _listeningTask;
    private readonly CancellationTokenSource _listeningTaskCts = new();
    private event Action<object, DateTime>? _packetReceived;
    
    public IJavaProtocol Protocol { get; }
    IProtocol IPacketTransceiver.Protocol => Protocol;
    // todo: Disconnected -> None ???
    // todo: Disconnected -> null ???
    public JavaConnectionState ConnectionState { get; private set; } = JavaConnectionState.Disconnected;
    public bool IsConnected => _tcpClient.Connected;

    public event Action<object, DateTime> PacketReceived
    {
        add => _packetReceived += value;
        remove => _packetReceived -= value;
    }

    public JavaProtocolClient(IJavaProtocol protocol, ILogger<JavaProtocolClient>? logger = null)
    {
        Protocol = protocol ?? throw new ArgumentNullException(nameof(protocol));
        _logger  = logger ?? NullLogger<JavaProtocolClient>.Instance;
    }

    public static ValueTask<(IServerInfo Info, TimeSpan Ping)> PingAsync(
        string host, int port = 25565, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask ConnectAsync(string host, int port = 25565, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(host);

        /*if (_isDisposed)
            throw new ObjectDisposedException(GetType().FullName);*/

        _logger.LogInformation($"Connecting to {host}:{port}");
        await _tcpClient.ConnectAsync(host, port, cancellationToken);
        _logger.LogInformation($"Connected to {host}:{port}");

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
                Send();
                ConnectionState = nextState;
            }
        }
        else
        {
            Send();
        }

        void Send() // awful method
        {
            JavaPacketSerializer.Serialize(_writer, packet, Protocol, context);
            //_logger.LogDebug($"Packet {packet.GetType().Name} (0x{packetId:X2}, {ConnectionState} state) sended");
            _logger.LogDebug($"Packet {packet.GetType().Name} ({ConnectionState} state) sended");
        }
    }

    public Action<object, DateTime> OnPacket<TPacket>(Action<TPacket, DateTime> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        Action<object, DateTime> internalHandler = (packet, dateTime) =>
        {
            if (packet.GetType() == typeof(TPacket))
            {
                handler((TPacket)packet, dateTime);
            }
        };

        PacketReceived += internalHandler;

        return internalHandler;
    }

    public async ValueTask DisconnectAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _listeningTaskCts.Cancel();
        
        if (_listeningTask is not null)
        {
            await _listeningTask;
        }

        _tcpClient.Close();

        if (_writer is not null)
        {
            await _writer.DisposeAsync();
        }

        ConnectionState = JavaConnectionState.Disconnected;
        _logger.LogInformation("Disconnected.");
        //Disconnected?.Invoke();
    }

    void IDisposable.Dispose() => DisconnectAsync().GetAwaiter().GetResult();

    async ValueTask IAsyncDisposable.DisposeAsync() => await DisconnectAsync();

    private void StartListening()
    {
        using var stream = _tcpClient.GetStream();
        using var reader = new MinecraftReader(stream);

        while (true)
        {
            _listeningTaskCts.Token.ThrowIfCancellationRequested();

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
                        Task.Run(DisconnectAsync);

                        return;
                    }

                    var packetBytes = reader.ReadBytes(packetLength);
                    var context     = new JavaPacketContext(ConnectionState, PacketDirection.FromServer);
                    
                    object packet;

                    try
                    {
                        packet = JavaPacketSerializer.Deserialize(packetBytes, Protocol, context);

                        //_logger.LogDebug($"Packet {packet.Data.GetType().Name} was received (0x{packet.Id:X2}, {context.ConnectionState} state).");
                        _logger.LogDebug($"Packet {packet.GetType().Name} was received (state: {context.ConnectionState}).");
                    }
                    //catch (NotSupportedPacketException exception)
                    //{
                    //    _logger.LogWarning(exception.Message);
                    //}
                    //catch (PacketDeserializationException exception)
                    //{
                    //    _logger.LogWarning(exception.Message);
                    //}
                    catch (Exception exception)
                    {
                        _logger.LogCritical(exception, $"Error while reading packet.");
                        Task.Run(DisconnectAsync);

                        return;
                    }

                    Task.Run(() => _packetReceived?.Invoke(packet, DateTime.Now));
                    ConnectionState = Protocol.GetNextConnectionState(packet, context);
                }
            }
        }
    }
}