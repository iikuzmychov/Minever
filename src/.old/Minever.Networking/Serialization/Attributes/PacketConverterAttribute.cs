using Minever.Networking.Serialization;

namespace Minever.Networking.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public class PacketConverterAttribute : Attribute
{
    public Type ConverterType { get; }

    public PacketConverterAttribute(Type converterType)
    {
        ArgumentNullException.ThrowIfNull(converterType);

        if (!converterType.IsSubclassOf(typeof(PacketConverter)))
            throw new ArgumentException($"{converterType} must be inthered from {nameof(PacketConverter)}.", nameof(converterType));

        ConverterType = converterType;
    }
}
