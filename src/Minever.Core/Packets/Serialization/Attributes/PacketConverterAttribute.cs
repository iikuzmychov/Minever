using Minever.Core.Packets.Serialization.Converters;

namespace Minever.Core.Packets.Serialization.Attributes;

public abstract class PacketConverterAttribute : Attribute
{
    public abstract Type ConverterType { get; }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public class PacketConverterAttribute<TConverter> : PacketConverterAttribute
    where TConverter : PacketConverter
{
    public override Type ConverterType { get; } = typeof(TConverter);
}
