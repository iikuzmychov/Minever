using Minever.Networking.DataTypes;
using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record Respawn
{
    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(PacketEnumConverter<Dimension, int>))]
    public Dimension Dimension { get; init; }

    [PacketPropertyOrder(2)]
    public Difficulty Difficulty { get; init; }

    [PacketPropertyOrder(3)]
    public Gamemode Gamemode { get; init; }

    public Respawn() { }

    public Respawn(Dimension dimension, Difficulty difficulty, Gamemode gamemode)
    {
        Gamemode   = gamemode;
        Difficulty = difficulty;
        Dimension  = dimension;
    }
}
