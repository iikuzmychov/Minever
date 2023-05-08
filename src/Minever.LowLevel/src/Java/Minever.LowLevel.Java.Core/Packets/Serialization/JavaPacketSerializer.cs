using Minever.LowLevel.Core.IO;
using Minever.LowLevel.Core.Packets.Serialization;

namespace Minever.LowLevel.Java.Core.Packets.Serialization;

public static class JavaPacketSerializer
{
    public static byte[] Serialize(object packet, IJavaProtocol protocol, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(packet);
        ArgumentNullException.ThrowIfNull(protocol);

        using var memoryStream = new MemoryStream();
        Serialize(memoryStream, packet, protocol, context);

        return memoryStream.ToArray();
    }

    public static void Serialize(Stream stream, object packet, IJavaProtocol protocol, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(packet);
        ArgumentNullException.ThrowIfNull(protocol);

        using var writer = new MinecraftWriter(stream);
        Serialize(writer, packet, protocol, context);
    }

    public static void Serialize(MinecraftWriter writer, object packet, IJavaProtocol protocol, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(packet);
        ArgumentNullException.ThrowIfNull(protocol);

        // todo: protocol.IsPacketSupported ???

        var id           = protocol.GetPacketId(packet.GetType(), context);
        var bytes        = PacketSerializer.Serialize(packet);
        var idLength     = EncodingHelper.GetVarIntBytesLength(id);
        var packetLength = idLength + bytes.Length;

        writer.WriteVarInt(packetLength);
        writer.WriteVarInt(id);
        writer.Write(bytes);
    }

    public static object Deserialize(MinecraftReader reader, IJavaProtocol protocol, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(protocol);

        var packetId   = reader.ReadVarInt();
        var packetType = protocol.GetPacketType(packetId, context);
        var packet     = PacketSerializer.Deserialize(reader, packetType);

        return packet;
    }

    public static object Deserialize(Stream stream, IJavaProtocol protocol, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(protocol);

        using var reader = new MinecraftReader(stream);
        
        return Deserialize(reader, protocol, context);
    }

    public static object Deserialize(byte[] packetBytes, IJavaProtocol protocol, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(packetBytes);
        ArgumentNullException.ThrowIfNull(protocol);

        using var memoryStream = new MemoryStream(packetBytes);

        return Deserialize(memoryStream, protocol, context);
    }
}
