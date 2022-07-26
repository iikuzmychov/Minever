using Minever.Networking;
using Minever.Networking.Exceptions;
using Minever.Networking.IO;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Minever.Client;

public sealed class MinecraftPacketClient : IDisposable
{
    private readonly TcpClient _tcpClient = new();
    private bool _isListeningPaused = true;
    private CancellationTokenSource? _listenCancellationSource;
    private Task? _listenTask;
    private MinecraftWriter? _writer;
    private ILogger<MinecraftPacketClient>? _logger;

    public event Action<MinecraftPacket<object>>? PacketReceived;
    public event Action? OnDisconnected;

    public MinecraftProtocol Protocol { get; }
    public bool IsConnected => _tcpClient.Connected;
    public MinecraftConnectionState ConnectionState { get; private set; } = MinecraftConnectionState.Handshake;

    public MinecraftPacketClient(MinecraftProtocol protocol, ILogger<MinecraftPacketClient>? logger = null)
    {
        ArgumentNullException.ThrowIfNull(protocol);

        Protocol = protocol;
        _logger  = logger;
    }

    private void ListenStream()
    {
        using var newtworkStream  = _tcpClient.GetStream();
        using var minecraftStream = new MinecraftStream(newtworkStream, false);
        using var reader          = new MinecraftReader(minecraftStream);
        
        while (true)
        {
            if (_listenCancellationSource!.IsCancellationRequested)
                break;

            if (!_isListeningPaused && newtworkStream.DataAvailable)
            {
                int packetLength;

                try
                {
                    packetLength = reader.Read7BitEncodedInt();
                }
                catch (Exception exception)
                {
                    _logger?.LogCritical(exception, $"Error while reading packet length.");
                    Task.Run(Disconnect);
                    
                    return;
                }

                var packet                = (MinecraftPacket<object>?)null;
                var startReadedBytesCount = minecraftStream.TotalReadedBytesCount;
                var packetKind            = new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, ConnectionState);

                try
                {
                    packet = reader.ReadPacket(packetLength, packetKind, Protocol);

                    _logger?.LogDebug($"Packet {packet.Data.GetType().Name} was received (0x{packet.Id:X2}, {packetKind.ConnectionState} state).");
                }
                catch (NotSupportedPacketException exception)
                {
                    _logger?.LogWarning(exception.Message);
                }
                catch (IOException exception)
                {
                    _logger?.LogCritical(exception, $"An 'IOException' occured.");
                    Task.Run(Disconnect);

                    return;
                }
                catch (Exception exception)
                {
                    _logger?.LogError(exception, $"Error while reading packet.");
                }

                var readedBytesCount = minecraftStream.TotalReadedBytesCount - startReadedBytesCount;

                if (readedBytesCount != packetLength)
                {
                    var packetInfoString = packet is null ? "packet ???" : $"packet { packet.Data.GetType().Name} (0x{ packet.Id:X2}, {packetKind.ConnectionState} state)";

                    if (readedBytesCount < packetLength)
                    {
                        _logger?.LogError($"Count of bytes read less than the packet length: {packetInfoString}. Remaining packet bytes will be skipped.");
                        reader.ReadBytes(packetLength - readedBytesCount);
                    }
                    else
                    {
                        _logger?.LogCritical($"Count of bytes read more than the packet length: {packetInfoString}.");
                        Task.Run(Disconnect);

                        return;
                    }
                }

                if (packet is not null)
                {
                    Task.Run(() => PacketReceived?.Invoke(packet));

                    ConnectionState = Protocol.GetNewState(packet);
                }
            }
        }
    }

    public async Task ConnectAsync(string serverAddress, ushort serverPort, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        await _tcpClient.ConnectAsync(serverAddress, serverPort, cancellationToken);
        _logger?.LogInformation("Connection established.");

        _writer                   = new(_tcpClient.GetStream());
        _listenCancellationSource = new();
        _isListeningPaused        = false;
        _listenTask               = Task.Run(ListenStream, _listenCancellationSource.Token);
    }

    public async Task ConnectAsync(string serverAddress, ushort serverPort, TimeSpan timeout)
    {
        using (var cancellationTokenSource = new CancellationTokenSource(timeout))
            await ConnectAsync(serverAddress, serverPort, cancellationTokenSource.Token);
    }

    public async Task ConnectAsync(string serverAddress, ushort serverPort = 25565)
    {
        using (var cancellationTokenSource = new CancellationTokenSource())
            await ConnectAsync(serverAddress, serverPort, cancellationTokenSource.Token);
    }

    public void Disconnect()
    {
        if (IsConnected)
        {
            _isListeningPaused = true;

            if (PacketReceived is not null)
                foreach (var handler in PacketReceived.GetInvocationList())
                    PacketReceived -= (handler as Action<MinecraftPacket<object>>);

            _listenCancellationSource?.Cancel();
            _listenTask?.Wait();
            _tcpClient.Close();
            _logger?.LogInformation("Disconnected.");
            OnDisconnected?.Invoke();
        }
    }

    void IDisposable.Dispose() => Disconnect();

    public Action<MinecraftPacket<object>> OnPacket<TData>(Action<MinecraftPacket<TData>> action)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(action);

        Action<MinecraftPacket<object>> handler = packet =>
        {
            if (packet.Data is TData)
                action((MinecraftPacket<TData>)packet);
        };

        PacketReceived += handler;

        return handler;
    }

    public void OnceOnPacket<TData>(Action<MinecraftPacket<TData>> action)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(action);

        Action<MinecraftPacket<object>>? handler = null;

        handler = OnPacket<TData>(packet =>
        {
            PacketReceived -= handler;
            action(packet);
        });
    }

    /*public void SendCustomPacket(int packetId, object packetData)
    {
        ArgumentNullException.ThrowIfNull(packetData);

        var packetKind
        var packet = new MinecraftPacket<object(packetId, );

        _isListeningPaused = true;

        _writer!.WritePacket(packetId, packetData);

        ConnectionState    = Protocol.GetNewState(ConnectionState, packetData);
        _isListeningPaused = false;
    }*/

    public void SendPacket(object packetData)
    {
        ArgumentNullException.ThrowIfNull(packetData);

        var packetKind = new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, ConnectionState);
        var packetId   = Protocol.GetPacketId(packetData.GetType(), packetKind);
        var packet     = new MinecraftPacket<object>(packetId, packetKind, packetData);

        _isListeningPaused = true;

        try
        {
            _writer!.WritePacket(packet);
            _logger?.LogDebug($"Packet {packet.Data.GetType().Name} (0x{packetId:X2}, {packetKind.ConnectionState} state) was sended.");
        }
        catch (IOException exception)
        {
            _logger?.LogCritical(exception, $"An 'IOException' occured.");
            Disconnect();

            return;
        }
        catch
        {
            throw;
        }

        ConnectionState    = Protocol.GetNewState(packet);
        _isListeningPaused = false;
    }

    public async Task<MinecraftPacket<TResponseData>> SendRequestAsync<TResponseData>(
        object requestPacketData, TimeSpan timeout)
        where TResponseData : notnull
    {
        ArgumentNullException.ThrowIfNull(requestPacketData);

        var taskCompletionSource = new TaskCompletionSource<MinecraftPacket<TResponseData>>();

        _isListeningPaused = true;

        SendPacket(requestPacketData);
        OnceOnPacket<TResponseData>(packet => taskCompletionSource.SetResult(packet));
        
        _isListeningPaused = false;

        var responsePacket = await taskCompletionSource.Task.WaitAsync(timeout);

        return responsePacket;
    }
}
