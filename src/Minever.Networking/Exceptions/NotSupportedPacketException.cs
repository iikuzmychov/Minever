using Minever.Networking.Packets;

namespace Minever.Networking.Exceptions;

public sealed class NotSupportedPacketException : Exception
{
    public override string Message =>
        $"Packet 0x{Packet.Id:X2} ({Packet.Kind.ConnectionState} state, {Packet.Kind.Direction}) is not supported by the used protocol.";

    public MinecraftPacket<byte[]> Packet { get; }

    public NotSupportedPacketException(MinecraftPacket<byte[]> packet)
    {
        Packet = packet ?? throw new ArgumentNullException(nameof(packet));
    }
}
