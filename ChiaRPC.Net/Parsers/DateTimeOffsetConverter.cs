using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChiaRPC.Parsers
{
    public sealed class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            double timestamp = reader.GetDouble();
            return DateTimeOffset.FromUnixTimeMilliseconds((long)(timestamp * 1000));
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToUnixTimeSeconds());
        }
    }
}
