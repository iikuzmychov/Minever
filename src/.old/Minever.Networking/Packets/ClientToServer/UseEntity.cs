using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

/// <summary>
/// Maybe "EntityInteractionType" would be a better name?
/// </summary>
[PacketConverter(typeof(PacketEnumConverter<UseEntityAction, byte>))]
public enum UseEntityAction : byte
{ 
    LeftClick  = 0,
    RigthClick = 1,
}

/// <summary>
/// Maybe "EntityInteraction" would be a better name?
/// </summary>
public sealed record UseEntity
{
    [PacketPropertyOrder(1)]
    public int TargetEntityId { get; init; }

    [PacketPropertyOrder(2)]
    public UseEntityAction Action { get; init; }

    public UseEntity() { }

    public UseEntity(int targetEntityId, UseEntityAction action)
    {
        TargetEntityId = targetEntityId;
        Action         = action;
    }
}
