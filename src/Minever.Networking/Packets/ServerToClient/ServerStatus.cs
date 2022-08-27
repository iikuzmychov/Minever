using Minever.Networking.DataTypes;
using Minever.Networking.Serialization;
using Minever.Networking.Serialization.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.Packets;

[PacketConverter(typeof(JsonDataPacketConverter))]
public sealed record ServerStatus
{
    private MinecraftVersion _version = new();
    private ServerPlayersInfo _playersInfo = new();
    private string _description = string.Empty;

    [JsonPropertyName("version")]
    public MinecraftVersion Version
    {
        get => _version;
        init => _version = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("players")]
    public ServerPlayersInfo PlayersInfo
    {
        get => _playersInfo;
        init => _playersInfo = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("description")]
    public string Description
    {
        get => _description;
        init => _description = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("favicon")]
    [JsonConverter(typeof(JsonIconBase64Converter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconBase64 { get; init; }

    public ServerStatus() { }

    public ServerStatus(MinecraftVersion version, ServerPlayersInfo playersInfo, string desription, string? iconBase64 = null)
    {
        Version = version;
        PlayersInfo = playersInfo;
        Description = desription;
        IconBase64 = iconBase64;
    }
}
