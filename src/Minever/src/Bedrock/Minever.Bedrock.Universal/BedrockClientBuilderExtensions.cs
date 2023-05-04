using Minever.Core;
using Minever.Bedrock.Core;

namespace Minever.Bedrock.Universal;

public static class BedrockClientBuilderExtensions
{
    public static BedrockClientBuilder AddChatController(this BedrockClientBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        IChatController chat = builder.ProtocolClient.Protocol switch
        {


            _ => throw new NotSupportedException()
        };

        builder.AddController<IChatController>(chat);

        return builder;
    }
}
