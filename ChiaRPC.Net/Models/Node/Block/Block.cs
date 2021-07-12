using ChiaRPC.Parsers;
using System.Text.Json.Serialization;

namespace ChiaRPC.Models
{
    public sealed class FullBlock
    {
        [JsonPropertyName("header_hash")]
        [JsonConverter(typeof(HexBytesConverter))]
        public HexBytes HeaderHash { get; init; }

        [JsonPropertyName("finished_sub_slots")]
        public EndOfSubSlotBundle[] FinishedSubSlots { get; init; }

        [JsonPropertyName("foliage")]
        public Foliage Foliage { get; init; }

        [JsonConstructor]
        public FullBlock()
        {
        }
    }
}
