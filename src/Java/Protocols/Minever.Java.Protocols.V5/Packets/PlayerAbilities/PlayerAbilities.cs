﻿using Minever.Core.Packets.Serialization.Attributes;

namespace Minever.Java.Protocols.V5.Packets;

// todo: think about naming
public sealed record PlayerAbilities
{
    [PacketPropertyOrder(1)]
    public PlayerAbilitiesFlags Flags { get; init; }

    [PacketPropertyOrder(2)]
    public float FlyingSpeed { get; init; }

    [PacketPropertyOrder(3)]
    public float WalkingSpeed { get; init; }
}
