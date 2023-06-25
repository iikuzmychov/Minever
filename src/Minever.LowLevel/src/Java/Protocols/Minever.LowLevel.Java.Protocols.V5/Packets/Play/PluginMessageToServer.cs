using Minever.LowLevel.Java.Core;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Play;

/// <summary>
/// Id: 0x17 <br/>
/// State: <see cref="JavaConnectionState.Play"/> <br/>
/// Direction: <see cref="PacketDirection.ToServer"/> <br/>
/// <br/>
/// See <see href="https://wiki.vg/index.php?title=Protocol&amp;oldid=6003#Plugin_Message_2">Play/Serverbound/PluginMessage</see> on wiki.vg.
/// </summary>
[JavaPacket<JavaProtocol5>(0x17, JavaConnectionState.Play, PacketDirection.ToServer)]
public sealed record PluginMessageToServer : PluginMessage;
