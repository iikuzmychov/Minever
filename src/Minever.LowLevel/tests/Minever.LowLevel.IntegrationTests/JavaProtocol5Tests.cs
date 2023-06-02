using Minever.LowLevel.Java.Core;
using Minever.LowLevel.Java.Protocols.V5;
using Minever.LowLevel.Java.Protocols.V5.Packets;

namespace Minever.LowLevel.IntegrationTests;

[Collection(nameof(JavaServer1_7_10Collection))]
public class JavaProtocol5Tests : TestsBase
{
    private readonly JavaServer1_7_10 _server;

    public JavaProtocol5Tests(JavaServer1_7_10 server)
    {
        _server = server;
    }

    /// <summary>
    /// Test scenario:
    /// <code>
    /// C ----Handshake(Status)---&gt; S <br/>
    /// <br/>
    /// C ---ServerStatusRequest--&gt; S <br/>
    /// C &lt;------ServerStatus------ S <br/>
    /// </code>
    /// </summary>
    [Fact]
    public async Task Should_GetServerStatus()
    {
        // Arrange
        await using var client = new JavaProtocolClient(JavaProtocol5.Instance);

        // Act
        await client.ConnectAsync(_server.Host, _server.Port, new CancellationTokenSource(DefaultTimeout).Token);
        
        client.SendPacket(new Handshake() { NextConnectionState = HandshakeNextConnectionState.Status });
        var (serverStatus, _) = await client.GetPacketAsync<ServerStatus>(new ServerStatusRequest(), new CancellationTokenSource(DefaultTimeout).Token);

        await client.DisconnectAsync();

        // Assert
        Assert.Null(serverStatus.IconBytes);
        Assert.NotNull(serverStatus.PlayersInfo);
        Assert.False(string.IsNullOrEmpty(serverStatus.Description));
        Assert.Equal(JavaProtocol5.Instance.Version, serverStatus.Version.ProtocolVersion);
        Assert.Equal(_server.Version, serverStatus.Version.Name);
    }

    /// <summary>
    /// Test scenario:
    /// <code>
    /// C ----Handshake(Status)---&gt; S <br/>
    /// <br/>
    /// C ---ServerStatusRequest--&gt; S <br/>
    /// C &lt;------ServerStatus------ S <br/>
    /// <br/>
    /// C -----------Ping---------&gt; S <br/>
    /// C &lt;----------Ping---------- S <br/>
    /// </code>
    /// </summary>
    [Fact]
    public async Task Should_GetPing()
    {
        // Arrange
        var pingRequest = Ping.FromDateTime(new DateTime(2023, 01, 01));
        
        await using var client = new JavaProtocolClient(JavaProtocol5.Instance);

        // Act
        await client.ConnectAsync(_server.Host, _server.Port, new CancellationTokenSource(DefaultTimeout).Token);
        
        client.SendPacket(new Handshake() { NextConnectionState = HandshakeNextConnectionState.Status });
        _ = await client.GetPacketAsync<ServerStatus>(new ServerStatusRequest(), new CancellationTokenSource(DefaultTimeout).Token);

        var (pingResponse, _) = await client.GetPacketAsync<Ping>(pingRequest, new CancellationTokenSource(DefaultTimeout).Token);

        await client.DisconnectAsync();

        // Assert
        Assert.Equal(pingRequest.Payload, pingResponse.Payload);
    }
}