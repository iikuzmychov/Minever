using Minever.Networking.DataTypes.Text;
using Minever.Networking.Packets.Serialization.Converters.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.DataTypes;
public record ServerStatus
{
    private MinecraftVersion _version = new();
    private ServerPlayersInfo _playersInfo = new();
    private MinecraftStringText _description = new MinecraftStringText();

    [JsonPropertyName("favicon")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(IconBase64JsonConverter))]
    public string? IconBase64 { get; set; }

    [JsonPropertyName("version")]
    public MinecraftVersion Version
    {
        get => _version;
        set => _version = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("players")]
    public ServerPlayersInfo PlayersInfo
    {
        get => _playersInfo;
        set => _playersInfo = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("description")]
    [JsonConverter(typeof(MinecraftTextJsonConverter))]
    public MinecraftStringText Description
    {
        get => _description;
        set => _description = value ?? throw new ArgumentNullException(nameof(value));
    }
}
