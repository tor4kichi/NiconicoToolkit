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
    /// スレッドにコメントを送信するためのポストキーを取得します。
    /// </summary>
    /// <param name="threadId">スレッドID。WatchApiResponse.WatchApiData.Comment.Threads から取得できます。</param>
    /// <returns></returns>
    [RequireLogin]
    public async Task<ThreadPostKeyResponse> GetPostKeyAsync(string threadId, CancellationToken ct = default)
    {
        return await _context.GetJsonAsAsync<ThreadPostKeyResponse>(
            $"{NvApiCommentKeysUrl}post?threadId={threadId}", ct: ct
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
        string server,
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
                    $"{MakeNVCommentThreadsUrl(server)}/{threadId}/comments", requestParamsJson, ct: ct);
    }


    [RequireLogin]
    public async Task<ThreadEasyPostKeyResponse> GetEasyPostKeyAsync(string threadId, CancellationToken ct = default)
    {
        return await _context.GetJsonAsAsync<ThreadEasyPostKeyResponse>(
            $"{NvApiCommentKeysUrl}post-easy?threadId={threadId}", ct: ct
            );
    }

    [RequireLogin]
    public async Task<ThreadPostResponse> EasyPostCommentAsync(
        string server,
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
                    $"{MakeNVCommentThreadsUrl(server)}/{threadId}/easy-comments", requestParamsJson, ct: ct);
    }
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

sealed class ThreadPostRequest
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

sealed class ThreadEasyPostRequest
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

public sealed class ThreadDeleteKeyResponse : ResponseWithMeta
{
    [JsonPropertyName("data")]
    public ThreadDeleteKeyData? Data { get; set; }

    public sealed class ThreadDeleteKeyData
    {
        [JsonPropertyName("deleteKey")]
        public string DeleteKey { get; set; }
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

