using Minever.LowLevel.Core.Packets.Serialization.Converters;

namespace Minever.LowLevel.Core.Packets.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public sealed class PacketConverterAttribute<TConverter> : Attribute
    where TConverter : PacketConverter
{
}
