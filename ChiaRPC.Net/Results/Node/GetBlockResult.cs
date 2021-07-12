using ChiaRPC.Models;
using System.Text.Json.Serialization;

namespace ChiaRPC.Results
{
    internal sealed class GetBlockResult : ChiaResult
    {
        [JsonPropertyName("block")]
        public FullBlock Block { get; init; }

        [JsonConstructor]
        public GetBlockResult()
        {
        }
    }
}
