using Minever.Networking.IO;

namespace Minever.Networking.Serialization;

public class PacketVarIntConverter : PacketConverter<int>
{
    public override int Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return reader.Read7BitEncodedInt();
    }

    public override void Write(MinecraftWriter writer, int value)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write7BitEncodedInt(value);
    }
}
