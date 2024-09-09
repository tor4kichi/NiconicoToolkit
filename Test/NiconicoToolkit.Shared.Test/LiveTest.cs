using Microsoft.VisualStudio.TestTools.UnitTesting;
using NiconicoToolkit.Live;
using NiconicoToolkit.SearchWithPage.Live;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NiconicoToolkit.Tests
{
    [TestClass]
    public sealed class LiveTest
    {
        [TestInitialize]
        public void Initialize()
        {
            _context = new NiconicoContext(AccountTestHelper.Site);
            _liveClient = _context.Live;
        }

        NiconicoContext _context;
        LiveClient _liveClient;


        [TestMethod]
        [DataRow("lv331925323")]
        public async Task CasApi_GetLiveProgramAsync(string liveId)
        {
            var res = await _liveClient.CasApi.GetLiveProgramAsync(liveId);

            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Meta);
            Assert.IsTrue(res.Meta.Status == 200);

            Assert.IsNotNull(res.Data);
            var data = res.Data;
            Assert.IsTrue(!string.IsNullOrWhiteSpace(data.Description));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(data.ProviderId));

            Assert.IsNotNull(data.ThumbnailUrl);
            Assert.IsNotNull(data.Timeshift);
        }

        [TestMethod]
        [DataRow("lv331925323")]
        [DataRow("lv337817244")]
        public async Task GetLiveWatchDataPropAsync(string liveId)
        {
            var res = await _liveClient.GetLiveWatchPageDataPropAsync(liveId);
        }


        //[TestMethod]
        public async Task ConnectLiveWatchSessionWithSearchResultAsync()
        {
            var query = LiveSearchOptionsQuery.Create("ゲーム", LiveStatus.Onair);
            var searchResult = await _context.SearchWithPage.Live.GetLiveSearchPageScrapingResultAsync(query, CancellationToken.None);
            var watchPageRes = await _liveClient.GetLiveWatchPageDataPropAsync(searchResult.Data.OnAirItems[0].LiveId);
            using (var session = LiveClient.CreateWatchSession(watchPageRes, _context.UserAgent))
            {
                var result = await session.StartWachingAsync(Live.WatchSession.LiveQualityType.Abr, isLowLatency: false);

                Assert.IsTrue(result);
            }
        }


        [TestMethod]
        public async Task ConnectLiveCommentSessionWithSearchResultAsync()
        {
            var query = LiveSearchOptionsQuery.Create("ゲーム", LiveStatus.Onair);
            var searchResult = await _context.Search.Live.LiveSearchAsync("ゲーム", null, Search.Live.Status.ON_AIR);
            //var searchResult = await _context.SearchWithPage.Live.GetLiveSearchPageScrapingResultAsync(query, CancellationToken.None);
            var watchPageRes = await _liveClient.GetLiveWatchPageDataPropAsync(searchResult.Items[0].ProgramId);
            using (var session = LiveClient.CreateWatchSession(watchPageRes, _context.UserAgent))
            {
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                async void Session_RecieveRoom(Live.WatchSession.Live2MessageServerEventArgs e)
                {                    
                    //await session.SendNotifyNewVisitAsync(true, true);
                    //var (_, _, userId) = await _context.Account.GetCurrentSessionAsync();                  
                    //var commentsession = e.CreateCommentClientForLiveStreaming(_context.UserAgent, userId.ToString());
                    //commentsession.Connected += (s, commentArgs) =>
                    //{
                    //    tcs.SetResult(true);
                    //};
                    //try
                    //{
                    //    await commentsession.OpenAsync(CancellationToken.None);
                    //}
                    //catch (Exception ex)
                    //{
                    //    tcs.SetException(ex);
                    //}
                }
                void Session_RecieveStatistics(Live.WatchSession.Live2StatisticsEventArgs e)
                {
                    tcs.SetResult(true);                    
                }

                session.MessageServer += Session_RecieveRoom;
                session.RecieveStatistics += Session_RecieveStatistics;
                await session.StartWachingAsync(Live.WatchSession.LiveQualityType.Abr, isLowLatency: false);

                var end = await Task.WhenAny(
                    tcs.Task,
                    Task.Delay(5000)
                    );

                var result = await tcs.Task;
                Assert.IsTrue(result);
            }
        }        
    }
}
