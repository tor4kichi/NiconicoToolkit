using NiconicoToolkit.Video;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Ranking.Video
{
    public sealed class VideoRankingResponse : ResponseWithMeta
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public sealed class Data
    {
        [JsonPropertyName("items")]
        public List<NvapiVideoItem> Items { get; set; }

        [JsonPropertyName("hasNext")]
        public bool HasNext { get; set; }
    }
}
