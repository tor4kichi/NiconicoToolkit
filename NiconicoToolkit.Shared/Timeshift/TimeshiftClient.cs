using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AngleSharp.Dom;
using System.Text.RegularExpressions;
using U8Xml;
using AngleSharp.Html.Dom;
#if WINDOWS_UWP
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#else
using System.Net.Http;
using System.Net.Http.Headers;
#endif

namespace NiconicoToolkit.Live.Timeshift
{
    public sealed class TimeshiftClient
    {
        private readonly NiconicoContext _context;
        private readonly JsonSerializerOptions _defaultOptions;
        private readonly JsonSerializerOptions _timeshiftReservationOptions;

        internal TimeshiftClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
        {
            _context = context;
            _defaultOptions = defaultOptions;


            _timeshiftReservationOptions = new JsonSerializerOptions(defaultOptions)
            {
                Converters = 
                {
                    new JsonStringEnumMemberConverter(new JsonSnakeCaseNamingPolicy(), false),
                    new DateTimeOffsetConverterUsingDateTimeParse()
                }
            };
        }


        internal static class Urls
        {
            public const string TimeshiftReservationApiUrl = $"{NiconicoUrls.NicoLivePageUrl}api/timeshift.reservations";

            public const string MyTimeshiftReservationsEmbedPageUrl = $"{NiconicoUrls.NicoLivePageUrl}embed/timeshift-reservations";

            public const string MyTimeshiftPageUrl = $"{NiconicoUrls.NicoLivePageUrl}my"; //.php";

            public const string TimeshiftWachingReservationApiUrl = $"https://live.nicovideo.jp/api/watchingreservation";
        }


        /// <remarks>[Require Login]</remarks>
        [RequireLogin]
        public Task<ReserveTimeshiftResponse> ReserveTimeshiftAsync(LiveId liveId, bool overwrite)
        {
            var dict = new NameValueCollection()
            {
                { "vid", liveId.ToStringWithoutPrefix() },
                { "overwrite", overwrite.ToString1Or0() },
            };

            return _context.SendJsonAsAsync<ReserveTimeshiftResponse>(httpMethod: HttpMethod.Post, Urls.TimeshiftReservationApiUrl, httpContent: null);
        }

        /// <summary>
        /// [Require Login] タイムシフト予約の削除用トークンを取得します。
        /// </summary>
        /// <returns></returns>
        /// [RequireLogin]
        public async Task<ReservationToken> GetReservationTokenAsync()
        {
            try
            {
                using var res = await _context.GetAsync(Urls.MyTimeshiftReservationsEmbedPageUrl);

                return await res.Content.ReadHtmlDocumentActionAsync(document =>
                {
                    var tokenNode = document.QuerySelector("input#confirm");
                    return new ReservationToken(tokenNode.GetAttribute("value"));
                });
            }
            catch
            {
                return ReservationToken.InavalidToken;
            }
        }

        /// <remarks>[Require Login]</remarks>
        [RequireLogin]
        public Task DeleteTimeshiftReservationAsync(LiveId liveId, ReservationToken reservationDeleteToken)
        {
            return DeleteTimeshiftReservationAsync(new string[] { liveId }, reservationDeleteToken);
        }

        /// <remarks>[Require Login]</remarks>
        [RequireLogin]
        public async Task DeleteTimeshiftReservationAsync(IEnumerable<string> liveIds, ReservationToken reservationDeleteToken)
        {
            if (ReservationToken.InavalidToken == reservationDeleteToken)
            {

            }

            var dict = new NameValueCollection()
            {
                { "delete", "timeshift" },
                { "confirm", reservationDeleteToken.Token },
            };

            foreach (var liveId in liveIds)
            {
                var nonPrefixLiveId = ContentIdHelper.RemoveContentIdPrefix(liveId);
                dict.Add("vid[]", nonPrefixLiveId);
            }

            var url = new StringBuilder(Urls.MyTimeshiftPageUrl)
                .AppendQueryString(dict)
                .ToString();

            using var _ = await _context.PostAsync(url);
        }

        /// <remarks>[Require Login]</remarks>
        [RequireLogin]
        public async Task<TimeshiftReservationsResponse> GetTimeshiftReservationsAsync(System.Threading.CancellationToken ct = default)
        {
            using var res = await _context.GetAsync(Urls.MyTimeshiftReservationsEmbedPageUrl, ct: ct);
            return await res.Content.ReadHtmlDocumentActionAsync((doc) => 
            {
                var embededdData = doc.DocumentElement
                    .FindChild<IHtmlBodyElement>()
                    .Children.First(x => x.GetAttribute("id") == "embedded-data")
                    .GetAttribute("data-props");
                return JsonSerializer.Deserialize<TimeshiftReservationsResponse>(embededdData, _timeshiftReservationOptions);
            });
        }


        /// <remarks>[Require Login]</remarks>
        [RequireLogin]
        public async Task<UseTimeshiftViewingAuthorityResponse> UseTimeshiftViewingAuthorityAsync(string vid, ReservationToken token)
        {
            var nonPrefixLiveId = ContentIdHelper.RemoveContentIdPrefix(vid);

            var dict = new Dictionary<string, string>()
            {
                { "accept", "true" },
                { "mode", "use" },
                { "vid", nonPrefixLiveId },
                { "token", token.Token},
            };

            using var res = await _context.PostAsync(Urls.TimeshiftWachingReservationApiUrl, dict);
            return ResponseWithMeta.CreateFromStatusCode<UseTimeshiftViewingAuthorityResponse>(res.StatusCode);
        }
    }


}
