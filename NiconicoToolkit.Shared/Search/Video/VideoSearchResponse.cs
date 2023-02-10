using NiconicoToolkit.Channels;
using NiconicoToolkit.Ranking.Video;
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

        [JsonPropertyName("keyword")]
        public string Keyword { get; set; }

        [JsonPropertyName("tag")]
        public string Tag { get; set; }

        [JsonPropertyName("genres")]
        public Genre[] Genres { get; set; }

        [JsonPropertyName("searchId")]
        public string SearchId { get; set; }

        [JsonPropertyName("totalCount")]
        public string TotalCount { get; set; }

        [JsonPropertyName("additionals")]
        public Additionals Additionals { get; set; }
    }

    public sealed class Additionals
    {
        [JsonPropertyName("tags")]
        public Tag[] Tags { get; set; }
    }

    public sealed class Tag
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
