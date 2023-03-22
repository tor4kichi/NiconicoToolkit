using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;
#if WINDOWS_UWP
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#else
using System.Net.Http;
using System.Net.Http.Headers;
#endif

namespace NiconicoToolkit.Video.Watch.NV_Comment;

public sealed class NvCommentSubClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _option;

    public NvCommentSubClient(NiconicoContext context, JsonSerializerOptions option)
    {
        _context = context;
        _option = option;
    }


    const string NVCommentApiUrl = "https://nvcomment.nicovideo.jp/v1/threads";

    /// <summary>
    /// 動画に投稿されたコメントを取得します。
    /// </summary>
    /// <param name="videoComment">VideoClient.VideoWatch.GetInitialWatchDataAsync() のレスポンスデータに含まれる NvComment を指定します。</param>   
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<ThreadResponseContainer> GetCommentsAsync(NvComment videoComment, CancellationToken ct = default)
    {
        string requestParamsJson = JsonSerializer.Serialize(new ThreadRequestContainer() 
        {
            ThreadKey = videoComment.ThreadKey,
            Params = new ThreadRequestContainer.ThreadRequestParams()
            {
                Targets = videoComment.Params.Targets,
                Language = videoComment.Params.Language,
            }
        });        
        return await _context.SendJsonAsAsync<ThreadResponseContainer>(
            HttpMethod.Post,
            NVCommentApiUrl, 
            requestParamsJson,
            ct: ct
            );        
    }

    /// <summary>
    /// スレッドにコメントを送信するためのポストキーを取得します。
    /// </summary>
    /// <param name="threadId">スレッドID。WatchApiResponse.WatchApiData.Comment.Threads から取得できます。</param>
    /// <returns></returns>
    [RequireLogin]
    public async Task<ThreadPostKeyResponse> GetPostKeyAsync(string threadId, CancellationToken ct = default)
    {
        return await _context.GetJsonAsAsync<ThreadPostKeyResponse>(
            $"https://nvapi.nicovideo.jp/v1/comment/keys/post?threadId={threadId}", ct: ct
            );
    }

    /// <summary>
    /// 動画の指定スレッドに対してコメント投稿を送信します。
    /// </summary>
    /// <param name="threadId">スレッドID。WatchApiResponse.WatchApiData.Comment.Threads から取得できます。</param>
    /// <param name="videoId">動画ID。stringのままでもVideoIdへの暗黙の型変換により指定可能です。</param>
    /// <param name="commands">184などのコマンドを指定します</param>
    /// <paramref name="commands">https://dic.nicovideo.jp/a/%E3%82%B3%E3%83%9E%E3%83%B3%E3%83%89</paramref>
    /// <param name="comment">コメント本文。Uriエンコードは不要です。</param>
    /// <param name="vPosMs">コメントする動画時間をマイクロ秒単位で指定します。動画秒数に対して1000を掛け算することでマイクロ秒へ変換できます。</param>
    /// <param name="postKey">GetPostKeyAsync()を使用して予め取得してください。</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <remarks> パラメータエラーによる失敗は全て INVALID_PARAMETER としてまとめられるため、何が間違いかは判別できません。 </remarks>
    [RequireLogin]
    public async Task<ThreadPostResponse> PostCommentAsync(
        string threadId,
        VideoId videoId,
        IEnumerable<string> commands,
        string comment,
        int vPosMs,
        string postKey,
        CancellationToken ct = default
        )
    {
        string requestParamsJson = JsonSerializer.Serialize(new ThreadPostRequest()
        {
            VideoId = videoId.ToString(),
            Commands = commands.ToList(),
            Body = comment,
            VposMs = vPosMs,
            PostKey = postKey,
        });

        return await _context.SendJsonAsAsync<ThreadPostResponse>(HttpMethod.Post,
                    $"{NVCommentApiUrl}/{threadId}/comments", requestParamsJson, ct: ct);        
    }


    [RequireLogin]
    public async Task<ThreadEasyPostKeyResponse> GetEasyPostKeyAsync(string threadId, CancellationToken ct = default)
    {
        return await _context.GetJsonAsAsync<ThreadEasyPostKeyResponse>(
            $"https://nvapi.nicovideo.jp/v1/comment/keys/post-easy?threadId={threadId}", ct: ct
            );
    }

    [RequireLogin]
    public async Task<ThreadPostResponse> EasyPostCommentAsync(
        string threadId,
        VideoId videoId,
        string comment,
        int vPosMs,
        string easyPostKey,
        CancellationToken ct = default
        )
    {
        string requestParamsJson = JsonSerializer.Serialize(new ThreadEasyPostRequest()
        {
            VideoId = videoId.ToString(),
            Body = comment,
            VposMs = vPosMs,
            EasyPostKey = easyPostKey,
        });

        return await _context.SendJsonAsAsync<ThreadPostResponse>(HttpMethod.Post,
                    $"{NVCommentApiUrl}/{threadId}/easy-comments", requestParamsJson, ct: ct);
    }
}

public static class ThreadTargetIdConstatns
{
    public const string Easy = "easy";
    public const string Main = "main";
    public const string Owner = "owner";
}


public sealed class ThreadPostKeyResponse : ResponseWithMeta
{
    [JsonPropertyName("data")]
    public ThreadPostKeyData? Data { get; set; }

    public sealed class ThreadPostKeyData
    {
        [JsonPropertyName("postKey")]
        public string PostKey { get; set; }
    }
}

public sealed class ThreadPostRequest
{
    [JsonPropertyName("videoId")]
    public string VideoId { get; set; }

    [JsonPropertyName("commands")]
    public List<string> Commands { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("vposMs")]
    public int VposMs { get; set; }

    [JsonPropertyName("postKey")]
    public string PostKey { get; set; }
}

public sealed class ThreadEasyPostRequest
{
    [JsonPropertyName("videoId")]
    public string VideoId { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("vposMs")]
    public int VposMs { get; set; }

    [JsonPropertyName("postEasyKey")]
    public string EasyPostKey { get; set; }
}

public sealed class ThreadPostResponse : ResponseWithMeta
{
    [JsonPropertyName("data")]
    public ThreadPostResponseData? Data { get; set; }

    public sealed class ThreadPostResponseData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("no")]
        public int Number { get; set; }
    }
}

public sealed class ThreadEasyPostKeyResponse : ResponseWithMeta
{
    [JsonPropertyName("data")]
    public ThreadEasyPostKeyData? Data { get; set; }

    public sealed class ThreadEasyPostKeyData
    {
        [JsonPropertyName("postEasyKey")]
        public string EasyPostKey { get; set; }
    }
}

public class ThreadRequestContainer
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


public class ThreadResponseContainer : ResponseWithMeta
{
    [JsonPropertyName("data")]
    public ThreadResponseData Data { get; set; }
}


// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
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

