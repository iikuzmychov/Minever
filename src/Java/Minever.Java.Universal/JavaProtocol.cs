using Minever.Core;
using Minever.Java.Core;

namespace Minever.Java.Universal;

public static class JavaProtocol
{
    public static IJavaProtocol FromVersion(int protocolVersion) =>
        protocolVersion switch
        {
            5 => Protocols.V5.JavaProtocol5.Instance,

            _ => throw new NotSupportedException(nameof(protocolVersion))
        };

    public static async Task<IProtocol> DetectAsync(string host, int port = 25565, CancellationToken cancellationToken = default)
    {
        // todo: IJavaPingModule.PingAsync ???
        var (serverInfo, _) = await JavaProtocolClient.PingAsync(host, port, cancellationToken);
        return FromVersion(serverInfo.ProtocolVersion);
    }
}
