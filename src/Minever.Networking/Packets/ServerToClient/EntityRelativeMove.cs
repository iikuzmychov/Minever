using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record EntityRelativeMove
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

    public EntityRelativeMove() { }

    public EntityRelativeMove(int entityId, double deltaX, double deltaY, double deltaZ)
    {
        EntityId = entityId;
        DeltaX   = deltaX;
        DeltaY   = deltaY;
        DeltaZ   = deltaZ;
    }
}
