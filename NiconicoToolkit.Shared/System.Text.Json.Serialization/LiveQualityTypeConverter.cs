using NiconicoToolkit.Live.WatchSession;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.Text.Json.Serialization
{
    internal class LiveQualityTypeConverter : JsonConverter<LiveQualityLimitType>
    {
        public override LiveQualityLimitType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string type = reader.GetString();
            return type switch
            {
                "super_low" => LiveQualityLimitType.SuperLow,
                "low" => LiveQualityLimitType.Low,
                "normal" => LiveQualityLimitType.Normal,
                "high" => LiveQualityLimitType.High,
                "super_high" => LiveQualityLimitType.SuperHigh,
                "6Mbps1080p30fps" => LiveQualityLimitType._6Mbps1080p30fps,
                _ => LiveQualityLimitType.SuperHigh,
            };
        }

        public override void Write(Utf8JsonWriter writer, LiveQualityLimitType value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
