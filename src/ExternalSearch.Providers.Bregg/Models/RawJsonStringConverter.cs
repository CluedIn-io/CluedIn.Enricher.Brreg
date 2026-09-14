using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
    internal class RawJsonStringConverter : JsonConverter<string>
    {
        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return null;
                case JsonToken.String:
                    return (string)reader.Value;
                case JsonToken.Boolean:
                    return Convert.ToString(reader.Value, System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();
                default:
                    return JToken.Load(reader).ToString(Formatting.None);
            }
        }

        public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
