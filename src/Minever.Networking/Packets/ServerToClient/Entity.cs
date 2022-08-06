using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record Entity
{
    [PacketPropertyOrder(1)]
    public int Id { get; init; }

    public Entity() { }

    public Entity(int entityId)
    {
        Id = entityId;
    }
}
