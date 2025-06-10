using System.Text.Json;
using System.Text.Json.Serialization;
using Entities;

namespace JsonConverters;
public class PalletJsonConverter : JsonConverter<Pallet>
{
    public override Pallet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        Guid id = Guid.NewGuid();
        double height = 0, width = 0, depth = 0;
        List<Box> boxes = new List<Box>();
        bool idRead = false;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                var pallet = idRead
                    ? new Pallet(id, height, width, depth)
                    : new Pallet(height, width, depth);

                foreach (var box in boxes)
                {
                    pallet.AddBox(box);
                }
                return pallet;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString()?.ToLowerInvariant();
                reader.Read();

                switch (propertyName)
                {
                    case "id":
                        id = reader.GetGuid();
                        idRead = true;
                        break;
                    case "height":
                        height = reader.GetDouble();
                        break;
                    case "width":
                        width = reader.GetDouble();
                        break;
                    case "depth":
                        depth = reader.GetDouble();
                        break;
                    case "boxes":
                        var deserializedBoxes = JsonSerializer.Deserialize<List<Box>>(ref reader, options);
                        if (deserializedBoxes != null)
                        {
                            boxes.AddRange(deserializedBoxes);
                        }
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }
        throw new JsonException("JSON payload is incomplete");
    }

    public override void Write(Utf8JsonWriter writer, Pallet pallet, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("id", pallet.Id);
        writer.WriteNumber("height", pallet.Height);
        writer.WriteNumber("width", pallet.Width);
        writer.WriteNumber("depth", pallet.Depth);
        writer.WriteNumber("weight", pallet.Weight);

        writer.WritePropertyName("boxes");
        JsonSerializer.Serialize(writer, pallet.Boxes, options);

        writer.WriteEndObject();
    }
}