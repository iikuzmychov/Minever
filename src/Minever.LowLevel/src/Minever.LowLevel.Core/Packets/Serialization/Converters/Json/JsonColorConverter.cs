using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.LowLevel.Core.Packets.Serialization.Converters.Json;

//public class JsonColorConverter : JsonConverter<Color?>
//{
//    private const string DefaultColorName = "reset";

//    private static BidirectionalDictionary<string, Color> s_knownMinecraftColors = new()
//    {
//        { "black",         Color.FromArgb(0, 0, 0)       },
//        { "dark_blue",     Color.FromArgb(0, 0, 170)     },
//        { "dark_green",    Color.FromArgb(0, 170, 0)     },
//        { "dark_aqua",     Color.FromArgb(0, 170, 170)   },
//        { "dark_red",      Color.FromArgb(170, 0, 0)     },
//        { "dark_purple",   Color.FromArgb(170, 0, 170)   },
//        { "gold",          Color.FromArgb(255, 170, 0)   },
//        { "gray",          Color.FromArgb(170, 170, 170) },
//        { "dark_gray",     Color.FromArgb(85, 85, 85)    },
//        { "blue",          Color.FromArgb(85, 85, 255)   },
//        { "green",         Color.FromArgb(85, 255, 85)   },
//        { "aqua",          Color.FromArgb(85, 255, 255)  },
//        { "red",           Color.FromArgb(255, 85, 85)   },
//        { "light_purple",  Color.FromArgb(255, 85, 255)  },
//        { "yellow",        Color.FromArgb(255, 255, 85)  },
//        { "white",         Color.FromArgb(255, 255, 255) },
//    };

//    public override Color? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        var colorString = reader.GetString();

//        if (colorString is null)
//            return null;
//        else if (s_knownMinecraftColors.TryGetValue(colorString, out var color))
//            return color;
//        else
//            return ColorTranslator.FromHtml(colorString);
//    }

//    public override void Write(Utf8JsonWriter writer, Color? value, JsonSerializerOptions options)
//    {
//        if (value is null)
//            writer.WriteStringValue(DefaultColorName);
//        else if (s_knownMinecraftColors.Inverse.TryGetValue(value.Value, out var colorName))
//            writer.WriteStringValue(colorName);
//        else
//            writer.WriteStringValue(ColorTranslator.ToHtml(value.Value));
//    }
//}
