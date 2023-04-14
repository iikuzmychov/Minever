using Minever.Core.Packets.Serialization.Attributes;

namespace Minever.Java.Protocols.V5.Packets;

public sealed record KeepAlive
{
    [PacketPropertyOrder(1)]
    public int Id { get; init; }

    private KeepAlive()
    {
    }

    public KeepAlive(int id)
    {
        Id = id;
    }
}
