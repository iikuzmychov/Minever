namespace Minever.Networking.DataTypes;

public readonly struct BlockPosition : IEquatable<BlockPosition>
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public BlockPosition(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static explicit operator BlockPosition(Position position) =>
        new((int)position.X, (int)position.Y, (int)position.Z);

    public static bool operator ==(BlockPosition left, BlockPosition right) => left.Equals(right);

    public static bool operator !=(BlockPosition left, BlockPosition right) => !(left == right);

    public override string ToString() => $"({X}; {Y}; {Z})";

    public override bool Equals(object? obj) => obj is BlockPosition position && Equals(position);

    public bool Equals(BlockPosition other) =>
        X == other.X &&
        Y == other.Y &&
        Z == other.Z;

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
}
