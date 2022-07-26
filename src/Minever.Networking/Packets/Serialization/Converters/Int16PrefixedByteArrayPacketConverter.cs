using Minever.Networking.IO;

namespace Minever.Networking.Packets.Serialization.Converters;

public class Int16PrefixedByteArrayPacketConverter : MinecraftPacketConverter<byte[]>
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
