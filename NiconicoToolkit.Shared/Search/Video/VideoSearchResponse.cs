using NiconicoToolkit.Video;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Search.Video
{
    public sealed class VideoSearchResponse : ResponseWithMeta
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public sealed class Data
    {
        [JsonPropertyName("hasNext")]
        public bool HasNext { get; set; }

        [JsonPropertyName("items")]
        public NvapiVideoItem[] Items { get; set; }
    }
}
