using Minever.LowLevel.Java.Core;
using Minever.LowLevel.Java.Protocols.V5.Packets;

namespace Minever.LowLevel.Java.Protocols.V5.Extensions;

public static class HandshakeNextConnectionStateExtensions
{
    public static JavaConnectionState ToJavaConnectionState(this HandshakeNextConnectionState state)
        => state switch
        {
            HandshakeNextConnectionState.Status => JavaConnectionState.Status,
            HandshakeNextConnectionState.Login => JavaConnectionState.Login,

            _ => throw new ArgumentOutOfRangeException(nameof(state))
        };
}
