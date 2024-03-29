﻿using System;
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
#if WINDOWS_UWP
using Windows.Web.Syndication;
#else
using System.ServiceModel.Syndication;
using System.Xml;
#endif

namespace NiconicoToolkit.Ranking.Video
{


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


        private static string MakeRankingUrl(RankingGenre genre, string tag = null, RankingTerm? term = null, int page = 1, bool withRss = false)
        {
            var dict = new NameValueCollection();
                        
            if (term is not null)
            {
                dict.Add(nameof(term), term.Value.GetDescription());
            }
            if (string.IsNullOrEmpty(tag) is false)
            {
                if (genre is RankingGenre.HotTopic)
                {
                    dict.Add("key", tag);
                }
                else
                {
                    dict.Add(nameof(tag), tag);
                }
            }
            if (page != 1)
            {
                dict.Add(nameof(page), page.ToString());
            }
            if (withRss)
            {
                dict.Add("rss", "2.0");
                dict.Add("lang", "ja-jp");
            }

            if (genre is RankingGenre.HotTopic)
            {                
                return new StringBuilder(VideoRankingConstants.NiconicoRankingHotTopicDomain)
                    .AppendQueryString(dict)
                    .ToString();
            }
            else
            {
                return new StringBuilder(VideoRankingConstants.NiconicoRankingGenreDomain)
                    .Append(genre.GetDescription())
                    .AppendQueryString(dict)
                    .ToString();
            }
        }

        //async Task<List<RankingGenrePickedTag>> Internal_GetPickedTagAsync(string url, bool isHotTopic, CancellationToken ct)
        //{
        //    await _context.WaitPageAccessAsync();

        //    using var res = await _context.GetAsync(url, ct: ct);
        //    return await res.Content.ReadHtmlDocumentActionAsync(document =>
        //    {
                // ページ上の .RankingFilterTag となる要素を列挙する
        //        var tagAnchorElements = isHotTopic
        //            ? document.QuerySelectorAll(@"section.HotTopicsContainer > ul > li > a")
        //            : document.QuerySelectorAll(@"section.RepresentedTagsContainer > ul > li > a")
        //            ;

        //        List<RankingGenrePickedTag> items = new();
        //        foreach (var element in tagAnchorElements)
        //        {
        //            var tag = new RankingGenrePickedTag();
        //            tag.DisplayName = element.TextContent.Trim('\n', ' ');
        //            var hrefAttr = element.GetAttribute("href");
        //            var splited = hrefAttr.Split('=', '&');
        //            var first = splited.ElementAtOrDefault(1);
        //            tag.Tag = Uri.UnescapeDataString(first?.Trim('\n') ?? String.Empty);

        //            items.Add(tag);
        //        }

        //        return items;
        //    });
        //}

        public Task<HotTopicResponse> GetHotTopicAsync(CancellationToken ct = default)
        {
            return _context.GetJsonAsAsync<HotTopicResponse>(
                $"{NiconicoUrls.NvApiV1Url}hot-topics",
                _options, ct
                );
        }

        public Task<PopularTagResponse> GetPopularTagAsync(RankingGenre genre, CancellationToken ct = default)
        {
            return _context.GetJsonAsAsync<PopularTagResponse>(
                $"{NiconicoUrls.NvApiV1Url}genres/{genre.GetDescription()}/popular-tags",
                _options, ct
                );
        }

