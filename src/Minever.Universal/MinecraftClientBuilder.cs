using Minever.Bedrock.Core;
using Minever.Core;
using Minever.Java.Core;

namespace Minever.Universal;

public static class MinecraftClientBuilder
{
    public static AbstractMinecraftClientBuilder ForProtocol(IProtocol protocol) =>
        protocol switch
        {
            IJavaProtocol javaProtocol => JavaClientBuilder.ForProtocol(javaProtocol),
            IBedrockProtocol bedrockProtocol => BedrockClientBuilder.ForProtocol(bedrockProtocol),

            _ => throw new NotSupportedException()
        };
}
