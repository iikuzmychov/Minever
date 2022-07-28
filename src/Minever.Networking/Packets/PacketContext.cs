namespace Minever.Networking.Packets;

public enum PacketDirection
{ 
    ClientToServer,
    ServerToClient
}

public readonly struct PacketContext
{
    public PacketDirection Direction { get; }
    public ConnectionState ConnectionState { get; }

    public PacketContext(PacketDirection direction, ConnectionState connectionState)
    {
        Direction       = direction;
        ConnectionState = connectionState;
    }

    public override bool Equals(object? obj) =>
        obj is PacketContext info &&
        Direction == info.Direction &&
        ConnectionState == info.ConnectionState;

    public override int GetHashCode() => HashCode.Combine(Direction, ConnectionState);

    public static bool operator ==(PacketContext left, PacketContext right) => left.Equals(right);

    public static bool operator !=(PacketContext left, PacketContext right) => !(left == right);
}
