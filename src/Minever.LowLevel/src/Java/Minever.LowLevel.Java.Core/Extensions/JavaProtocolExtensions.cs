namespace Minever.LowLevel.Java.Core.Extensions;

public static class JavaProtocolExtensions
{
    public static void EnsureSupportedPacket(this IJavaProtocol protocol, object packet, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(protocol);
        ArgumentNullException.ThrowIfNull(packet);

        if (!protocol.IsPacketSupported(packet, context))
        {
            // todo: use different exception class
            throw new ArgumentException("The packet is not supported by the protocol in the specified context.");
        }
    }
}
