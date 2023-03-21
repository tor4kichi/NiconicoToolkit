using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
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

    public async Task<ThreadResponseContainer> GetComments(NvComment videoComment, CancellationToken ct = default)
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
        return await _context.SendJsonAsAsync<ThreadResponseContainer>(HttpMethod.Post,
                    NVCommentApiUrl, requestParamsJson);        
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

