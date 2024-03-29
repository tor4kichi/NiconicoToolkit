﻿using NiconicoToolkit.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#else
using System.Net.Http;
using System.Net.Http.Headers;
#endif

namespace NiconicoToolkit.Likes
{
    public sealed class LikesClient
    {
        private readonly NiconicoContext _context;
        private readonly JsonSerializerOptions _options;

        internal LikesClient(NiconicoContext context, System.Text.Json.JsonSerializerOptions options)
        {
            _context = context;
            _options = options;
        }

        internal static class Urls
        {
            public const string NvApiV1LikeApiUrl = $"{NiconicoUrls.NvApiV1Url}users/me/likes";
            public const string NvApiV1LikeVideoItemsApiUrl = $"{NvApiV1LikeApiUrl}/items?videoId=";
            
        }

        /// <remarks>[Require Login]</remarks>
        [RequireLogin]
        public async Task<LikeActionResponse> DoLikeVideoAsync(string videoId)
        {
            return await _context.SendJsonAsAsync<LikeActionResponse>(httpMethod: HttpMethod.Post, Urls.NvApiV1LikeVideoItemsApiUrl + videoId, httpContent: null, options: _options);
        }

        /// <remarks>[Require Login]</remarks>
        [RequireLogin]
        public async Task<LikeActionResponse> UnDoLikeVideoAsync(string videoId)
        {
            return await _context.SendJsonAsAsync<LikeActionResponse>(httpMethod: HttpMethod.Delete, Urls.NvApiV1LikeVideoItemsApiUrl + videoId, httpContent: null, options: _options);
        }

        /// <remarks>[Require Login]</remarks>
        [RequireLogin]
        public async Task<LikesListResponse> GetLikesAsync(int page, int pageSize)
        {
            return await _context.GetJsonAsAsync<LikesListResponse>($"{Urls.NvApiV1LikeApiUrl}?pageSize={pageSize}&page={page + 1}", options: _options);
        }
    }


    public sealed class LikeActionResponse : ResponseWithMeta
    {
        [JsonPropertyName("data")]
        public LikesResponseData Data { get; set; }

        public string ThanksMessage => Data?.ThanksMessage;

        public sealed class LikesResponseData
        {
            [JsonPropertyName("thanksMessage")]
            public string ThanksMessage { get; set; }
        }
    }

    public sealed class LikesListResponse : ResponseWithMeta
    {
        [JsonPropertyName("data")]
        public LikesListResponseData Data { get; set; }


        public sealed class LikesListResponseData
        {
            [JsonPropertyName("items")]
            public VideoLikesItem[] Items { get; set; }

            [JsonPropertyName("summary")]
            public PagenationInfo PageInfo { get; set; }
        }

        public sealed class VideoLikesItem
        {
            [JsonPropertyName("likedAt")]
            public DateTimeOffset LikedAt { get; set; }

            [JsonPropertyName("thanksMessage")]
            public string ThanksMessage { get; set; }

            [JsonPropertyName("video")]
            public NvapiVideoItem Video { get; set; }

            [JsonPropertyName("status")]
            public string Status { get; set; }
        }


        public sealed class PagenationInfo
        {
            [JsonPropertyName("hasNext")]
            public bool HasNext { get; set; }

            [JsonPropertyName("canGetNextPage")]
            public bool CanGetNextPage { get; set; }

            [JsonPropertyName("getNextPageNgReason")]
            public string GetNextPageNgReason { get; set; }
        }
    }

}
