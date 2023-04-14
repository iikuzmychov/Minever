namespace Minever.Java.Core;

public record struct JavaPacketContext(JavaConnectionState ConnectionState, PacketDirection Direction);