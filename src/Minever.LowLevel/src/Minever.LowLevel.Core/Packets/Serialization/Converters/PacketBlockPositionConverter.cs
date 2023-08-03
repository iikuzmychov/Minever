using Minever.LowLevel.Core.IO;
using Minever.LowLevel.Core.Types;

namespace Minever.LowLevel.Core.Packets.Serialization.Converters;

public class PacketBlockPositionConverter : PacketConverter<BlockPosition>
{
    public override BlockPosition Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var x = reader.ReadInt();
        var y = reader.ReadInt();
        var z = reader.ReadInt();

        return new(x, y, z);
    }

    public override void Write(MinecraftWriter writer, BlockPosition value)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
    }
}
