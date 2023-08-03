using Minever.LowLevel.Java.Core;
using Minever.LowLevel.Java.Protocols.V5;
using Minever.LowLevel.Java.Protocols.V5.Packets.Handshake;
using Minever.LowLevel.Java.Protocols.V5.Packets.Login;
using Minever.LowLevel.Java.Protocols.V5.Packets.Play;
using Minever.LowLevel.Java.Protocols.V5.Packets.Status;
using Xunit.Abstractions;

namespace Minever.LowLevel.Tests.Integration;

public class JavaProtocol5Tests : TestBase, IClassFixture<JavaServer1_7_10>
{
    private readonly JavaServer1_7_10 _server;

    public JavaProtocol5Tests(JavaServer1_7_10 server, ITestOutputHelper output) : base(output)
    {
        _server = server;
    }

    /// <summary>
    /// Test scenario:
    /// <code>
    /// C ----Handshake(Status)---&gt; S<br/>
    /// <br/>
    /// C ---ServerStatusRequest--&gt; S<br/>
    /// C &lt;------ServerStatus------ S
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

        await using var client = new JavaProtocolClient(JavaProtocol5.Instance, ClientLogger);

        // Act
        await client.ConnectAsync(_server.Host, _server.GetPort(), CreateDefaultTimeoutCancellationToken());
        
        client.SendPacket(handshake);
        var serverStatus = await client.GetPacketAsync<ServerStatus>(new ServerStatusRequest(), CreateDefaultTimeoutCancellationToken());

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
    /// C ----Handshake(Status)---&gt; S<br/>
    /// <br/>
    /// C ---ServerStatusRequest--&gt; S<br/>
    /// C &lt;------ServerStatus------ S<br/>
    /// <br/>
    /// C -------PingToServer-----&gt; S<br/>
    /// C &lt;-----PingFromServer----- S
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

        var pingToServer = PingToServer.FromDateTime(DateTime.Now); // todo: create it before GetPacketAsync<PingFromServer> & check delay

        await using var client = new JavaProtocolClient(JavaProtocol5.Instance, ClientLogger);

        // Act
        await client.ConnectAsync(_server.Host, _server.GetPort(), CreateDefaultTimeoutCancellationToken());
        
        client.SendPacket(handshake);
        _ = await client.GetPacketAsync<ServerStatus>(new ServerStatusRequest(), CreateDefaultTimeoutCancellationToken());

        var pingFromServer = await client.GetPacketAsync<PingFromServer>(pingToServer, CreateDefaultTimeoutCancellationToken());

        await client.DisconnectAsync();

        // Assert
        Assert.Equal(pingToServer.Payload, pingFromServer.Payload);
    }

    /// <summary>
    /// Test scenario:
    /// <code>
    /// C ---Handshake(Login)--&gt; S<br/>
    /// <br/>
    /// C ------LoginStart-----&gt; S<br/>
    /// C &lt;----LoginSuccess----- S
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

        var loginStart = new LoginStart()
        {
            Name = "player"
        };

        await using var client = new JavaProtocolClient(JavaProtocol5.Instance, ClientLogger);

        // Act
        await client.ConnectAsync(_server.Host, _server.GetPort(), CreateDefaultTimeoutCancellationToken());

        client.SendPacket(handshake);
        var loginSuccess = await client.GetPacketAsync<LoginSuccess>(loginStart, CreateDefaultTimeoutCancellationToken());

        await client.DisconnectAsync();

        // Assert
        Assert.Equal(loginStart.Name, loginSuccess.Name);
    }

    /// <summary>
    /// Test scenario:
    /// <code>
    /// C -------Handshake(Login)-----&gt; S<br/>
    /// <br/>
    /// C ----------LoginStart--------&gt; S<br/>
    /// C &lt;--------LoginSuccess-------- S<br/>
    /// C &lt;--PluginMessageFromServer--- S
    /// </code>
    /// </summary>
    [Fact]
    public async Task Should_GetBrandPluginMessage()
    {
        // Arrange
        var handshake = new Handshake()
        {
            ProtocolVersion = JavaProtocol5.Instance.Version,
            NextConnectionState = HandshakeNextConnectionState.Login,
        };

        var loginStart = new LoginStart()
        {
            Name = "player"
        };

        await using var client = new JavaProtocolClient(JavaProtocol5.Instance, ClientLogger);

        // Act
        await client.ConnectAsync(_server.Host, _server.GetPort(), CreateDefaultTimeoutCancellationToken());

        client.SendPacket(handshake);
        _ = await client.GetPacketAsync<LoginSuccess>(loginStart, CreateDefaultTimeoutCancellationToken());

        // todo: waiting should be started once LoginSuccess is received (the current call doesn't guarantee that awaiting will be started before the server processes the next packet)
        var pluginMessage = await client.WaitForPacketAsync<PluginMessageFromServer>(CreateDefaultTimeoutCancellationToken());

        await client.DisconnectAsync();

        // Assert
        Assert.Equal("MC|Brand", pluginMessage.Channel);
        // todo: Assert.Equal("vanilla", pluginMessage.Data.Name);
    }

    /// <summary>
    /// Test scenario:
    /// <code>
    /// C -------Handshake(Login)-----&gt; S <br/>
    /// <br/>
    /// C ----------LoginStart--------&gt; S <br/>
    /// C &lt;--------LoginSuccess-------- S <br/>
    /// C &lt;-------SpawnPosition-------- S
    /// </code>
    /// </summary>
    [Fact]
    public async Task Should_GetSpawnPosition()
    {
        // Arrange
        var handshake = new Handshake()
        {
            ProtocolVersion = JavaProtocol5.Instance.Version,
            NextConnectionState = HandshakeNextConnectionState.Login,
        };

        var loginStart = new LoginStart()
        {
            Name = "player"
        };

        await using var client = new JavaProtocolClient(JavaProtocol5.Instance, ClientLogger);

        // Act
        await client.ConnectAsync(_server.Host, _server.GetPort(), CreateDefaultTimeoutCancellationToken());

        client.SendPacket(handshake);
        _ = await client.GetPacketAsync<LoginSuccess>(loginStart, CreateDefaultTimeoutCancellationToken());

        // todo: waiting should be started once LoginSuccess is received (the current call doesn't guarantee that awaiting will be started before the server processes the next packet)
        var spawnPosition = await client.WaitForPacketAsync<SpawnPosition>(CreateDefaultTimeoutCancellationToken());

        await client.DisconnectAsync();

        // Assert
        Assert.NotEqual(default, spawnPosition.Value);
    }
}