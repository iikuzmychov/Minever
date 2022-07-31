﻿using Minever.Networking.IO;
using System;

namespace Minever.Networking.Serialization.Converters;

public class EnumPacketConverter<TEnum, TValue> : PacketConverter<TEnum>
    where TEnum : Enum
    where TValue : notnull
{
    private PacketConverter _valueConverter;

    public EnumPacketConverter(PacketConverter valueConverter)
    {
        _valueConverter = valueConverter ?? throw new ArgumentNullException(nameof(valueConverter));
    }

    public EnumPacketConverter() : this(PacketSerializer.GetTypeConverter(typeof(TValue))) { }

    public override TEnum Read(MinecraftReader reader)
    {
        var value = (TValue)_valueConverter.Read(reader, typeof(TValue));
        return (TEnum)Enum.ToObject(typeof(TEnum), value)!;
    }

    public override void Write(TEnum value, MinecraftWriter writer)
    {
        var convertedValue = (TValue)Convert.ChangeType(value, typeof(TValue));
        _valueConverter.Write(convertedValue, writer);
    }
}

public sealed class EnumPacketConverter<TEnum, TValue, TValueConverter> : EnumPacketConverter<TEnum, TValue>
    where TEnum : Enum
    where TValue : notnull
    where TValueConverter : PacketConverter, new()
{
    public EnumPacketConverter() : base(new TValueConverter()) { }
}