namespace Minever.Networking.Packets;

public class MinecraftPacket<TData>
{
    public int Id { get; }
    public MinecraftPacketKind Kind { get; }
    public TData Data { get; }

    public MinecraftPacket(int id, MinecraftPacketKind kind, TData data)
    {
        Id   = id;
        Kind = kind;
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }

    public static explicit operator MinecraftPacket<TData>(MinecraftPacket<object> packet) =>
        new(packet.Id, packet.Kind, (TData)packet.Data);
}
