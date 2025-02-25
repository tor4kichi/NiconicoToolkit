using NiconicoToolkit.Account;
using NiconicoToolkit.Channels;
using NiconicoToolkit.Mylist;
using NiconicoToolkit.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Web;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;
#else
using System.Net;
using System.Net.Http;
#endif

namespace NiconicoToolkit.Follow
{

    public sealed class FollowClient
    {
        private readonly NiconicoContext _context;
        private readonly JsonSerializerOptions _defaultOptions;

        public TagsFollowSubClient Tag { get; }
        public UserFollowSubClient User { get; }
        public MylistFollowSubClient Mylist { get; }
        public ChannelFollowSubClient Channel { get; }

        public FollowClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
        {
            _context = context;
            _defaultOptions = defaultOptions;

            Tag = new TagsFollowSubClient(this, _context, _defaultOptions);
            User = new UserFollowSubClient(this, _context, _defaultOptions);
            Mylist = new MylistFollowSubClient(this, _context, _defaultOptions);
            Channel = new ChannelFollowSubClient(this, _context, _defaultOptions);
        }


        internal static class Urls
        {
            public const string NvapiV1FollowingApiUrl = $"{NiconicoUrls.NvApiV1Url}users/me/following/";
            
            public const string PublicV1FollowingApiUrl = $"{NiconicoUrls.PublicApiV1Url}user/followees/";
            public const string UserFollowingApiUrl = $"{NiconicoUrls.UserApiV1Url}user/followees/";
        }


        public sealed class TagsFollowSubClient
        {
            private readonly FollowClient _followClient;
            private readonly NiconicoContext _context;
            private readonly JsonSerializerOptions _options;

            internal TagsFollowSubClient(FollowClient followClient, NiconicoContext context, JsonSerializerOptions options)
            {
                _followClient = followClient;
                _context = context;
                _options = options;
            }

            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public async Task<FollowTagsResponse> GetFollowTagsAsync()
            {
                var uri = $"https://nvapi.nicovideo.jp/v1/users/me/following/tags";
                await _context.PrepareCorsAsscessAsync(HttpMethod.Get, uri);
                return await _context.GetJsonAsAsync<FollowTagsResponse>(uri, _options);
            }


            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public Task<ContentManageResult> AddFollowTagAsync(string tag)
            {
                return _followClient.AddFollowInternalAsync($"{Urls.NvapiV1FollowingApiUrl}tags?tag={tag}");
            }

            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public Task<ContentManageResult> RemoveFollowTagAsync(string tag)
            {
                return _followClient.RemoveFollowInternalAsync($"{Urls.NvapiV1FollowingApiUrl}tags?tag={tag}");
            }

            /*
            public static Task<bool> IsFollowingTagAsync(string tag)
            {
                return _followClient..GetFollowedInternalAsync(context, $"https://nvapi.nicovideo.jp/v1/users/me/following/tags?tag={Uri.EscapeDataString(tag)}");
            }
            */

        }


        public sealed class UserFollowSubClient
        {
            private readonly FollowClient _followClient;
            private readonly NiconicoContext _context;
            private readonly JsonSerializerOptions _options;

            internal UserFollowSubClient(FollowClient followClient, NiconicoContext context, JsonSerializerOptions options)
            {
                _followClient = followClient;
                _context = context;
                _options = options;
            }


            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public async Task<FollowUsersResponse> GetFollowUsersAsync(int pageSize, FollowUsersResponse lastUserResponse = null)
            {
                var uri = $"{Urls.NvapiV1FollowingApiUrl}users?pageSize={pageSize}";
                if (lastUserResponse != null)
                {
                    uri += "&cursor=" + lastUserResponse.Data.Summary.Cursor;
                }

                await _context.PrepareCorsAsscessAsync(HttpMethod.Get, uri);
                return await _context.GetJsonAsAsync<FollowUsersResponse>(uri, _options);
            }

            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public Task<ContentManageResult> AddFollowUserAsync(UserId userId)
            {
                return _followClient.AddFollowInternalAsync($"{Urls.UserFollowingApiUrl}niconico-users/{userId}.json");
            }


            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public Task<ContentManageResult> RemoveFollowUserAsync(UserId userId)
            {
                return _followClient.RemoveFollowInternalAsync($"{Urls.UserFollowingApiUrl}niconico-users/{userId}.json");
            }

            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public Task<bool> IsFollowingUserAsync(UserId userId)
            {
                return _followClient.GetFollowedInternalAsync($"{Urls.UserFollowingApiUrl}niconico-users/{userId}.json");
            }
        }

        public sealed class MylistFollowSubClient
        {
            private readonly FollowClient _followClient;
            private readonly NiconicoContext _context;
            private readonly JsonSerializerOptions _options;

