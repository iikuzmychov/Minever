﻿using Minever.Core.Packets.Serialization.Attributes;

namespace Minever.Java.Protocols.V5.Packets;

public sealed record TimeUpdate
{
    private readonly long _worldAge; // todo: do we need this ???
    private readonly long _dayTime;

    [PacketPropertyOrder(1)]
    public required long WorldAge
    {
        get => _worldAge;
        init => _worldAge = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    [PacketPropertyOrder(2)]
    public required long DayTime
    {
        get => _dayTime;
        init => _dayTime = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }
}
