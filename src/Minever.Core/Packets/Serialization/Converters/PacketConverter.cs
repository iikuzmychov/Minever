using Minever.Core.IO;
using Minever.Core.Packets.Serialization.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace Minever.Core.Packets.Serialization.Converters;

public abstract class PacketConverter
{
    public static PacketConverter GetConverter(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

        var typeConverterAttribute = typeToConvert.GetCustomAttributes().OfType<PacketConverterAttribute>().SingleOrDefault();

        return typeConverterAttribute is null
            ? DefaultPacketConverter.Instance
            : (PacketConverter)Activator.CreateInstance(typeConverterAttribute.ConverterType)!;
    }

    public static PacketConverter GetPropertyConverter(PropertyInfo property)
    {
        Debug.Assert(property is not null);

        var propertyConverterAttribute = property.GetCustomAttributes().OfType<PacketConverterAttribute>().SingleOrDefault();

        return propertyConverterAttribute is null
            ? GetConverter(property.PropertyType)
            : (PacketConverter)Activator.CreateInstance(propertyConverterAttribute.ConverterType)!;
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
        return typeof(T) == type;
    }

    public abstract T Read(MinecraftReader reader);

    public sealed override object Read(MinecraftReader reader, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(targetType);

        // todo:
        //if (targetType != typeof(T)) // is?
        //{
        //    throw new ArgumentException("Target type ");
        //}

        return Read(reader);
    }

    public abstract void Write(MinecraftWriter writer, T value);

    public sealed override void Write(MinecraftWriter writer, object value)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        Write(writer, (T)value);
    }
}
