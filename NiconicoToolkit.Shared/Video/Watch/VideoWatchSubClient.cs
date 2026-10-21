using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;
using System.Collections.Specialized;
using NiconicoToolkit.Video.Watch.Dmc;
using NiconicoToolkit.Video.Watch.Domand;
using CommunityToolkit.Diagnostics;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using System.Reflection.PortableExecutable;



#if WINDOWS_UWP
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#else
using System.Net.Http;
using System.Net.Http.Headers;
#endif

namespace NiconicoToolkit.Video.Watch;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(NicoVideoWatchApiResponse))]
[JsonSerializable(typeof(NicoVideoWatchApiResponseV3))]
[JsonSerializable(typeof(NicoVideoWatchApiResponseV4))]
[JsonSerializable(typeof(WatchJsonResponse))]
[JsonSerializable(typeof(DomandHlsAccessRightResponse))]
public sealed partial class VideoWatchJsonSourceGenerationContext : JsonSerializerContext
{
}

public sealed class VideoWatchSubClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _options;

    public VideoWatchSubClient(NiconicoContext context, JsonSerializerOptions options)
    {
        _context = context;
        _options = new JsonSerializerOptions(options)
        {
            TypeInfoResolverChain = { VideoWatchJsonSourceGenerationContext.Default }
        };
    }

    /// <summary>
    /// 2026/10/20：nvpcClientVersionの指定が不足しているとして取得できなくなっている。GetV3WatchDataAsync（SPWeb版)を利用するように。
    /// </summary>
    /// <param name="videoId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [Obsolete]
    public async Task<NicoVideoWatchApiResponse> GetWatchDataAsync(VideoId videoId, CancellationToken ct = default)
    {
        var url = new StringBuilder(NiconicoUrls.WatchPageUrl)
            .Append(videoId.ToString())
            .Append("?responseType=json")
            .ToString();

        await _context.WaitPageAccessAsync();
        return await _context.GetJsonAsAsync<NicoVideoWatchApiResponse>(url, _options);       
    }

    public async Task<string> GetActionTrackIdAsync(VideoId videoId, CancellationToken ct = default)
    {
        using var res = await _context.GetAsync(
            $"https://sp.nicovideo.jp/watch/{videoId}?redirected=1",
            ct: ct);

        if (!res.IsSuccessStatusCode)
        {
            throw new WebException($"Failed to access watch page: {res.StatusCode}");
        }

        var scriptText = await res.Content.ReadHtmlDocumentActionAsync(document =>
            document.GetElementById("S:0")?.QuerySelector("script")?.TextContent
            ?? throw new InvalidOperationException("The S:0 script element was not found."));

        var match = Regex.Match(
            scriptText,
            @"\\?""actionTrackId\\?""\s*,\s*\\?""([^""\\]+)\\?""");

        if (!match.Success)
        {
            throw new InvalidOperationException("actionTrackId was not found in the S:0 script.");
        }

        return match.Groups[1].Value;
    }

    /// <summary>
    /// 事前にGetActionTrackIdAsyncを呼び出してTrackActionIdを取得する（セッション中は使いまわせる）<br />
    /// また、視聴権取得には GetNvApiV1DomandHlsAccessRightAsync を利用すること
    /// </summary>
    /// <param name="videoId"></param>
    /// <param name="actionTrackId"></param>
    /// <param name="isGuest"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="WebException"></exception>
    public async Task<NicoVideoWatchApiResponseV3> GetV3WatchDataAsync(VideoId videoId, string actionTrackId, bool isGuest, CancellationToken ct = default)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var url =
            $"https://www.nicovideo.jp/api/watch/{(isGuest? "v3_guest" : "v3")}/{videoId}" +
            $"?_clientOsType=others&_frontendId=3&_frontendVersion=0.1.0" +
            $"&actionTrackId={Uri.EscapeDataString(actionTrackId)}" +
            "&skips=harmful&additionals=pcWatchPage%2Cseries" +
            $"&i18nLanguage=ja-jp&t={timestamp}";

        await _context.WaitPageAccessAsync();

        using var res = await _context.SendAsync(
            HttpMethod.Get,
            url,
            null,
            headers =>
            {
                headers.Add("Origin", "https://sp.nicovideo.jp");
#if WINDOWS_UWP
            headers.Referer = new Uri("https://sp.nicovideo.jp/");
#else
                headers.Referrer = new Uri("https://sp.nicovideo.jp/");
#endif
            },
            ct: ct);

        if (!res.IsSuccessStatusCode)
        {
#if WINDOWS_UWP
            var errorBody = await res.Content.ReadAsStringAsync().AsTask(ct);
#else
            var errorBody = await res.Content.ReadAsStringAsync(ct);
#endif
            throw new WebException(
                $"watch/v3 returned {(int)res.StatusCode} ({res.ReasonPhrase}): {errorBody}");
        }

        return await res.Content.ReadJsonAsAsync<NicoVideoWatchApiResponseV3>(
            _options,
            ct);
    }

    public async Task<NicoVideoWatchApiResponseV4> GetV4WatchDataAsync(VideoId videoId, CancellationToken ct = default)
    {
        var url = new StringBuilder(NiconicoUrls.WatchPageUrl)
            .Append(videoId.ToString())
            .Append("?responseType=json")
            .ToString();

        await _context.WaitPageAccessAsync();
        return await _context.GetJsonAsAsync<NicoVideoWatchApiResponseV4>(url, _options);
    }


    public async Task<WatchJsonResponse> GetDmcWatchJsonAsync(VideoId videoId, bool isLoggedIn, string actionTrackId)
    {
        var dict = new NameValueCollection
        {
            { "_frontendId", "6" },
            { "_frontendVersion", "0" },
            { "actionTrackId", actionTrackId },
            { "skips", "harmful" },
            { "additionals", WebUtility.UrlEncode("pcWatchPage,external,marquee,series") },
            { "isContinueWatching", "true" },
            { "i18nLanguage", "ja-jp" },
            { "t", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() }
        };

        var url = new StringBuilder("https://www.nicovideo.jp/api/watch/")
            .Append(isLoggedIn ? "v3" : "v3_guest")
            .Append("/")
            .Append(videoId.ToString())
            .AppendQueryString(dict)
            .ToString();

        try
        {
            using var res = await _context.SendAsync(HttpMethod.Get, url, null, headers => 
            {
                headers.Add("Accept", "*/*");
                headers.Add("Sec-Fetch-Site", "same-origin");
#if WINDOWS_UWP
                headers.Referer = new Uri($"https://www.nicovideo.jp/watch/{videoId}");
                headers.Host = new Windows.Networking.HostName("www.nicovideo.jp");
#else
                headers.Referrer = new Uri($"https://www.nicovideo.jp/watch/{videoId}");
                headers.Host = "www.nicovideo.jp";
#endif
            });

            if (res.ReasonPhrase == "Forbidden")
            {
                throw new WebException("require payment.");
            }

            return await res.Content.ReadJsonAsAsync<WatchJsonResponse>(_options);
        }
        catch (Exception e)
        {
            throw new WebException("access failed watch/" + videoId, e);
        }
    }



    #region Domand Video Play


    public async Task<DomandHlsAccessRightResponse> GetDomandHlsAccessRightAsync(
        VideoId videoId,
        WatchDomand domand,
        string? videoQualityId,
        string? audioQualityId,
        string? watchTrackId = null,
        CancellationToken ct = default)
    {
        List<string> qualities = new ();
        if (videoQualityId != null)
        {
            qualities.Add(videoQualityId);
        }
        if (audioQualityId != null)
        {
            qualities.Add(audioQualityId);
        }

        return await _context.SendJsonAsAsync<DomandHlsAccessRightResponse>(
            HttpMethod.Post,
            $"{NiconicoUrls.NvApiV4Url}watch/{videoId}?retry=0",
            $"{{\"outputs\":[[{string.Join(',', qualities.Select(x => $"\"{x}\""))}]]}}",
            _options,
            (header) =>
            {
                header.Add("X-Access-Right-Key", domand.AccessRightKey);
                header.Add("X-Frontend-Version", "0");
                header.Add("X-Frontend-Id", "6");
                header.Add("X-Request-With", "https://www.nicovideo.jp");
            },
            ct);
    }
    public async Task<DomandHlsAccessRightResponse> GetV4VariantsDomandHlsAccessRightAsync(
        VideoId videoId,
        WatchDomand domand,
        string? watchTrackId = null,
        CancellationToken ct = default)
    {
        List<string> qualities = new();
        foreach (var video in domand.Videos.Where(x => x.IsAvailable))
        {
            foreach (var audio in domand.Audios.Where(x => x.IsAvailable))
            {
                qualities.Add($"[\"{video.Id}\", \"{audio.Id}\"]");
            }
        }

        return await _context.SendJsonAsAsync<DomandHlsAccessRightResponse>(
            HttpMethod.Post,
            $"{NiconicoUrls.NvApiV4Url}watch/{videoId}",
            $"{{\"outputs\":[{string.Join(',', qualities)}]}}",
            _options,
            (header) =>
            {
                header.Add("X-Access-Right-Key", domand.AccessRightKey);
                header.Add("X-Frontend-Version", "0");
                header.Add("X-Frontend-Id", "6");
                header.Add("X-Request-With", "https://www.nicovideo.jp");
            },
            ct);
    }

    public async Task<DomandHlsAccessRightResponse> GetVariantsDomandHlsAccessRightAsync(
        VideoId videoId,
        WatchDomand domand,
        string? watchTrackId = null,
        CancellationToken ct = default)
    {
        List<string> qualities = new();
        foreach (var video in domand.Videos.Where(x => x.IsAvailable))
        {
            foreach (var audio in domand.Audios.Where(x => x.IsAvailable))
            {
                qualities.Add($"[\"{video.Id}\", \"{audio.Id}\"]");
            }
        }

        return await _context.SendJsonAsAsync<DomandHlsAccessRightResponse>(
            HttpMethod.Post,
            $"{NiconicoUrls.NvApiV1Url}watch/{videoId}/access-rights/hls{(watchTrackId != null ? $"?actionTrackId={watchTrackId}" : "")}",
            $"{{\"outputs\":[{string.Join(',', qualities)}]}}",
            _options,
            (header) =>
            {
                header.Add("X-Access-Right-Key", domand.AccessRightKey);
                header.Add("X-Frontend-Version", "0");
                header.Add("X-Frontend-Id", "6");
                header.Add("X-Request-With", "https://www.nicovideo.jp");
            },
            ct);
    }


    public async Task<DomandHlsAccessRightResponse> GetNvApiV1DomandHlsAccessRightAsync(
        VideoId videoId,
        WatchDomand domand,
        string? watchTrackId,
        CancellationToken ct = default)
    {
        List<string> qualities = new();
        foreach (var video in domand.Videos.Where(x => x.IsAvailable))
        {
            foreach (var audio in domand.Audios.Where(x => x.IsAvailable))
            {
                qualities.Add($"[\"{video.Id}\", \"{audio.Id}\"]");
            }
        }

        return await _context.SendJsonAsAsync<DomandHlsAccessRightResponse>(
            HttpMethod.Post,
            $"{NiconicoUrls.NvApiV1Url}watch/{videoId}/access-rights/hls{(watchTrackId != null ? $"?actionTrackId={watchTrackId}" : "")}",
            $"{{\"outputs\":[{string.Join(',', qualities)}]}}",
            _options,
            (header) =>
            {
                header.Add("Origin", "https://sp.nicovideo.jp");
#if WINDOWS_UWP
                header.Referer = new Uri("https://sp.nicovideo.jp/");
#else
                header.Referrer = new Uri("https://sp.nicovideo.jp/");
#endif
                header.Add("X-Access-Right-Key", domand.AccessRightKey);
                header.Add("X-Frontend-Version", "0.1.0");
                header.Add("X-Frontend-Id", "3");
                header.Add("X-Request-With", "nicovideo");
            },
            ct);
    }


    //public async Task<DomandHlsAccessRightResponse> GetNvApiV1DomandHlsAccessRightAsync(
    //    VideoId videoId,
    //    WatchDomand domand,
    //    string? videoQualityId,
    //    string? audioQualityId,
    //    string? watchTrackId = null,
    //    CancellationToken ct = default)
    //{
    //    List<string> qualities = new();
    //    if (videoQualityId != null)
    //    {
    //        qualities.Add(videoQualityId);
    //    }
    //    if (audioQualityId != null)
    //    {
    //        qualities.Add(audioQualityId);
    //    }

    //    return await _context.SendJsonAsAsync<DomandHlsAccessRightResponse>(
    //        HttpMethod.Post,
    //        $"{NiconicoUrls.NvApiV1Url}watch/{videoId}/access-rights/hls{(watchTrackId != null ? $"?actionTrackId={watchTrackId}" : "")}",
    //        $"{{\"outputs\":[[{string.Join(',', qualities.Select(x => $"\"{x}\""))}]]}}",
    //        _options,
    //        (header) =>
    //        {
    //            header.Add("X-Access-Right-Key", domand.AccessRightKey);
    //            header.Add("X-Frontend-Version", "0");
    //            header.Add("X-Frontend-Id", "6");
    //            header.Add("X-Request-With", "https://www.nicovideo.jp");
    //        },
    //        ct);
    //}

    //public async Task<DomandHlsAccessRightResponse> GetNvApiV1DomandHlsAccessRightAsync(
    //    VideoId videoId,
    //    WatchDomand domand,
    //    VideoContent? videoQuality,
    //    AudioContent? audioQuality,
    //    string? watchTrackId = null,
    //    CancellationToken ct = default
    //    )
    //{
    //    return await GetDomandHlsAccessRightAsync(videoId, domand, videoQuality?.Id, audioQuality?.Id, watchTrackId, ct);
    //}

    #endregion


