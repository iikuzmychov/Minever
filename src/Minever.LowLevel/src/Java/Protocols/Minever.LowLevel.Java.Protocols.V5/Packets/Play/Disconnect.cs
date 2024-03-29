﻿using Minever.LowLevel.Core.Packets.Serialization.Attributes;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Play;

public sealed record Disconnect
{
    private readonly string _reason = string.Empty;

    [PacketPropertyOrder(1)]
    public string Reason
    {
        get => _reason;
        init => _reason = value ?? throw new ArgumentNullException(nameof(value));
    }
}
