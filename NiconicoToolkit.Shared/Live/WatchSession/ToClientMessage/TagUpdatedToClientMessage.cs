using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Live.WatchSession.ToClientMessage
{
    internal sealed class TagUpdated_WatchSessionToClientMessage : WatchServerToClientMessage
    {
        [JsonPropertyName("tags")]
        public TagUpdated_Tags Tags { get; set; }
    }

    public class TagUpdated_Tags
    {
        [JsonPropertyName("items")]
        public TagUpdated_TagItem[] Items { get; set; }

        [JsonPropertyName("ownerLocked")]
        public bool OwnerLocked { get; set; }
    }

    public class TagUpdated_TagItem
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("locked")]
        public bool Locked { get; set; }

        [JsonPropertyName("reserved")]
        public bool Reserved { get; set; }

        [JsonPropertyName("nicopediaArticleUrl")]
        public Uri NicopediaArticleUrl { get; set; }
    }
}
