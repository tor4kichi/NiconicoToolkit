using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.FollowingsActivity;

public class Activity
{
    [JsonPropertyName("sensitive")]
    public bool Sensitive { get; set; }

    [JsonPropertyName("message")]
    public Message Message { get; set; }

    [JsonPropertyName("thumbnailUrl")]
    public string ThumbnailUrl { get; set; }

    [JsonPropertyName("label")]
    public Label Label { get; set; }

    [JsonPropertyName("content")]
    public Content Content { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("actor")]
    public Actor Actor { get; set; }

    // マイリスト登録時に入ってくる
    [JsonPropertyName("relatedLinks")]
    public List<RelatedLink> RelatedLinks { get; set; }
}

public class Actor
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("iconUrl")]
    public string IconUrl { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("isLive")]
    public bool IsLive { get; set; }

    public const string ActorType_User = "user";
    public const string ActorType_Channel = "channel";
}

public class Content
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("startedAt")]
    public DateTime StartedAt { get; set; }

    [JsonPropertyName("video")]
    public Video? Video { get; set; }

    [JsonPropertyName("program")]
    public Program? Program { get; set; }


    public const string ContentType_Video = "video";
    public const string ContentType_LiveProgram = "program";
}


public class Label
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class Message
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class RelatedLink
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("subType")]
    public string SubType { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; }
}

public class FollowingsActivityResponse
{
    [JsonPropertyName("activities")]
    public List<Activity> Activities { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("impressionId")]
    public string ImpressionId { get; set; }

    [JsonPropertyName("nextCursor")]
    public string NextCursor { get; set; }


    public bool IsOk => Code == "ok";
}

public class Video
{
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
}


public class Program
{
    [JsonPropertyName("statusCode")]
    public string StatusCode { get; set; }

    [JsonPropertyName("providerType")]
    public string ProviderType { get; set; }

    public const string StatusCode_Timeshift = "TIMESHIFT";

    public const string ProviderType_User = "USER";
}
