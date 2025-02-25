using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Ranking.Video
{
    public sealed class HotTopicResponse : ResponseWithMeta
    {
        [JsonPropertyName("data")]
        public HotTopicData Data { get; set; }
    }

    public sealed class HotTopicData
    {
        [JsonPropertyName("startAt")]
        public DateTime StartAt { get; set; }

        [JsonPropertyName("hotTopics")]
        public HotTopic[] HotTopics { get; set; }
    }

    public sealed class HotTopic
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("conditions")]
        public HotTopicCondition[] Conditions { get; set; }
    }

    public sealed class HotTopicCondition
    {
        [JsonPropertyName("genre")]
        public HotTopicGenre Genre { get; set; }

        [JsonPropertyName("tag")]
        public string Tag { get; set; }
    }

    public sealed class HotTopicGenre
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }
    }
}
