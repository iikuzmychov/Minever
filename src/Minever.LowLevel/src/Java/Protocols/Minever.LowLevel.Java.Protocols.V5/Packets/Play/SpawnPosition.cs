using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Types;
using Minever.LowLevel.Java.Core;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Play;

/// <summary>
/// Id: 0x05<br/>
/// State: <see cref="JavaConnectionState.Play"/> <br/>
/// Direction: <see cref="PacketDirection.FromServer"/> <br/>
/// <br/>
/// See <see href="https://wiki.vg/index.php?title=Protocol&amp;oldid=6003#Spawn_Position">Play/Clientbound/SpawnPosition</see> on wiki.vg.
/// </summary>
[JavaPacket<JavaProtocol5>(0x05, JavaConnectionState.Play, PacketDirection.FromServer)]
public sealed record SpawnPosition
{
    [PacketPropertyOrder(1)]
    public required BlockPosition Value { get; init; }
}