using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using System.Buffers;
using NiconicoToolkit.Live;
using NiconicoToolkit.Account;
using System.Text.Json.Serialization;
using NiconicoToolkit.User;
using NiconicoToolkit.Video;
using NiconicoToolkit.Activity;
using NiconicoToolkit.SearchWithPage;
using NiconicoToolkit.Recommend;
using NiconicoToolkit.Channels;
using NiconicoToolkit.Mylist;
using NiconicoToolkit.Follow;
using NiconicoToolkit.Series;
using NiconicoToolkit.Likes;
using NiconicoToolkit.Community;
using NiconicoToolkit.Ichiba;
using NiconicoToolkit.Live.Timeshift;
using NiconicoToolkit.SnapshotSearch;
using NiconicoToolkit.Search;
using System.IO;
using U8Xml;
using NiconicoToolkit.ExtApi.Video;
using NiconicoToolkit.FollowingsActivity;


#if WINDOWS_UWP
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#else
using System.Net.Http;
using System.Net.Http.Headers;
#endif

namespace NiconicoToolkit
{
    public sealed partial class NiconicoContext 
    {
        public string RawUserAgent { get; }
        public string UserAgent { get; }
        public NiconicoContext(string yourSiteUrl)
            : this(new HttpClient())
        {            
            RawUserAgent = yourSiteUrl;
            UserAgent = $"{nameof(NiconicoToolkit)}/1.0 (+{yourSiteUrl})";
            HttpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
        }

        internal static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions()
        {
            Converters =
            {
                new JsonStringEnumMemberConverter(),
                new NiconicoIdJsonConverter(),
                new UserIdJsonConverter(),
                new VideoIdJsonConverter(),
                new LiveIdJsonConverter(),
                new MylistIdJsonConverter(),
                new ChannelIdJsonConverter(),
                new CommunityIdJsonConverter(),
            },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        };

        internal static readonly JsonSerializerOptions DefaultOptionsSnakeCase = new JsonSerializerOptions()
        {
            Converters =
            {
                new JsonStringEnumMemberConverter(JsonSnakeCaseNamingPolicy.Instance),
                new NiconicoIdJsonConverter(),
                new UserIdJsonConverter(),
                new VideoIdJsonConverter(),
                new LiveIdJsonConverter(),
                new MylistIdJsonConverter(),
                new ChannelIdJsonConverter(),
                new CommunityIdJsonConverter(),
            }
        };


        public NiconicoContext(
            HttpClient httpClient
            )
        {
            HttpClient = httpClient;            
            Live = new LiveClient(this, DefaultOptions);
            Account = new AccountClient(this);
            User = new UserClient(this, DefaultOptions);
            Video = new VideoClient(this, DefaultOptions);
            History = new HistoryClient(this, DefaultOptions);
            Search = new SearchClient(this, DefaultOptions);
            SearchWithPage = new SearchWithPageClient(this);
            Recommend = new RecommendClient(this, DefaultOptions);
            Channel = new ChannelClient(this, DefaultOptions);
            Mylist = new MylistClient(this, DefaultOptions);
            Follow = new FollowClient(this, DefaultOptions);
            Series = new SeriesClient(this, DefaultOptions);
            FollowingsActivity = new FollowingsActivityClient(this, DefaultOptions);
            Likes = new LikesClient(this, DefaultOptions);
            Community = new CommunityClient(this, DefaultOptions);
            Ichiba = new IchibaClient(this, DefaultOptions);
            Timeshift = new TimeshiftClient(this, DefaultOptions);
            VideoSnapshotSearch = new VideoSnapshotSearchClient(this, DefaultOptions);
            ExtApiClient = new ExtApiClient(this);
        }


        public HttpClient HttpClient { get; }

