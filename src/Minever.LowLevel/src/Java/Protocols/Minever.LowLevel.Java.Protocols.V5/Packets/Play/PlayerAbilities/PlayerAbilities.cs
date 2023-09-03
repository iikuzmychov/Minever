using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Java.Core;
using Minever.LowLevel.Java.Protocols.V5.Packets.Serialization.Converters;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Play.PlayerAbilities;

// todo: think about naming
/// <summary>
/// Id: 0x39 <br/>
/// State: <see cref="JavaConnectionState.Play"/> <br/>
/// Direction: <see cref="PacketDirection.FromServer"/> <br/>
/// <br/>
/// See <see href="https://wiki.vg/index.php?title=Protocol&amp;oldid=6003#Player_Abilities">Play/Clientbound/PlayerAbilities</see> on wiki.vg.
/// </summary>
[JavaPacket<JavaProtocol5>(0x39, JavaConnectionState.Play, PacketDirection.FromServer)]
public sealed record PlayerAbilities
{
    [PacketPropertyOrder(1)]
    public required PlayerAbilitiesFlags Flags { get; init; }

    [PacketPropertyOrder(2)]
    [PacketConverter<PacketFloat255Converter>]
    public required float FlyingSpeed { get; init; }

    [PacketPropertyOrder(3)]
    [PacketConverter<PacketFloat255Converter>]
    public required float WalkingSpeed { get; init; }
}
