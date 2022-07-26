namespace Minever.Networking.Packets;

public enum MinecraftPacketDirection
{ 
    ClientToServer,
    ServerToClient
}

public struct MinecraftPacketKind
{
    public MinecraftPacketDirection Direction { readonly get; set; }
    public MinecraftConnectionState ConnectionState { readonly get; set; }

    public MinecraftPacketKind(MinecraftPacketDirection direction, MinecraftConnectionState connectionState)
    {
        Direction       = direction;
        ConnectionState = connectionState;
    }

    public override bool Equals(object? @object) =>
        @object is MinecraftPacketKind info &&
        Direction == info.Direction &&
        ConnectionState == info.ConnectionState;

    public override int GetHashCode() => HashCode.Combine(Direction, ConnectionState);

    public static bool operator ==(MinecraftPacketKind left, MinecraftPacketKind right) => left.Equals(right);

    public static bool operator !=(MinecraftPacketKind left, MinecraftPacketKind right) => !(left == right);
}
