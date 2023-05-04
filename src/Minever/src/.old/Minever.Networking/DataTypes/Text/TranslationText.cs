using System.Text;
using System.Text.Json.Serialization;

namespace Minever.Networking.DataTypes.Text;

public class TranslationText : MinecraftText
{
    private string _name = string.Empty;

    [JsonPropertyName("translate")]
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("with")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MinecraftText[]? Arguments { get; set; }

    protected override void BuildString(StringBuilder builder)
    {
        if (Name == "chat.type.text" && Arguments is not null && Arguments.Length >= 2)
            builder.Append($"<{Arguments![0]}> {Arguments![1]}");
        else if (Name == "chat.type.emote" && Arguments is not null && Arguments.Length >= 2)
            builder.Append($"* {Arguments![0]} {Arguments![1]}");
        else
            builder.Append($"[{Name}]");
    }
}
