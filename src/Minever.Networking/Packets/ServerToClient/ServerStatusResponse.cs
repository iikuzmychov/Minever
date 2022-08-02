using Minever.Networking.DataTypes;
using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record ServerStatusResponse
{
    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(JsonDataPacketConverter))]
    public ServerStatus Status { get; init; } = new();

    public ServerStatusResponse() { }

    public ServerStatusResponse(ServerStatus status)
    {
        Status = status ?? throw new ArgumentNullException(nameof(status));
    }
}
