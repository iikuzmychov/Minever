using Minever.LowLevel.Core.Packets.Serialization.Attributes;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

public sealed record ChatMessageToServer
{
    private readonly string _text = default!;

    [PacketPropertyOrder(1)]
    public required string Text
    {
        get => _text;
        init => _text = value ?? throw new ArgumentNullException(nameof(value));
    }
}
