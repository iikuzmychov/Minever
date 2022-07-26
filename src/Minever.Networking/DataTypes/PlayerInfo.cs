using System.Text.Json.Serialization;

namespace Minever.Networking.DataTypes;

public record PlayerInfo
{
    private string _name = string.Empty;
    private string _id = string.Empty;

    [JsonPropertyName("name")]
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("id")]
    public string Id
    {
        get => _id;
        set => _id = value ?? throw new ArgumentNullException(nameof(value));
    }
}
