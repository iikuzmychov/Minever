using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

public sealed record ChatMessageToServer
{
    private readonly string _text = default!;

    [PacketPropertyOrder(1)]
    public string Text
    {
        get => _text;
        init => _text = value ?? throw new ArgumentNullException(nameof(value));
    }

    private ChatMessageToServer()
    {
    }

    public ChatMessageToServer(string text)
    {
        Text = text;
    }
}