        /// <summary>
        /// 指定ジャンルの「人気のタグ」を取得します。 <br />
        /// RankingGenre.All を指定した場合のみ、常に空のListを返します。（RankingGenre.All は「人気のタグ」を指定できないため）
        /// </summary>
        /// <param name="genre">RankingGenre.All"以外"を指定します。</param>
        /// <remarks></remarks>
        /// <returns></returns>
        public async Task<List<RankingGenrePickedTag>> GetGenrePickedTagAsync(RankingGenre genre, CancellationToken ct = default)
        {
            var items = new List<RankingGenrePickedTag>();

            if (genre == RankingGenre.All) { return items; }

            if (genre == RankingGenre.HotTopic)
            {
                var topics = await GetHotTopicAsync(ct);
                foreach (var topic in topics.Data.HotTopics)
                {
                    var tag = new RankingGenrePickedTag();
                    tag.DisplayName = topic.Label;
                    tag.Tag = topic.Key;

                    items.Add(tag);
                }

                return items;
            }

            var tags = await GetPopularTagAsync(genre, ct);
            foreach(var popularTag in tags.Data.Tags)
            {
                var tag = new RankingGenrePickedTag();
                tag.DisplayName = popularTag;
                tag.Tag = popularTag;

                items.Add(tag);
            }

            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="tag"></param>
        /// <param name="term"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<RssVideoResponse> GetRankingRssAsync(RankingGenre genre, string tag = null, RankingTerm term = RankingTerm.Hour, int page = 1, CancellationToken ct = default)
        {
            if (genre != RankingGenre.HotTopic)
            {
                if (!IsGenreWithTagAcceptTerm(term))
                {
                    throw new ArgumentOutOfRangeException($"out of range {nameof(RankingTerm)}. accept with {string.Join(" or ", VideoRankingConstants.GenreWithTagAccepteRankingTerms)}.");
                }
            }
            else
            {
                if (!IsHotTopicAcceptTerm(term))
                {
                    throw new ArgumentOutOfRangeException($"out of range {nameof(RankingTerm)}. accept with {string.Join(" or ", VideoRankingConstants.HotTopicAccepteRankingTerms)}.");
                }
            }

            if (page == 0 || page > VideoRankingConstants.MaxPageWithTag)
            {
                throw new ArgumentOutOfRangeException($"out of range {nameof(page)}. inside btw from 1 to {VideoRankingConstants.MaxPageWithTag} in with tag.");
            }

            return await GetRssVideoResponseAsync(MakeRankingUrl(genre, tag, term, page, withRss: true), ct);
        }


        private async Task<RssVideoResponse> GetRssVideoResponseAsync(string url, CancellationToken ct)
        {
#if WINDOWS_UWP
            var text = await _context.GetStringAsync(url);
            {
                var feed = new SyndicationFeed();
                feed.Load(text);
                var items = new List<RssVideoData>();
                foreach (var item in feed.Items)
                {
                    items.Add(new RssVideoData()
                    {
                        RawTitle = item.Title.Text,
                        WatchPageUrl = item.Links[0].Uri,
                        Description = item.Summary.Text
                    });
                }

                return new RssVideoResponse()
                {
                    IsOK = true,
                    Items = items,
                    Language = feed.Language,
                    Link = feed.BaseUri
                };
            }
#else
            var text = await _context.GetStringAsync(url);
            using (var textReader = new StringReader(text))
            using (var xmlreader = XmlReader.Create(textReader))
            {
                var feed = SyndicationFeed.Load(xmlreader);
                var items = new List<RssVideoData>();
                foreach (var item in feed.Items)
                {
                    items.Add(new RssVideoData()
                    {
                        RawTitle = item.Title.Text,
                        WatchPageUrl = item.Links[0].Uri,
                        Description = item.Summary.Text
                    });
                }

                return new RssVideoResponse()
                {
                    IsOK = true,
                    Items = items,
                    Language = feed.Language,
                    Link = feed.BaseUri
                };
            }
#endif
        }

        /*
        public async Task<VideoRankingResponse> GetRankingFromWebAsync(RankingGenre genre, string tag = null, RankingTerm term = RankingTerm.Hour, int page = 1, CancellationToken ct = default)
        {
            string url = MakeRankingUrl(genre, tag, term, page);
            await _context.WaitPageAccessAsync();
            using var res = await _context.GetAsync(url, ct: ct);
            return await res.Content.ReadHtmlDocumentActionAsync(document =>
            {
                // ページ上の .RankingFilterTag となる要素を列挙する

                var videoListContainerElement = document.QuerySelector("body > div.BaseLayout-main > div.BaseRankingLayout-content.BaseLayout-block > div.SpecifiedRanking-main > div.RankingVideoListContainer");

                if (videoListContainerElement is null) { throw new NiconicoToolkitException("Failed parsing page on the web video ranking. missing 'class=\"RankingVideoListContainer\"'."); }


                int ParseCountText(string text)
                {
                    if (text is null)
                    {
                        return -1;
                    }
                    else if (text.EndsWith("万"))
                    {
#if NET6_0_OR_GREATER
                        return (int)(float.Parse(text.AsSpan(0, text.Length - 1)) * 10000);
#else 
                        return (int)(float.Parse(text.Substring(0, text.Length - 1)) * 10000);
#endif
                    }
                    else
                    {
                        return text.ToInt();
                    }
                }

                DateTime ParseTime(string text)
                {
                    if (text.EndsWith("時間前"))
                    {
#if NET6_0_OR_GREATER
                        return DateTime.Now + TimeSpan.FromHours(int.Parse(text.AsSpan(0, text.Length - 3)));
#else
                        return DateTime.Now + TimeSpan.FromHours(int.Parse(text.Substring(0, text.Length - 3)));
#endif
                    }
                    else if (text.EndsWith("分前"))
                    {
#if NET6_0_OR_GREATER
                        return DateTime.Now + TimeSpan.FromMinutes(int.Parse(text.AsSpan(0, text.Length - 2)));
#else
                        return DateTime.Now + TimeSpan.FromMinutes(int.Parse(text.Substring(0, text.Length - 2)));
#endif
                    }
                    else
                    {
                        return DateTime.Parse(text);
                    }
                }

                List<VideoRankingItem> resultItems = new();
                foreach (var videoElement in videoListContainerElement.Children.Where(x => x.GetAttribute("class")?.Contains("NC-VideoMediaObjectWrapper") ?? false))
                {
                    if (videoElement.HasAttribute("data-sensitive"))
                    {
                        var mediaObject = videoElement.LastElementChild;
                        var rank = mediaObject.QuerySelector("div.RankingRowRank");
                        resultItems.Add(new()
                        {
                            IsSensitiveContent = true,
                            VideoId = mediaObject.GetAttribute("data-video-id"),
                            OwnerId = videoElement.GetAttribute("data-owner-id"),
                            Rank = rank.TextContent.Trim('\n', ' ').ToInt(),
                        });
                    }
                    else
                    {
                        var mediaObject = videoElement.LastElementChild;
                        var content = mediaObject.QuerySelector("a");
                        var media = content.GetElementsByClassName("NC-MediaObject-media").FirstOrDefault();
                        var thumbnail = media.QuerySelector("div > div > div");
                        var thumbnailImage = thumbnail.GetElementsByClassName("NC-Thumbnail-image").FirstOrDefault();
                        var videoLength = thumbnail.GetElementsByClassName("NC-VideoLength").FirstOrDefault();
                        var body = content.GetElementsByClassName("NC-MediaObject-body").FirstOrDefault();
                        var bodyTitle = body.GetElementsByClassName("NC-MediaObject-bodyTitle").FirstOrDefault();
                        var bodySecondary = body.GetElementsByClassName("NC-MediaObject-bodySecondary").FirstOrDefault();
                        var description = bodySecondary.GetElementsByClassName("NC-VideoMediaObject-description").FirstOrDefault();
                        var meta = bodySecondary.GetElementsByClassName("NC-VideoMediaObject-meta").FirstOrDefault();
                        var metaAdditional = meta.GetElementsByClassName("NC-VideoMediaObject-metaAdditional").FirstOrDefault();
                        var metaCount = meta.GetElementsByClassName("NC-VideoMediaObject-metaCount").FirstOrDefault();
                        var rank = mediaObject.QuerySelector("div.RankingRowRank");
                        resultItems.Add(new()
                        {
                            Title = thumbnailImage.GetAttribute("aria-label"),
                            VideoId = mediaObject.GetAttribute("data-video-id"),
                            OwnerId = videoElement.GetAttribute("data-owner-id"),
                            Duration = videoLength.TextContent.ToTimeSpan(),
                            Thumbnail = thumbnailImage.GetAttribute("data-background-image"),
                            RegisteredAt = ParseTime(metaAdditional.QuerySelector("span > span").TextContent.Trim()),
                            Rank = rank.TextContent.Trim('\n', ' ').ToInt(),
                            ViewCount = ParseCountText(metaCount.GetElementsByClassName("NC-VideoMetaCount_view").FirstOrDefault()?.TextContent),
                            CommentCount = ParseCountText(metaCount.GetElementsByClassName("NC-VideoMetaCount_comment").FirstOrDefault()?.TextContent),
                            MylistCount = ParseCountText(metaCount.GetElementsByClassName("NC-VideoMetaCount_mylist").FirstOrDefault()?.TextContent),
                            LikeCount = ParseCountText(metaCount.GetElementsByClassName("NC-VideoMetaCount_like").FirstOrDefault()?.TextContent),
                            Description = description.TextContent,
                        });
                    }                    
                }

                return new VideoRankingResponse 
                {
                    Meta = new Meta() { Status = (int)HttpStatusCode.OK },
                    Items = resultItems, 
                };
            });
        }
        */

        public Task<VideoRankingResponse> GetRankingAsync(
            RankingGenre genre,
            RankingTerm term = RankingTerm.Hour,
            string tag = null,
            int? pageCount = null,
            CancellationToken ct = default)
        {
            var query = new NameValueCollection() { };

            if (pageCount is not null)
                query.Add("page", pageCount.ToString());

            if (genre == RankingGenre.HotTopic)
            {
                if (term != RankingTerm.Hour && term != RankingTerm.Day)
                    term = RankingTerm.Day;

                query.Add("term", term.GetDescription());

                if (tag is not null)
                    query.Add("key", tag);

                var url = new StringBuilder(NiconicoUrls.NvApiV1Url)
                    .Append("ranking/hot-topic")
                    .AppendQueryString(query)
                    .ToString();

                return _context.GetJsonAsAsync<VideoRankingResponse>(url ,_options, ct);
            }
            else
            {
                if (tag is not null)
                {
                    if (term != RankingTerm.Hour && term != RankingTerm.Day)
                        term = RankingTerm.Day;

                    query.Add("tag", tag);
                }

                query.Add("term", term.GetDescription());

                var url = new StringBuilder(NiconicoUrls.NvApiV1Url)
                    .Append($"ranking/genre/{genre.GetDescription()}")
                    .AppendQueryString(query)
                    .ToString();

                return _context.GetJsonAsAsync<VideoRankingResponse>(url, _options, ct);
            }
        }
    }
}