            internal MylistFollowSubClient(FollowClient followClient, NiconicoContext context, JsonSerializerOptions options)
            {
                _followClient = followClient;
                _context = context;
                _options = options;
            }

            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public async Task<FollowMylistResponse> GetFollowMylistsAsync(int sampleItemCount = 3)
            {
                var uri = $"{Urls.NvapiV1FollowingApiUrl}mylists?sampleItemCount={sampleItemCount}";
                await _context.PrepareCorsAsscessAsync(HttpMethod.Get, uri);
                return await _context.GetJsonAsAsync<FollowMylistResponse>(uri, _options);
            }

            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public Task<ContentManageResult> AddFollowMylistAsync(MylistId mylistId)
            {
                return _followClient.AddFollowInternalAsync($"{Urls.NvapiV1FollowingApiUrl}mylists/{mylistId}");
            }

            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public Task<ContentManageResult> RemoveFollowMylistAsync(MylistId mylistId)
            {
                return _followClient.RemoveFollowInternalAsync($"{Urls.NvapiV1FollowingApiUrl}mylists/{mylistId}");
            }

            /*
            public static Task<bool> IsFollowingMylistAsync(string mylistId)
            {
                return _followClient.GetFollowedInternalAsync(context, $"https://nvapi.nicovideo.jp/v1/users/me/following/mylists/{mylistId}");
            }
            */
        }


        public sealed class ChannelFollowSubClient
        {
            private readonly FollowClient _followClient;
            private readonly NiconicoContext _context;
            private readonly JsonSerializerOptions _options;

            internal ChannelFollowSubClient(FollowClient followClient, NiconicoContext context, JsonSerializerOptions options)
            {
                _followClient = followClient;
                _context = context;
                _options = options;
            }


            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public async Task<FollowChannelResponse> GetFollowChannelAsync(int offset = 0, int limit = 25)
            {
                var uri = $"{Urls.PublicV1FollowingApiUrl}channels.json?limit={limit}&offset={offset}";
                await _context.PrepareCorsAsscessAsync(HttpMethod.Get, uri);
                return await _context.GetJsonAsAsync<FollowChannelResponse>(uri, _options);
            }

            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public async Task<ChannelAuthorityResponse> GetChannelAuthorityAsync(ChannelId channelNumberId)
            {
                return await _context.GetJsonAsAsync<ChannelAuthorityResponse>(
                    $"{NiconicoUrls.PublicApiV1Url}channel/channelapp/channels/{channelNumberId.ToStringWithoutPrefix()}.json", _options
                    );
            }


            struct ChannelFollowApiInfo
            {
                public string AddApi { get; set; }
                public string DeleteApi { get; set; }
                public string Params { get; set; }
            }


            private async Task<ChannelFollowApiInfo> GetFollowChannelApiInfo(string channelId)
            {
                using var res = await _context.GetAsync(NiconicoUrls.MakeChannelPageUrl(channelId));

                return await res.Content.ReadHtmlDocumentActionAsync(document =>
                {
                    var bookmarkAnchorNode = document.QuerySelector("#head_cp_menu > div > div > div:nth-child(1) > a");
                    return new ChannelFollowApiInfo()
                    {
                        AddApi = bookmarkAnchorNode.Attributes["api_add"].Value,
                        DeleteApi = bookmarkAnchorNode.Attributes["api_delete"].Value,
                        Params = System.Net.WebUtility.HtmlDecode(bookmarkAnchorNode.Attributes["params"].Value)
                    };
                });
            }

            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public async Task<ChannelFollowResult> AddFollowChannelAsync(ChannelId channelId)
            {
                var apiInfo = await GetFollowChannelApiInfo(channelId);
                return await _context.GetJsonAsAsync<ChannelFollowResult>($"{apiInfo.AddApi}?{apiInfo.Params}", _options);
            }

            /// <remarks>[Require Login]</remarks>
            [RequireLogin]
            public async Task<ChannelFollowResult> DeleteFollowChannelAsync(ChannelId channelId)
            {
                var apiInfo = await GetFollowChannelApiInfo(channelId);
                return await _context.GetJsonAsAsync<ChannelFollowResult>($"{apiInfo.DeleteApi}?{apiInfo.Params}", _options);

            }
        }



        private async Task<bool> GetFollowedInternalAsync(string uri)
        {
            using var res = await _context.SendAsync(HttpMethod.Get, uri, content: null, headers =>
            {
                headers.Add("X-Request-With", "https://www.nicovideo.jp/my/follow");
            });

            var result = await res.Content.ReadJsonAsAsync<FollowedResultResponce>(_defaultOptions);
            return result.Data.IsFollowing;
        }

        private async Task<ContentManageResult> AddFollowInternalAsync(string uri)
        {
            await _context.PrepareCorsAsscessAsync(HttpMethod.Post, uri);
            using var res = await _context.SendAsync(HttpMethod.Post, uri, content: null, headers =>
            {
                headers.Add("X-Request-With", "https://www.nicovideo.jp");
            }
            , HttpCompletionOption.ResponseHeadersRead
            );
            return res.IsSuccessStatusCode ? ContentManageResult.Success : ContentManageResult.Failed;
        }

        private async Task<ContentManageResult> RemoveFollowInternalAsync(string uri)
        {
            //            await _context.PrepareCorsAsscessAsync(HttpMethod.Delete, uri);
            using var res = await _context.SendAsync(HttpMethod.Delete, uri, content: null, headers =>
            {
                headers.Add("X-Request-With", "https://www.nicovideo.jp");
            }
            , HttpCompletionOption.ResponseHeadersRead
            );
            return res.IsSuccessStatusCode ? ContentManageResult.Success : ContentManageResult.Failed;
        }
    }


}
