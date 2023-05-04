using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record SetExperience
{
    private float _barValue = 0f;

    [PacketPropertyOrder(1)]
    public float BarValue
    {
        get => _barValue;
        init => _barValue = (value is >= 0 and < 1) ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    [PacketPropertyOrder(2)]
    public short Level { get; init; }

    [PacketPropertyOrder(3)]
    public short TotalAmount { get; init; }

    public SetExperience() { }

    public SetExperience(float barValue, short level, short totalAmount)
    {
        BarValue    = barValue;
        Level       = level;
        TotalAmount = totalAmount;
    }
}