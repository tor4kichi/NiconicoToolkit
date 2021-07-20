using NiconicoToolkit.Video;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace System.Text.Json.Serialization
{
    public class PlaybackPositionConverter : JsonConverter<PlaybackPosition>
    {
        public override PlaybackPosition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                TimeSpan? time = reader.TokenType switch
                {
                    JsonTokenType.Number => TimeSpan.FromSeconds(reader.GetInt32()),
                    JsonTokenType.String => throw new JsonException(reader.GetString()),
                    _ => throw new JsonException(),
                };

                return new PlaybackPosition() { Position = time };
            }
            catch
            {
                return new PlaybackPosition();
            }
        }

        public override void Write(Utf8JsonWriter writer, PlaybackPosition value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
