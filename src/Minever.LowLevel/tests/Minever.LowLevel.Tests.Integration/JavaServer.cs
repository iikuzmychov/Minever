using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Minever.LowLevel.Tests.Integration;

public abstract class JavaServer : IAsyncLifetime
{
    private const int DefaultPort = 25565;

    private readonly IContainer _javaServerContainer;
    
    public string Host => _javaServerContainer.Hostname;
    public abstract string Version { get; } // todo: rename to VersionName ???

    public JavaServer()
    {
        _javaServerContainer = new ContainerBuilder()
            .WithImage("itzg/minecraft-server")
            .WithName($"minever-java-server-{Guid.NewGuid()}")
            .WithEnvironment("VERSION", Version)
            .WithEnvironment("EULA", "TRUE")
            .WithEnvironment("ONLINE_MODE", "FALSE")
            .WithEnvironment("ENABLE_RCON", "FALSE")
            .WithEnvironment("ENABLE_AUTOPAUSE", "FALSE")
            .WithPortBinding(DefaultPort, assignRandomHostPort: true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync() => await _javaServerContainer.StartAsync();

    public async Task DisposeAsync() => await _javaServerContainer.DisposeAsync();

    public int GetPort() => _javaServerContainer.GetMappedPublicPort(25565);
}