using NiconicoToolkit.User;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.FollowingsActivity;

public enum ActivityType
{
    [Description("publish")]
    Publish, // コンテンツ投稿
    [Description("video")]
    Video,   // 動画投稿
    [Description("live")]
    Live,    // 生放送開始
    [Description("all")]
    All,     // 全部
}

public enum ActivityActorType
{
    [Description("users")]
    User,
    [Description("channels")]
    Channel,
}

public readonly struct NiconicoActivityActor
{
    public NiconicoActivityActor(ActivityActorType actorType, NiconicoId actorId)
    {
        ActorType = actorType;
        ActorId = actorId;
    }
    public readonly ActivityActorType ActorType;
    public readonly NiconicoId ActorId;
}


public sealed class FollowingsActivityClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _options;

    internal FollowingsActivityClient(NiconicoContext context, JsonSerializerOptions options)
    {
        _context = context;
        _options = options;
    }

    internal static class Urls
    {
        public const string FollowingsActivityApiUrl = $"https://api.feed.nicovideo.jp/v1/activities/followings/";
    }

    /// <remarks>[Require Login]</remarks>
    [RequireLogin]
    public Task<FollowingsActivityResponse> GetFollowingsActivityAsync(ActivityType type, NiconicoActivityActor? filteringActor, string? nextCursor, CancellationToken ct = default)
    {
        NameValueCollection dict = new()
        {
            { "context", "my_timeline"}
        };
        dict.AddIfNotNull("cursor", nextCursor);
        var url = new StringBuilder(Urls.FollowingsActivityApiUrl)
            .Append(filteringActor is { } filter ? $"{filter.ActorType.GetDescription()}/{filter.ActorId}/" : "")
            .Append(type.GetDescription())                    
            .AppendQueryString(dict)
            .ToString();
        return _context.GetJsonAsAsync<FollowingsActivityResponse>(url, _options);
    }

    public async Task<bool> ReadActivityAsync(NiconicoActivityActor actor, CancellationToken ct = default)
    {
        var res = await _context.PostAsync($"https://api.feed.nicovideo.jp/v1/read/{actor.ActorType.GetDescription()}/{actor.ActorId}", ct);
        var code = await res.Content.ReadJsonAsAsync<CodeOnly>(ct: ct);
        return code.Code == "ok";
    }

    record class CodeOnly
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
