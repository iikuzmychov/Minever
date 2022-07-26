using Minever.Networking.Packets.Serialization.Attributes;
using Minever.Networking.Packets.Serialization.Converters;

namespace Minever.Networking.DataTypes;

[PacketConverter(typeof(ByteEnumPacketConverter<Gamemode>))]
public enum Gamemode
{
    Survival  = 0,
    Creative  = 1,
    Adventure = 2,
    Hardcore  = 8,
}
