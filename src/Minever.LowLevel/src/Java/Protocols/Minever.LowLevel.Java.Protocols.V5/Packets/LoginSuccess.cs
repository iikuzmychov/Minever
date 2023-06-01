using Minever.LowLevel.Core.Packets.Serialization.Attributes;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

public sealed record LoginSuccess
{
    private string _name = default!;

    [PacketPropertyOrder(1)]
    public required Guid Uuid { get; init; }

    [PacketPropertyOrder(2)]
    public required string Name
    {
        get => _name;
        init
        {
            ArgumentException.ThrowIfNullOrEmpty(value);

            _name = value;
        }
    }
}
