using Minever.Networking.DataTypes;
using Minever.Networking.Packets.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record PlayerPositionAndLookWithStance
{
    [PacketPropertyOrder(1)]
    public double X { get; init; }

    [PacketPropertyOrder(2)]
    public double Y { get; init; }

    [PacketPropertyOrder(3)]
    public double Stance { get; init; }

    [PacketPropertyOrder(4)]
    public double Z { get; init; }

    [PacketPropertyOrder(5)]
    public float Yaw { get; init; }

    [PacketPropertyOrder(6)]
    public float Pitch { get; init; }

    [PacketPropertyOrder(7)]
    public bool IsOnGround { get; init; }

    public PlayerPositionAndLookWithStance() { }

    public PlayerPositionAndLookWithStance(double x, double y, double z, double stance, float yaw, float pitch, bool isOnGround)
    {
        X          = x;
        Y          = y;
        Z          = z;
        Stance     = stance;
        Yaw        = yaw;
        Pitch      = pitch;
        IsOnGround = isOnGround;
    }

    public PlayerPositionAndLookWithStance(Position position, double stance, float yaw, float pitch, bool isOnGround)
        : this(position.X, position.Y, position.Z, stance, yaw, pitch, isOnGround) { }

    public PlayerPositionAndLookWithStance(PlayerPositionAndLook positionAndLook, double stance)
        : this(positionAndLook.Position, stance, positionAndLook.Yaw, positionAndLook.Pitch, positionAndLook.IsOnGround) { }
}
