using Minever.Bedrock.Core;
using Minever.Bedrock.Universal;
using Minever.Core;
using Minever.Java.Core;
using Minever.Java.Universal;

namespace Minever.Universal;

public static class MinecraftClientBuilderExtensions
{
    public static AbstractMinecraftClientBuilder AddChatController(this AbstractMinecraftClientBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder switch
        {
            JavaClientBuilder javaClientBuilder       => JavaClientBuilderExtensions.AddChatController(javaClientBuilder),
            BedrockClientBuilder bedrockClientBuilder => BedrockClientBuilderExtensions.AddChatController(bedrockClientBuilder),

           _ => throw new NotSupportedException()
        };
    }
}
