using Minever.Networking.DataTypes.Text;
using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record Disconnect
{
    private string _reason = string.Empty;

    [PacketPropertyOrder(1)]
    public string Reason
    {
        get => _reason;
        init => _reason = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Disconnect() { }

    public Disconnect(string reason)
    {
        Reason = reason;
    }
}
