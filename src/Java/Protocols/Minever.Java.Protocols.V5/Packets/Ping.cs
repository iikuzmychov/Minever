using Minever.Core.Packets.Serialization.Attributes;
using System;

namespace Minever.Java.Protocols.V5.Packets;

public sealed record Ping
{
    [PacketPropertyOrder(1)]
    public long Payload { get; init; }

    public static Ping FromDateTime(DateTime dateTime) => new(dateTime.ToBinary());

    private Ping() { }

    public Ping(long payload)
    {
        Payload = payload;
    }

    // todo: think about it
    //public TimeSpan CalculateDelay(DateTime dateTime) => dateTime.Subtract(DateTime.FromBinary(Payload));
}
