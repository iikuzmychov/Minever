namespace Minever.LowLevel.Java.Core;

public enum JavaConnectionState
{
    Disconnected,
    Handshake,
    Status,
    Login,
    Play
}

public static class JavaConnectionStateExtensions
{
    public static string ToString4(this JavaConnectionState connectionState)
        => connectionState switch
        {
            JavaConnectionState.Handshake => "HAND",
            JavaConnectionState.Status    => "STAT",
            JavaConnectionState.Login     => "LGIN",
            JavaConnectionState.Play      => "PLAY",

            _ => throw new NotSupportedException()
        };
}
