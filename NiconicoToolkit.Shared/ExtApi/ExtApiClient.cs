#nullable enable
using NiconicoToolkit.Video;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.ExtApi.Video;

public sealed class ExtApiClient
{
    private readonly NiconicoContext _niconicoContext;

    internal ExtApiClient(NiconicoContext niconicoContext)
    {
        _niconicoContext = niconicoContext;
    }

    public async Task<ThumbInfoResponse> GetVideoInfoAsync(VideoId videoId, CancellationToken ct = default)
    {
        return await _niconicoContext.GetXmlAsAsync<ThumbInfoResponse>($"https://ext.nicovideo.jp/api/getthumbinfo/{videoId}", ct);
    }
}


