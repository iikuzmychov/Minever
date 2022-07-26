namespace Minever.Networking.Packets;

public class MinecraftPacket<TData>
{
    public int Id { get; }
    public PacketContext Context { get; }
    public TData Data { get; }

    public MinecraftPacket(int id, PacketContext context, TData data)
    {
        Id      = id;
        Context = context;
        Data    = data ?? throw new ArgumentNullException(nameof(data));
    }

    public static explicit operator MinecraftPacket<TData>(MinecraftPacket<object> packet) =>
        new(packet.Id, packet.Context, (TData)packet.Data);
}
