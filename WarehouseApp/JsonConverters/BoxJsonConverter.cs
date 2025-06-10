using System.Text.Json;
using System.Text.Json.Serialization;
using Entities;

namespace JsonConverters;
public class BoxJsonConverter : JsonConverter<Box>
{
    public override Box Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        Guid id = Guid.NewGuid();
        double height = 0, width = 0, depth = 0, weight = 0;
        string? dateOfProduction = null;
        string? expirationDate = null;
        bool idRead = false;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                if (dateOfProduction == null)
                {
                    throw new JsonException("DateOfProduction is required.");
                }

                if (expirationDate == null)
                {
                    return idRead
                       ? new Box(id, height, width, depth, weight, dateOfProduction)
                       : new Box(height, width, depth, weight, dateOfProduction);
                }
                else
                {
                    return idRead
                        ? new Box(id, height, width, depth, weight, dateOfProduction, expirationDate)
                        : new Box(height, width, depth, weight, dateOfProduction, expirationDate);
                }
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string? propertyName = reader.GetString()?.ToLowerInvariant();
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
                    case "weight":
                        weight = reader.GetDouble();
                        break;
                    case "dateofproduction":
                        dateOfProduction = reader.GetString();
                        break;
                    case "expirationdate":
                        expirationDate = reader.GetString();
                        break;
                    case "volume":
                        reader.Skip();
                        break;
                }
            }
        }
        throw new JsonException("JSON payload is incomplete.");
    }

    public override void Write(Utf8JsonWriter writer, Box box, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("id", box.Id);
        writer.WriteNumber("height", box.Height);
        writer.WriteNumber("width", box.Width);
        writer.WriteNumber("depth", box.Depth);
        writer.WriteNumber("weight", box.Weight);
        writer.WriteNumber("volume", box.Volume);

        if (box.DateOfProduction.HasValue)
        {
            writer.WriteString("dateOfProduction", box.DateOfProduction.Value.ToString("dd.MM.yyyy"));
        }
        writer.WriteString("expirationDate", box.ExpirationDate.ToString("dd.MM.yyyy"));

        writer.WriteEndObject();
    }
}