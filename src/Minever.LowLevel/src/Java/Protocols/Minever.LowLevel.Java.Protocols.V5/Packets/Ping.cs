using Minever.LowLevel.Core.Packets.Serialization.Attributes;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

public sealed record Ping
{
    [PacketPropertyOrder(1)]
    public required long Payload { get; init; }

    public static Ping FromDateTime(DateTime dateTime) => new() { Payload = dateTime.ToBinary() };

    // todo: think about it
    //public TimeSpan CalculateDelay(DateTime dateTime) => dateTime.Subtract(DateTime.FromBinary(Payload));
}
