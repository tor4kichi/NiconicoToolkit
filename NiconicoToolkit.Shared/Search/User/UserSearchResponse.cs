using NiconicoToolkit.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Search.User
{
    public sealed class UserSearchResponse : ResponseWithMeta
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public sealed class Data
    {
        [JsonPropertyName("hasNext")]
        public bool HasNext { get; set; }

        [JsonPropertyName("items")]
        public UserSearchItem[] Items { get; set; }

        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }

        [JsonPropertyName("totalCount")]
        public string TotalCount { get; set; }
    }

    public sealed class UserSearchItem
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public UserId Id { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("strippedDescription")]
        public string StrippedDescription { get; set; }

        [JsonPropertyName("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonPropertyName("followerCount")]
        public int FollowerCount { get; set; }

        [JsonPropertyName("videoCount")]
        public int VideoCount { get; set; }

        [JsonPropertyName("liveCount")]
        public int LiveCount { get; set; }

        [JsonPropertyName("isPremium")]
        public bool IsPremium { get; set; }

        [JsonPropertyName("icons")]
        public Icons Icons { get; set; }

        [JsonPropertyName("relationships")]
        public Relationships Relationships { get; set; }
    }

    public sealed class Icons
    {
        [JsonPropertyName("small")]
        public Uri Small { get; set; }

        [JsonPropertyName("large")]
        public Uri Large { get; set; }
    }

    public sealed class Relationships
    {
        [JsonPropertyName("sessionUser")]
        public SessionUser SessionUser { get; set; }
    }

    public sealed class SessionUser
    {
        [JsonPropertyName("isFollowing")]
        public bool IsFollowing { get; set; }
    }
}
