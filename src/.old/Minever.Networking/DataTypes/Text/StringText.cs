using System.Text;
using System.Text.Json.Serialization;

namespace Minever.Networking.DataTypes.Text;

public class StringText : MinecraftText
{
    private string _text = string.Empty;

    [JsonPropertyName("text")]
    public string Text
    {
        get => _text;
        set => _text = value ?? throw new ArgumentNullException(nameof(value));
    }

    public StringText() { }

    public StringText(string text)
    {
        Text = text;
    }

    protected override void BuildString(StringBuilder builder)
    {
        builder.Append(Text);
    }
}
