using Minever.Core;

namespace Minever.Java.Core;

// stupid shit
public class JavaClientBuilder : AbstractMinecraftClientBuilder
{
    public static JavaClientBuilder ForProtocol(IJavaProtocol protocol) => new(protocol);

    private JavaClientBuilder(IJavaProtocol protocol) : base(new JavaProtocolClient(protocol)) { }
}
