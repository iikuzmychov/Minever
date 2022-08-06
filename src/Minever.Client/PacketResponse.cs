using Minever.Networking.Packets;

namespace Minever.Client;

public class ReceivedPacketInfo<TData>
    where TData : notnull
{
    public MinecraftPacket<TData> Packet { get; }
    public DateTime ReceivedDateTime { get; }
    public PacketContext Context { get; }

    public ReceivedPacketInfo(MinecraftPacket<TData> packet, DateTime receivedDateTime, PacketContext context)
    {
        Packet           = packet ?? throw new ArgumentNullException(nameof(packet));
        ReceivedDateTime = receivedDateTime;
        Context          = context;
    }
}
