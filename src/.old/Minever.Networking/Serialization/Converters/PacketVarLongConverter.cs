using Minever.Networking.IO;

namespace Minever.Networking.Serialization;

public class PacketVarLongConverter : PacketConverter<long>
{
    public override long Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return reader.Read7BitEncodedInt64();
    }

    public override void Write(MinecraftWriter writer, long value)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write7BitEncodedInt64(value);
    }
}
