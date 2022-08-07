using Minever.Networking.Serialization;

namespace Minever.Networking.Packets;

public sealed record Statistics
{
    private Entry[] _entries = Array.Empty<Entry>();

    [PacketPropertyOrder(1)]
    [PacketConverter(typeof(PrefixedArrayPacketConverter<int, Entry, PacketVarIntConverter, PacketDefaultConverter>))]
    public Entry[] Entries
    {
        get => _entries;
        init => _entries = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Statistics() { }

    public Statistics(Entry[] entries)
    {
        _entries = entries;
    }

    public sealed record Entry
    {
        private string _name = string.Empty;

        [PacketPropertyOrder(1)]
        public string Name
        {
            get => _name;
            init => _name = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        [PacketPropertyOrder(2)]
        [PacketConverter(typeof(PacketVarIntConverter))]
        public int Amount { get; set; }

        public Entry() { }

        public Entry(string name, int amount)
        {
            Name   = name;
            Amount = amount;
        }
    }
}
