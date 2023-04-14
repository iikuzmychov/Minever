using Minever.Core.IO;

namespace Minever.Core.Packets.Serialization.Converters;

public class PacketVarIntConverter : PacketConverter<int>
{
    public override int Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return reader.ReadVarInt();
    }

    public override void Write(MinecraftWriter writer, int value)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteVarInt(value);
    }
}
