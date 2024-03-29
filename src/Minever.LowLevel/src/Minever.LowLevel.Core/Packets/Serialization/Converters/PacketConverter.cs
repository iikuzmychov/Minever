﻿using Minever.LowLevel.Core.IO;
using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using System.Reflection;

namespace Minever.LowLevel.Core.Packets.Serialization.Converters;

public abstract class PacketConverter
{
    public static PacketConverter GetTypeConverter(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

        var converterAttribute = typeToConvert.GetCustomAttribute(typeof(PacketConverterAttribute<>));

        if (converterAttribute is null)
        {
            return DefaultPacketConverter.Instance;
        }

        var converterType = converterAttribute.GetType().GetGenericArguments()[0];

        return (PacketConverter)Activator.CreateInstance(converterType)!;
    }

    public static PacketConverter GetPropertyConverter(PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(property);

        var converterAttribute = property.GetCustomAttribute(typeof(PacketConverterAttribute<>));

        if (converterAttribute is null)
        {
            return GetTypeConverter(property.PropertyType);
        }

        var converterType = converterAttribute.GetType().GetGenericArguments()[0];

        return (PacketConverter)Activator.CreateInstance(converterType)!;
    }

    public abstract bool CanConvert(Type type);

    public abstract object Read(MinecraftReader reader, Type targetType);

    public abstract void Write(MinecraftWriter writer, object value);
}

public abstract class PacketConverter<T> : PacketConverter
    where T : notnull
{
    public override bool CanConvert(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return type.IsAssignableFrom(typeof(T));
    }
    
    public sealed override object Read(MinecraftReader reader, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(targetType);

        if (!targetType.IsAssignableFrom(typeof(T)))
        {
            throw new ArgumentException($"{typeof(T)} is not assignable from {targetType}");
        }

        return Read(reader);
    }

    public sealed override void Write(MinecraftWriter writer, object value)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        Write(writer, (T)value);
    }

    public abstract T Read(MinecraftReader reader);

    public abstract void Write(MinecraftWriter writer, T value);
}
