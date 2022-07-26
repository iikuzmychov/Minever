using Minever.Networking.Packets.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record PlayerListItem
{
    private string _playerName = string.Empty;
    private short _ping;

    [PacketPropertyOrder(1)]
    public string PlayerName
    {
        get => _playerName;
        init => _playerName = value is not null ? value : throw new ArgumentNullException(value);
    }
    
    [PacketPropertyOrder(2)]
    public bool IsConnected { get; init; }
    
    [PacketPropertyOrder(3)]
    public short Ping
    {
        get => _ping;
        init => _ping = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    public PlayerListItem() { }

    public PlayerListItem(string playerName, bool isConnected, short ping)
    {
        PlayerName  = playerName;
        IsConnected = isConnected;
        Ping        = ping;
    }
}
