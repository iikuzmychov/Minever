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
        string hostname, ushort port, ILoggerFactory loggerFactory, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(hostname);

        await using var client = new JavaPacketClient(new JavaProtocol0(), loggerFactory);
        await client.ConnectAsync(hostname, port, cancellationToken);

        var handshake = new Handshake(client.Protocol.Version, hostname, port, HandshakeNextState.Status);
        client.SendPacket(handshake);

        var status       = await client.SendRequestAsync<ServerStatus>(new ServerStatusRequest(), cancellationToken);
        var pingRequest  = new Ping(DateTime.Now);
        var pingResponse = await client.SendRequestAsync<Ping>(pingRequest, cancellationToken);
        var ping         = pingResponse.CalculateDelay(DateTime.Now);

        return (status, ping);
    }

    public static async Task<(ServerStatus Status, TimeSpan Ping)> PingServerAsync
        (string hostname, ushort port = 25565, CancellationToken cancellationToken = default) =>
        await PingServerAsync(hostname, port, NullLoggerFactory.Instance, cancellationToken);

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
