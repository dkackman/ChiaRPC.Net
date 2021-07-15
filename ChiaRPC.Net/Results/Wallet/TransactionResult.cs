using ChiaRPC.Models;
using ChiaRPC.Parsers;
using System.Text.Json.Serialization;

namespace ChiaRPC.Results
{
    internal class TransactionResult : ChiaResult
    {
        [JsonPropertyName("transaction")]
        public TransactionRecord Transaction { get; init; }

        [JsonPropertyName("transaction_id")]
        [JsonConverter(typeof(HexBytesConverter))]
        public HexBytes TransactionId { get; init; }

        public TransactionResult()
        {
        }
    }
}
