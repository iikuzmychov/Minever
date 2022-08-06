using Microsoft.Extensions.Logging;
using Minever.Networking.DataTypes;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;

namespace Minever.Client;

public class MinecraftClient : IAsyncDisposable, IDisposable
{
    private readonly ILogger<MinecraftClient> _logger;
    private readonly MinecraftPacketClient _packetClient;

    public static async Task<(ServerStatus ServerStatus, TimeSpan Delay)> CheckServerAsync(string serverAddress, ushort serverPort)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        await using var client = new MinecraftPacketClient(new Protocol0());
        await client.ConnectAsync(serverAddress, serverPort);

        var handshake = new Handshake(client.Protocol.Version, serverAddress, serverPort, HandshakeNextState.Status);
        client.SendPacket(handshake);

        var serverStatus  = (await client.SendRequestAsync<ServerStatusResponse>(new ServerStatusRequest())).Packet.Data.Status;        
        var delayResponse = await client.SendRequestAsync<Ping>(new Ping(DateTime.Now));
        var delay         = delayResponse.Packet.Data.CalculateDelay(delayResponse.ReceivedDateTime);

        return (serverStatus, delay);
    }

    public MinecraftClient(MinecraftProtocol protocol, ILoggerFactory loggerFactory)
    {
        _logger       = loggerFactory?.CreateLogger<MinecraftClient>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        _packetClient = new MinecraftPacketClient(protocol);

        _packetClient.OnPacket<KeepAlive>(keepAlive => _packetClient.SendPacket(new KeepAlive(keepAlive.Id)));
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
