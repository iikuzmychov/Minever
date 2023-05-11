using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Minever.LowLevel.Java.Core;
using Minever.LowLevel.Java.Protocols.V5;
using Minever.LowLevel.Java.Protocols.V5.Packets;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace Minever.LowLevel.IntegrationTests;

public partial class JavaProtocol5Tests : IAsyncLifetime
{
    private const int Port = 25565;
    private const string MineraftVersion = "1.7.10";

    private readonly IContainer _javaServerContainer = new ContainerBuilder()
        .WithName("minecraft_java_server")
        .WithImage("itzg/minecraft-server")
        .WithEnvironment("VERSION", MineraftVersion)
        .WithEnvironment("EULA", "TRUE")
        .WithEnvironment("ONLINE_MODE", "FALSE")
        .WithPortBinding(Port)
        .WithExposedPort(Port)
        .WithCommand("-d -it")
        .WithCleanUp(true)
        .WithWaitStrategy(Wait.ForWindowsContainer().UntilMessageIsLogged("Done."))
        .Build();

    public async Task InitializeAsync()
    {
        await _javaServerContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _javaServerContainer.DisposeAsync();
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
        await client.ConnectAsync("localhost", Port, new CancellationTokenSource(millisecondsDelay: 1000).Token);
        
        client.SendPacket(new Handshake() { NextConnectionState = HandshakeNextConnectionState.Status });
        var (serverStatus, _) = await client.GetPacketAsync<ServerStatus>(new ServerStatusRequest(), new CancellationTokenSource(millisecondsDelay: 1000).Token);

        await client.DisconnectAsync();

        // Assert
        Assert.Null(serverStatus.IconBytes);
        Assert.NotNull(serverStatus.PlayersInfo);
        Assert.False(string.IsNullOrEmpty(serverStatus.Description));
        Assert.Equal(JavaProtocol5.Instance.Version, serverStatus.Version.ProtocolVersion);
        Assert.Equal(MineraftVersion, serverStatus.Version.Name);

        var playersInfo = serverStatus.PlayersInfo;
        Assert.Null(playersInfo.SamplePlayers);
        Assert.Equal(0, playersInfo.PlayersCount);
        Assert.Equal(20, playersInfo.MaxPlayersCount);
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
        await client.ConnectAsync("localhost", Port, new CancellationTokenSource(millisecondsDelay: 1000).Token);
        
        client.SendPacket(new Handshake() { NextConnectionState = HandshakeNextConnectionState.Status });
        _ = await client.GetPacketAsync<ServerStatus>(new ServerStatusRequest(), new CancellationTokenSource(millisecondsDelay: 1000).Token);

        var (pingResponse, _) = await client.GetPacketAsync<Ping>(pingRequest, new CancellationTokenSource(millisecondsDelay: 1000).Token);

        await client.DisconnectAsync();

        // Assert
        Assert.Equal(pingRequest.Payload, pingResponse.Payload);
    }
}