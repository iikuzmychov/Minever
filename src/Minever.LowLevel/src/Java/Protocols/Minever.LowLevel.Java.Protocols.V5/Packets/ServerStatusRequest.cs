using Minever.LowLevel.Java.Core;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

[JavaPacket<JavaProtocol5>(0x00, JavaConnectionState.Status, PacketDirection.ToServer)]
public sealed record ServerStatusRequest;
