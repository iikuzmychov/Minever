using Minever.LowLevel.Core.Packets.Serialization.Attributes;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

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
