using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json.Serialization;
#if WINDOWS_UWP
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#else
using System.Net.Http;
using System.Net.Http.Headers;
#endif

namespace NiconicoToolkit.Video.Watch.NV_Comment;

public partial class NvCommentSubClient
{

    /// <summary>
    /// 動画に投稿されたコメントを取得します。
    /// </summary>
    /// <param name="videoComment">VideoClient.VideoWatch.GetInitialWatchDataAsync() のレスポンスデータに含まれる NvComment を指定します。</param>   
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<ThreadResponse> GetCommentsAsync(NvComment videoComment, CancellationToken ct = default)
    {
        string requestParamsJson = JsonSerializer.Serialize(new ThreadRequest()
        {
            ThreadKey = videoComment.ThreadKey,
            Params = new ThreadRequest.ThreadRequestParams()
            {
                Targets = videoComment.Params.Targets,
                Language = videoComment.Params.Language,
            }
        });
        return await _context.SendJsonAsAsync<ThreadResponse>(
            HttpMethod.Post,
            MakeNVCommentThreadsUrl(videoComment.Server),
            requestParamsJson,
            ct: ct
            );
    }

    /// <summary>
    /// 動画に投稿されたコメントを取得します。
    /// </summary>
    /// <param name="videoComment">VideoClient.VideoWatch.GetInitialWatchDataAsync() のレスポンスデータに含まれる NvComment を指定します。</param>   
    /// <param name="targetForks">取得対象とするコメントForkを指定します。参考:ThreadTargetIdConstants</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <remarks>VideoClient.VideoWatch.GetInitialWatchDataAsync() のレスポンスデータに含まれる NvComment のデータを引数に渡してください。</remarks>
    public async Task<ThreadResponse> GetCommentsAsync(NvComment videoComment, IEnumerable<string> targetForks, CancellationToken ct = default)
    {
        var forks = targetForks.ToHashSet();
        string requestParamsJson = JsonSerializer.Serialize(new ThreadRequest()
        {
            ThreadKey = videoComment.ThreadKey,
            Params = new ThreadRequest.ThreadRequestParams()
            {
                Targets = videoComment.Params.Targets.Where(x => forks.Contains(x.Fork)).ToList(),
                Language = videoComment.Params.Language,
            }
        });
        return await _context.SendJsonAsAsync<ThreadResponse>(
            HttpMethod.Post,
            MakeNVCommentThreadsUrl(videoComment.Server),
            requestParamsJson,
            ct: ct
            );
    }
}


class ThreadRequest
{
    [JsonPropertyName("params")]
    public ThreadRequestParams Params { get; set; }

    [JsonPropertyName("threadKey")]
    public string ThreadKey { get; set; }

    [JsonPropertyName("additionals")]
    public ThreadRequestAdditionals Additionals { get; set; } = new ThreadRequestAdditionals();


    public class ThreadRequestAdditionals
    {
    }

    public class ThreadRequestParams
    {
        [JsonPropertyName("targets")]
        public List<NvCommentParamsTarget> Targets { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }
    }
}


public class ThreadResponse : ResponseWithMeta
{
    [JsonPropertyName("data")]
    public ThreadResponseData Data { get; set; }


    public class ThreadResponseData
    {
        [JsonPropertyName("globalComments")]
        public List<GlobalComment> GlobalComments { get; set; }

        [JsonPropertyName("threads")]
        public List<Thread> Threads { get; set; }
    }
   
    public class GlobalComment
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    public class Thread
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("fork")]
        public string Fork { get; set; }

        [JsonPropertyName("commentCount")]
        public int CommentCount { get; set; }

        [JsonPropertyName("comments")]
        public List<Comment> Comments { get; set; }
    }

    public class Comment
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("no")]
        public int No { get; set; }

        [JsonPropertyName("vposMs")]
        public int VposMs { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("commands")]
        public List<string> Commands { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("isPremium")]
        public bool IsPremium { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("postedAt")]
        public DateTime PostedAt { get; set; }

        [JsonPropertyName("nicoruCount")]
        public int NicoruCount { get; set; }

        [JsonPropertyName("nicoruId")]
        public object NicoruId { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("isMyPost")]
        public bool IsMyPost { get; set; }
    }
}

