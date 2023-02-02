using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using NiconicoToolkit.Video;

#nullable enable

namespace NiconicoToolkit.Series;

public class NvapiSeriesVidoesResponseContainer 
{
    [JsonPropertyName("meta")]
    public Meta Meta { get; set; }

    [JsonPropertyName("data")]
    public NvapiSeriesVidoesResponse Data { get; set; }
}

public class NvapiSeriesVidoesResponse
{
    [JsonPropertyName("detail")]
    public SeriesVideosDetail Detail { get; set; }

    [JsonPropertyName("totalCount")]
    public int? TotalCount { get; set; }

    [JsonPropertyName("items")]
    public List<SeriesVideoItem> Items { get; set; }
}


public class SeriesVideosDetail
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("owner")]
    public SeriesVideoItemVideoOwner Owner { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("decoratedDescriptionHtml")]
    public string DecoratedDescriptionHtml { get; set; }

    [JsonPropertyName("thumbnailUrl")]
    public string ThumbnailUrl { get; set; }

    [JsonPropertyName("isListed")]
    public bool? IsListed { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class Icons
{
    [JsonPropertyName("small")]
    public string Small { get; set; }

    [JsonPropertyName("large")]
    public string Large { get; set; }
}

public class SeriesVideoItem
{
    [JsonPropertyName("meta")]
    public SeriesVideoItemMeta Meta { get; set; }

    [JsonPropertyName("video")]
    public NvapiVideoItem Video { get; set; }
}

public class SeriesVideoItemMeta
{
    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; } = 0;

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class SeriesVideoItemVideoOwner
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("user")]
    public DetailOwnerUser User { get; set; }

    [JsonPropertyName("ownerType")]
    public OwnerType OwnerType { get; set; }

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("iconUrl")]
    public string IconUrl { get; set; }
}

public class DetailOwnerUser
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("isPremium")]
    public bool? IsPremium { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("strippedDescription")]
    public string StrippedDescription { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("nickname")]
    public string Nickname { get; set; }

    [JsonPropertyName("icons")]
    public Icons Icons { get; set; }
}


