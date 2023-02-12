using NiconicoToolkit.Ranking.Video;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.Search.Video
{
    public sealed class VideoSearchSubClient
    {
        private readonly NiconicoContext _context;
        private readonly JsonSerializerOptions _option;

        public VideoSearchSubClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
        {
            _context = context;
            _option = defaultOptions;
        }

        /// <summary>
        /// キーワードかタグで動画を検索します<br />
        /// sortKeyがHot/PersonalizedのときはsortOrderはNoneになります<br />
        /// (min/max)RegisteredAtよりもrangeが優先されます
        /// </summary>
        /// <returns></returns>
        public Task<VideoSearchResponse> VideoSearchAsync(
            string keyword,
            bool isTagSearch = false,
            int? pageCount = null,
            SortKey sortKey = SortKey.Hot,
            SortOrder sortOrder = SortOrder.None,
            RankingGenre[] genres = null,
            Range? range = null,
            DateTime? minRegisteredAt = null,
            DateTime? maxRegisteredAt = null,
            int? maxDuration = null,
            CancellationToken ct = default)
        {
            var query = new NameValueCollection() { };
            
            if (isTagSearch)
            {
                query.Add("tag", keyword);
            }
            else
            {
                query.Add("keyword", keyword);
            }

            if (pageCount is not null)
                query.Add("page", pageCount.ToString());

            if (sortKey == SortKey.Hot || sortKey == SortKey.Personalized)
            {
                query.Add("sortKey", sortKey.GetDescription());
                query.Add("sortOrder", "none");
            }
            else
            {
                if(sortOrder == SortOrder.None)
                {
                    query.Add("sortKey", sortKey.GetDescription());
                    query.Add("sortOrder", "desc");
                }
                else
                {
                    query.Add("sortKey", sortKey.GetDescription());
                    query.Add("sortOrder", sortOrder.GetDescription());
                }
            }

            if (genres is not null && genres.Length >= 1)
                query.Add("genres", string.Join(",", genres));

            if (range is not null)
            {
                query.Add(
                    "minRegisteredAt",
                    RangeExtention.ToDateTime(range.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"));
            }
            else
            {
                if (minRegisteredAt is not null)
                {
                    query.Add(
                        "minRegisteredAt",
                        minRegisteredAt.Value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                }
                if (maxRegisteredAt is not null)
                {
                    query.Add(
                        "maxRegisteredAt",
                        minRegisteredAt.Value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                }
            }

            if (maxDuration is not null)
                query.Add("maxDuration", maxDuration.ToString());

            var url = new StringBuilder(NiconicoUrls.NvApiV2Url)
                .Append("search/video")
                .AppendQueryString(query)
                .ToString();

            return _context.GetJsonAsAsync<VideoSearchResponse>(url, _option, ct);
        }
    }
}
