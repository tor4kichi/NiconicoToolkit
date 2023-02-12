using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NiconicoToolkit.User
{
    public sealed class UserDetailResponse : ResponseWithMeta
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public sealed class Data
    {
        [JsonPropertyName("user")]
        public UserDetail User { get; set; }

        [JsonPropertyName("relationships")]
        public Relationships Relationships { get; set; }
    }

    public sealed class Relationships
    {
        [JsonPropertyName("sessionUser")]
        public SessionUser SessionUser { get; set; }

        [JsonPropertyName("isMe")]
        public bool IsMe { get; set; }
    }

    public partial class SessionUser
    {
        [JsonPropertyName("isFollowing")]
        public bool IsFollowing { get; set; }
    }

    public sealed class UserDetail
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("decoratedDescriptionHtml")]
        public string DecoratedDescriptionHtml { get; set; }

        [JsonPropertyName("strippedDescription")]
        public string StrippedDescription { get; set; }

        [JsonPropertyName("isPremium")]
        public bool IsPremium { get; set; }

        [JsonPropertyName("registeredVersion")]
        public string RegisteredVersion { get; set; }

        [JsonPropertyName("followeeCount")]
        public long FolloweeCount { get; set; }

        [JsonPropertyName("followerCount")]
        public long FollowerCount { get; set; }

        [JsonPropertyName("userLevel")]
        public UserLevel UserLevel { get; set; }

        [JsonPropertyName("userChannel")]
        public UserChannel UserChannel { get; set; }

        [JsonPropertyName("isNicorepoReadable")]
        public bool IsNicorepoReadable { get; set; }

        [JsonPropertyName("sns")]
        public List<Sns> Sns { get; set; }

        [JsonPropertyName("coverImage")]
        public CoverImage CoverImage { get; set; }

        [JsonPropertyName("id")]
        public UserId Id { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("icons")]
        public Icons Icons { get; set; }
    }

    public enum SnsType { twitter, youtube, facebook, instagram };

    public sealed class Sns
    {
        [JsonPropertyName("type")]
        public SnsType Type { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("iconUrl")]
        public Uri IconUrl { get; set; }

        [JsonPropertyName("screenName")]
        public string ScreenName { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }
    }

    public sealed class CoverImage
    {
        [JsonPropertyName("ogpUrl")]
        public Uri OgpUrl { get; set; }

        [JsonPropertyName("pcUrl")]
        public Uri PcUrl { get; set; }

        [JsonPropertyName("smartphoneUrl")]
        public Uri SmartphoneUrl { get; set; }
    }

    public sealed class Icons
    {
        [JsonPropertyName("small")]
        public Uri Small { get; set; }

        [JsonPropertyName("large")]
        public Uri Large { get; set; }
    }

    public sealed class UserLevel
    {
        [JsonPropertyName("currentLevel")]
        public long CurrentLevel { get; set; }

        [JsonPropertyName("nextLevelThresholdExperience")]
        public long NextLevelThresholdExperience { get; set; }

        [JsonPropertyName("nextLevelExperience")]
        public long NextLevelExperience { get; set; }

        [JsonPropertyName("currentLevelExperience")]
        public long CurrentLevelExperience { get; set; }
    }

    public partial class UserChannel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnailUrl")]
        public Uri ThumbnailUrl { get; set; }

        [JsonPropertyName("thumbnailSmallUrl")]
        public Uri ThumbnailSmallUrl { get; set; }
    }
}
