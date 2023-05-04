namespace Minever.Data.Generators.Utils;

internal static class StringExtensions
{
    public static string ToPascalCase(this string @string)
    {
        var words = @string
            .Split(new[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(word => word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower());

        return string.Concat(words);
    }

    public static string ToMinecraftEdition(this string @string) =>
        @string switch
        {
            "bedrock" => "Bedrock",
            "pc"      => "Java",

            _ => throw new NotSupportedException()
        };

    public static string ToMinecraftVersion(this string @string) =>
        "V" + @string.Replace(".", ".V").Replace('-', '.').Replace("pre", "Pre").Replace("rc", "RC");
}
