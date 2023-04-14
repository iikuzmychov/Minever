using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record RemoveEntityEffect
{
    [PacketPropertyOrder(1)]
    public int EntityId { get; init; }

    [PacketPropertyOrder(2)]
    public sbyte EffectId { get; init; }

    public RemoveEntityEffect() { }

    public RemoveEntityEffect(int entityId, sbyte effectId)
    {
        EntityId = entityId;
        EffectId = effectId;
    }
}