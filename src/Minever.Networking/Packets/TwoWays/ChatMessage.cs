using Minever.Networking.DataTypes.Text;
using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record ChatMessage
{
    private MinecraftText _text = new MinecraftStringText();

    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(JsonDataPacketConverter))]
    public MinecraftText Text
    {
        get => _text;
        init => _text = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ChatMessage() { }

    public ChatMessage(MinecraftText text)
    {
        Text = text;
    }
}
