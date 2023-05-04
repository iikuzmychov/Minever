﻿using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record PluginMessage
{
    private string _channelName = string.Empty;
    private byte[] _data = Array.Empty<byte>();

    [PacketPropertyOrder(1)]
    public string ChannelName
    {
        get => _channelName;
        init => _channelName = value ?? throw new ArgumentNullException(nameof(value));
    }

    [PacketPropertyOrder(2)]
    [PacketConverter(typeof(PacketPrefixedArrayConverter<short, byte>))]
    public byte[] Data
    {
        get => _data;
        init => _data = value ?? throw new ArgumentNullException(nameof(value));
    }

    public PluginMessage() {}

    public PluginMessage(string channelName, byte[] data)
    {
        ChannelName = channelName;
        Data        = data;
    }
}
