using System.Text.Json.Serialization;

namespace Minever.LowLevel.Java.Protocols.V5.Types;

public sealed record ServerPlayersInfo
{
    private int _maxPlayersCount;
    private int _playersCount;

    [JsonPropertyName("max")]
    public int MaxPlayersCount
    {
        get => _maxPlayersCount;
        init => _maxPlayersCount = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    [JsonPropertyName("online")]
    public int PlayersCount
    {
        get => _playersCount;
        init => _playersCount = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    [JsonPropertyName("sample")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<PlayerInfo>? SamplePlayers { get; init; }

    public ServerPlayersInfo() { }

    public ServerPlayersInfo(int maxPlayersCount, int playersCount, List<PlayerInfo>? samplePlayers)
    {
        MaxPlayersCount = maxPlayersCount;
        PlayersCount = playersCount;
        SamplePlayers = samplePlayers;
    }
}
