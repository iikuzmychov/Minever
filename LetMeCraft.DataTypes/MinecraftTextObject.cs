namespace LetMeCraft.DataTypes;

public class MinecraftTextObject
{
    private string _text = string.Empty;

    public string Text
    {
        get => _text;
        set => _text = value ?? throw new ArgumentNullException(nameof(value));
    }

}
