using Minever.Networking.Packets.Serialization.Attributes;
using Minever.Networking.Packets.Serialization.Converters;

namespace Minever.Networking.DataTypes;

[PacketConverter(typeof(ByteEnumPacketConverter<Difficulty>))]
public enum Difficulty
{
    Peaceful = 0,
    Easy     = 1,
    Normal   = 2,
    Hard     = 3,
}
