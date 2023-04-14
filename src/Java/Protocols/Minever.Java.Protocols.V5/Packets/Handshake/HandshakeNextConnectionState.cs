using Minever.Core.Packets.Serialization.Attributes;
using Minever.Core.Packets.Serialization.Converters;

namespace Minever.Java.Protocols.V5.Packets;

[PacketConverter<EnumPacketConverter<HandshakeNextConnectionState, int, PacketVarIntConverter>>]
public enum HandshakeNextConnectionState
{
    Status = 1,
    Login  = 2,
}
