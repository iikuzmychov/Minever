using Minever.Networking.Packets.Serialization.Attributes;
using Minever.Networking.Packets.Serialization.Converters;

namespace Minever.Networking.Packets;

[PacketConverter(typeof(ByteEnumPacketConverter<ClientStatusAction>))]
public enum ClientStatusAction
{ 
    PerformRespawn           = 0,
    RequestStats             = 1,
    OpenInventoryAchievement = 2,
}

public sealed record ClientStatus
{
    [PacketPropertyOrder(1)]
    public ClientStatusAction Action { get; init; }

    public ClientStatus() { }

    public ClientStatus(ClientStatusAction action)
    {
        Action = action;
    }
}
