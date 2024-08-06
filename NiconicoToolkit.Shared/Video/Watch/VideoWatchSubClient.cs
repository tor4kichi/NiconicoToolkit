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
#if WINDOWS_UWP
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#else
using System.Net.Http;
using System.Net.Http.Headers;
#endif

namespace NiconicoToolkit.Video.Watch;

public sealed class VideoWatchSubClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _options;
    private readonly JsonSerializerOptions _dmcSessionSerializerOptions;
    public VideoWatchSubClient(NiconicoContext context, JsonSerializerOptions options)
    {
        _context = context;
        _options = options;
        _dmcSessionSerializerOptions = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    }


    public async Task<NicoVideoWatchApiResponse> GetWatchDataAsync(VideoId videoId, CancellationToken ct = default)
    {
        var url = new StringBuilder(NiconicoUrls.WatchPageUrl)
            .Append(videoId.ToString())
            .Append("?responseType=json")
            .ToString();

        await _context.WaitPageAccessAsync();
        return await _context.GetJsonAsAsync<NicoVideoWatchApiResponse>(url, _options);       
    }




    public async Task<WatchJsonResponse> GetDmcWatchJsonAsync(VideoId videoId, bool isLoggedIn, string actionTrackId)
    {
        var dict = new NameValueCollection();
        dict.Add("_frontendId", "6");
        dict.Add("_frontendVersion", "0");
        dict.Add("actionTrackId", actionTrackId);
        dict.Add("skips", "harmful");
        dict.Add("additionals", WebUtility.UrlEncode("pcWatchPage,external,marquee,series"));
        dict.Add("isContinueWatching", "true");
        dict.Add("i18nLanguage", "ja-jp");
        dict.Add("t", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString());

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
            $"{NiconicoUrls.NvApiV1Url}watch/{videoId}/access-rights/hls{(watchTrackId != null ? $"?actionTrackId={watchTrackId}" : "")}",
            $"{{\"outputs\":[[{string.Join(',', qualities.Select(x => $"\"{x}\""))}]]}}",
            null, 
            (header) => 
            {
                header.Add("X-Access-Right-Key", domand.AccessRightKey);
                header.Add("X-Frontend-Version", "0");
                header.Add("X-Frontend-Id", "6");
                header.Add("X-Request-With", "https://www.nicovideo.jp");
            },
            ct);
    }

    public async Task<DomandHlsAccessRightResponse> GetDomandHlsAccessRightAsync(
        VideoId videoId,
        WatchDomand domand,
        VideoContent? videoQuality,
        AudioContent? audioQuality,
        string? watchTrackId = null,
        CancellationToken ct = default
        )
    {
        return await GetDomandHlsAccessRightAsync(videoId, domand, videoQuality?.Id, audioQuality?.Id, watchTrackId, ct);
    }

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
