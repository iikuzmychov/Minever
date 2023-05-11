namespace Minever.LowLevel.Java.Core.Extensions;

public static class JavaProtocolExtensions
{
    public static void ThrowIfPacketIsNotSupported(this IJavaProtocol protocol, object packet, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(protocol);
        ArgumentNullException.ThrowIfNull(packet);

        if (!protocol.IsPacketSupported(packet, context))
        {
            throw new ArgumentException("The packet is not supported by the protocol in the specified context.");
        }
    }
}
