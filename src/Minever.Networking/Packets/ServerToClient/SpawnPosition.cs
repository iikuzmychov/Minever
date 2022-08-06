using Minever.Networking.DataTypes;
using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record SpawnPosition
{
    [PacketPropertyOrder(1)]
    public BlockPosition BlockPosition { get; init; }

    public SpawnPosition() { }

    public SpawnPosition(BlockPosition blockPosition)
    {
        BlockPosition = blockPosition;
    }

    public SpawnPosition(int x, int y, int z) : this(new BlockPosition(x, y, z)) { }
}
