using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Minever.Networking.DataTypes.Text;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum TextClickAction
{
    [EnumMember(Value = "open_url")]
    OpenUrl,
    
    [EnumMember(Value = "open_file")]
    OpenFile,

    [EnumMember(Value = "run_command")]    
    SendTextToChat,

    [EnumMember(Value = "twitch_user_info")]
    ShowTwitchUserInfo,

    [EnumMember(Value = "suggest_command")]
    SetChatInputText,
    
    [EnumMember(Value = "change_page")]
    OpenBookPage,
    
    [EnumMember(Value = "copy_to_clipboard")]
    CopyText,
}

public class TextClickEvent
{
    private string _value = string.Empty;

    [JsonPropertyName("action")]
    public TextClickAction Action { get; set; }
    
    [JsonPropertyName("value")]
    public string Value
    {
        get => _value;
        set => _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public TextClickEvent() { }

    public TextClickEvent(TextClickAction action, string value)
    {
        Action = action;
        Value  = value ?? throw new ArgumentNullException(nameof(value));
    }
}
