using Minever.Networking.DataTypes;
using Minever.Networking.IO;

namespace Minever.Networking.Serialization.Converters;

public class DefaultPacketConverter : PacketConverter
{
    public static DefaultPacketConverter Shared { get; } = new();

    public override bool CanConvert(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return true;
    }

    public override object Read(MinecraftReader reader, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(targetType);

        if (!CanConvert(targetType))
            throw new NotSupportedException($"Converter does not support {targetType} type.");

        if (targetType == typeof(bool))
            return reader.ReadBoolean();
        else if (targetType == typeof(byte))
            return reader.ReadByte();
        else if (targetType == typeof(decimal))
            return reader.ReadDecimal();
        else if (targetType == typeof(double))
            return reader.ReadDouble();
        else if (targetType == typeof(Half))
            return reader.ReadHalf();
        else if (targetType == typeof(short))
            return reader.ReadInt16();
        else if (targetType == typeof(int))
            return reader.ReadInt32();
        else if (targetType == typeof(long))
            return reader.ReadInt64();
        else if (targetType == typeof(sbyte))
            return reader.ReadSByte();
        else if (targetType == typeof(float))
            return reader.ReadSingle();
        else if (targetType == typeof(string))
            return reader.ReadString();
        else if (targetType == typeof(ushort))
            return reader.ReadUInt16();
        else if (targetType == typeof(uint))
            return reader.ReadUInt32();
        else if (targetType == typeof(ulong))
            return reader.ReadUInt64();
        else if (targetType == typeof(Guid))
            return reader.ReadUuid();
        else if (targetType == typeof(BlockPosition))
            return reader.ReadBlockPosition();
        else if (targetType == typeof(Position))
            return reader.ReadPosition();
        else
            return PacketSerializer.DeserializeData(reader, targetType);
    }

    public override void Write(object value, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(writer);

        switch (value)
        {
            case bool boolValue:                writer.Write(boolValue); break;
            case byte byteValue:                writer.Write(byteValue); break;
            case decimal decimalValue:          writer.Write(decimalValue); break;
            case double doubleValue:            writer.Write(doubleValue); break;
            case Half halfValue:                writer.Write(halfValue); break;
            case short shortValue:              writer.Write(shortValue); break;
            case int intValue:                  writer.Write(intValue); break;
            case long longValue:                writer.Write(longValue); break;
            case sbyte sbyteValue:              writer.Write(sbyteValue); break;
            case float floatValue:              writer.Write(floatValue); break;
            case string stringValue:            writer.Write(stringValue); break;
            case ushort ushortValue:            writer.Write(ushortValue); break;
            case uint uintValue:                writer.Write(uintValue); break;
            case ulong ulongValue:              writer.Write(ulongValue); break;
            case Guid uuidValue:                writer.Write(uuidValue); break;
            case BlockPosition blockPosition:   writer.Write(blockPosition); break;
            case Position position:             writer.Write(position); break;

            default: PacketSerializer.SerializeData(value, writer); break;
        };
    }
}