        public AccountClient Account { get; }
        public LiveClient Live { get; }
        public UserClient User { get; }
        public VideoClient Video { get; }
        public HistoryClient History { get; }
        public SearchClient Search { get; }
        public SearchWithPageClient SearchWithPage { get; }
        public RecommendClient Recommend { get; }
        public ChannelClient Channel { get; }
        public MylistClient Mylist { get; }
        public FollowClient Follow { get; }
        public SeriesClient Series { get; }
        public FollowingsActivityClient FollowingsActivity { get; }
        public LikesClient Likes { get; }
        public CommunityClient Community { get; }
        public IchibaClient Ichiba { get; }
        public TimeshiftClient Timeshift { get; }
        public VideoSnapshotSearchClient VideoSnapshotSearch { get; }
        public ExtApiClient ExtApiClient { get; }

        TimeSpan _minPageAccessInterval = TimeSpan.FromSeconds(1);
        DateTime _prevPageAccessTime;

        internal async ValueTask WaitPageAccessAsync()
        {
            var now = DateTime.Now;
            var elapsedTime = now - _prevPageAccessTime;
            _prevPageAccessTime = now + _minPageAccessInterval;
            if (elapsedTime < _minPageAccessInterval)
            {
                await Task.Delay(_minPageAccessInterval - elapsedTime);
            }
        }

        public void SetupDefaultRequestHeaders()
        {
            HttpClient.DefaultRequestHeaders.Add("Referer", "https://www.nicovideo.jp/");
            HttpClient.DefaultRequestHeaders.Add("X-Frontend-Id", "6");
            HttpClient.DefaultRequestHeaders.Add("X-Frontend-Version", "0");
            HttpClient.DefaultRequestHeaders.Add("X-Niconico-Language", "ja-jp");

            HttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            HttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            HttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");
            HttpClient.DefaultRequestHeaders.Add("X-Request-With", "https://www.nicovideo.jp");

            HttpClient.DefaultRequestHeaders.Add("Origin", "https://www.nicovideo.jp");
        }

        #region 

        internal async Task PrepareCorsAsscessAsync(HttpMethod httpMethod, string uri)
        {
            using var _ =  await SendAsync(httpMethod, uri, content: null, headers => 
            {
                headers.Add("Access-Control-Request-Headers", "x-frontend-id,x-frontend-version,x-niconico-language,x-request-with");
                headers.Add("Access-Control-Request-Method", httpMethod.Method);
            }, HttpCompletionOption.ResponseHeadersRead);
        }


#if WINDOWS_UWP
        internal Task<HttpResponseMessage> GetAsync(string path, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken ct = default)
        {
            return HttpClient.GetAsync(new Uri(path), completionOption).AsTask(ct);
        }

        internal Task<HttpResponseMessage> GetAsync(Uri path, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken ct = default)
        {
            return HttpClient.GetAsync(path, completionOption).AsTask(ct);
        }
#else
        internal Task<HttpResponseMessage> GetAsync(string path, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken ct = default)
        {
            return HttpClient.GetAsync(path, completionOption, ct);
        }

        internal Task<HttpResponseMessage> GetAsync(Uri path, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken ct = default)
        {
            return HttpClient.GetAsync(path, completionOption, ct);
        }
#endif

        internal Task<string> GetStringAsync(string path, CancellationToken ct = default)
        {
            return GetStringAsync(new Uri(path), ct);
        }

        internal async Task<string> GetStringAsync(Uri path, CancellationToken ct = default)
        {
#if WINDOWS_UWP
            return await HttpClient.GetStringAsync(path).AsTask(ct);
#else
            return await HttpClient.GetStringAsync(path, ct);
#endif
        }

        internal Task<T> GetJsonAsAsync<T>(string path, JsonSerializerOptions options = null, CancellationToken ct = default)
        {
            return GetJsonAsAsync<T>(new Uri(path), options, ct);
        }

        internal async Task<T> GetJsonAsAsync<T>(Uri path, JsonSerializerOptions options = null, CancellationToken ct = default)
        {
#if WINDOWS_UWP
            using var res = await HttpClient.GetAsync(path).AsTask(ct);
#else
            using var res = await HttpClient.GetAsync(path, ct);
#endif
            return await res.Content.ReadJsonAsAsync<T>(options, ct);
        }


