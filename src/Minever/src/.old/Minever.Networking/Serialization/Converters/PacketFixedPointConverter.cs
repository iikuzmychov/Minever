﻿using Minever.Networking.IO;

namespace Minever.Networking.Serialization;

/// <summary>
/// <see href="https://wiki.vg/Data_types#Fixed-point_numbers"/>
/// </summary>
public class PacketFixedPointConverter<T> : PacketConverter<double>
    where T : notnull
{
    private readonly PacketConverter _typeConverter = PacketSerializer.GetConverter(typeof(T));

    public override double Read(MinecraftReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var encodedValue = (T)_typeConverter.Read(reader, typeof(T));
        var value        = Convert.ToDouble(encodedValue) / 32d;

        return value;
    }

    public override void Write(MinecraftWriter writer, double value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(writer);

        var encodedValue = (T)Convert.ChangeType(value * 32d, typeof(T));

        _typeConverter.Write(writer, encodedValue);
    }
}
