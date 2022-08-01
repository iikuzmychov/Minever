namespace Minever.Networking.Packets;

public enum PacketDirection
{ 
    ClientToServer,
    ServerToClient
}

public readonly struct PacketContext
{
    public PacketDirection Direction { get; init; }
    public ConnectionState ConnectionState { get; init; }

    public PacketContext(PacketDirection direction, ConnectionState connectionState)
    {
        Direction       = direction;
        ConnectionState = connectionState;
    }

    public override bool Equals(object? obj) =>
        obj is PacketContext context &&
        Direction == context.Direction &&
        ConnectionState == context.ConnectionState;

    public override int GetHashCode() => HashCode.Combine(Direction, ConnectionState);

    public static bool operator ==(PacketContext left, PacketContext right) => left.Equals(right);

    public static bool operator !=(PacketContext left, PacketContext right) => !(left == right);
}
