using NiconicoToolkit.Live.Notify;
using NiconicoToolkit.Live.WatchPageProp;
using NiconicoToolkit.Live.WatchSession;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace NiconicoToolkit.Live
{
    public sealed class LiveClient
    {
        private readonly NiconicoContext _context;

        internal LiveClient(NiconicoContext context, JsonSerializerOptions defaultOptions)
        {
            _context = context;
            CasApi = new Cas.CasLiveClient(context, defaultOptions);
            LiveNotify = new LiveNotifyClient(context, defaultOptions);

            _watchPageJsonSerializerOptions = NiconicoContext.DefaultOptionsSnakeCase;
        }

        JsonSerializerOptions _watchPageJsonSerializerOptions;

        public Cas.CasLiveClient CasApi { get; }
        public LiveNotifyClient LiveNotify { get; }


        public async Task<LiveWatchPageDataProp> GetLiveWatchPageDataPropAsync(LiveId liveId, CancellationToken ct = default)
        {
            await _context.WaitPageAccessAsync();

            using var res = await _context.GetAsync(NiconicoUrls.MakeLiveWatchPageUrl(liveId));
            return await res.Content.ReadHtmlDocumentActionAsync(document =>
            {
                var embeddedDataNode = document.QuerySelector("#embedded-data");
                var dataPropText = embeddedDataNode.GetAttribute("data-props");
                return JsonDeserializeHelper.Deserialize<LiveWatchPageDataProp>(dataPropText, _watchPageJsonSerializerOptions);
            });
        }        


        public static Live2WatchSession CreateWatchSession(LiveWatchPageDataProp prop, string userAgent)
        {
            bool isWatchTimeshift =
                prop.Program.Status == ProgramLiveStatus.ENDED 
                ;
            return new Live2WatchSession(prop.Site.Relive.WebSocketUrl, isWatchTimeshift, userAgent);
        }


        public static string MakeSeekedHLSUri(string hlsUri, TimeSpan position)
        {
            if (position > TimeSpan.FromSeconds(1))
            {
                return hlsUri += $"&start={position.TotalSeconds.ToString("F2")}";
            }
            else
            {
                return hlsUri;
            }
        }
    }
}
