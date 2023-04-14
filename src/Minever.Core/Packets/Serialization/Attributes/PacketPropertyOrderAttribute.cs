namespace Minever.Core.Packets.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PacketPropertyOrderAttribute : Attribute
{
    public int Order { get; }

    public PacketPropertyOrderAttribute(int order)
    {
        Order = order;
    }
}
