using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record TimeUpdate
{
    private readonly long _worldAge;
    private readonly long _dayTime;

    [PacketPropertyOrder(1)]
    public long WorldAge
    {
        get => _worldAge;
        init => _worldAge = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    [PacketPropertyOrder(2)]
    public long DayTime
    {
        get => _dayTime;
        init => _dayTime = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    public TimeUpdate() { }

    public TimeUpdate(long worldAge, long dayTime)
    {
        WorldAge = worldAge;
        DayTime  = dayTime;
    }
}
