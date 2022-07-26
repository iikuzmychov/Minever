using Minever.Networking.DataTypes;
using Minever.Networking.Packets.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record SpawnPosition
{
    [PacketPropertyOrder(1)]
    public BlockPosition Position { get; init; }

    public SpawnPosition() { }

    public SpawnPosition(BlockPosition position)
    {
        Position = position;
    }

    public SpawnPosition(int x, int y, int z) : this(new BlockPosition(x, y, z)) { }
}
