using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.Search.User;


[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(UserSearchResponse))]
public sealed partial class UserSearchJsonSourceGenerationContext : JsonSerializerContext
{
}


public sealed class UserSearchSubClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _option;

    public UserSearchSubClient(NiconicoContext context, JsonSerializerOptions options)
    {
        _context = context;
        _option = new(options)
        {
            TypeInfoResolverChain = { UserSearchJsonSourceGenerationContext.Default }
        };
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

        var url = new StringBuilder(NiconicoUrls.NvApiV1Url)
            .Append("search/user")
            .AppendQueryString(query)
            .ToString();

        return _context.GetJsonAsAsync<UserSearchResponse>(url, _option, ct);
    }
}
