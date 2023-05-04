﻿// when many packages that the protocol should support are not yet implemented and added to it,
// many exceptions of the type NotSupportedPacketException are thrown that greatly slow down debugging
#define DONT_THROW_NOT_SUPPORTED_PACKET_EXCEPTION

using Minever.Networking.Exceptions;
using Minever.Networking.IO;
using Minever.Networking.Packets;
using Minever.Networking.Protocols;
using System.Reflection;

namespace Minever.Networking.Serialization;

public static class PacketSerializer
{
    public static PacketConverter GetConverter(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

        var typeConverterAttribute = typeToConvert.GetCustomAttribute<PacketConverterAttribute>();

        if (typeConverterAttribute is not null)
            return (PacketConverter)Activator.CreateInstance(typeConverterAttribute.ConverterType)!;
        else
            return PacketDefaultConverter.Shared;
    }

    private static PacketConverter GetPropertyConverter(PropertyInfo property)
    {
        var propertyConverterAttribute = property.GetCustomAttribute<PacketConverterAttribute>();

        if (propertyConverterAttribute is not null)
            return (PacketConverter)Activator.CreateInstance(propertyConverterAttribute.ConverterType)!;
        else
            return GetConverter(property.PropertyType);
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
            converter.Write(writer, packetData);
        }
        else
        {
            var packetDataProperties = GetSerializableProperties(packetDataType);

            foreach (var property in packetDataProperties)
            {
                var converter = GetPropertyConverter(property);

                if (!converter.CanConvert(property.PropertyType))
                    throw new NotSupportedException($"The converter does not support {property.PropertyType} type.");

                var propertyValue = property.GetValue(packetData)!;

                converter.Write(writer, propertyValue);
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

    public static void Serialize<TData>(MinecraftPacket<TData> packet, MinecraftWriter writer)
         where TData : notnull
    {
        ArgumentNullException.ThrowIfNull(packet);
        ArgumentNullException.ThrowIfNull(writer);

        var packetBytes = Serialize(packet);

        writer.Write7BitEncodedInt(packetBytes.Length);
        writer.Write(packetBytes);
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
            packetData    = converter.Read(reader, packetDataType);
        }
        else
        { 
            var packetDataProperties = GetSerializableProperties(packetDataType);
            packetData               = Activator.CreateInstance(packetDataType)!;

            foreach (var property in packetDataProperties)
            {
                var converter = GetPropertyConverter(property);

                if (!converter.CanConvert(property.PropertyType))
                    throw new NotSupportedException($"The converter does not support {property.PropertyType} type.");

                var propertyValue = converter.Read(reader, property.PropertyType);

                property.SetValue(packetData, propertyValue);
            }
        }

        return packetData;
    }

    public static MinecraftPacket<object> Deserialize(byte[] packetBytes, PacketContext context, JavaProtocol protocol)
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

#if DEBUG && DONT_THROW_NOT_SUPPORTED_PACKET_EXCEPTION
            return null!;
#else
            throw new NotSupportedPacketException(new(packetId, packetDataBytes), context);
#endif
        }

        var packetDataType = protocol.GetPacketDataType(packetId, context);
        var packetData     = DeserializeData(reader, packetDataType);
        var packet         = new MinecraftPacket<object>(packetId, packetData);

        if (memoryStream.Position != packetBytes.Length)
            throw new PacketDeserializationException(packet, context, packetDataType, packetBytes.Length, (int)memoryStream.Position);

        return packet;
    }

    public static MinecraftPacket<object> Deserialize(MinecraftReader reader,
        int packetLength, PacketContext context, JavaProtocol protocol)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(protocol);

        if (packetLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(packetLength));

        var packetBytes = reader.ReadBytes(packetLength);

        return Deserialize(packetBytes, context, protocol);
    }
}
