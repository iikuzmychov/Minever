using Minever.Networking.Serialization;

namespace Minever.Networking.DataTypes;

[PacketConverter(typeof(PacketEnumConverter<Gamemode, byte>))]
public enum Gamemode : byte
{
    Survival  = 0,
    Creative  = 1,
    Adventure = 2,
    Hardcore  = 8,
}
