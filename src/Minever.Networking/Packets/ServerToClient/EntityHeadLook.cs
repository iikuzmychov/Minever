using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record EntityHeadLook
{
    [PacketPropertyOrder(1)]
    public int EntityId { get; init; }
    
    [PacketPropertyOrder(2)]
    public sbyte HeadYaw { get; init; }

    public EntityHeadLook() { }

    public EntityHeadLook(int entityId, sbyte headYaw)
    {
        EntityId = entityId;
        HeadYaw  = headYaw;
    }
}
