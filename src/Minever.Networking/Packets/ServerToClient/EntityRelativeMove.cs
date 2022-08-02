using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record EntityRelativeMove
{
    private double _deltaX;
    private double _deltaY;
    private double _deltaZ;

    [PacketPropertyOrder(1)]
    public int EntityId { get; init; }

    [PacketPropertyOrder(2)]
    [PacketConverter(typeof(FixedPointPacketConverter<sbyte>))]
    public double DeltaX
    {
        get => _deltaX;
        init => _deltaX = (value is >= -4 and <= 4) ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    [PacketPropertyOrder(3)]
    [PacketConverter(typeof(FixedPointPacketConverter<sbyte>))]
    public double DeltaY
    {
        get => _deltaY;
        init => _deltaY = (value is >= -4 and <= 4) ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    [PacketPropertyOrder(4)]
    [PacketConverter(typeof(FixedPointPacketConverter<sbyte>))]
    public double DeltaZ
    {
        get => _deltaZ;
        init => _deltaZ = (value is >= -4 and <= 4) ? value : throw new ArgumentOutOfRangeException(nameof(value));
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
