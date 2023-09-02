using Minever.LowLevel.Java.Core;
using Minever.LowLevel.Java.Protocols.V5.Extensions;
using Minever.LowLevel.Java.Protocols.V5.Packets.Handshake;
using Minever.LowLevel.Java.Protocols.V5.Packets.Login;

namespace Minever.LowLevel.Java.Protocols.V5;

public sealed class JavaProtocol5 : JavaProtocolBase
{
    private static readonly IReadOnlyDictionary<JavaPacketContext, IReadOnlyBidirectionalDictionary<int, Type>> _packets;

    public static JavaProtocol5 Instance { get; } = new();

    public override int Version => 5;
    public override IReadOnlyDictionary<JavaPacketContext, IReadOnlyBidirectionalDictionary<int, Type>> Packets => _packets;

    static JavaProtocol5()
    {
        _packets = FindPacketsInAssembly<JavaProtocol5>(typeof(JavaProtocol5).Assembly);
    }

    private JavaProtocol5()
    {
    }

    public override JavaConnectionState GetNextConnectionState(object packet, JavaPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(packet);

        if (!IsPacketSupported(packet, context))
        {
            throw new NotSupportedException($"The packet of type {packet.GetType()} is not supported by the protocol for specified context.");
        }

        return (context.ConnectionState, packet) switch
        {
            (JavaConnectionState.Handshake, Handshake handshake) => handshake.NextConnectionState.ToJavaConnectionState(),
            (JavaConnectionState.Login, LoginSuccess)            => JavaConnectionState.Play,

            _ => context.ConnectionState,
        };
    }
}
