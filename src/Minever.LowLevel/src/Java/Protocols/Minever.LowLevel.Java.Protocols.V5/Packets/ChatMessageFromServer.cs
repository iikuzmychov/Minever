﻿using Minever.LowLevel.Core.Packets.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record ChatMessageFromServer
{
    //private MinecraftText _text = new StringText();

    //[PacketPropertyOrder(1)]
    //[PacketConverter(typeof(JsonDataPacketConverter))]
    //public MinecraftText Text
    //{
    //    get => _text;
    //    init => _text = value ?? throw new ArgumentNullException(nameof(value));
    //}

    [PacketPropertyOrder(1)]
    public required string Text { get; init; }
}
