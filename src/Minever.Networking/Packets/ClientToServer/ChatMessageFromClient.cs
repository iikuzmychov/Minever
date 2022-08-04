using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record ChatMessageFromClient
{
    private string _text = string.Empty;

    [PacketPropertyOrder(1)]
    public string Text
    {
        get => _text;
        init => _text = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ChatMessageFromClient() { }

    public ChatMessageFromClient(string text)
    {
        Text = text;
    }
}
