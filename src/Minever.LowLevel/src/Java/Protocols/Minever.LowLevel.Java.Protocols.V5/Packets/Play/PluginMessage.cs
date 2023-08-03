using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Packets.Serialization.Converters;

namespace Minever.LowLevel.Java.Protocols.V5.Packets.Play;

// TODO: looks like it isn't the best idea to have a base abstract class
// for PluginMessageFromServer and PluginMessageToServer because we can call
// client.WaitForPacketAsync<PluginMessage> or a similar one by mistake and we will have no compile time errors.
// Of course we also have no compile-time errors for the other "wrong" classes,
// but they can be POTENTIALLY acceptable, unlike abstract ones.
// TODO 2: make data strong typed
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
