using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.SnapshotSearch.JsonFilters
{
    public sealed class JsonFilterDataJsonConverter : JsonConverter<IJsonSearchFilterData>
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeof(IJsonSearchFilterData).IsAssignableFrom(typeToConvert);

        public override IJsonSearchFilterData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument document = JsonDocument.ParseValue(ref reader);

            var type = document.RootElement.GetProperty("type").GetString();
            switch (type)
            {
                case "not" :
                    {
                        NotJsonFilterData not = new();
                        var filter = document.RootElement.GetProperty("filter");
                        not.Filter = filter.ToObject<IJsonSearchFilterData>(options);
                        return not;
                    }
                case "or" :
                    {
                        OrJsonFilterData or = new() { Filters = new List<IJsonSearchFilterData>() };
                        var filters = document.RootElement.GetProperty("filters");
                        foreach (var filterJson in filters.EnumerateArray())
                        {
                            or.Filters.Add(filterJson.ToObject<IJsonSearchFilterData>(options));
                        }
                        return or;
                    }
                case "and" :
                    {
                        AndJsonFilterData and = new() { Filters = new List<IJsonSearchFilterData>() };
                        var filters = document.RootElement.GetProperty("filters");
                        foreach (var filterJson in filters.EnumerateArray())
                        {
                            and.Filters.Add(filterJson.ToObject<IJsonSearchFilterData>(options));
                        }
                        return and;
                    }
                case "range" : 
                    return document.RootElement.ToObject<RangeJsonFilterData>(); 

                case "equal" :
                    return document.RootElement.ToObject<EqualJsonFilterData>();

                default:
                    throw new JsonException();
            };
        }



        public override void Write(Utf8JsonWriter writer, IJsonSearchFilterData value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case NotJsonFilterData not:
                    writer.WriteStartObject();
                    writer.WriteString("type", value.Type);
                    writer.WritePropertyName("filter");
                    JsonSerializer.Serialize(writer, not.Filter, options);
                    writer.WriteEndObject();
                    break;
                case OrJsonFilterData or:
                    writer.WriteStartObject();
                    writer.WriteString("type", value.Type);
                    writer.WriteStartArray("filters");
                    foreach (var filter in or.Filters)
                    {
                        JsonSerializer.Serialize(writer, filter, options);
                    }
                    writer.WriteEndArray();
                    writer.WriteEndObject();
                    break;
                case AndJsonFilterData and:
                    writer.WriteStartObject();
                    writer.WriteString("type", value.Type);
                    writer.WriteStartArray("filters");
                    foreach (var filter in and.Filters)
                    {
                        JsonSerializer.Serialize(writer, filter, options);
                    }
                    writer.WriteEndArray();
                    writer.WriteEndObject();
                    break;
                case RangeJsonFilterData range:
                    JsonSerializer.Serialize(writer, range, JsonFilterSerializeHelper.SerializerOptionsOnWriterValue);
                    break;
                case EqualJsonFilterData equal:
                    JsonSerializer.Serialize(writer, equal, JsonFilterSerializeHelper.SerializerOptionsOnWriterValue);
                    break;
            };
        }
    }
}
