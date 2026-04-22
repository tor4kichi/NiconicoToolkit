using NiconicoToolkit.Video;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel;

#if WINDOWS_UWP
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#else
using System.Net.Http;
using System.Net.Http.Headers;
#endif


namespace NiconicoToolkit.Activity.VideoWatchHistory;

public sealed class VideoWatchHisotrySubClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _options;

    public VideoWatchHisotrySubClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
    {
        _context = context;
        _options = defaultOptions;
    }        

    internal static class Urls
    {
        public const string WatchHitoryApi = $"{NiconicoUrls.NvApiV2Url}users/me/watch/history";
    }

    [RequireLogin]
    public Task<VideoWatchHistory> GetShortVideoWatchHistoryAsync(int limit = 6, VideoWatchHistory? prevResponse = null)
    {
        var parameters = new NameValueCollection()
        {
            { "limit", limit.ToString() },
            { "selectContentType", "short" },
        };
        if (prevResponse?.Data != null
            && string.IsNullOrEmpty(prevResponse.Data.NextCursor))
        {
            parameters.Add("cursor", prevResponse.Data.NextCursor);
        }

        var url = new StringBuilder(Urls.WatchHitoryApi)
            .AppendQueryString(parameters)
            .ToString();

        return _context.GetJsonAsAsync<VideoWatchHistory>(url, _options);
    }

    /// <remarks>[Require Login]</remarks>
    [RequireLogin]
    public Task<VideoWatchHistory> GetWatchHistoryAsync(int limit = 6, VideoWatchHistory? prevResponse = null)
    {
        var parameters = new NameValueCollection()
        {
            { "limit", limit.ToString() },
            { "selectContentType", "long" },
        };
        if (prevResponse?.Data != null
            && string.IsNullOrEmpty(prevResponse.Data.NextCursor))
        {
            parameters.Add("cursor", prevResponse.Data.NextCursor);
        }

        var url = new StringBuilder(Urls.WatchHitoryApi)
            .AppendQueryString(parameters)
            .ToString();

        return _context.GetJsonAsAsync<VideoWatchHistory>(url, _options);
    }

    /// <remarks>[Require Login]</remarks>
    [RequireLogin]
    public Task<VideoWatchHistoryDeleteResult> DeleteWatchHistoriesAsync(VideoId target)
    {
        var url = new StringBuilder(Urls.WatchHitoryApi)
            .AppendQueryString(new NameValueCollection()
            {
                { "target", target },
            })
            .ToString();

        return _context.SendJsonAsAsync<VideoWatchHistoryDeleteResult>(HttpMethod.Delete, url, _options);
    }

    /// <remarks>[Require Login]</remarks>
    [RequireLogin]
    public Task<VideoWatchHistoryDeleteResult> DeleteWatchHistoriesAsync(IEnumerable<VideoId> targets)
    {
        var url = new StringBuilder(Urls.WatchHitoryApi)
            .AppendQueryString(new NameValueCollection()
            {
                { "target", string.Join(',', targets) },
            })
            .ToString();

        return _context.SendJsonAsAsync<VideoWatchHistoryDeleteResult>(HttpMethod.Delete, url, _options);
    }


    /// <remarks>[Require Login]</remarks>
    [RequireLogin]
    public Task<VideoWatchHistoryDeleteResult> DeleteAllWatchHistoriesAsync()
    {
        var url = new StringBuilder(Urls.WatchHitoryApi)
            .AppendQueryString(new NameValueCollection()
            {
                { "target", "all" },
            })
            .ToString();

        return _context.SendJsonAsAsync<VideoWatchHistoryDeleteResult>(HttpMethod.Delete, url, _options);
    }
}

public class VideoWatchHistoryDeleteResult : ResponseWithMeta
{
}

public class VideoWatchHistory : ResponseWithMeta
{
    [JsonPropertyName("data")]
    public VideoWatchHistoryData Data { get; set; }
}

//public class VideoWatchHistoryData
//{
//    [JsonPropertyName("totalCount")]
//    public long TotalCount { get; set; }

//    [JsonPropertyName("items")]
//    public VideoWatchHistoryItem[] Items { get; set; }
//}

//public class VideoWatchHistoryItem
//{
//    [JsonPropertyName("watchId")]
//    public string WatchId { get; set; }

//    //[JsonPropertyName("frontendId")]
//    //public long FrontendId { get; set; }

//    [JsonPropertyName("views")]
//    public long? Views { get; set; }

//    [JsonPropertyName("lastViewedAt")]
//    public DateTimeOffset? LastViewedAt { get; set; }

//    [JsonPropertyName("playbackPosition")]
//    [JsonConverter(typeof(PlaybackPositionConverter))]
//    public PlaybackPosition PlaybackPosition { get; set; }

//    [JsonPropertyName("video")]
//    public NvapiVideoItem Video { get; set; }
//}

public class VideoWatchHistoryDataItem
{

    [JsonPropertyName("itemId")]
    public string ItemId { get; set; }

    [JsonPropertyName("viewedAt")]
    public DateTime ViewedAt { get; set; }

    [JsonPropertyName("isMaybeLikeUserItem")]
    public bool IsMaybeLikeUserItem { get; set; }

    [JsonPropertyName("video")]
    public NvapiVideoItem Video { get; set; }
}

public class VideoWatchHistoryData
{

    [JsonPropertyName("items")]
    public List<VideoWatchHistoryDataItem> Items { get; set; }

    [JsonPropertyName("nextCursor")]
    public string NextCursor { get; set; }
}
