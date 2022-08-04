using Minever.Networking.Serialization.Converters.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Minever.Networking.DataTypes;

/// <summary>
/// Provides information about Minecraft namespaced location.
/// </summary>
[JsonConverter(typeof(JsonIdentifierConverter))]
public record Identifier
{
    private string _namespace = string.Empty;
    private string _name      = string.Empty;

    /// <summary>
    /// Get or sets the namespace. Only characters 01​​234​5​6​78​9abcdefghijklmnopqrstuvwxyz-_ shoud be used.
    /// </summary>
    public string Namespace
    {
        get => _namespace;
        set
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!Regex.IsMatch(value, @"^[0-9a-z_-]*$"))
                throw new FormatException("Namespace should only use 01​​234​5​6​78​9abcdefghijklmnopqrstuvwxyz-_ characters.");

            _namespace = value;
        }
    }

    /// <summary>
    /// Gets or sets the name. Only characters 01​​234​5​6​78​9abcdefghijklmnopqrstuvwxyz-_./ shoud be used.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!Regex.IsMatch(value, @"^[0-9a-z_\-./]*$"))
                throw new FormatException("Namespace should only use 01​​234​5​6​78​9abcdefghijklmnopqrstuvwxyz-_./ characters.");

            _name = value;
        }
    }

    /// <summary>
    /// Initialize a new instance of <see cref="Identifier"/> record with a specified namespace and name.
    /// </summary>
    /// <param name="namespace">The namespace. Only characters 01​​234​5​6​78​9abcdefghijklmnopqrstuvwxyz-_ shoud be used.</param>
    /// <param name="name">The name. Only characters 01​​234​5​6​78​9abcdefghijklmnopqrstuvwxyz-_./ shoud be used.</param>
    public Identifier(string @namespace, string name)
    {
        Namespace = @namespace;
        Name      = name;
    }

    /// <summary>
    /// Initialize a new instance of <see cref="Identifier"/> record with a 'minecarft' namespace and specified name.
    /// </summary>
    /// <param name="name">The name. Only characters 01​​234​5​6​78​9abcdefghijklmnopqrstuvwxyz-_./ shoud be used.</param>
    public Identifier(string name) : this("minecraft", name) { }

    /// <summary>
    /// Converts a string representation of Minecraft identifier to a new instance of <see cref="Identifier"/> record.
    /// </summary>
    /// <param name="string">The string representation of Minecraft identifier, must match the pattern 'namespace:name'.</param>
    /// <returns>The Minecraft identifier equialent of <paramref name="string"/>.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    public static Identifier Parse(string @string)
    {
        if (@string is null)
            throw new ArgumentNullException(nameof(@string));

        var splited = @string.Split(':');

        if (splited.Length != 2)
            throw new FormatException("String must match the pattern 'namespace:name'.");

        return new Identifier(splited[0], splited[1]);
    }

    /// <summary>
    /// Converts a string representation of Minecraft identifier to a new instance of <see cref="Identifier"/> record.
    /// A return value indicates whether the conversation successed.
    /// </summary>
    /// <param name="string">The string representation of Minecraft identifier, must match the pattern 'namespace:name'.</param>
    /// <param name="result">The Minecraft identifier equialent of <paramref name="string"/>. <see langword="null"/> if conversation failed.</param>
    /// <returns><see langword="true"/> if <paramref name="string"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string @string, out Identifier? result)
    {
        try
        {
            result = Parse(@string);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public override string ToString() => $"{Namespace}:{Name}";
}