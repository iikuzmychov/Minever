using Minever.Core;

namespace Minever.Bedrock.Core;

public sealed class BedrockProtocolClient : IProtocolClient
{
    public IBedrockProtocol Protocol { get; }
    IProtocol IPacketTransceiver.Protocol => Protocol;

    public bool IsConnected => throw new NotImplementedException();

    public event Action<object, DateTime>? PacketReceived;

    public BedrockProtocolClient(IBedrockProtocol protocol)
    {
        Protocol = protocol ?? throw new ArgumentNullException(nameof(protocol));
    }

    public ValueTask ConnectAsync(string host, int port = 25565, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void SendPacket(object packet)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public static ValueTask<(IServerInfo Info, TimeSpan Ping)> PingAsync(
        string host, int port = 25565, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}