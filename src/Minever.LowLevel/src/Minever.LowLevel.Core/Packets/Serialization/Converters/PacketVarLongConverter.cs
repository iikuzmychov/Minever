using Minever.LowLevel.Core.IO;

namespace Minever.LowLevel.Core.Packets.Serialization.Converters;

public class PacketVarLongConverter : PacketConverter<long>
{
    public override long Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return reader.ReadVarLong();
    }

    public override void Write(MinecraftWriter writer, long value)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteVarLong(value);
    }
}
