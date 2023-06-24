using Minever.LowLevel.Core.Packets.Serialization.Attributes;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Play.PlayerAbilities;

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
