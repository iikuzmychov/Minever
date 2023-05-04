using Minever.Core;
using Minever.Java.Core;

namespace Minever.Java.Universal;

public static class JavaClientBuilderExtensions
{
    public static JavaClientBuilder AddChatController(this JavaClientBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var chat = builder.ProtocolClient.Protocol switch
        {
            Protocols.V5.JavaProtocol5 => new Protocols.V5.ChatController(),

            _ => throw new NotSupportedException()
        };

        builder.AddController<IChatController>(chat);

        return builder;
    }
}
