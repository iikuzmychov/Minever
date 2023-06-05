using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Java.Core;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

[JavaPacket<JavaProtocol5>(0x01, JavaConnectionState.Status, PacketDirection.ToServer)]
public sealed record PingToServer
{
    [PacketPropertyOrder(1)]
    public required long Payload { get; init; }

    public static PingToServer FromDateTime(DateTime dateTime) => new() { Payload = dateTime.ToBinary() };
}
