namespace Minever.Networking.Packets;

public record MinecraftPacket<TData>
    where TData : notnull
{
    public int Id { get; }
    public TData Data { get; }

    public MinecraftPacket(int id, TData data)
    {
        Id   = id;
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }

    public static explicit operator MinecraftPacket<TData>(MinecraftPacket<object> packet) =>
        new(packet.Id, (TData)packet.Data);
}
