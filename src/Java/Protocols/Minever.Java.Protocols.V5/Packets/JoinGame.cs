using Minever.Core.Packets.Serialization.Attributes;
using Minever.Core.Packets.Serialization.Converters;
using Minever.Java.Protocols.V5.Enums;

namespace Minever.Java.Protocols.V5.Packets;

public sealed record JoinGame
{
    [PacketPropertyOrder(1)]
    public int PlayerEntityId { get; init; }

    [PacketPropertyOrder(2)]
    public Gamemode Gamemode { get; init; }

    [PacketPropertyOrder(3)]
    [PacketConverter<PacketEnumConverter<Dimension, sbyte>>]
    public Dimension Dimension { get; init; }

    [PacketPropertyOrder(4)]
    public Difficulty Difficulty { get; init; }

    [PacketPropertyOrder(5)]
    public byte MaxPlayersCount { get; init; }
}
