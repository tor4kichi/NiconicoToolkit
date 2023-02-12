using NiconicoToolkit.Mylist;
using NiconicoToolkit.Search.User;
using NiconicoToolkit.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Search.List
{
    public sealed class ListSearchResponse : ResponseWithMeta
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public sealed class Data
    {
        [JsonPropertyName("hasNext")]
        public bool HasNext { get; set; }
        
        [JsonPropertyName("items")]
        public ListItem[] Items { get; set; }

        [JsonPropertyName("searchId")]
        public string SearchId { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
    }

    public sealed class ListItem
    {
        [JsonPropertyName("type")]
        public ListType Type { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnailUrl")]
        public Uri ThumbnailUrl { get; set; }

        [JsonPropertyName("videoCount")]
        public int VideoCount { get; set; }

        [JsonPropertyName("owner")]
        public Owner Owner { get; set; }
    }

    public sealed class Owner
    {
        [JsonPropertyName("ownerType")]
        public string OwnerType { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("visibility")]
        public Visibility Visibility { get; set; }

        [JsonPropertyName("id")]
        public UserId Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("iconUrl")]
        public Uri IconUrl { get; set; }
    }

    public enum Visibility { visible, hidden };

    public enum ListType { mylist, series };
}
