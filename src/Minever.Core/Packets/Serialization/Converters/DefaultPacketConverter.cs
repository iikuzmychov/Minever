using Minever.Core.IO;

namespace Minever.Core.Packets.Serialization.Converters;

public class DefaultPacketConverter : PacketConverter
{
    private static DefaultPacketConverter? _instance;

    public static DefaultPacketConverter Instance => _instance ??= new();

    public override bool CanConvert(Type type) => true;

    public override object Read(MinecraftReader reader, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(targetType);

        return Type.GetTypeCode(targetType) switch
        {
            TypeCode.Boolean => reader.ReadBool(),
            TypeCode.Byte    => reader.ReadByte(),
            TypeCode.Double  => reader.ReadDouble(),
            TypeCode.Int16   => reader.ReadShort(),
            TypeCode.Int32   => reader.ReadInt(),
            TypeCode.Int64   => reader.ReadLong(),
            TypeCode.SByte   => reader.ReadSByte(),
            TypeCode.Single  => reader.ReadFloat(),
            TypeCode.UInt16  => reader.ReadUShort(),
            TypeCode.UInt32  => reader.ReadUInt(),
            TypeCode.UInt64  => reader.ReadULong(),
            TypeCode.String  => reader.ReadString(),

            TypeCode.Object when (targetType == typeof(Guid)) => reader.ReadGuid(),

            _ => PacketSerializer.DeserializeProperties(reader, targetType),
        };
    }

    public override void Write(MinecraftWriter writer, object value)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        switch (value)
        {
            case bool @bool:     writer.Write(@bool);   break;
            case byte @byte:     writer.Write(@byte);   break;
            case double @double: writer.Write(@double); break;
            case short @short:   writer.Write(@short);  break;
            case int @int:       writer.Write(@int);    break;
            case long @long:     writer.Write(@long);   break;
            case sbyte @sbyte:   writer.Write(@sbyte);  break;
            case float @float:   writer.Write(@float);  break;
            case ushort @ushort: writer.Write(@ushort); break;
            case uint @uint:     writer.Write(@uint);   break;
            case ulong @ulong:   writer.Write(@ulong);  break;
            case string @string: writer.Write(@string); break;
            case Guid guid:      writer.Write(guid);    break;

            default: PacketSerializer.SerializeProperties(writer, value); break;
        };
    }
}
