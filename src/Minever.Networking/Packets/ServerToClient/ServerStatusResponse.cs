using Minever.Networking.DataTypes;
using Minever.Networking.Packets.Serialization.Attributes;
using Minever.Networking.Packets.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record ServerStatusResponse
{
    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(JsonObjectPacketConverter))]
    public ServerStatus Status { get; init; } = new();

    public ServerStatusResponse() { }

    public ServerStatusResponse(ServerStatus status)
    {
        Status = status ?? throw new ArgumentNullException(nameof(status));
    }
}
