using System.Text.Json.Serialization;

namespace Minever.Networking.DataTypes;

/// <summary>
/// Provides information about Minecraft version.
/// </summary>
public record MinecraftVersion
{
    private string _name = string.Empty;

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets version of protocol used by the current version of Minecraft.
    /// </summary>
    [JsonPropertyName("protocol")]
    public int ProtocolVersion { get; set; }

    public MinecraftVersion() { }

    public MinecraftVersion(string name, int protocolVersion)
    {
        Name            = name;
        ProtocolVersion = protocolVersion;
    }
}
