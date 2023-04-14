using Minever.Core;
using Minever.Bedrock.Core;

namespace Minever.Bedrock.Universal;

public static class BedrockProtocol
{
    public static IProtocol FromVersion(int protocolVersion) =>
        protocolVersion switch
        {
            //431 => new Protocols.V431.BedrockProtocol431(),

            _ => throw new NotSupportedException(nameof(protocolVersion))
        };

    public static async Task<IProtocol> DetectAsync(string host, int port, CancellationToken cancellationToken = default)
    {
        var (serverInfo, _) = await BedrockProtocolClient.PingAsync(host, port, cancellationToken);
        return FromVersion(serverInfo.ProtocolVersion);
    }
}
