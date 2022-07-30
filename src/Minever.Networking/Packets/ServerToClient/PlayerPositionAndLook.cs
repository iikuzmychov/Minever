using Minever.Networking.DataTypes;
using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record PlayerPositionAndLook
{
    [PacketPropertyOrder(1)]
    public Position Position { get; init; }

    [PacketPropertyOrder(2)]
    public float Yaw { get; init; }

    [PacketPropertyOrder(3)]
    public float Pitch { get; init; }

    [PacketPropertyOrder(4)]
    public bool IsOnGround { get; init; }

    public PlayerPositionAndLook() { }

    public PlayerPositionAndLook(Position position, float yaw, float pitch, bool isOnGround)
    {
        Position   = position;
        Yaw        = yaw;
        Pitch      = pitch;
        IsOnGround = isOnGround;
    }
}
