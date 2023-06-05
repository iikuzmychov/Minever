using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Java.Core;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

[JavaPacket<JavaProtocol5>(0x01, JavaConnectionState.Status, PacketDirection.FromServer)]
public sealed record PingFromServer
{
    [PacketPropertyOrder(1)]
    public required long Payload { get; init; }

    // todo: think about it
    //public TimeSpan CalculateDelay(DateTime dateTime) => dateTime.Subtract(DateTime.FromBinary(Payload));
}
