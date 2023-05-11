namespace Minever.LowLevel.Core;

// todo: do we need IPacketTransceiver ???
public interface IProtocolClient : IPacketTransceiver, IAsyncDisposable, IDisposable
{
    public bool IsConnected { get; }

    public ValueTask ConnectAsync(string host, int port, CancellationToken cancellationToken = default);

    public ValueTask DisconnectAsync();
}