        internal Task<T> GetXmlAsAsync<T>(string path, CancellationToken ct)
        {
            return GetXmlAsAsync<T>(new Uri(path), ct);
        }

        internal async Task<T> GetXmlAsAsync<T>(Uri path, CancellationToken ct)
        {
#if WINDOWS_UWP
            using var res = await HttpClient.GetAsync(path).AsTask(ct);
#else
            using var res = await HttpClient.GetAsync(path, ct);
#endif
            return await res.Content.ReadXmlAsAsync<T>(ct);
        }



#if WINDOWS_UWP
        internal Task<HttpResponseMessage> PostAsync(string path, CancellationToken ct = default)
        {
            return HttpClient.PostAsync(new Uri(path), null).AsTask(ct);
        }

        internal Task<HttpResponseMessage> PostAsync(string path, IHttpContent httpContent, CancellationToken ct = default)
        {
            return HttpClient.PostAsync(new Uri(path), httpContent).AsTask(ct);
        }

        internal Task<HttpResponseMessage> PostAsync(Uri path, IHttpContent httpContent, CancellationToken ct = default)
        {
            return HttpClient.PostAsync(path, httpContent).AsTask(ct);
        }


        internal async Task<HttpResponseMessage> PostAsync(string path, IEnumerable<KeyValuePair<string, string>> form, CancellationToken ct = default)
        {
            using var content = new HttpFormUrlEncodedContent(form);
            return await HttpClient.PostAsync(new Uri(path), content).AsTask(ct);
        }

        internal async Task<HttpResponseMessage> PostAsync(Uri path, IEnumerable<KeyValuePair<string, string>> form, CancellationToken ct = default)
        {
            using var content = new HttpFormUrlEncodedContent(form);
            return await HttpClient.PostAsync(path, content).AsTask(ct);
        }

#else
        internal Task<HttpResponseMessage> PostAsync(string path, CancellationToken ct = default)
        {
            return HttpClient.PostAsync(new Uri(path), null);
        }

        internal Task<HttpResponseMessage> PostAsync(string path, HttpContent httpContent, CancellationToken ct = default)
        {
            return HttpClient.PostAsync(path, httpContent, ct);
        }
        internal Task<HttpResponseMessage> PostAsync(Uri path, HttpContent httpContent, CancellationToken ct = default)
        {
            return HttpClient.PostAsync(path, httpContent, ct);
        }

        internal Task<HttpResponseMessage> PostAsync(string path, IEnumerable<KeyValuePair<string, string>> form, CancellationToken ct = default)
        {
            var content = new FormUrlEncodedContent(form);
            return HttpClient.PostAsync(path, content, ct);
        }
        internal Task<HttpResponseMessage> PostAsync(Uri path, IEnumerable<KeyValuePair<string, string>> form, CancellationToken ct = default)
        {
            var content = new FormUrlEncodedContent(form);
            return HttpClient.PostAsync(path, content, ct);
        }
#endif


#if WINDOWS_UWP

        internal Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod, string path, IHttpContent content = null, Action<HttpRequestHeaderCollection> headerFiller = null, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken ct = default)
        {
            return SendAsync(httpMethod, new Uri(path), content, headerFiller, completionOption, ct);
        }

        internal async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod, Uri path, IHttpContent content = null, Action<HttpRequestHeaderCollection> headerFiller = null, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(httpMethod, path);
            headerFiller?.Invoke(req.Headers);
            req.Content = content;
            return await HttpClient.SendRequestAsync(req, completionOption).AsTask(ct);
        }
#else
        internal Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod, string path, HttpContent content = null, Action<HttpRequestHeaders> headerFiller = null, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken ct = default)
        {
            return SendAsync(httpMethod, new Uri(path), content, headerFiller, completionOption, ct);
        }

        internal async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod, Uri path, HttpContent content, Action<HttpRequestHeaders> headerFiller = null, HttpCompletionOption completionOption = HttpCompletionOption.ResponseHeadersRead, CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(httpMethod, path);
            req.Content = content;
            headerFiller?.Invoke(req.Headers);
            return await HttpClient.SendAsync(req, completionOption, ct);
        }
