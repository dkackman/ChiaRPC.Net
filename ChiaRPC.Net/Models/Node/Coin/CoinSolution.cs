using ChiaRPC.Parsers;
using System.Text.Json.Serialization;

namespace ChiaRPC.Models
{
    public sealed class CoinSolution
    {
        [JsonPropertyName("coin")]
        public Coin Coin { get; init; }

        [JsonPropertyName("puzzle_reveal")]
        [JsonConverter(typeof(HexBytesConverter))]
        public HexBytes PuzzleReveal { get; init; }

        [JsonPropertyName("solution")]
        [JsonConverter(typeof(HexBytesConverter))]
        public HexBytes Solution { get; init; }

        public CoinSolution()
        {
        }
    }
}
