using Minever.Networking.IO;
using System;

namespace Minever.Networking.Serialization.Converters;

public class PacketEnumConverter<TEnum, TValue> : PacketConverter<TEnum>
    where TEnum : Enum
    where TValue : notnull
{
    private PacketConverter _valueConverter;

    public PacketEnumConverter(PacketConverter valueConverter)
    {
        _valueConverter = valueConverter ?? throw new ArgumentNullException(nameof(valueConverter));
    }

    public PacketEnumConverter() : this(PacketSerializer.GetTypeConverter(typeof(TValue))) { }

    public override TEnum Read(MinecraftReader reader)
    {
        var value = (TValue)_valueConverter.Read(reader, typeof(TValue));
        return (TEnum)Enum.ToObject(typeof(TEnum), value)!;
    }

    public override void Write(MinecraftWriter writer, TEnum value)
    {
        var convertedValue = (TValue)Convert.ChangeType(value, typeof(TValue));
        _valueConverter.Write(writer, convertedValue);
    }
}

public sealed class EnumPacketConverter<TEnum, TValue, TValueConverter> : PacketEnumConverter<TEnum, TValue>
    where TEnum : Enum
    where TValue : notnull
    where TValueConverter : PacketConverter, new()
{
    public EnumPacketConverter() : base(new TValueConverter()) { }
}
