using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.Search.List
{
    public sealed class ListSearchSubClient
    {
        private readonly NiconicoContext _context;
        private readonly JsonSerializerOptions _option;

        public ListSearchSubClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
        {
            _context = context;
            _option = defaultOptions;
        }

        public Task<ListSearchResponse> ListSearchAsync(
            string keyword,
            int? pageCount = null,
            ListType? types = null,
            SortKey? sortKey = null,
            SortOrder? sortOrder = null,
            CancellationToken ct = default)
        {
            var query = new NameValueCollection() { };

            query.Add("keyword", keyword);

            if (pageCount is not null)
                query.Add("page", pageCount.ToString());

            if (types is not null)
                query.Add("types", types.Value.GetDescription());

            if (sortKey is not null)
                query.Add("sortKey", sortKey.Value.GetDescription());

            if (sortOrder is not null)
                query.Add("sortOrder", sortOrder.Value.GetDescription());

            var url = new StringBuilder(NiconicoUrls.NvApiV1Url)
                .Append("search/mylist")
                .AppendQueryString(query)
                .ToString();

            return _context.GetJsonAsAsync<ListSearchResponse>(url, _option, ct);
        }
    }
}
