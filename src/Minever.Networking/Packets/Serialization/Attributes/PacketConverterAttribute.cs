using Minever.Networking.Packets.Serialization.Converters;

namespace Minever.Networking.Packets.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public class PacketConverterAttribute : Attribute
{
    public Type ConverterType { get; }

    public PacketConverterAttribute(Type converterType)
    {
        ArgumentNullException.ThrowIfNull(converterType);

        if (!converterType.IsSubclassOf(typeof(MinecraftPacketConverter)))
            throw new ArgumentException($"{converterType} must be inthered from {nameof(MinecraftPacketConverter)}.", nameof(converterType));

        ConverterType = converterType;
    }
}
