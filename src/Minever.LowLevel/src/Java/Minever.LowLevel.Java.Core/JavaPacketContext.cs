namespace Minever.LowLevel.Java.Core;

public readonly record struct JavaPacketContext(JavaConnectionState ConnectionState, PacketDirection Direction);