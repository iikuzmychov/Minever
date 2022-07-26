using Minever.Networking.DataTypes;
using Minever.Networking.Packets.Serialization.Attributes;
using Minever.Networking.Packets.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record Respawn
{
    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(Int32EnumPacketConverter<Dimension>))]
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
