using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.Search.Live;


[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(LiveSearchResponse))]
public sealed partial class LiveSearchJsonSourceGenerationContext : JsonSerializerContext
{
}

public sealed class LiveSearchSubClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _options;

    public LiveSearchSubClient(NiconicoContext context, JsonSerializerOptions options)
    {
        _context = context;
        _options = new(options)
        {
            TypeInfoResolverChain = { LiveSearchJsonSourceGenerationContext.Default }
        };
    }

    public Task<LiveSearchResponse> LiveSearchAsync(
        string keyword,
        int? pageCount = null,
        SearchLiveStatus? status = null,
        Sort? sort = null,
        LiveProvider? provider = null,
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

        return _context.GetJsonAsAsync<LiveSearchResponse>(url, _options, ct);
    }
}