#endif

#if WINDOWS_UWP
        internal Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, string url, JsonSerializerOptions options = null, Action<HttpRequestHeaderCollection> headerModifier = null, CancellationToken ct = default)
#else
        internal Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, string url, JsonSerializerOptions options = null, Action<System.Net.Http.Headers.HttpRequestHeaders> headerModifier = null, CancellationToken ct = default)
#endif
        {
            return SendJsonAsAsync<T>(httpMethod, url, httpContent: null, options, headerModifier, ct);
        }



#if WINDOWS_UWP
        internal async Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, string url, string stringHttpContent, JsonSerializerOptions options = null, Action<HttpRequestHeaderCollection> headerModifier = null, CancellationToken ct = default)
#else
        internal async Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, string url, string stringHttpContent, JsonSerializerOptions options = null, Action<System.Net.Http.Headers.HttpRequestHeaders> headerModifier = null, CancellationToken ct = default)
#endif
        {
#if WINDOWS_UWP
            using var content = new HttpStringContent(stringHttpContent);
#else
            using var content = new StringContent(stringHttpContent);
#endif
            return await SendJsonAsAsync<T>(httpMethod, url, content, options, headerModifier, ct);
        }



#if WINDOWS_UWP
        internal Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, string url, Dictionary<string, string> pairs, JsonSerializerOptions options = null, Action<HttpRequestHeaderCollection> headerModifier = null, CancellationToken ct = default)
#else
        internal Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, string url, Dictionary<string, string> pairs, JsonSerializerOptions options = null, Action<System.Net.Http.Headers.HttpRequestHeaders> headerModifier = null, CancellationToken ct = default)
#endif
        {
            return SendJsonAsAsync<T>(httpMethod, new Uri(url), pairs, options, headerModifier, ct);
        }

#if WINDOWS_UWP
        internal async Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, Uri url, Dictionary<string, string> pairs, JsonSerializerOptions options = null, Action<HttpRequestHeaderCollection> headerModifier = null, CancellationToken ct = default)
#else
        internal async Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, Uri url, Dictionary<string, string> pairs, JsonSerializerOptions options = null, Action<System.Net.Http.Headers.HttpRequestHeaders> headerModifier = null, CancellationToken ct = default)
#endif
        {
#if WINDOWS_UWP
            using var content = new HttpFormUrlEncodedContent(pairs);
#else
            using var content = new FormUrlEncodedContent(pairs);
#endif
            return await SendJsonAsAsync<T>(httpMethod, url, content, options, headerModifier, ct);
        }


#if WINDOWS_UWP
        internal async Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, string url, IHttpContent httpContent, JsonSerializerOptions options = null, Action<HttpRequestHeaderCollection> headerModifier = null, CancellationToken ct = default)
#else
        internal async Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, string url, HttpContent httpContent = null, JsonSerializerOptions options = null, Action<System.Net.Http.Headers.HttpRequestHeaders> headerModifier = null, CancellationToken ct = default)
#endif
        {
            using var message = await SendAsync(httpMethod, new Uri(url), httpContent, headerModifier, ct: ct);
            return await message.Content.ReadJsonAsAsync<T>(options, ct: ct);
        }

#if WINDOWS_UWP
        internal async Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, Uri url, IHttpContent httpContent, JsonSerializerOptions options = null, Action<HttpRequestHeaderCollection> headerModifier = null, CancellationToken ct = default)
#else
        internal async Task<T> SendJsonAsAsync<T>(HttpMethod httpMethod, Uri url, HttpContent httpContent = null, JsonSerializerOptions options = null, Action<System.Net.Http.Headers.HttpRequestHeaders> headerModifier = null, CancellationToken ct = default)
#endif
        {
            using var message = await SendAsync(httpMethod, url, httpContent, headerModifier, ct: ct);
            return await message.Content.ReadJsonAsAsync<T>(options, ct: ct);
        }

        #endregion
    }

}
