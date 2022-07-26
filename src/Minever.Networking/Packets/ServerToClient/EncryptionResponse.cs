using Minever.Networking.Packets.Serialization.Attributes;
using Minever.Networking.Packets.Serialization.Converters;

namespace Minever.Networking.Packets;

public sealed record EncryptionResponse
{
    private byte[] _publicKey = Array.Empty<byte>();
    private byte[] _verifyToken = Array.Empty<byte>();

    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(Int16PrefixedByteArrayPacketConverter))]
    public byte[] PublicKey
    {
        get => _publicKey;
        init => _publicKey = value ?? throw new ArgumentNullException(nameof(value));
    }

    [PacketPropertyOrder(2)]
    [PacketConverter(typeof(Int16PrefixedByteArrayPacketConverter))]
    public byte[] VerifyToken
    {
        get => _verifyToken;
        init => _verifyToken = value ?? throw new ArgumentNullException(nameof(value));
    }

    public EncryptionResponse() { }

    public EncryptionResponse(byte[] publicKey, byte[] verifyToken)
    {
        PublicKey   = publicKey;
        VerifyToken = verifyToken;
    }
}
