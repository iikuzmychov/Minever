using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record HeldItemChange
{
    private sbyte _slotNumber;

    [PacketPropertyOrder(1)]
    public sbyte SlotNumber
    {
        get => _slotNumber;
        init => _slotNumber = (value is >= 0 and <= 8) ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    public HeldItemChange() { }

    public HeldItemChange(sbyte slotNumber)
    {
        SlotNumber = slotNumber;
    }
}
