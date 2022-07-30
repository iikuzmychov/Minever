using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;
using System.Buffers.Binary;

namespace Minever.Networking.Packets;

[Flags]
[PacketConverter(typeof(ByteEnumPacketConverter<PlayerAbilitiesFlags>))]
public enum PlayerAbilitiesFlags
{
    None         = 0,
    Invulnerable = 1,
    Flying       = 2,
    AllowFlying  = 4,
    CreativeMode = 8
}

public sealed record PlayerAbilities
{
    [PacketPropertyOrder(1)]
    public PlayerAbilitiesFlags Flags { get; init; }

    [PacketPropertyOrder(2)]
    public float FlyingSpeed { get; init; }

    [PacketPropertyOrder(3)]
    public float WalkingSpeed { get; init; }
}
