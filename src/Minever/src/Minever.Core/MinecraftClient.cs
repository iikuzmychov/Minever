using Microsoft.Extensions.Logging;
using Minever.Core.Controllers;
using Minever.Core.Extensions;
using Minever.LowLevel.Core;

namespace Minever.Core;

public sealed class MinecraftClient : IClientDataProvider, IAsyncDisposable, IDisposable
{
    private readonly IProtocolClient _protocolClient;

    public bool Connected => _protocolClient.IsConnected;
    public IPacketTransceiver PacketTransceiver => _protocolClient;
    public IControllerProvider Controllers { get; }
    public ILoggerFactory LoggerFactory { get; }

    internal MinecraftClient(IProtocolClient protocolClient, IControllerCollection controllerCollection, ILoggerFactory loggerFactory)
    {
        _protocolClient = protocolClient;
        Controllers     = controllerCollection.BuildServiceProvider(this);
        LoggerFactory   = loggerFactory;
    }

    public async ValueTask ConnectAsync(string host, int port, CancellationToken cancellationToken = default)
        => await _protocolClient.ConnectAsync(host, port, cancellationToken);

    public async ValueTask DisconnectAsync() => await _protocolClient.DisconnectAsync();

    async ValueTask IAsyncDisposable.DisposeAsync() => await _protocolClient.DisposeAsync();

    void IDisposable.Dispose() => _protocolClient.Dispose();
}
