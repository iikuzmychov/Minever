using Minever.Networking.DataTypes.Text;
using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record ServerToClientChatMessage
{
    private MinecraftText _text = new StringText();

    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(JsonDataPacketConverter))]
    public MinecraftText Text
    {
        get => _text;
        init => _text = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ServerToClientChatMessage() { }

    public ServerToClientChatMessage(MinecraftText text)
    {
        Text = text;
    }
}
