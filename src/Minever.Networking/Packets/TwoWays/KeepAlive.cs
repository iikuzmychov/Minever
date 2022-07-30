using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record KeepAlive
{
    [PacketPropertyOrder(1)]
    public int Id { get; init; }

    public KeepAlive() { }

    public KeepAlive(int id)
    {
        Id = id;
    }
}
