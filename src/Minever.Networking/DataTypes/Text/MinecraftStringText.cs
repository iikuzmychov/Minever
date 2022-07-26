using System.Text.Json.Serialization;

namespace Minever.Networking.DataTypes.Text;

public class MinecraftStringText : MinecraftText
{
    private string _text = string.Empty;

    [JsonPropertyName("text")]
    public string Text
    {
        get => _text;
        set => _text = value ?? throw new ArgumentNullException(nameof(value));
    }

    public MinecraftStringText() { }

    public MinecraftStringText(string text)
    {
        Text = text;
    }

    public override string ToString() => Text;
}
