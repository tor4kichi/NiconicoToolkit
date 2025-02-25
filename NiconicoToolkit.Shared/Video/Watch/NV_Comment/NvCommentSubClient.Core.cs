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


[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ThreadDeleteRequest))]
[JsonSerializable(typeof(ThreadDeleteKeyResponse))]
[JsonSerializable(typeof(ThreadDeleteResponse))]
[JsonSerializable(typeof(ThreadResponse))]
[JsonSerializable(typeof(ThreadRequest))]
[JsonSerializable(typeof(ThreadPostKeyResponse))]
[JsonSerializable(typeof(ThreadPostResponse))]
[JsonSerializable(typeof(ThreadPostRequest))]
[JsonSerializable(typeof(ThreadEasyPostKeyResponse))]
[JsonSerializable(typeof(ThreadEasyPostRequest))]
public sealed partial class NV_CommentJsonSourceGenerationContext : JsonSerializerContext
{
}

public sealed partial class NvCommentSubClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _options;

    public NvCommentSubClient(NiconicoContext context, JsonSerializerOptions option)
    {
        _context = context;
        _options = new(option)
        {
            TypeInfoResolverChain = { NV_CommentJsonSourceGenerationContext.Default }
        };
    }

    public string MakeNVCommentThreadsUrl(string server)
    {
        return $"{server}/v1/threads";
    }

    //public const string NVCommentThreadsUrl = "https://nv-comment.nicovideo.jp/v1/threads";
    public const string NvApiCommentKeysUrl = "https://nvapi.nicovideo.jp/v1/comment/keys/";
}

public static class ThreadTargetForkConstants
{
    public const string Easy = "easy";
    public const string Main = "main";
    public const string Owner = "owner";
}




