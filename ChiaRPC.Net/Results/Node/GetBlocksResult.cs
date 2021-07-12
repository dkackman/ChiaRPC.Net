using ChiaRPC.Models;
using System.Text.Json.Serialization;

namespace ChiaRPC.Results
{
    internal sealed class GetBlocksResult : ChiaResult
    {
        [JsonPropertyName("blocks")]
        public FullBlock[] Blocks { get; init; }

        [JsonConstructor]
        public GetBlocksResult()
        {
        }
    }
}
