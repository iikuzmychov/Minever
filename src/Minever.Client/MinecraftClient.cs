using Minever.Networking.DataTypes;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;

namespace Minever.Client;

public class MinecraftClient : IDisposable
{
    public MinecraftPacketClient PacketClient { get; }

    public MinecraftClient()
    {
        PacketClient = new MinecraftPacketClient(MinecraftProtocol.FromVersion(0));

        PacketClient.OnPacket<KeepAlive>(packet => PacketClient.SendPacket(new KeepAlive(packet.Data.Id)));
    }

    // need to fix timeout
    public static async Task<(ServerStatus ServerStatus, TimeSpan Delay)> CheckServerAsync(
        string serverAddress, ushort serverPort, TimeSpan timeout)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        using var client = new MinecraftPacketClient(MinecraftProtocol.FromVersion(0));
        await client.ConnectAsync(serverAddress, serverPort, timeout);

        var handshake = new Handshake(client.Protocol.Version, serverAddress, serverPort, HandshakeNextState.Status);
        client.SendPacket(handshake);

        var serverStatus = (await client.SendRequestAsync<ServerStatusResponse>(new ServerStatusRequest(), timeout))
            .Data
            .Status;
        
        var delay = (await client.SendRequestAsync<Ping>(new Ping(DateTime.UtcNow), timeout))
            .Data
            .CalculateDelay(DateTime.UtcNow);

        return (serverStatus, delay);
    }

    public async Task ConnectAsync(string serverAddress, ushort serverPort = 25565)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        await PacketClient.ConnectAsync(serverAddress, serverPort);
        
        throw new NotImplementedException();
    }

    public void Disconnect() => PacketClient.Disconnect();

    void IDisposable.Dispose() => Disconnect();
}
