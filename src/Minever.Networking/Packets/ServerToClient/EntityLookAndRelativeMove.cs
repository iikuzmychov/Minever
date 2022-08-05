using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record EntityLookAndRelativeMove
{
    private double _deltaX;
    private double _deltaY;
    private double _deltaZ;

    [PacketPropertyOrder(1)]
    public int EntityId { get; init; }

    [PacketPropertyOrder(2)]
    [PacketConverter(typeof(PacketFixedPointConverter<sbyte>))]
    public double DeltaX
    {
        get => _deltaX;
        init
        {
            if (value < sbyte.MinValue / 32 || value > sbyte.MaxValue / 32d)
                throw new ArgumentOutOfRangeException(nameof(value));

            _deltaX = value;
        }
    }

    [PacketPropertyOrder(3)]
    [PacketConverter(typeof(PacketFixedPointConverter<sbyte>))]
    public double DeltaY
    {
        get => _deltaY;
        init
        {
            if (value < sbyte.MinValue / 32 || value > sbyte.MaxValue / 32d)
                throw new ArgumentOutOfRangeException(nameof(value));

            _deltaY = value;
        }
    }

    [PacketPropertyOrder(4)]
    [PacketConverter(typeof(PacketFixedPointConverter<sbyte>))]
    public double DeltaZ
    {
        get => _deltaZ;
        init
        {
            if (value < sbyte.MinValue / 32 || value > sbyte.MaxValue / 32d)
                throw new ArgumentOutOfRangeException(nameof(value));

            _deltaZ = value;
        }
    }

    [PacketPropertyOrder(5)]
    public sbyte Yaw { get; init; }

    [PacketPropertyOrder(6)]
    public sbyte Pitch { get; init; }

    public EntityLookAndRelativeMove() { }

    public EntityLookAndRelativeMove(int entityId, double deltaX, double deltaY, double deltaZ, sbyte yaw, sbyte pitch)
    {
        EntityId = entityId;
        DeltaX   = deltaX;
        DeltaY   = deltaY;
        DeltaZ   = deltaZ;
        Yaw      = yaw;
        Pitch    = pitch;
    }

    public EntityLookAndRelativeMove(EntityRelativeMove relativeMove, sbyte yaw, sbyte pitch)
        : this(relativeMove.EntityId, relativeMove.DeltaX, relativeMove.DeltaY, relativeMove.DeltaZ, yaw, pitch) { }

    public EntityLookAndRelativeMove(EntityLook look, double deltaX, double deltaY, double deltaZ)
        : this(look.EntityId,deltaX, deltaY, deltaZ, look.Yaw, look.Pitch) { }
}
