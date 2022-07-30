using Minever.Networking.IO;

namespace Minever.Networking.Serialization.Converters;

public class SByteEnumPacketConverter<TEnum> : PacketConverter<TEnum>
    where TEnum : Enum
{
    public override TEnum Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return (TEnum)(object)(int)reader.ReadSByte();
    }

    public override void Write(TEnum value, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write((sbyte)(int)(object)value);
    }
}
