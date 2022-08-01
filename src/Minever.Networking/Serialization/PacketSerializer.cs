using Minever.Networking.Exceptions;
using Minever.Networking.IO;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;
using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;
using System.Reflection;

namespace Minever.Networking.Serialization;

public static class PacketSerializer
{
    public static PacketConverter GetTypeConverter(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var typeConverterAttribute = type.GetCustomAttribute<PacketConverterAttribute>();

        if (typeConverterAttribute is not null)
            return (PacketConverter)Activator.CreateInstance(typeConverterAttribute.ConverterType)!;
        else
            return DefaultPacketConverter.Shared;
    }

    private static PacketConverter GetPropertyConverter(PropertyInfo property)
    {
        var propertyConverterAttribute = property.GetCustomAttribute<PacketConverterAttribute>();

        if (propertyConverterAttribute is not null)
            return (PacketConverter)Activator.CreateInstance(propertyConverterAttribute.ConverterType)!;
        else
            return GetTypeConverter(property.PropertyType);
    }

    private static IOrderedEnumerable<PropertyInfo> GetSerializableProperties(Type packetDataType) =>
        packetDataType
            .GetProperties()
            .Where(property => property.GetCustomAttribute<PacketIgnoreAttribute>() is null)
            .OrderBy(property => property.GetCustomAttribute<PacketPropertyOrderAttribute>()?.Order ?? int.MaxValue);

    public static void SerializeData(object packetData, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(packetData);
        ArgumentNullException.ThrowIfNull(writer);

        var packetDataType          = packetData.GetType();
        var packetDataConverterType = packetDataType.GetCustomAttribute<PacketConverterAttribute>()?.ConverterType;

        if (packetDataConverterType is not null)
        {
            var converter = (PacketConverter)Activator.CreateInstance(packetDataConverterType)!;
            converter.Write(packetData, writer);
        }
        else
        {
            var packetDataProperties = GetSerializableProperties(packetDataType);

            foreach (var property in packetDataProperties)
            {
                var converter     = GetPropertyConverter(property);
                var propertyValue = property.GetValue(packetData)!;

                converter.Write(propertyValue, writer);
            }
        }
    }

    public static byte[] Serialize<TData>(MinecraftPacket<TData> packet)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(packet);

        using var memoryStream = new MemoryStream();
        using var writer       = new MinecraftWriter(memoryStream);

        writer.Write7BitEncodedInt(packet.Id);
        SerializeData(packet.Data, writer);
        
        return memoryStream.ToArray();
    }

    public static object DeserializeData(MinecraftReader reader, Type packetDataType)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(packetDataType);

        var packetDataConverterType = packetDataType.GetCustomAttribute<PacketConverterAttribute>()?.ConverterType;
        object packetData;

        if (packetDataConverterType is not null)
        {
            var converter = (PacketConverter)Activator.CreateInstance(packetDataConverterType)!;
            packetData = converter.Read(reader, packetDataType);
        }
        else
        { 
            var packetDataProperties = GetSerializableProperties(packetDataType);
            packetData               = Activator.CreateInstance(packetDataType)!;

            foreach (var property in packetDataProperties)
            {
                var converter     = GetPropertyConverter(property);
                var propertyValue = converter.Read(reader, property.PropertyType);

                property.SetValue(packetData, propertyValue);
            }
        }

        return packetData;
    }

    public static MinecraftPacket<object> Deserialize(byte[] packetBytes, PacketContext context, MinecraftProtocol protocol)
    {
        ArgumentNullException.ThrowIfNull(packetBytes);
        ArgumentNullException.ThrowIfNull(protocol);

        using var memoryStream = new MemoryStream(packetBytes);
        using var reader       = new MinecraftReader(memoryStream);
        var packetId           = reader.Read7BitEncodedInt();
        var packetIdLength     = (int)memoryStream.Position;
        var packetDataLength   = packetBytes.Length - packetIdLength;

        if (!protocol.IsPacketSupported(packetId, context))
        {
            var packetDataBytes = reader.ReadBytes(packetDataLength);
            throw new NotSupportedPacketException(new(packetId, packetDataBytes), context);
        }

        var packetDataType = protocol.GetPacketDataType(packetId, context);
        var packetData     = DeserializeData(reader, packetDataType);
        var packet         = new MinecraftPacket<object>(packetId, packetData);

        if (memoryStream.Position != packetBytes.Length)
            throw new PacketDeserializationException(packet, context, packetDataType, packetBytes.Length, (int)memoryStream.Position);

        return packet;
    }
}
