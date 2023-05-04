using Minever.Networking.Serialization;

namespace Minever.Networking.DataTypes;

[PacketConverter(typeof(PacketEnumConverter<Difficulty, byte>))]
public enum Difficulty : byte
{
    Peaceful = 0,
    Easy     = 1,
    Normal   = 2,
    Hard     = 3,
}
