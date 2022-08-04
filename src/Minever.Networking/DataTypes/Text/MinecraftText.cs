using Minever.Networking.Serialization.Converters.Json;
using System.Drawing;
using System.Text;
using System.Text.Json.Serialization;

namespace Minever.Networking.DataTypes.Text;

[JsonConverter(typeof(JsonMinecraftTextConverter))]
public abstract class MinecraftText
{    
    private Identifier _font = new("default");

    [JsonPropertyName("bold")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsBold { get; set; }

    [JsonPropertyName("italic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsItalic { get; set; }

    [JsonPropertyName("strikethrough")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsStrikethrough { get; set; }

    [JsonPropertyName("obfuscated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsObfuscated { get; set; }

    [JsonPropertyName("font")]
    public Identifier Font
    {
        get => _font;
        set => _font = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("color")]
    [JsonConverter(typeof(JsonColorConverter))]
    public Color? Color { get; set; }

    [JsonPropertyName("insertion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TextToInsert { get; set; }      

    [JsonPropertyName("clickEvent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextClickEvent? ClickEvent { get; set; }

    [JsonPropertyName("extra")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<MinecraftText>? Siblings { get; set; }

    public sealed override string ToString()
    {
        var builder = new StringBuilder();

        BuildString(builder);

        if (Siblings is not null)
            builder.Append(string.Join("", Siblings));

        return builder.ToString();
    }

    protected abstract void BuildString(StringBuilder builder);
}
