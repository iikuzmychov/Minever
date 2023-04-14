using Minever.Networking.Packets;

namespace Minever.Networking.Exceptions;

public class PacketDeserializationException : Exception
{
    public override string Message =>
        $"The last {PacketLength - DeserializedPacketLength} bytes were not used in packet 0x{Packet.Id:X2} ({Context.ConnectionState} state, {Context.Direction}) deserialization.";
    public MinecraftPacket<object> Packet { get; }
    public PacketContext Context { get; }
    public Type PacketDataType { get; }
    public int PacketLength { get; }
    public int DeserializedPacketLength { get; }

    public PacketDeserializationException(MinecraftPacket<object> packet, PacketContext context,
        Type packetDataType, int packetLength, int deserializedPacketLength)
    {
        if (packetLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(packetLength));

        if (deserializedPacketLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(deserializedPacketLength));

        Packet                   = packet ?? throw new ArgumentNullException(nameof(packet));
        Context                  = context;
        PacketDataType           = packetDataType ?? throw new ArgumentNullException(nameof(packetDataType));
        PacketLength             = packetLength;
        DeserializedPacketLength = deserializedPacketLength;
    }
}
