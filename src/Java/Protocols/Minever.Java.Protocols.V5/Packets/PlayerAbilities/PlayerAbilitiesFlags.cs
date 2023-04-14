using Minever.Core.Packets.Serialization.Attributes;
using Minever.Core.Packets.Serialization.Converters;

namespace Minever.Java.Protocols.V5.Packets;

[Flags]
[PacketConverter<PacketEnumConverter<PlayerAbilitiesFlags, sbyte>>]
public enum PlayerAbilitiesFlags : sbyte
{
    None         = 0,
    Invulnerable = 1,
    Flying       = 2,
    AllowFlying  = 4,
    CreativeMode = 8
}
