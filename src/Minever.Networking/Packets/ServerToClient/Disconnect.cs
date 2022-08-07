using Minever.Networking.DataTypes.Text;
using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record Disconnect
{
    private StringText _reason = new();

    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(JsonDataPacketConverter))]
    public StringText Reason
    {
        get => _reason;
        init => _reason = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Disconnect() { }

    public Disconnect(StringText reason)
    {
        Reason = reason;
    }
}
