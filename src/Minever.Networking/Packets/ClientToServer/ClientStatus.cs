using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

[PacketConverter(typeof(EnumPacketConverter<ClientStatus, sbyte>))]
public enum ClientStatus : sbyte
{ 
    PerformRespawn           = 0,
    RequestStats             = 1,
    OpenInventoryAchievement = 2,
}
