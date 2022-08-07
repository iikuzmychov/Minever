using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;

namespace Minever.Client;

public class MinecraftClient : IAsyncDisposable, IDisposable
{
    private readonly ILogger<MinecraftClient> _logger;
    private readonly MinecraftPacketClient _packetClient;

    public static async Task<(ServerStatus Status, TimeSpan Delay)> PingServerAsync(
        string serverAddress, ushort serverPort, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        await using var client = new MinecraftPacketClient(new Protocol0(), loggerFactory);
        await client.ConnectAsync(serverAddress, serverPort);

        var handshake = new Handshake(client.Protocol.Version, serverAddress, serverPort, HandshakeNextState.Status);
        client.SendPacket(handshake);

        var statusResponse = await client.SendRequestAsync<ServerStatus>(new ServerStatusRequest());
        var status         = statusResponse.Packet.Data;
        var delayResponse  = await client.SendRequestAsync<Ping>(new Ping(DateTime.Now));
        var delay          = delayResponse.Packet.Data.CalculateDelay(delayResponse.ReceivedDateTime);

        return (status, delay);
    }

    public static async Task<(ServerStatus Status, TimeSpan Delay)> PingServerAsync(string serverAddress, ushort serverPort) =>
        await PingServerAsync(serverAddress, serverPort, NullLoggerFactory.Instance);

    public MinecraftClient(MinecraftProtocol protocol, ILoggerFactory loggerFactory)
    {
        _logger       = loggerFactory?.CreateLogger<MinecraftClient>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        _packetClient = new MinecraftPacketClient(protocol);

        _packetClient.OnPacket<KeepAlive>(keepAlive => _packetClient.SendPacket(keepAlive));
    }

    public async Task ConnectAsync(string serverAddress, ushort serverPort = 25565)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        await _packetClient.ConnectAsync(serverAddress, serverPort);
    }

    public async Task DisconnectAsync() => await _packetClient.DisconnectAsync();
    
    public void Disconnect() => _packetClient.Disconnect();

    public async ValueTask DisposeAsync() => await DisconnectAsync();

    void IDisposable.Dispose() => Disconnect();
}
