using Minever.LowLevel.Java.Core;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Play;

/// <summary>
/// Id: 0x3F <br/>
/// State: <see cref="JavaConnectionState.Play"/> <br/>
/// Direction: <see cref="PacketDirection.FromServer"/> <br/>
/// <br/>
/// See <see href="https://wiki.vg/index.php?title=Protocol&amp;oldid=6003#Plugin_Message">Play/Clientbound/PluginMessage</see> on wiki.vg.
/// </summary>
[JavaPacket<JavaProtocol5>(0x3F, JavaConnectionState.Play, PacketDirection.FromServer)]
public sealed record PluginMessageFromServer : PluginMessage;
