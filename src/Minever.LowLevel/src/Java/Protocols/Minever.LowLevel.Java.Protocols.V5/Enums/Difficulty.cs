using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Packets.Serialization.Converters;

namespace Minever.LowLevel.Java.Protocols.V5.Enums;

[PacketConverter<PacketEnumConverter<Difficulty, byte>>]
public enum Difficulty : byte
{
    Peaceful = 0,
    Easy     = 1,
    Normal   = 2,
    Hard     = 3,
}
