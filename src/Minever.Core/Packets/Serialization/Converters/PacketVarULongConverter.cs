using Minever.Core.IO;

namespace Minever.Core.Packets.Serialization.Converters;

public class PacketVarULongConverter : PacketConverter<ulong>
{
    public override ulong Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return reader.ReadVarULong();
    }

    public override void Write(MinecraftWriter writer, ulong value)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteVarULong(value);
    }
}
