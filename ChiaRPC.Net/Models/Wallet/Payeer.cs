using ChiaRPC.Util;
using System.Text.Json.Serialization;

namespace ChiaRPC.Models
{
    public sealed class Payeer
    {
        [JsonPropertyName("amount")]
        public ulong Amount { get; }

        [JsonPropertyName("puzzle_hash")]
        public HexBytes TargetPuzzleHash { get; }

        public Payeer(ulong amount, HexBytes targetPuzzleHash)
        {
            Amount = amount;
            TargetPuzzleHash = targetPuzzleHash;
        }
        public Payeer(ulong amount, string targetAddress)
        {
            Amount = amount;
            TargetPuzzleHash = Bech32M.AddressToPuzzleHash(targetAddress);
        }
    }
}
