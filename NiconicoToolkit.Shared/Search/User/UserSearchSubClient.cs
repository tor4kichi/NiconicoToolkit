using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.Search.User
{
    public sealed class UserSearchSubClient
    {
        private readonly NiconicoContext _context;
        private readonly JsonSerializerOptions _option;

        public UserSearchSubClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
        {
            _context = context;
            _option = defaultOptions;
        }

        public Task<UserSearchResponse> UserSearchAsync(
            string keyword,
            int? pageCount = null,
            SortKey? sortKey = null,
            CancellationToken ct = default)
        {
            var query = new NameValueCollection() { };

            query.Add("keyword", keyword);

            if (pageCount is not null)
                query.Add("page", pageCount.ToString());

            if (sortKey is not null)
                query.Add("sortKey", sortKey.Value.GetDescription());

            var url = new StringBuilder(NiconicoUrls.NvApiV2Url)
                .Append("search/user")
                .AppendQueryString(query)
                .ToString();

            return _context.GetJsonAsAsync<UserSearchResponse>(url, _option, ct);
        }
    }
}
