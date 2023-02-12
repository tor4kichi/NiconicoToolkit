using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.Search.Live
{
    public sealed class LiveSearchSubClient
    {
        private readonly NiconicoContext _context;
        private readonly JsonSerializerOptions _option;

        public LiveSearchSubClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
        {
            _context = context;
            _option = defaultOptions;
        }

        public Task<LiveSearchResponse> LiveSearchAsync(
            string keyword,
            int? pageCount = null,
            Status? status = null,
            Sort? sort = null,
            Provider? provider = null,
            CancellationToken ct = default)
        {
            var query = new NameValueCollection() { };

            query.Add("keyword", keyword);

            if (pageCount is not null)
                query.Add("page", pageCount.ToString());

            if (status is not null)
                query.Add("status", status.Value.GetDescription());

            if (sort is not null)
                query.Add("sort", sort.Value.GetDescription());

            if (provider is not null)
                query.Add("providerType", provider.Value.GetDescription());

            var url = new StringBuilder(NiconicoUrls.LiveApiV1Url)
                .Append("search/program/list")
                .AppendQueryString(query)
                .ToString();

            return _context.GetJsonAsAsync<LiveSearchResponse>(url, _option, ct);
        }
    }
}
