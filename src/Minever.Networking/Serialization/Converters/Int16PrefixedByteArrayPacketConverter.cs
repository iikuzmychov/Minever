using Minever.Networking.IO;

namespace Minever.Networking.Serialization.Converters;

public class Int16PrefixedByteArrayPacketConverter : PacketConverter<byte[]>
{
    public override byte[] Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var bytesLength = reader.ReadInt16();
        var bytes = reader.ReadBytes(bytesLength);

        return bytes;
    }

    public override void Write(byte[] value, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write((short)value.Length);
        writer.Write(value);
    }
}
