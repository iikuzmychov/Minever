using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

[PacketConverter(typeof(EnumPacketConverter<ClientStatus, byte>))]
public enum ClientStatus : byte
{ 
    PerformRespawn           = 0,
    RequestStats             = 1,
    OpenInventoryAchievement = 2,
}
