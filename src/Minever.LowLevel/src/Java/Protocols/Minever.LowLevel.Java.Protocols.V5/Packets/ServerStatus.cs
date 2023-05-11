using Minever.LowLevel.Core;
using Minever.LowLevel.Core.Packets.Serialization.Attributes;
using Minever.LowLevel.Core.Packets.Serialization.Converters;
using Minever.LowLevel.Core.Packets.Serialization.Converters.Json;
using Minever.LowLevel.Java.Protocols.V5.Types;
using System.Text.Json.Serialization;

namespace Minever.LowLevel.Java.Protocols.V5.Packets;

[PacketConverter<PacketJsonDataConverter>]
public sealed record ServerStatus : IServerStatus
{
    private MinecraftVersion _version = new();
    private ServerPlayersInfo _playersInfo = new();
    private string _description = string.Empty;

    [JsonPropertyName("version")]
    public MinecraftVersion Version
    {
        get => _version;
        init => _version = value ?? throw new ArgumentNullException(nameof(value));
    }

    int IServerStatus.ProtocolVersion => Version.ProtocolVersion;

    [JsonPropertyName("players")]
    public ServerPlayersInfo PlayersInfo
    {
        get => _playersInfo;
        init => _playersInfo = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("description")]
    public string Description
    {
        get => _description;
        init => _description = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonPropertyName("favicon")]
    [JsonConverter(typeof(JsonIconBase64Converter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? IconBytes { get; init; }

    public ServerStatus() { }

    public ServerStatus(MinecraftVersion version, ServerPlayersInfo playersInfo, string desription, byte[]? iconBytes = null)
    {
        Version = version;
        PlayersInfo = playersInfo;
        Description = desription;
        IconBytes = iconBytes;
    }
}
