namespace Minever.Networking.Serialization;

[AttributeUsage(AttributeTargets.Property)]
public class PacketPropertyOrderAttribute : Attribute
{
    public int Order { get; }

    public PacketPropertyOrderAttribute(int order)
    {
        Order = order;
    }
}
