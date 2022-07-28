using Minever.Networking.Exceptions;
using Minever.Networking.IO;
using Minever.Networking.Packets.Serialization.Attributes;
using Minever.Networking.Packets.Serialization.Converters;
using Minever.Networking.Protocols;
using System.Reflection;

namespace Minever.Networking.Packets.Serialization;

public static class PacketSerializer
{
    private static PacketConverter GetPropertyConverter(PropertyInfo property)
    {
        var propertyConverterAttribute = property.GetCustomAttribute<PacketConverterAttribute>();

        if (propertyConverterAttribute is not null)
            return (PacketConverter)Activator.CreateInstance(propertyConverterAttribute.ConverterType)!;

        var propertyTypeConverterAttribute = property.PropertyType.GetCustomAttribute<PacketConverterAttribute>();

        if (propertyTypeConverterAttribute is not null)
            return (PacketConverter)Activator.CreateInstance(propertyTypeConverterAttribute.ConverterType)!;

        return DefaultPacketConverter.Shared;
    }

    private static IOrderedEnumerable<PropertyInfo> GetSerializableProperties(Type packetDataType) =>
        packetDataType
            .GetProperties()
            .Where(property => property.GetCustomAttribute<PacketIgnoreAttribute>() is null)
            .OrderBy(property => property.GetCustomAttribute<PacketPropertyOrderAttribute>()?.Order ?? int.MaxValue);

    public static byte[] Serialize<TData>(MinecraftPacket<TData> packet)
        where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(packet);

        using var memoryStream = new MemoryStream();
        using var writer       = new MinecraftWriter(memoryStream);

        writer.Write7BitEncodedInt(packet.Id);

        var packetIdLength          = memoryStream.Length;
        var packetDataType          = packet.Data.GetType();
        var packetDataConverterType = packetDataType.GetCustomAttribute<PacketConverterAttribute>()?.ConverterType;

        if (packetDataConverterType is not null)
        {
            var converter = (PacketConverter)Activator.CreateInstance(packetDataConverterType)!;
            converter.Write(packet.Data, writer);
        }
        else
        {
            var packetDataProperties = GetSerializableProperties(packetDataType);

            foreach (var property in packetDataProperties)
            {
                var converter     = GetPropertyConverter(property);
                var propertyValue = property.GetValue(packet.Data)!;

                converter.Write(propertyValue, writer);
            }
        }

        var dataLength  = memoryStream.Length - packetIdLength;
        var packetBytes = memoryStream.ToArray();

        return packetBytes;
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

        var packetDataType          = protocol.GetPacketDataType(packetId, context);
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

        var packet = new MinecraftPacket<object>(packetId, packetData);

        if (memoryStream.Position != memoryStream.Length)
            throw new PacketDeserializationException(packet, context, packetDataType, packetBytes.Length, (int)memoryStream.Position);

        return packet;
    }
}
