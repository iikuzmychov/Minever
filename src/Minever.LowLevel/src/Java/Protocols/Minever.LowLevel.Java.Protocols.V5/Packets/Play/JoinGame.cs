using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Packets.Serialization.Converters;
using Minever.LowLevel.Java.Core;
using Minever.LowLevel.Java.Protocols.V5.Enums;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Play;

/// <summary>
/// Id: 0x01 <br/>
/// State: <see cref="JavaConnectionState.Play"/> <br/>
/// Direction: <see cref="PacketDirection.FromServer"/> <br/>
/// <br/>
/// See <see href="https://wiki.vg/index.php?title=Protocol&amp;oldid=6003#Join_Game">Play/Clientbound/JoinGame</see> on wiki.vg.
/// </summary>
[JavaPacket<JavaProtocol5>(0x01, JavaConnectionState.Play, PacketDirection.FromServer)]
public sealed record JoinGame
{
    private string _levelType = default!;

    [PacketPropertyOrder(1)]
    public required int PlayerEntityId { get; init; }

    [PacketPropertyOrder(2)]
    public required Gamemode Gamemode { get; init; }

    [PacketPropertyOrder(3)]
    [PacketConverter<PacketEnumConverter<Dimension, sbyte>>]
    public required Dimension Dimension { get; init; }

    [PacketPropertyOrder(4)]
    public required Difficulty Difficulty { get; init; }

    [PacketPropertyOrder(5)]
    public required byte MaxPlayersCount { get; init; }

    [PacketPropertyOrder(6)]
    public required string LevelType
    {
        get => _levelType;
        init => _levelType = value ?? throw new ArgumentNullException(nameof(value));
    }
}
