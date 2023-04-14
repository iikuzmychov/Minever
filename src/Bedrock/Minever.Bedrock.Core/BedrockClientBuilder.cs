using Minever.Core;

namespace Minever.Bedrock.Core;

public class BedrockClientBuilder : AbstractMinecraftClientBuilder
{
    public static BedrockClientBuilder ForProtocol(IBedrockProtocol protocol) => new(protocol);

    private BedrockClientBuilder(IBedrockProtocol protocol) : base(new BedrockProtocolClient(protocol)) { }
}
