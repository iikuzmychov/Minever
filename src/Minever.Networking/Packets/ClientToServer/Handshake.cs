using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;

namespace Minever.Networking.Packets;

[PacketConverter(typeof(EnumPacketConverter<HandshakeNextState, int, VarIntPacketConverter>))]
public enum HandshakeNextState
{ 
    Status = 1,
    Login  = 2
}

public static class HandshakeNextStateExtensions
{ 
    public static ConnectionState ToConnectionState(this HandshakeNextState handhakeNextState) =>
        handhakeNextState switch
        {
            HandshakeNextState.Status => ConnectionState.Status,
            HandshakeNextState.Login  => ConnectionState.Login,
            _ => throw new NotImplementedException()
        };
}

public sealed record Handshake
{
    private string _serverAddress = string.Empty;

    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(VarIntPacketConverter))]
    public int ProtocolVersion { get; init; }

    [PacketPropertyOrder(2)]
    public string ServerAddress
    {
        get => _serverAddress;
        init
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            const int MaxServerAddressLength = 255;

            if (value.Length > MaxServerAddressLength)
                throw new ArgumentOutOfRangeException(nameof(value), $"Server address length greater {MaxServerAddressLength}.");

            _serverAddress = value;
        }
    }

    [PacketPropertyOrder(3)]
    public ushort ServerPort { get; init; }

    [PacketPropertyOrder(4)]
    public HandshakeNextState NextState { get; init; }

    public Handshake() { }

    public Handshake(int protocolVersion, string serverAddress, ushort serverPort, HandshakeNextState nextState)
    {
        ProtocolVersion = protocolVersion;
        ServerAddress   = serverAddress;
        ServerPort      = serverPort;
        NextState       = nextState;
    }
}
