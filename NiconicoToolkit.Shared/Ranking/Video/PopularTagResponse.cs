using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Ranking.Video
{
    public sealed class PopularTagResponse : ResponseWithMeta
    {
        [JsonPropertyName("data")]
        public PopularTagData Data { get; set; }
    }

    public sealed class PopularTagData
    {
        [JsonPropertyName("startAt")]
        public DateTime StartAt { get; set; }

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
    }
}
