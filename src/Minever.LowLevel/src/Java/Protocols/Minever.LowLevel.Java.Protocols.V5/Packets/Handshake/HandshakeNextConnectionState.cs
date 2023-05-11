using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Packets.Serialization.Converters;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

[PacketConverter<PacketEnumConverter<HandshakeNextConnectionState, int, PacketVarIntConverter>>]
public enum HandshakeNextConnectionState
{
    Status = 1,
    Login  = 2,
}
