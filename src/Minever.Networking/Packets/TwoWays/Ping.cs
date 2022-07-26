using Minever.Networking.Packets.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record Ping
{
    [PacketPropertyOrder(1)]
    public long Payload { get; init; }

    public Ping() { }

    public Ping(long payload)
    {
        Payload = payload;
    }

    public Ping(DateTime dateTime) : this(dateTime.ToBinary()) { }

    public TimeSpan CalculateDelay(DateTime dateTime) =>
        dateTime.Subtract(DateTime.FromBinary(Payload));
}
