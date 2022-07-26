using Minever.Networking.Packets.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record HeldItemChange
{
    [PacketPropertyOrder(1)]
    public byte SlotNumber { get; init; }

    public HeldItemChange() { }

    public HeldItemChange(byte slotNumber)
    {
        SlotNumber = slotNumber;
    }
}
