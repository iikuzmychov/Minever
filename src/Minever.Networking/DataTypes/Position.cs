namespace Minever.Networking.DataTypes;

public readonly struct Position : IEquatable<Position>
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Position(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static explicit operator Position(BlockPosition blockPosition) =>
        new(blockPosition.X, blockPosition.Y, blockPosition.Z);

    public static bool operator ==(Position left, Position right) => left.Equals(right);

    public static bool operator !=(Position left, Position right) => !(left == right);

    public override string ToString() => $"{X}, {Y}, {Z}";

    public override bool Equals(object? obj) => obj is Position position && Equals(position);

    public bool Equals(Position other) =>
        X == other.X &&
        Y == other.Y &&
        Z == other.Z;

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
}
