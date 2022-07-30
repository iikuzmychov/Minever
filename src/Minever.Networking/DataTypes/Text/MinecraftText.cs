using Minever.Networking.Serialization.Converters.Json;
using System.Drawing;
using System.Text.Json.Serialization;

namespace Minever.Networking.DataTypes.Text;

[JsonConverter(typeof(MinecraftTextJsonConverter))]
public abstract class MinecraftText
{    
    private Identifier _font = new("default");

    [JsonPropertyName("bold")]
    public bool IsBold { get; set; }

    [JsonPropertyName("italic")]
    public bool IsItalic { get; set; }

    [JsonPropertyName("strikethrough")]
    public bool IsStrikethrough { get; set; }

    [JsonPropertyName("obfuscated")]
    public bool IsObfuscated { get; set; }

    [JsonPropertyName("font")]
    public Identifier Font
    {
        get => _font;
        set => _font = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("color")]
    [JsonConverter(typeof(ColorJsonConverter))]
    public Color? Color { get; set; }

    [JsonPropertyName("insertion")]
    public string? Insertion { get; set; }

    [JsonPropertyName("extra")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<MinecraftText>? Siblings { get; set; }
}
