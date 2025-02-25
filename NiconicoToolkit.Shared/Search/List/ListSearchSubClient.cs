using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.Search.List;


[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ListSearchResponse))]
public sealed partial class ListSearchJsonSourceGenerationContext : JsonSerializerContext
{
}


public sealed class ListSearchSubClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _options;

    public ListSearchSubClient(NiconicoContext context, JsonSerializerOptions options)
    {
        _context = context;
        _options = new(options) 
        {
            TypeInfoResolverChain = { ListSearchJsonSourceGenerationContext.Default }
        };
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
            .Append("search/list")
            .AppendQueryString(query)
            .ToString();

        return _context.GetJsonAsAsync<ListSearchResponse>(url, _options, ct);
    }
}
