namespace Minever.LowLevel.Core;

// todo: do we need IPacketTransceiver ???
public interface IProtocolClient : IPacketTransceiver, IAsyncDisposable, IDisposable
{
    public bool IsConnected { get; }

    // todo: remove
    public abstract static ValueTask<(IServerInfo Info, TimeSpan Ping)> PingAsync(
        string host, int port, CancellationToken cancellationToken = default);

    public ValueTask ConnectAsync(string host, int port, CancellationToken cancellationToken = default);

    public ValueTask DisconnectAsync();
}