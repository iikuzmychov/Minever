using System.Text.Json.Serialization;

namespace Minever.Networking.DataTypes;

public record ServerPlayersInfo
{
    private int _maxPlayersCount;
    private int _playersCount;

    [JsonPropertyName("max")]
    public int MaxPlayersCount
    {
        get => _maxPlayersCount;
        set => _maxPlayersCount = (value >= 0) ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    [JsonPropertyName("online")]
    public int PlayersCount
    {
        get => _playersCount;
        set => _playersCount = (value >= 0) ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    [JsonPropertyName("sample")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<PlayerInfo>? SamplePlayers { get; set; }
}
