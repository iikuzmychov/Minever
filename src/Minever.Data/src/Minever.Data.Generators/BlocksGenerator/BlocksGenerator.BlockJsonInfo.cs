using Newtonsoft.Json;

namespace Minever.Data.Generators;

public partial class BlocksGenerator
{
    internal class BlockJsonInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = default!;

        [JsonProperty("displayName")]
        public string DisplayName { get; set; } = default!;
    }
}