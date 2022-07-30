using Minever.Networking.IO;

namespace Minever.Networking.Serialization.Converters;

public class VarIntEnumPacketConverter<TEnum> : PacketConverter<TEnum>
    where TEnum : Enum
{
    public override TEnum Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return (TEnum)(object)reader.Read7BitEncodedInt();
    }

    public override void Write(TEnum value, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write7BitEncodedInt((int)(object)value);
    }
}
