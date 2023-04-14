using Minever.Networking.Serialization.Json;
using System.Text.Json.Serialization;

namespace Minever.Java.Protocols.V5.Types;

public sealed record PlayerInfo
{
    private string _name = string.Empty;

    [JsonPropertyName("name")]
    public string Name
    {
        get => _name;
        init => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("id")]
    [JsonConverter(typeof(JsonGuidConverter))]
    public Guid Uuid { get; init; }

    public PlayerInfo() { }

    public PlayerInfo(string name, Guid uuid)
    {
        Name = name;
        Uuid = uuid;
    }
}
