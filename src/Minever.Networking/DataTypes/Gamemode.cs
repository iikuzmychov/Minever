using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.DataTypes;

[PacketConverter(typeof(ByteEnumPacketConverter<Gamemode>))]
public enum Gamemode
{
    Survival  = 0,
    Creative  = 1,
    Adventure = 2,
    Hardcore  = 8,
}