#region nvapi Watch

    public async Task<bool> SendOfficialHlsWatchAsync(
        string contentId,
        string trackId
        )
    {
        var uri = new Uri($"https://nvapi.nicovideo.jp/v1/2ab0cbaa/watch?t={Uri.EscapeDataString(trackId)}");
        var refererUri = new Uri($"https://www.nicovideo.jp/watch/{contentId}");

        {
            using var optionRes = await _context.SendAsync(HttpMethod.Options, uri, null, 
                headers => 
                {
                    headers.Add("Access-Control-Request-Headers", "x-frontend-id,x-frontend-version");
                    headers.Add("Access-Control-Request-Method", "GET");
                    headers.Add("Origin", "https://www.nicovideo.jp");
#if WINDOWS_UWP
                    headers.Referer = refererUri;
#else
                    headers.Referrer = refererUri;
#endif
                }
                , HttpCompletionOption.ResponseHeadersRead
                );

            if (!optionRes.IsSuccessStatusCode) { return false; }
        }
        {
            using var watchRes = await _context.SendAsync(HttpMethod.Get, uri, null, 
                headers => 
                {
                    headers.Add("X-Frontend-Id", "6");
                    headers.Add("X-Frontend-Version", "0");
#if WINDOWS_UWP
                    headers.Referer = refererUri;
#else
                    headers.Referrer = refererUri;
#endif
                    headers.Add("Origin", "https://www.nicovideo.jp");

                }
                , HttpCompletionOption.ResponseHeadersRead
                );

            return watchRes.IsSuccessStatusCode;
        }
    }

#endregion

}
