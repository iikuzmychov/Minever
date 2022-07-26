using Minever.Networking.Packets.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record LoginSuccess
{
    private string _name = string.Empty;

    [PacketPropertyOrder(1)]
    public Guid Uuid { get; init; }

    [PacketPropertyOrder(2)]
    public string Name
    {
        get => _name;
        init => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public LoginSuccess() { }

    public LoginSuccess(string name, Guid uuid)
    {
        Name = name;
        Uuid = uuid;
    }
}
