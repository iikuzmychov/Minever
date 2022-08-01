using Minever.Networking.IO;

namespace Minever.Networking.Serialization.Converters;

/// <summary>
/// <see href="https://wiki.vg/Data_types#Fixed-point_numbers"/>
/// </summary>
public class FixedPointPacketConverter : PacketConverter
{
    public override bool CanConvert(Type type) => true;

    public override object Read(MinecraftReader reader, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(targetType);

        var fixedPointValue = reader.ReadDouble();
        var value           = Convert.ChangeType(fixedPointValue * 32d, targetType);

        return value;
    }

    public override void Write(object value, MinecraftWriter writer)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(writer);

        var fixedPointValue = Convert.ToDouble(value) / 32d;

        writer.Write(fixedPointValue);
    }
}
