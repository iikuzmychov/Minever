using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record EntityEffect
{
    private TimeSpan _duration = TimeSpan.Zero;

    [PacketPropertyOrder(1)]
    public int EntityId { get; init; }

    [PacketPropertyOrder(2)]
    public sbyte EffectId { get; init; }

    [PacketPropertyOrder(3)]
    public sbyte Strength { get; init; }

    [PacketPropertyOrder(4)]
    [PacketConverter(typeof(PacketTimeSpanFromToMinecraftTicksConverter<short>))]
    public TimeSpan Duration
    {
        get => _duration;
        init
        {
            if (value < TimeSpan.Zero || value > TimeSpan.FromSeconds(short.MaxValue))
                throw new ArgumentOutOfRangeException(nameof(value));

            _duration = value;
        }
    }

    public EntityEffect() { }

    public EntityEffect(int entityid, sbyte effectId, sbyte strength, TimeSpan duration)
    {
        EntityId = entityid;
        EffectId = effectId;
        Strength = strength;
        Duration = duration;
    }
}
