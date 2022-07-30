using Microsoft.Extensions.Logging;
using Minever.Networking.DataTypes;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;

namespace Minever.Client;

public class MinecraftClient : IAsyncDisposable, IDisposable
{
    private readonly ILogger? _logger;
    private readonly MinecraftPacketClient _packetClient;

    public static async Task<(ServerStatus ServerStatus, TimeSpan Delay)> CheckServerAsync(string serverAddress, ushort serverPort)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        await using var client = new MinecraftPacketClient(new Protocol0());
        await client.ConnectAsync(serverAddress, serverPort);

        var handshake = new Handshake(client.Protocol.Version, serverAddress, serverPort, HandshakeNextState.Status);
        client.SendPacket(handshake);

        var serverStatus = (await client.SendRequestAsync<ServerStatusResponse>(new ServerStatusRequest()))
            .Data
            .Status;
        
        var delay = (await client.SendRequestAsync<Ping>(new Ping(DateTime.UtcNow)))
            .Data
            .CalculateDelay(DateTime.UtcNow);

        return (serverStatus, delay);
    }

    public MinecraftClient(MinecraftProtocol protocol, ILogger<MinecraftPacketClient>? logger = null)
    {
        ArgumentNullException.ThrowIfNull(protocol);

        _logger       = logger;
        _packetClient = new MinecraftPacketClient(protocol);

        _packetClient.OnPacket<KeepAlive>(packet => _packetClient.SendPacket(new KeepAlive(packet.Data.Id)));
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
