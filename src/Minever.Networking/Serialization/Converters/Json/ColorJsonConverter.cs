using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.Serialization.Converters.Json;

public class ColorJsonConverter : JsonConverter<Color?>
{
    private const string DefaultColorName = "reset";

    private Dictionary<string, Color> _knownMinecraftColors = new()
    {
        ["black"] = Color.FromArgb(0, 0, 0),
        ["dark_blue"] = Color.FromArgb(0, 0, 170),
        ["dark_green"] = Color.FromArgb(0, 170, 0),
        ["dark_aqua"] = Color.FromArgb(0, 170, 170),
        ["dark_red"] = Color.FromArgb(170, 0, 0),
        ["dark_purple"] = Color.FromArgb(170, 0, 170),
        ["gold"] = Color.FromArgb(255, 170, 0),
        ["gray"] = Color.FromArgb(170, 170, 170),
        ["dark_gray"] = Color.FromArgb(85, 85, 85),
        ["blue"] = Color.FromArgb(85, 85, 255),
        ["green"] = Color.FromArgb(85, 255, 85),
        ["aqua"] = Color.FromArgb(85, 255, 255),
        ["red"] = Color.FromArgb(255, 85, 85),
        ["light_purple"] = Color.FromArgb(255, 85, 255),
        ["yellow"] = Color.FromArgb(255, 255, 85),
        ["white"] = Color.FromArgb(255, 255, 255),
    };

    public override Color? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert is null)
            throw new ArgumentNullException(nameof(typeToConvert));

        var colorString = reader.GetString();

        if (colorString is null)
            return null;
        else if (_knownMinecraftColors.ContainsKey(colorString))
            return _knownMinecraftColors[colorString];
        else
            return ColorTranslator.FromHtml(colorString);
    }

    public override void Write(Utf8JsonWriter writer, Color? value, JsonSerializerOptions options)
    {
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));

        if (value is null)
            writer.WriteStringValue(DefaultColorName);
        else if (_knownMinecraftColors.ContainsValue(value.Value))
            writer.WriteStringValue(_knownMinecraftColors.Single(color => color.Value == value).Key);
        else
            writer.WriteStringValue(ColorTranslator.ToHtml(value.Value));
    }
}
