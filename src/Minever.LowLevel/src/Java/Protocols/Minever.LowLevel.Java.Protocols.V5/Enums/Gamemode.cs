using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Packets.Serialization.Converters;

namespace Minever.LowLevel.Java.Protocols.V5.Enums;

[PacketConverter<PacketEnumConverter<Gamemode, byte>>]
public enum Gamemode : byte
{
    Survival  = 0,
    Creative  = 1,
    Adventure = 2,
    Hardcore  = 8,
}
