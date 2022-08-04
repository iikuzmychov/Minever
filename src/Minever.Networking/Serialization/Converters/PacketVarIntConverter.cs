using Minever.Networking.IO;

namespace Minever.Networking.Serialization.Converters;

public class PacketVarIntConverter : PacketConverter<int>
{
    public override int Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return reader.Read7BitEncodedInt();
    }

    public override void Write(int value, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write7BitEncodedInt(value);
    }
}
