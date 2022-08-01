using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record Entity
{
    [PacketPropertyOrder(1)]
    public int EntityId { get; init; }

    public Entity() { }

    public Entity(int entityId)
    {
        EntityId = entityId;
    }
}
