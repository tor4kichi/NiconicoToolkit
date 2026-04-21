using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;

namespace NiconicoToolkit.Channels
{
    public sealed class ChannelClient
    {
        private readonly NiconicoContext _context;
        private readonly JsonSerializerOptions _options;

        public ChannelClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
        {
            _context = context;
            _options = defaultOptions;
        }

        internal static class Urls
        {

        }



        public enum ChannelAdmissionAdditinals
        {
            [Description("channelMemberProduct")]
            ChannelMemberProduct,
        }


        public async Task<ChannelAdmissionResponse> GetChannelAdmissionAsync(ChannelId channelId, params ChannelAdmissionAdditinals[] additinals)
        {
            NameValueCollection dict = new NameValueCollection() 
            {
                { "_frontendId",  "6" },
            };

            foreach (var add in additinals)
            {
                dict.Add("additionalResources", add.GetDescription());
            }

            var url = new StringBuilder(NiconicoUrls.ChannelPublicApiV2Url)
                .Append("open/channels/")
                .Append(channelId.ToStringWithoutPrefix())
                .AppendQueryString(dict)
                .ToString();

            return await _context.GetJsonAsAsync<ChannelAdmissionResponse>(url, _options);
        }

        public const int OneTimeItemsCountOnGetChannelVideoAsync = 20;

        public Task<ChannelVideoResponse> GetChannelVideoAsync(ChannelId channelId, int page, ChannelVideoSortKey? sortKey = null, ChannelVideoSortOrder? sortOrder = null)
        {
            return GetChannelVideoAsync_Internal(NiconicoUrls.MakeChannelPageUrl(channelId), page, sortKey, sortOrder);
        }


        public Task<ChannelVideoResponse> GetChannelVideoAsync(string channelIdOrScreenName, int page, ChannelVideoSortKey? sortKey = null, ChannelVideoSortOrder? sortOrder = null)
        {
            return GetChannelVideoAsync_Internal(NiconicoUrls.MakeChannelPageUrl(channelIdOrScreenName), page, sortKey, sortOrder);
        }

        async Task<ChannelVideoResponse> GetChannelVideoAsync_Internal(string channelPageUrl, int page, ChannelVideoSortKey? sortKey, ChannelVideoSortOrder? sortOrder)
        {
            var dict = new NameValueCollection() { { "page", (page + 1).ToString() } };

            if (sortKey is not null) { dict.Add("sort", sortKey.Value.GetDescription()); }
            if (sortOrder is not null) { dict.Add("order", sortOrder.Value.GetDescription()); }

            var url = new StringBuilder(channelPageUrl)
                .Append("/video")
                .AppendQueryString(dict)
                .ToString();

            await _context.WaitPageAccessAsync();

            using var res = await _context.GetAsync(url);

            ChannelVideoResponse channelVideoResponse = new()
            {
                Meta = new Meta() { Status = (int)res.StatusCode }
            };
            
            if (!res.IsSuccessStatusCode) { return channelVideoResponse; }

            return await res.Content.ReadHtmlDocumentActionAsync(document => 
            {
                static string GetTitle(IHtmlDocument doc)
                {
                    return doc.QuerySelector("#head_cp_breadcrumb > h1 > a").TextContent;
                }
                // 件数
                static int GetCount(IHtmlDocument document)
                {                    
                    try
                    {
                        var countNode = document.QuerySelector("#video_page > section > article > section > div > div > span > var");
                        return countNode?.TextContent.ToInt() ?? 0;
                    }
                    catch
                    {
                        return 0;
                    }
                }

                static IEnumerable<ChannelVideoItem> GetChannelVideos(IHtmlDocument document)
                {                    
                    var itemNodes = document.QuerySelectorAll("#video_page > section > article > section > ul > li");
                    foreach (var itemNode in itemNodes)
                    {
                        ChannelVideoItem item = new();
                        var anchorNode = itemNode.QuerySelector("a");
                        {
                            var href = anchorNode.GetAttribute("href");
                            if (href != null
                            && href.LastIndexOf('/') is int lastSlashPos
                            && lastSlashPos != -1)
                            {
                                item.ItemId = href.Substring(lastSlashPos + 1);
                            }
                            var imageNode = anchorNode.QuerySelector("div.thumbnail > img");

                            item.ThumbnailUrl = imageNode.GetAttribute("src");
                            //item.CommentSummary = lastResNode?.TextContent ?? string.Empty;
                        }

                        var itemInfoNode = anchorNode.QuerySelector(".metadata");
                        {
                            var titleNode = itemInfoNode.QuerySelector("h3");
                            var countsNode = itemInfoNode.QuerySelectorAll("ul > li");

                            item.Title = titleNode.TextContent.Trim();
                            item.ViewCount = itemInfoNode.QuerySelector("dl > div:nth-child(1) > dd").TextContent.ToInt();
                            item.CommentCount = itemInfoNode.QuerySelector("dl > div:nth-child(2) > dd").TextContent.ToInt();
                            item.MylistCount = itemInfoNode.QuerySelector("dl > div:nth-child(3) > dd").TextContent.ToInt();

                            item.PostedAt = itemInfoNode.QuerySelector("p:nth-child(4)").TextContent.ToDateTimeOffsetFromIso8601().DateTime;

                        }
                        var thumbnialNode = anchorNode.QuerySelector(".thumbnail");
                        {
                            item.Length = thumbnialNode.QuerySelector("span[data-style='videoLength'] > strong").TextContent.ToTimeSpan();
                            var ppv = thumbnialNode.QuerySelector("span.c-labelPaymentType > strong");
                            if (ppv != null && ppv.TextContent is { } token)
                            {
                                switch (token)
                                {
                                    case "有料": item.IsRequirePayment = true;break;
                                    case "all_pay": item.IsRequirePayment = true; break;
                                    case "会員無料": item.IsFreeForMember = true; break;
                                    case "member_unlimited_access": item.IsMemberUnlimitedAccess = true; break;
                                    case "無料": break;
                                }
                            }
                        }
                        yield return item;
                    }
                }

                channelVideoResponse.Data = new()
                {
                    Page = page,
                    Title = GetTitle(document),
                    TotalCount = GetCount(document),
                    Videos = GetChannelVideos(document).ToArray(),
                };

                return channelVideoResponse;
            });
        }
    }
}
