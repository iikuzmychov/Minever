using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Minever.LowLevel.Java.Core;
using Minever.LowLevel.Java.Protocols.V5;
using Minever.LowLevel.Java.Protocols.V5.Packets;
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

    [Fact]
    public async Task Should_GetServerStatus()
    {
        // Arrange
        using var connectionCancellationTokenSource = new CancellationTokenSource();
        using var waitPacketCancellationTokenSource = new CancellationTokenSource();

        await using var client = new JavaProtocolClient(JavaProtocol5.Instance);

        // Act
        var serverStatusTask = client.WaitPacketAsync<ServerStatus>(waitPacketCancellationTokenSource.Token);

        connectionCancellationTokenSource.CancelAfter(millisecondsDelay: 1000);
        await client.ConnectAsync("localhost", Port, connectionCancellationTokenSource.Token);

        client.SendPacket(new Handshake() { NextConnectionState = HandshakeNextConnectionState.Status });
        client.SendPacket(new ServerStatusRequest());
        waitPacketCancellationTokenSource.CancelAfter(millisecondsDelay: 1000);

        var (serverStatus, _) = await serverStatusTask;
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

    [GeneratedRegex(@"^\[[0-9:]+\] \[Server thread/INFO\]: Done")]
    private static partial Regex ServerReadyLogMessageRegex();
}