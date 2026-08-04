using NiconicoToolkit.Video;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Ranking.Video;


// New Response
public class VideoRankingResponse : ResponseWithData<VideoRankingResponseData>
{
}


public class AvailableTerm
{
    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public class VideoRankingResponseData
{
    [JsonPropertyName("response")]
    public Response Response { get; set; }


}

public class RankingVideoData
{
    [JsonPropertyName("featuredKey")]
    public string FeaturedKey { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    [JsonPropertyName("maxItemCount")]
    public int? MaxItemCount { get; set; }

    [JsonPropertyName("items")]
    public List<Item> Items { get; set; }

    [JsonPropertyName("hasNext")]
    public bool? HasNext { get; set; }

    [JsonPropertyName("isTopLevel")]
    public bool? IsTopLevel { get; set; }

    [JsonPropertyName("isImmoral")]
    public bool? IsImmoral { get; set; }

    [JsonPropertyName("trendTags")]
    public List<string> TrendTags { get; set; }
}

public class ForyouRanking
{
    [JsonPropertyName("recommendId")]
    public string RecommendId { get; set; }

    [JsonPropertyName("featuredKey")]
    public string FeaturedKey { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("tag")]
    public string Tag { get; set; }

    [JsonPropertyName("items")]
    public List<Item> Items { get; set; }
}

public class GetTeibanRanking
{
    [JsonPropertyName("data")]
    public RankingVideoData Data { get; set; }
}

public class GetTeibanRankingFeaturedKeyAndTrendTags
{
    [JsonPropertyName("data")]
    public RankingVideoData Data { get; set; }
}

public class GetTeibanRankingFeaturedKeys
{
    [JsonPropertyName("data")]
    public RankingVideoData Data { get; set; }
}

public class Item : NvapiVideoItem
{
    [JsonPropertyName("featuredKey")]
    public string FeaturedKey { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("isEnabledTrendTag")]
    public bool? IsEnabledTrendTag { get; set; }

    [JsonPropertyName("isMajorFeatured")]
    public bool? IsMajorFeatured { get; set; }

    [JsonPropertyName("isTopLevel")]
    public bool? IsTopLevel { get; set; }

    [JsonPropertyName("isImmoral")]
    public bool? IsImmoral { get; set; }

    [JsonPropertyName("isEnabled")]
    public bool? IsEnabled { get; set; }
}

public class LinkTag
{
    [JsonPropertyName("rel")]
    public string Rel { get; set; }

    [JsonPropertyName("href")]
    public string Href { get; set; }

    //[JsonPropertyName("attrs")]
    //public object Attrs { get; set; }
}

public class MetaTag
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("property")]
    public string Property { get; set; }
}

public class NiconewsRanking
{
    [JsonPropertyName("rank")]
    public int? Rank { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("thumbnailUrl")]
    public string ThumbnailUrl { get; set; }

    [JsonPropertyName("commentCount")]
    public int? CommentCount { get; set; }
}

public class Owner
{
    [JsonPropertyName("ownerType")]
    public string OwnerType { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("iconUrl")]
    public string IconUrl { get; set; }
}

public class Page
{
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; }

    //[JsonPropertyName("currentTag")]
    //public string? CurrentTag { get; set; }

    [JsonPropertyName("currentTerm")]
    public string CurrentTerm { get; set; }

    [JsonPropertyName("availableTerms")]
    public List<AvailableTerm> AvailableTerms { get; set; }

    [JsonPropertyName("niconewsRanking")]
    public List<NiconewsRanking> NiconewsRanking { get; set; }

    [JsonPropertyName("foryouRanking")]
    public ForyouRanking ForyouRanking { get; set; }
}

public class Pagination
{
    [JsonPropertyName("page")]
    public int? Page { get; set; }

    [JsonPropertyName("pageSize")]
    public int? PageSize { get; set; }

    [JsonPropertyName("totalCount")]
    public int? TotalCount { get; set; }

    [JsonPropertyName("maxPage")]
    public int? MaxPage { get; set; }
}

public class Response
{
    [JsonPropertyName("$getTeibanRanking")]
    public GetTeibanRanking GetTeibanRanking { get; set; }

    [JsonPropertyName("$getTeibanRankingFeaturedKeyAndTrendTags")]
    public GetTeibanRankingFeaturedKeyAndTrendTags GetTeibanRankingFeaturedKeyAndTrendTags { get; set; }

    [JsonPropertyName("$getTeibanRankingFeaturedKeys")]
    public GetTeibanRankingFeaturedKeys GetTeibanRankingFeaturedKeys { get; set; }

    [JsonPropertyName("page")]
    public Page Page { get; set; }
}


public class Thumbnail
{
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("middleUrl")]
    public string MiddleUrl { get; set; }

    [JsonPropertyName("largeUrl")]
    public string LargeUrl { get; set; }

    [JsonPropertyName("listingUrl")]
    public string ListingUrl { get; set; }

    [JsonPropertyName("nHdUrl")]
    public string NHdUrl { get; set; }

    //[JsonPropertyName("shortUrl")]
    //public object ShortUrl { get; set; }
}

public class User
{
    [JsonPropertyName("login_status")]
    public string LoginStatus { get; set; }
}

