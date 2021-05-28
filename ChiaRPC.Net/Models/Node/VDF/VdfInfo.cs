﻿using System.Text.Json.Serialization;

namespace ChiaRPC.Models
{
    public sealed class VdfInfo
    {
        [JsonPropertyName("challenge")]
        public string Challenge { get; init; }

        [JsonPropertyName("number_of_iterations")]
        public long Iterations { get; init; }

        [JsonPropertyName("output")]
        public ClassgroupElement Output { get; init; }

        public VdfInfo()
        {
        }
    }
}