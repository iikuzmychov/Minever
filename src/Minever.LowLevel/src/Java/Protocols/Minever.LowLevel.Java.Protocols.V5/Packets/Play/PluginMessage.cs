using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Packets.Serialization.Converters;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Play;

public abstract record PluginMessage
{
    private readonly string _channel = default!;
    private readonly byte[] _data = default!;

    [PacketPropertyOrder(1)]
    public required string Channel
    {
        get => _channel;
        init => _channel = value ?? throw new ArgumentNullException(nameof(value));
    }

    [PacketPropertyOrder(2)]
    [PacketConverter<PacketLengthPrefixedArrayConverter<short, byte>>]
    public required byte[] Data
    {
        get => _data;
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            // length is a short property
            if (value.Length > short.MaxValue)
            {
                throw new ArgumentException($"The data array legth should be a short positive value (0 <= length <= {short.MaxValue}).");
            }

            _data = value;
        }
    }
}
