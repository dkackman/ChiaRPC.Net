using ChiaRPC.Parsers;
using System;
using System.Text.Json.Serialization;

namespace ChiaRPC.Models
{
    public sealed class TransactionRecord
    {
        [JsonPropertyName("confirmed_at_height")]
        public uint ConfirmedAtHeight { get; init; }

        [JsonPropertyName("created_at_time")]
        [JsonConverter(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset CreatedAt { get; init; }

        [JsonPropertyName("to_puzzle_hash")]
        [JsonConverter(typeof(HexBytesConverter))]
        public HexBytes ToPuzzleHash { get; init; }

        [JsonPropertyName("amount")]
        public ulong Amount { get; init; }

        [JsonPropertyName("fee_amount")]
        public ulong Fee { get; init; }

        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; init; }

        [JsonPropertyName("sent")]
        public uint Sent { get; init; }

        [JsonPropertyName("additions")]
        public Coin[] Additions { get; init; }

        [JsonPropertyName("removals")]
        public Coin[] Removals { get; init; }

        [JsonPropertyName("wallet_id")]
        public uint WalletId { get; init; }

        [JsonPropertyName("type")]
        public uint Type { get; init; }

        /// <summary>
        /// The name of the transaction. This is also used as the unique identifier.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonConverter(typeof(HexBytesConverter))]
        public HexBytes Name { get; init; }

        public TransactionRecord()
        {
        }
    }
}
