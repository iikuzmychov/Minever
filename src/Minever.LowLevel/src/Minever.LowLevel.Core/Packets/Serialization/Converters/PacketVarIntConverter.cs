using Minever.LowLevel.Core.IO;

namespace Minever.LowLevel.Core.Packets.Serialization.Converters;

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
