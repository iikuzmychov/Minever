using Minever.Core.Packets.Serialization.Attributes;
using Minever.Core.Packets.Serialization.Converters;
using Minever.Java.Core;

namespace Minever.Java.Protocols.V5.Packets;

public sealed record Handshake
{
    private const int MaxHostLength = 255; // todo: is it needed?

    private string _host = default!;

    [PacketPropertyOrder(1)]
    [PacketConverter<PacketVarIntConverter>]
    public int ProtocolVersion { get; init; }

    [PacketPropertyOrder(2)]
    public required string Host
    {
        get => _host;
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value.Length > MaxHostLength)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Server address length greater {MaxHostLength}.");
            }

            _host = value;
        }
    }

    [PacketPropertyOrder(3)]
    public ushort Port { get; init; }

    [PacketPropertyOrder(4)]
    [PacketConverter<EnumPacketConverter<JavaConnectionState, int, PacketVarIntConverter>>]
    public required HandshakeNextConnectionState NextConnectionState { get; init; }
}
