using Microsoft.VisualStudio.TestTools.UnitTesting;
using NiconicoToolkit.Live;
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
    public sealed class TimeshiftReservationTest
    {
        NiconicoContext _context;


        [TestInitialize]
        public async Task Initialize()
        {
            (_context, _, _, _) = await AccountTestHelper.CreateNiconicoContextAndLogInWithTestAccountAsync();
        }


        [TestMethod]
        public async Task GetTimeshiftReservationsAsync()
        {
            var res = await _context.Timeshift.GetTimeshiftReservationsAsync();

            Assert.IsNotNull(res.User);
            Assert.IsNotNull(res.Reservations);

            foreach (var item in res.Reservations.Items.Take(3))
            {
                Assert.IsNotNull(item.ProgramId, "item.Id is null");
                Assert.IsNotNull(item.Program.Title, "item.Title is null");
            }
        }


        [TestMethod]
        public async Task ConnectLiveTimeshiftCommentSessionAsync()
        {
            var reservations = await _context.Timeshift.GetTimeshiftReservationsAsync();

            foreach (var reservation in reservations.Reservations.Items)
            {
                var liveId = reservation.ProgramId;
                var watchPageRes = await _context.Live.GetLiveWatchPageDataPropAsync(liveId);

                if (watchPageRes.Program.Status != Live.WatchPageProp.ProgramLiveStatus.ENDED 
                    || string.IsNullOrEmpty(watchPageRes.Site.Relive.WebSocketUrl)) 
                {
                    continue; 
                }

                using var session = LiveClient.CreateWatchSession(watchPageRes, _context.UserAgent);
                
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                async void Session_RecieveRoom(Live.WatchSession.Live2CurrentRoomEventArgs e)
                {
                    var (_, _, userId) = await _context.Account.GetCurrentSessionAsync();
                    var commentsession = e.CreateCommentClientForTimeshift(_context.UserAgent, userId.ToString(), reservation.Program.Schedule.OpenTime, reservation.Program.Schedule.EndTime);

                    Debug.WriteLine($"{nameof(reservation.Statistics.Comments)}:{reservation.Statistics.Comments}");
                    int commentCount = (int)reservation.Statistics.Comments;
                    commentsession.Connected += (s, commentArgs) =>
                    {
                        Debug.WriteLine($"{nameof(commentArgs.LastRes)}:{commentArgs.LastRes}");
                        if (commentCount + 1 == commentArgs.LastRes)
                        {
                            tcs.SetResult(true);
                        }
                    };
                    int count = 0;
                    commentsession.CommentReceived += (s, commentArgs) =>
                    {
                        Debug.WriteLine($"{count}: [{commentArgs.Chat.VideoPosition}] : {commentArgs.Chat.Content}");
                        count++;
                    };

                    try
                    {
                        await commentsession.OpenAsync(CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }

                session.RecieveRoom += Session_RecieveRoom;

                await session.StartWachingAsync(Live.WatchSession.LiveQualityType.Abr, isLowLatency: false);

                var result = await tcs.Task;
                Assert.IsTrue(result);

                break;
            }
        }

        [TestMethod]
        public async Task ConnectLiveTimeshiftCommentSessionWithSeekAsync()
        {
            var reservations = await _context.Timeshift.GetTimeshiftReservationsAsync();

            foreach (var reservation in reservations.Reservations.Items)
            {
                var liveId = reservation.ProgramId;
                var watchPageRes = await _context.Live.GetLiveWatchPageDataPropAsync(liveId);

                if (watchPageRes.Program.Status != Live.WatchPageProp.ProgramLiveStatus.ENDED
                    || string.IsNullOrEmpty(watchPageRes.Site.Relive.WebSocketUrl))
                {
                    continue;
                }

                using var session = LiveClient.CreateWatchSession(watchPageRes, _context.UserAgent);

                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                async void Session_RecieveRoom(Live.WatchSession.Live2CurrentRoomEventArgs e)
                {
                    var (_, _, userId) = await _context.Account.GetCurrentSessionAsync();
                    var commentsession = e.CreateCommentClientForTimeshift(_context.UserAgent, userId.ToString(), reservation.Program.Schedule.OpenTime, reservation.Program.Schedule.EndTime);

                    Debug.WriteLine($"{nameof(reservation.Statistics.Comments)}:{reservation.Statistics.Comments}");
                    int commentCount = (int)reservation.Statistics.Comments;
                    commentsession.Connected += (s, commentArgs) =>
                    {
                        Debug.WriteLine($"{nameof(commentArgs.LastRes)}:{commentArgs.LastRes}");
                        if (commentArgs.LastRes != 0)
                        {
                            tcs.SetResult(true);
                        }
                    };
                    int count = 0;
                    commentsession.CommentReceived += (s, commentArgs) =>
                    {
                        Debug.WriteLine($"{count}: [{commentArgs.Chat.VideoPosition}] : {commentArgs.Chat.Content}");
                        count++;
                    };

                    try
                    {
                        await commentsession.OpenAsync(CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }

                    commentsession.Seek(CancellationToken.None, reservation.Program.Schedule.EndTime - reservation.Program.Schedule.OpenTime - TimeSpan.FromSeconds(20));
                }

                session.RecieveRoom += Session_RecieveRoom;

                await session.StartWachingAsync(Live.WatchSession.LiveQualityType.Abr, isLowLatency: false);

                var result = await tcs.Task;
                Assert.IsTrue(result);

                break;
            }
        }
    }
}
