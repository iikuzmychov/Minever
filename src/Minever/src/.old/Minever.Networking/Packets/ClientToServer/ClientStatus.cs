using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

[PacketConverter(typeof(PacketEnumConverter<ClientStatus, sbyte>))]
public enum ClientStatus : sbyte
{ 
    PerformRespawn           = 0,
    RequestStats             = 1,
    OpenInventoryAchievement = 2,
}
