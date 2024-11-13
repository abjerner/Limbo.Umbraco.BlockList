using System;
using Limbo.Umbraco.BlockList.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

namespace Limbo.Umbraco.BlockList.Json.Newtonsoft;

internal class BlockListJsonConverter : JsonConverter {

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {

        if (value is BlockListTypeConverter converter && !string.IsNullOrWhiteSpace(converter.Type)) {
            new JObject { { "type", converter.Type } }.WriteTo(writer);
            return;
        }

        writer.WriteNull();

    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
        switch (reader.TokenType) {
            case JsonToken.Null:
                return null;
            case JsonToken.String: {
                string? type = reader.Value as string;
                return string.IsNullOrWhiteSpace(type) ? null : new BlockListTypeConverter(type);
            }
            case JsonToken.StartObject: {
                JObject json = JObject.Load(reader);
                if (json.TryGetString("key", out string? key)) return new BlockListTypeConverter(key);
                if (json.TryGetString("type", out string? type)) return new BlockListTypeConverter(type);
                return null;
            }
            default:
                throw new Exception($"Unsupported token type '{reader.TokenType}'...");
        }
    }

    public override bool CanConvert(Type objectType) {
        return false;
    }

}