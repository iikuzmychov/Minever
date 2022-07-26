﻿using Minever.Networking.IO;

namespace Minever.Networking.Packets.Serialization.Converters;

public class SByteEnumPacketConverter<TEnum> : MinecraftPacketConverter<TEnum>
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
