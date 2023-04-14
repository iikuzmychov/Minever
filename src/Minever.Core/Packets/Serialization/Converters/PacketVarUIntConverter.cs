using Minever.Core.IO;

namespace Minever.Core.Packets.Serialization.Converters;

public class PacketVarUIntConverter : PacketConverter<uint>
{
    public override uint Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return reader.ReadVarUInt();
    }

    public override void Write(MinecraftWriter writer, uint value)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteVarUInt(value);
    }
}
