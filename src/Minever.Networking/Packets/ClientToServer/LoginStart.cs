using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record LoginStart
{
    private string _name = string.Empty;

    [PacketPropertyOrder(1)]
    public string Name
    {
        get => _name;
        init => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public LoginStart() { }

    public LoginStart(string name)
    {
        Name = name;
    }
}
