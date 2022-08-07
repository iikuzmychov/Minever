using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record ClientToServerChatMessage
{
    private string _text = string.Empty;

    [PacketPropertyOrder(1)]
    public string Text
    {
        get => _text;
        init => _text = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ClientToServerChatMessage() { }

    public ClientToServerChatMessage(string text)
    {
        Text = text;
    }
}
