// [CHANGE: Umbraco 17 upgrade] Related: see documentation/umbraco-17-upgrade.md

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Limbo.Umbraco.BlockList.Models;

namespace Limbo.Umbraco.BlockList.Json;

/// <summary>
/// JSON converter for reading and writing <see cref="BlockListTypeConverter"/>.
/// </summary>
/// <remarks>
/// Umbraco moved from Newtonsoft.Json to System.Text.Json in Umbraco 14, so this replaces the former
/// <c>Limbo.Umbraco.BlockList.Json.Newtonsoft.BlockListJsonConverter</c>. Reading is deliberately lenient so
/// configurations saved by older versions of the package still work - both a plain string, an object with a
/// <c>type</c> property, and the legacy object with a <c>key</c> property are supported. Writing is always
/// normalized to <c>{ "type": "..." }</c>.
/// </remarks>
internal class BlockListTypeConverterJsonConverter : JsonConverter<BlockListTypeConverter?> {

    public override BlockListTypeConverter? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

        switch (reader.TokenType) {

            case JsonTokenType.Null:
                return null;

            case JsonTokenType.String: {
                string? type = reader.GetString();
                return string.IsNullOrWhiteSpace(type) ? null : new BlockListTypeConverter(type);
            }

            case JsonTokenType.StartObject: {

                string? type = null;
                string? key = null;

                while (reader.Read()) {

                    if (reader.TokenType == JsonTokenType.EndObject) break;

                    if (reader.TokenType != JsonTokenType.PropertyName) continue;

                    string propertyName = reader.GetString()!;

                    reader.Read();

                    switch (propertyName) {

                        case "type":
                            if (reader.TokenType == JsonTokenType.String) {
                                type = reader.GetString();
                            } else {
                                // The value isn't a string, so it must still be skipped - otherwise the reader would
                                // continue inside a nested object or array, and its "EndObject" token would end the
                                // loop prematurely
                                reader.Skip();
                            }
                            break;

                        case "key":
                            if (reader.TokenType == JsonTokenType.String) {
                                key = reader.GetString();
                            } else {
                                reader.Skip();
                            }
                            break;

                        default:
                            reader.Skip();
                            break;

                    }

                }

                string? value = string.IsNullOrWhiteSpace(key) ? type : key;

                return string.IsNullOrWhiteSpace(value) ? null : new BlockListTypeConverter(value);

            }

            default:
                throw new JsonException($"Unsupported token type '{reader.TokenType}'...");

        }

    }

    public override void Write(Utf8JsonWriter writer, BlockListTypeConverter? value, JsonSerializerOptions options) {

        if (value is null || string.IsNullOrWhiteSpace(value.Type)) {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("type", value.Type);
        writer.WriteEndObject();

    }

}
