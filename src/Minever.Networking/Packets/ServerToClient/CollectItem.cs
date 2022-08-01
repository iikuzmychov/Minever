using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record CollectItem
{
    [PacketPropertyOrder(1)]
    public int CollectedEntityId { get; init; }

    [PacketPropertyOrder(2)]
    public int CollectorEntityId { get; init; }

    public CollectItem() { }

    public CollectItem(int collectedEntityId, int collectorEntityId)
    {
        CollectedEntityId = collectedEntityId;
        CollectorEntityId = collectorEntityId;
    }
}
