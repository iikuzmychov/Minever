using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.DataTypes;

[PacketConverter(typeof(EnumPacketConverter<Gamemode, byte>))]
public enum Gamemode : byte
{
    Survival  = 0,
    Creative  = 1,
    Adventure = 2,
    Hardcore  = 8,
}
