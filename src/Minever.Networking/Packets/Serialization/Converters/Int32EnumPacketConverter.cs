using Minever.Networking.IO;

namespace Minever.Networking.Packets.Serialization.Converters;

public class Int32EnumPacketConverter<TEnum> : PacketConverter<TEnum>
    where TEnum : Enum
{
    public override TEnum Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        return (TEnum)(object)reader.ReadInt32();
    }

    public override void Write(TEnum value, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write((int)(object)value);
    }
}
