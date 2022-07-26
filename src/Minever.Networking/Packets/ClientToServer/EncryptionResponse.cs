using Minever.Networking.Packets.Serialization.Attributes;
using Minever.Networking.Packets.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record EncryptionRequest
{
    private string _serverId = string.Empty;
    private byte[] _publicKey = Array.Empty<byte>();
    private byte[] _verifyToken = Array.Empty<byte>();

    [PacketPropertyOrder(1)]
    public string ServerId
    {
        get => _serverId;
        init => _serverId = value ?? throw new ArgumentNullException(nameof(value));
    }

    [PacketPropertyOrder(2)]
    [PacketConverter(typeof(Int16PrefixedByteArrayPacketConverter))]
    public byte[] PublicKey
    {
        get => _publicKey;
        init => _publicKey = value ?? throw new ArgumentNullException(nameof(value));
    }

    [PacketPropertyOrder(3)]
    [PacketConverter(typeof(Int16PrefixedByteArrayPacketConverter))]
    public byte[] VerifyToken
    {
        get => _verifyToken;
        init => _verifyToken = value ?? throw new ArgumentNullException(nameof(value));
    }

    public EncryptionRequest() { }

    public EncryptionRequest(string serverId, byte[] publicKey, byte[] verifyToken)
    {
        ServerId    = serverId;
        PublicKey   = publicKey;
        VerifyToken = verifyToken;
    }
}
