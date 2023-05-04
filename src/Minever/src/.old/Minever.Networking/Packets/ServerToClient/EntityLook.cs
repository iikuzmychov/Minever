using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record EntityLook
{
    [PacketPropertyOrder(1)]
    public int EntityId { get; init; }

    [PacketPropertyOrder(2)]
    public sbyte Yaw { get; init; }
    
    [PacketPropertyOrder(3)]
    public sbyte Pitch { get; init; }

    public EntityLook() { }

    public EntityLook(int entityid, sbyte yaw, sbyte pitch)
    {
        EntityId = entityid;
        Yaw      = yaw;
        Pitch    = pitch;
    }
}
