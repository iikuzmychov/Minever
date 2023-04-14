using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Minever.Core.Controllers;

namespace Minever.Core;

// todo: weird naming
public abstract class AbstractMinecraftClientBuilder
{
    private Action<ILoggingBuilder>? _configureLogging;

    public IProtocolClient ProtocolClient { get; }
    public IControllerCollection Controllers { get; } = new ControllerCollection();

    public AbstractMinecraftClientBuilder(IProtocolClient packetClient)
    {
        ProtocolClient = packetClient ?? throw new ArgumentNullException(nameof(packetClient));
    }

    // todo: replace with TMinecraftClientBuilder ???
    public AbstractMinecraftClientBuilder ConfigureLogging(Action<ILoggingBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        _configureLogging = configure;

        return this;
    }

    public MinecraftClient Build() => new(ProtocolClient, Controllers, CreateLoggerFactory());

    protected ILoggerFactory CreateLoggerFactory()
        => _configureLogging is null
            ? NullLoggerFactory.Instance
            : LoggerFactory.Create(_configureLogging);
}
