using ChiaRPC.Parsers;
using System.Net;
using System.Text.Json.Serialization;

namespace ChiaRPC.Models
{
    public sealed class ChiaConnection
    {
        [JsonPropertyName("node_id")]
        [JsonConverter(typeof(HexBytesConverter))]
        public HexBytes NodeId { get; init; }

        [JsonPropertyName("bytes_read")]
        public ulong BytesRead { get; init; }

        [JsonPropertyName("bytes_written")]
        public ulong BytesWritten { get; init; }

        [JsonPropertyName("peer_host")]
        [JsonConverter(typeof(IPAddressConverter))]
        public IPAddress PeerHost { get; init; }

        [JsonPropertyName("peer_port")]
        public ushort PeerPort { get; init; }

        [JsonPropertyName("peer_server_port")]
        public ushort PeerServerPort { get; init; }

        [JsonPropertyName("local_port")]
        public ushort LocalPort { get; init; }

        [JsonPropertyName("type")]
        public int Type { get; init; }

        [JsonPropertyName("last_message_time")]
        [JsonConverter(typeof(DateTimeOffsetConverter))]
        public DateTimeOffsetConverter LastMessageTime { get; init; }

        [JsonConstructor]
        public ChiaConnection()
        {
        }
    }
}
