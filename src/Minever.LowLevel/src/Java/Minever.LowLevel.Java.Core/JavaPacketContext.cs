﻿namespace Minever.LowLevel.Java.Core;

public record struct JavaPacketContext(JavaConnectionState ConnectionState, PacketDirection Direction);