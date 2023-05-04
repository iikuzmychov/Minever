namespace Minever.LowLevel.Java.Protocols.V5.Types;

public readonly record struct BlockPosition(int X, int Y, int Z)
{
    public override string ToString() => $"({X}; {Y}; {Z})";
}
