using ChiaRPC.Parsers;
using System.Text.Json.Serialization;

namespace ChiaRPC.Models
{
    public sealed class ChallengeChainSubSlot
    {
        [JsonPropertyName("challenge_chain_end_of_slot_vdf")]
        public VdfInfo ChallengeChainEndOfSlotVdf { get; init; }

        [JsonPropertyName("infused_challenge_chain_sub_slot_hash")]
        [JsonConverter(typeof(HexBytesConverter))]
        public HexBytes InfusedChallengeChainSubSlotHash { get; init; }

        [JsonPropertyName("subepoch_summary_hash")]
        [JsonConverter(typeof(HexBytesConverter))]
        public HexBytes SubepochSummaryHash { get; init; }

        [JsonPropertyName("new_sub_slot_iters")]
        public ulong NewSubSlotIters { get; init; }

        [JsonPropertyName("new_difficulty")]
        public ulong NewDifficulty { get; init; }

        public ChallengeChainSubSlot()
        {
        }
    }
}
