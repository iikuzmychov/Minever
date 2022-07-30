using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

[PacketConverter(typeof(EnumPacketConverter<ClientStatusAction, byte>))]
public enum ClientStatusAction : byte
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
