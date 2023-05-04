using Minever.LowLevel.Core.IO;
using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Packets.Serialization.Converters;
using System.Diagnostics;
using System.Reflection;

namespace Minever.LowLevel.Core.Packets.Serialization;

// todo: .GetCustomAttributes(inherit: true) ???
// todo: cache types
public static class PacketSerializer
{
    public static byte[] Serialize(object value)
    {
        using var memoryStream = new MemoryStream();
        Serialize(memoryStream, value);

        return memoryStream.ToArray();
    }

    public static void Serialize(Stream stream, object value)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(value);

        using var writer = new MinecraftWriter(stream);        
        Serialize(writer, value);
    }

    public static void Serialize(MinecraftWriter writer, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(writer);

        var converter = PacketConverter.GetTypeConverter(value.GetType());
        converter.Write(writer, value);
    }

    public static void SerializeProperties(MinecraftWriter writer, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(writer);

        var packetProperties = GetSerializableProperties(value.GetType());

        foreach (var property in packetProperties)
        {
            var converter = PacketConverter.GetPropertyConverter(property);

            if (!converter.CanConvert(property.PropertyType))
            {
                throw new NotSupportedException($"Converter {converter} does not support type {property.PropertyType}.");
            }

            var propertyValue = property.GetValue(value)!;
            converter.Write(writer, propertyValue);
        }
    }

    public static object Deserialize(byte[] bytes, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        ArgumentNullException.ThrowIfNull(targetType);

        using var memoryStream = new MemoryStream();

        return Deserialize(memoryStream, targetType);
    }

    public static object Deserialize(Stream stream, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(targetType);

        using var reader = new MinecraftReader(stream);

        return Deserialize(reader, targetType);
    }

    public static object Deserialize(MinecraftReader reader, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(targetType);

        var converter = PacketConverter.GetTypeConverter(targetType);

        return converter.Read(reader, targetType);
    }

    public static object DeserializeProperties(MinecraftReader reader, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(targetType);

        object? packet;

        try
        {
            packet = Activator.CreateInstance(targetType);
        }
        catch (Exception exception)
        { 
            throw new AggregateException($"Failed to create instance of type {targetType}.", exception);
        }

        if (packet is null)
        {
            throw new AggregateException($"Failed to create instance of type {targetType}.");
        }

        var packetProperties = GetSerializableProperties(targetType);

        foreach (var property in packetProperties)
        {
            var converter = PacketConverter.GetPropertyConverter(property);

            if (!converter.CanConvert(property.PropertyType))
            {
                throw new NotSupportedException($"The converter does not support type {property.PropertyType}.");
            }

            var propertyValue = converter.Read(reader, property.PropertyType);
            property.SetValue(packet, propertyValue);
        }

        return packet;
    }

    private static IOrderedEnumerable<PropertyInfo> GetSerializableProperties(Type type)
    {
        Debug.Assert(type is not null);

        return type.GetProperties()
            .Where(property => property.GetCustomAttribute<PacketIgnoreAttribute>() is null)
            .OrderBy(property => property.GetCustomAttribute<PacketPropertyOrderAttribute>()?.Order ?? int.MaxValue);
    }
}
