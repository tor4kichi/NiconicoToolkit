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

public sealed partial class NvCommentSubClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _option;

    public NvCommentSubClient(NiconicoContext context, JsonSerializerOptions option)
    {
        _context = context;
        _option = option;
    }

    public const string NVCommentThreadsUrl = "https://nvcomment.nicovideo.jp/v1/threads";
    public const string NvApiCommentKeysUrl = "https://nvapi.nicovideo.jp/v1/comment/keys/";
}

public static class ThreadTargetForkConstants
{
    public const string Easy = "easy";
    public const string Main = "main";
    public const string Owner = "owner";
}




