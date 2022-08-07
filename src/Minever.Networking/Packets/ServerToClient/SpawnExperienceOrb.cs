using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record SpawnExperienceOrb
{
    private double _x;
    private double _y;
    private double _z;

    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(PacketVarIntConverter))]
    public int EntityId { get; init; }

    [PacketPropertyOrder(2)]
    [PacketConverter(typeof(PacketFixedPointConverter<int>))]
    public double X
    {
        get => _x;
        init
        {
            if (value < int.MinValue / 32 || value > int.MaxValue / 32d)
                throw new ArgumentOutOfRangeException(nameof(value));

            _x = value;
        }
    }

    [PacketPropertyOrder(3)]
    [PacketConverter(typeof(PacketFixedPointConverter<int>))]
    public double Y
    {
        get => _y;
        init
        {
            if (value < int.MinValue / 32 || value > int.MaxValue / 32d)
                throw new ArgumentOutOfRangeException(nameof(value));

            _y = value;
        }
    }

    [PacketPropertyOrder(4)]
    [PacketConverter(typeof(PacketFixedPointConverter<int>))]
    public double Z
    {
        get => _z;
        init
        {
            if (value < int.MinValue / 32 || value > int.MaxValue / 32d)
                throw new ArgumentOutOfRangeException(nameof(value));

            _z = value;
        }
    }

    [PacketPropertyOrder(5)]
    public short ExperienceAmount { get; init; }

    public SpawnExperienceOrb() { }

    public SpawnExperienceOrb(int entityId, double x, double y, double z, short experienceAmount)
    {
        EntityId         = entityId;
        X                = x;
        Y                = y;
        Z                = z;
        ExperienceAmount = experienceAmount;
    }
}
