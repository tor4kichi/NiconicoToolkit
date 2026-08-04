using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NiconicoToolkit.Rss.Video;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

#if WINDOWS_UWP
using Windows.Web.Syndication;
#else
using System.ServiceModel.Syndication;
using System.Xml;
#endif

namespace NiconicoToolkit.Ranking.Video;

public sealed class VideoRankingSubClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _options;

    public VideoRankingSubClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
    {
        _context = context;
        _options = defaultOptions;
    }

    public static bool IsHotTopicAcceptTerm(RankingTerm term)
    {
        return VideoRankingConstants.HotTopicAccepteRankingTerms.Any(x => x == term);
    }

    public static bool IsGenreWithTagAcceptTerm(RankingTerm term)
    {
        return VideoRankingConstants.GenreWithTagAccepteRankingTerms.Any(x => x == term);
    }

    public Task<VideoRankingResponse> GetRankingAsync(
        string genreId,
        RankingTerm term = RankingTerm.Hour,
        string tag = null,
        int? pageCount = null,
        CancellationToken ct = default)
    {
        var query = new NameValueCollection() { };
        if (pageCount is not null)
            query.Add("page", pageCount.ToString());

        if (tag is not null)
        {
            if (term != RankingTerm.Hour && term != RankingTerm.Day)
                term = RankingTerm.Day;

            query.Add("tag", tag);
        }
        else
        {
            query.Add("tag", "all");
        }

        query.Add("term", term.GetDescription());
        query.Add("responseType", "json");        
        var url = new StringBuilder(NiconicoUrls.NicoHomePageUrl)
            .Append("ranking/genre/")
            .Append(genreId)
            .AppendQueryString(query)
            .ToString();

        return _context.GetJsonAsAsync<VideoRankingResponse>(url, _options, ct);
    }

    public async Task<VideoRankingResponse> GetRankingAsync(
        GenreInfo genre,
        RankingTerm term = RankingTerm.Hour,
        string tag = null,
        int? pageCount = null,
        CancellationToken ct = default)
    {
        return await GetRankingAsync(genre.Id, term, tag, pageCount, ct);
    }
}
