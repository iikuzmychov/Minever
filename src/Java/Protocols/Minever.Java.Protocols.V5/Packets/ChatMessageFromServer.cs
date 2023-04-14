using Minever.Core.Packets.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

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

    private ChatMessageFromServer()
    {
    }

    [SetsRequiredMembers]
    public ChatMessageFromServer(string text)
    {
        Text = text;
    }
}
