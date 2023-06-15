using Minever.LowLevel.Java.Core;
using Minever.LowLevel.Java.Protocols.V5;
using Minever.LowLevel.Java.Protocols.V5.Packets;

namespace Minever.LowLevel.IntegrationTests;

[Collection(nameof(JavaServer1_7_10Collection))]
public class JavaProtocol5Tests : TestBase
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
        var handshake = new Handshake()
        {
            ProtocolVersion = JavaProtocol5.Instance.Version,
            NextConnectionState = HandshakeNextConnectionState.Status,
        };

        await using var client = new JavaProtocolClient(JavaProtocol5.Instance);

        // Act
        await client.ConnectAsync(_server.Host, _server.Port, CreateDefaultTimeoutCancellationToken());
        
        client.SendPacket(handshake);
        var (serverStatus, _) = await client.GetPacketAsync<ServerStatus>(new ServerStatusRequest(), CreateDefaultTimeoutCancellationToken());

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
    /// C -------PingToServer-----&gt; S <br/>
    /// C &lt;-----PingFromServer----- S <br/>
    /// </code>
    /// </summary>
    [Fact]
    public async Task Should_GetPing()
    {
        // Arrange
        var handshake = new Handshake()
        {
            ProtocolVersion = JavaProtocol5.Instance.Version,
            NextConnectionState = HandshakeNextConnectionState.Status,
        };

        var pingToServer = PingToServer.FromDateTime(new DateTime(2023, 01, 01));
        
        await using var client = new JavaProtocolClient(JavaProtocol5.Instance);

        // Act
        await client.ConnectAsync(_server.Host, _server.Port, CreateDefaultTimeoutCancellationToken());
        
        client.SendPacket(handshake);
        _ = await client.GetPacketAsync<ServerStatus>(new ServerStatusRequest(), CreateDefaultTimeoutCancellationToken());

        var (pingFromServer, _) = await client.GetPacketAsync<PingFromServer>(pingToServer, CreateDefaultTimeoutCancellationToken());

        await client.DisconnectAsync();

        // Assert
        Assert.Equal(pingToServer.Payload, pingFromServer.Payload);
    }

    /// <summary>
    /// Test scenario:
    /// <code>
    /// C ---Handshake(Login)--&gt; S <br/>
    /// <br/>
    /// C ------LoginStart-----&gt; S <br/>
    /// C &lt;----LoginSuccess----- S <br/>
    /// </code>
    /// </summary>
    [Fact]
    public async Task Should_Login()
    {
        // Arrange
        var handshake = new Handshake()
        {
            ProtocolVersion = JavaProtocol5.Instance.Version,
            NextConnectionState = HandshakeNextConnectionState.Login,
        };

        var loginStart = new LoginStart() { Name = "player" };

        await using var client = new JavaProtocolClient(JavaProtocol5.Instance);

        // Act
        await client.ConnectAsync(_server.Host, _server.Port, CreateDefaultTimeoutCancellationToken());

        client.SendPacket(handshake);
        var (loginSuccess, _) = await client.GetPacketAsync<LoginSuccess>(loginStart, CreateDefaultTimeoutCancellationToken());

        await client.DisconnectAsync();

        // Assert
        Assert.Equal(loginStart.Name, loginSuccess.Name);
    }
}