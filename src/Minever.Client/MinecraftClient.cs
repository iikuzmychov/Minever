using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;

namespace Minever.Client;

public class MinecraftClient : IAsyncDisposable, IDisposable
{
    private readonly ILogger<MinecraftClient> _logger;
    private readonly JavaPacketClient _packetClient;

    public static async Task<(ServerStatus Status, TimeSpan Ping)> PingServerAsync(
        string serverAddress, ushort serverPort, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        await using var client = new JavaPacketClient(new JavaProtocol0(), loggerFactory);
        await client.ConnectAsync(serverAddress, serverPort);

        var handshake = new Handshake(client.Protocol.Version, serverAddress, serverPort, HandshakeNextState.Status);
        client.SendPacket(handshake);

        var status       = await client.SendRequestAsync<ServerStatus>(new ServerStatusRequest());
        var pingRequest  = new Ping(DateTime.Now);
        var pingResponse = await client.SendRequestAsync<Ping>(pingRequest);
        var ping         = pingResponse.CalculateDelay(DateTime.Now);

        return (status, ping);
    }

    public static async Task<(ServerStatus Status, TimeSpan Ping)> PingServerAsync(string serverAddress, ushort serverPort) =>
        await PingServerAsync(serverAddress, serverPort, NullLoggerFactory.Instance);

    public MinecraftClient(JavaProtocol protocol, ILoggerFactory loggerFactory)
    {
        _logger       = loggerFactory?.CreateLogger<MinecraftClient>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        _packetClient = new JavaPacketClient(protocol);

        _packetClient.OnPacket<KeepAlive>(keepAlive => _packetClient.SendPacket(keepAlive));
    }

    public async Task ConnectAsync(string serverAddress, ushort serverPort = 25565)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        await _packetClient.ConnectAsync(serverAddress, serverPort);
    }

    public async Task DisconnectAsync() => await _packetClient.DisconnectAsync();

    public async ValueTask DisposeAsync() => await DisconnectAsync();

    void IDisposable.Dispose() => ((IDisposable)_packetClient).Dispose();
}
