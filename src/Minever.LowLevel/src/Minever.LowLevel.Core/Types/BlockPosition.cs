using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Packets.Serialization.Converters;

namespace Minever.LowLevel.Core.Types;

[PacketConverter<PacketBlockPositionConverter>]
public readonly record struct BlockPosition(int X, int Y, int Z);
