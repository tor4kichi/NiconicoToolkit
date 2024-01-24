using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Threading;
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
    /// 自らが投稿したコメントを削除する際のキーを取得します。
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="fork">fork または forkLabel を指定。参考:ThreadTargetForkConstants</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [RequireLogin]
    public async Task<ThreadDeleteKeyResponse> GetDeleteKeyAsync(string threadId, string fork, CancellationToken ct = default)
    {
        return await _context.GetJsonAsAsync<ThreadDeleteKeyResponse>(
            $"{NvApiCommentKeysUrl}delete?threadId={threadId}&fork={fork}", ct: ct
            );
    }


    /// <summary>
    /// 自らが投稿したコメントを削除します。IsMyPost が true のコメント番号を指定して削除します。
    /// </summary>
    /// <param name="videoId"></param>
    /// <param name="threadId"></param>
    /// <param name="fork"></param>
    /// <param name="commentNumber">コメントの番号（"no"）</param>
    /// <param name="language"></param>
    /// <param name="deleteKey">GetDeleteKeyAsync() によって得られる削除キーを指定します。</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [RequireLogin]
    public async Task<ThreadDeleteResponse> DeleteCommentAsync(
        string server,
        VideoId videoId,
        string threadId,
        string fork,
        int commentNumber,
        string language,
        string deleteKey,
        CancellationToken ct = default
        )
    {
        string requestParamsJson = JsonSerializer.Serialize(new ThreadDeleteRequest()
        {
            VideoId = videoId.ToString(),
            Fork = fork,
            DeleteKey = deleteKey,
            Language = language,
            Targets = new() { new() { Number = commentNumber } }
        });

        return await _context.SendJsonAsAsync<ThreadDeleteResponse>(HttpMethod.Put,
                    $"{MakeNVCommentThreadsUrl(server)}/{threadId}/comment-comment-owner-deletions", requestParamsJson, ct: ct);
    }
}



public sealed class ThreadDeleteRequest
{
    [JsonPropertyName("videoId")]
    public string VideoId { get; set; }

    [JsonPropertyName("deleteKey")]
    public string DeleteKey { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("targets")]
    public List<ThreadDeleteTarget> Targets { get; set; }

    [JsonPropertyName("fork")]
    public string Fork { get; set; }

    public sealed class ThreadDeleteTarget
    {
        [JsonPropertyName("no")]
        public int Number { get; set; }

        [JsonPropertyName("operation")]
        public string Operation { get; set; } = "DELETE";
    }
}

public sealed class ThreadDeleteResponse : ResponseWithMeta
{

}

