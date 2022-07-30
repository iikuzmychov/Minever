using Minever.Networking.DataTypes;
using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record JoinGame
{
    [PacketPropertyOrder(1)]
    public int PlayerEntityId { get; init; }

    [PacketPropertyOrder(2)]
    public Gamemode Gamemode { get; init; }

    [PacketPropertyOrder(3)]
    [PacketConverter(typeof(SByteEnumPacketConverter<Dimension>))]
    public Dimension Dimension { get; init; }

    [PacketPropertyOrder(4)]
    public Difficulty Difficulty { get; init; }

    [PacketPropertyOrder(5)]
    public byte MaxPlayersCount { get; init; }

    public JoinGame() { }

    public JoinGame(int playerEntityId, Gamemode gamemode, Dimension dimension, Difficulty difficulty, byte maxPlayersCount)
    {
        PlayerEntityId  = playerEntityId;
        Gamemode        = gamemode;
        Dimension       = dimension;
        Difficulty      = difficulty;
        MaxPlayersCount = maxPlayersCount;
    }
}
