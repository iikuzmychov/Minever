using Minever.Networking.DataTypes.Text;
using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record Disconnect
{
    private MinecraftStringText _reason = new();

    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(JsonDataPacketConverter))]
    public MinecraftStringText Reason
    {
        get => _reason;
        init => _reason = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Disconnect() { }

    public Disconnect(MinecraftStringText reason)
    {
        Reason = reason;
    }
}
