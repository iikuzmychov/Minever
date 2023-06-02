using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Minever.LowLevel.IntegrationTests;

public abstract class JavaServer : IAsyncLifetime
{
    private readonly IContainer _javaServerContainer;
    
    public int Port => 25565;
    public string Host => _javaServerContainer.Hostname;
    public abstract string Version { get; } // todo: rename to VersionName ???

    public JavaServer()
    {
        _javaServerContainer = new ContainerBuilder()
            .WithName("minecraft_java_server")
            .WithImage("itzg/minecraft-server")
            .WithEnvironment("VERSION", Version)
            .WithEnvironment("EULA", "TRUE")
            .WithEnvironment("ONLINE_MODE", "FALSE")
            .WithEnvironment("ENABLE_RCON", "FALSE")
            .WithEnvironment("ENABLE_AUTOPAUSE", "FALSE")
            .WithPortBinding(Port, Port)
            .WithExposedPort(Port)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync() => await _javaServerContainer.StartAsync();

    public async Task DisposeAsync() => await _javaServerContainer.DisposeAsync();
}