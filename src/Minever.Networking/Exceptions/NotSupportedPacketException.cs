using Minever.Networking.Packets;

namespace Minever.Networking.Exceptions;

public sealed class NotSupportedPacketException : Exception
{
    public override string Message =>
        $"Packet 0x{Packet.Id:X2} ({Context.ConnectionState} state, {Context.Direction}) is not supported by the used protocol.";
    public MinecraftPacket<byte[]> Packet { get; }
    public PacketContext Context { get; }

    public NotSupportedPacketException(MinecraftPacket<byte[]> packet, PacketContext context)
    {
        Packet  = packet ?? throw new ArgumentNullException(nameof(packet));
        Context = context;
    }
}
