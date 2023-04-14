using Minever.Java.Core;

namespace Minever.Java.Protocols.V5.Packets;

public static class HandshakeNextConnectionStateExtensions
{
    public static JavaConnectionState ToJavaConnectionState(this HandshakeNextConnectionState state)
        => state switch
        {
            HandshakeNextConnectionState.Status => JavaConnectionState.Status,
            HandshakeNextConnectionState.Login  => JavaConnectionState.Login,

            _ => throw new ArgumentOutOfRangeException(nameof(state))
        };
}
