using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using NiconicoToolkit.Live.WatchSession;
using NiconicoToolkit.Live.WatchSession.ToClientMessage;
using NiconicoToolkit.Live.WatchSession.Events;
using System.Net.WebSockets;

namespace NiconicoToolkit.Live.WatchSession
{
    using AsyncLock = NeoSmart.AsyncLock.AsyncLock;

    public static class NiwavidedNicoLiveMessageHelper
    {
        public static string MakeNiwavidedMessage(int messageCount, string content)
        {
            var rs = messageCount;
            var ps = messageCount * 5;
            var pf = messageCount * 5;
            var rf = messageCount;
            return $"[{{\"ping\":{{\"content\":\"rs:{rs}\"}}}},{{\"ping\":{{\"content\":\"ps:{ps}\"}}}},{content},{{\"ping\":{{\"content\":\"pf:{pf}\"}}}},{{\"ping\":{{\"content\":\"rf:{rf}\"}}}}]";
        }
    }


    public sealed class LiveCommentSession : IDisposable
    {
        public static LiveCommentSession CreateForLiveStream(string messageServerUrl, string threadId, string userId, string userAgent)
        {
            return new LiveCommentSession(messageServerUrl, threadId, userId, userAgent);
        }

        public static LiveCommentSession CreateForTimeshift(string messageServerUrl, string threadId, string userId, string userAgent, string waybackkey, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            return new LiveCommentSession(messageServerUrl, threadId, userId, userAgent, waybackkey, startTime, endTime);
        }


        public event EventHandler<CommentPostedEventArgs> CommentPosted;
        public event EventHandler<CommentReceivedEventArgs> CommentReceived;
        public event EventHandler<CommentServerConnectedEventArgs> Connected;
        public event EventHandler<CommentServerDisconnectedEventArgs> Disconnected;



        const uint FirstGetRecentMessageCount = 50;

        private ClientWebSocket _ws;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly AsyncLock _CommentSessionLock = new AsyncLock();

        public string MessageServerUrl { get; }
        public string UserId { get; }
        public string ThreadId { get; }

        private string _ticket;
        private int _MessageSendCount = 0;

        public bool IsTimeshift { get; }
        private int _LastRes;
        private readonly string _waybackkey;
        private readonly DateTimeOffset _startTime;
        private readonly DateTimeOffset _endTime;

        private LiveCommentSession(string messageServerUrl, string threadId, string userId, string userAgent, string waybackkey, DateTimeOffset startTime, DateTimeOffset endTime)
            : this(messageServerUrl, threadId, userId, userAgent)
        {
            IsTimeshift = true;
            _waybackkey = waybackkey;
            _startTime = startTime;
            _endTime = endTime;
        }


        private LiveCommentSession(string messageServerUrl, string threadId, string userId, string userAgent)
        {
            MessageServerUrl = messageServerUrl;
            ThreadId = threadId;
            UserId = userId;
            _userAgent = userAgent;
            
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                Converters =
                {
                    new JsonStringEnumMemberConverter(),
                    new CommentSessionToClientMessageJsonConverter(),
                    new VideoPositionToTimeSpanConverter(),
                }
            };
        }

        private ClientWebSocket CreateWebSocket()
        {
            var ws = new ClientWebSocket();
            ws.Options.AddSubProtocol("msg.nicovideo.jp#json");
            ws.Options.SetRequestHeader("User-Agent", _userAgent);
            return ws;
        }

        private void _ws_MessageReceived(ReadOnlySpan<byte> messageBytes)
        {
            var message = JsonSerializer.Deserialize<CommentSessionToClientMessage>(messageBytes, _jsonSerializerOptions);
            bool _ = message switch
            {
                Chat_CommentSessionToClientMessage chat => ProcessChatMessage(chat),
                //ChatResult_CommentSessionToClientMessage chatResult => ProcessChatResultMessage(chatResult),
                Thread_CommentSessionToClientMessage thread => ProcessThreadMessage(thread),
                Ping_CommentSessionToClientMessage ping => ProcessPingMessage(ping),
                _ => true
            };
        }

        #region Process Message

        private bool ProcessChatMessage(Chat_CommentSessionToClientMessage chat)
        {
            CommentReceived?.Invoke(this, new CommentReceivedEventArgs()
            {
                Chat = new LiveChatData()
                {
                    Thread = chat.Thread.ToString(),
                    CommentId = chat.CommentId,
                    Content = chat.Content,
                    Date = chat.Date,
                    DateUsec = chat.DateUsec,
                    IsAnonymity = chat.Anonymity == 1,
                    __Premium = chat.Premium,
                    IsYourPost = chat.Yourpost == 1,
                    Mail = chat.Mail,
                    Score = chat.Score,
                    UserId = chat.UserId,
                    VideoPosition = chat.VideoPosition
                }
            });
            return true;
        }

        

        private bool ProcessChatResultMessage(ChatResult_CommentSessionToClientMessage chatResult)
        {
            CommentPosted?.Invoke(this, new CommentPostedEventArgs() 
            {
                ChatResult = chatResult.Status,
                Thread = chatResult.Thread,
                No = chatResult.CommentId ?? -1
            });
            return true;
        }

        private bool ProcessThreadMessage(Thread_CommentSessionToClientMessage thread)
        {
            _ticket = thread.Ticket;
            _LastRes = thread.LastRes ?? _LastRes;
            
            StartHeartbeatTimer();

            Connected?.Invoke(this, new CommentServerConnectedEventArgs()
            {
                LastRes = _LastRes,
                Resultcode = thread.Resultcode,
                Revision = thread.Revision,
                ServerTime = thread.ServerTime,
                Thread = thread.Thread.ToString(),
                Ticket = thread.Ticket,
            });

            return true;
        }

        private bool ProcessPingMessage(Ping_CommentSessionToClientMessage ping)
        {
            if (ping.Content.StartsWith("pf"))
            {
                _MessageSendCount += 1;
            }

            return true;
        }

        #endregion



        public void Dispose()
        {
            _connectionCts?.Cancel();
            _connectionCts?.Dispose();
            _ws?.Dispose();
            StopHeartbeatTimer();
            StopCommentPullTimingTimer();
        }

        CancellationTokenSource _connectionCts;
        public async Task OpenAsync(CancellationToken ct, TimeSpan seekTime = default)
        {
            await Close();
            _connectionCts = new CancellationTokenSource();
            
            using (var releaser = await _CommentSessionLock.LockAsync())
            {
                _ws = CreateWebSocket();
                await _ws.ConnectAsync(new Uri(MessageServerUrl), ct);

                _currentLoopTask = Task.Run(() => RunMessageRecievingLoopAsync(ct), ct);


                async Task RunMessageRecievingLoopAsync(CancellationToken ct)
                {
                    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, _connectionCts.Token);
                    var linkCt = linkedCts.Token;
                    try
                    {
                        byte[] buffer = new byte[1024 * 64];
                        while (_ws.State == WebSocketState.Open)
                        {
                            var result = await _ws.ReceiveAsync(buffer, linkCt);
                            if (result.MessageType == WebSocketMessageType.Text)
                            {
                                _ws_MessageReceived(new ReadOnlySpan<byte>(buffer, 0, result.Count));
                            }                            
                            else if (result.MessageType == WebSocketMessageType.Close)
                            {
                                CleanupConnection();
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, CancellationToken.None);
                    }

                    StopHeartbeatTimer();
                    StopCommentPullTimingTimer();
                }

            }

            if (!IsTimeshift)
            {
                await SendStartMessage();
            }
            else
            {
                await ResetConnectionForTimeshift(_startTime + seekTime);
            }
        }

        Task _currentLoopTask;
        public async Task Close()
        {
            _connectionCts?.Cancel();
            _connectionCts?.Dispose();
            _connectionCts = null;

            if (_currentLoopTask != null)
            {
                try
                {
                    await _currentLoopTask;
                }
                catch (OperationCanceledException) { }
                _currentLoopTask = null;
            }
        }

        private void CleanupConnection()
        {
            StopHeartbeatTimer();
        }

        private async Task SendMessageAsync(string message)
        {
            await SendMessageAsync(System.Text.Encoding.Default.GetBytes(message));
        }

        private async Task SendMessageAsync(byte[] message)
        {
            using (var releaser = await _CommentSessionLock.LockAsync())
            {
                await _ws.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }


        /// <summary>
        /// Niwavidedタイプのコメントメッセージを整形して送信
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private Task SendNiwavidedMessage(string content)
        {
            var message = NiwavidedNicoLiveMessageHelper.MakeNiwavidedMessage(_MessageSendCount, content);
            return SendMessageAsync(message);
        }

        #region LiveStreaming

        /// <summary>
        /// Niwavided コメントサーバーへのスレッド受信を開始するためのメッセージを送信
        /// </summary>
        /// <returns></returns>
        private Task SendStartMessage()
        {
            return SendNiwavidedMessage(
                $"{{\"thread\":{{\"thread\":\"{ThreadId}\",\"version\":\"20061206\",\"fork\":0,\"user_id\":\"{UserId ?? "guest"}\",\"res_from\":-{FirstGetRecentMessageCount},\"with_global\":1,\"scores\":1,\"nicoru\":0}}}}"
                );
            
        }


        public void PostComment(string comment, string command, string postKey, TimeSpan elapsedTime)
        {
            if (IsTimeshift) { throw new InvalidOperationException(""); }
            if (UserId == null) { throw new InvalidOperationException("Post comment is require loggedIn."); }

            var vpos = (uint)elapsedTime.TotalMilliseconds / 10;
            var ticket = _ticket;

            _ = SendNiwavidedMessage(
                $"{{\"chat\":{{\"thread\":\"{ThreadId}\",\"vpos\":{vpos},\"mail\":\"{command}\",\"ticket\":\"{ticket}\",\"user_id\":\"{UserId}\",\"content\":\"{comment}\",\"postkey\":\"{postKey}\"}}}}"
                );
        }


        static readonly TimeSpan HEARTBEAT_INTERVAL = TimeSpan.FromMinutes(1);
        private readonly string _userAgent;
        Timer HeartbeatTimer;

        private void StartHeartbeatTimer()
        {
            StopHeartbeatTimer();

            HeartbeatTimer = new Timer(state =>
            {
                _ = (state as LiveCommentSession).SendMessageAsync(string.Empty).ConfigureAwait(false);
            }
            , this, HEARTBEAT_INTERVAL, HEARTBEAT_INTERVAL);
        }

        private void StopHeartbeatTimer()
        {
            HeartbeatTimer?.Dispose();
            HeartbeatTimer = null;
        }


        #endregion



        #region Timeshift

        
        private AsyncLock _CommentPullTimingTimerLock = new AsyncLock();
        private Timer _CommentPullTimingTimer;
        private DateTimeOffset _NextCommentPullTiming;

        TimeSpan _CommentPullingInterval = TimeSpan.FromSeconds(25);

        private async Task ResetConnectionForTimeshift(DateTimeOffset initialTime)
        {
            if (!IsTimeshift) { throw new InvalidOperationException(); }

            using (var releaser = await _CommentPullTimingTimerLock.LockAsync())
            {
                StopCommentPullTimingTimer();

                // オープンからスタートまでのコメントをざっくり取得
                await SendStartMessage_Timeshift(-30, initialTime);

                _NextCommentPullTiming = initialTime + _CommentPullingInterval;

                if (_NextCommentPullTiming < _endTime)
                {
                    // これぐらい開けないと取得できなくなる
                    await Task.Delay(3000);

                    await SendStartMessage_Timeshift(_LastRes + 1, _NextCommentPullTiming);

                    _NextCommentPullTiming = _NextCommentPullTiming + _CommentPullingInterval;
                    // 次のコメント取得の準備
                    StartCommentPullTimingTimer();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res_from"></param>
        /// <param name="when"></param>
        /// <returns></returns>
        private Task SendStartMessage_Timeshift(int res_from, DateTimeOffset when)
        {
            if (!IsTimeshift) { throw new InvalidOperationException(); }

            var whenString = when.ToUnixTimeSeconds().ToString();
            var waybackKey = _waybackkey;
            return SendNiwavidedMessage(
                $"{{\"thread\":{{\"thread\":\"{ThreadId}\",\"version\":\"20061206\",\"fork\":0,\"when\":{whenString},\"user_id\":\"{UserId ?? "guest"}\",\"res_from\":{res_from},\"with_global\":1,\"scores\":1,\"nicoru\":0,\"waybackkey\":\"\"}}}}"
                );
        }


        public async void Seek(CancellationToken ct, TimeSpan timeSpan)
        {
            if (!IsTimeshift) { throw new InvalidOperationException(); }

            await OpenAsync(ct, timeSpan);
        }


        private void StopCommentPullTimingTimer()
        {
            _CommentPullTimingTimer?.Dispose();
            _CommentPullTimingTimer = null;
        }

        private void StartCommentPullTimingTimer()
        {
            StopCommentPullTimingTimer();

            _CommentPullTimingTimer = new Timer(async _ =>
            {
                using (var releaser = await _CommentPullTimingTimerLock.LockAsync())
                {
                    if (_NextCommentPullTiming < _endTime)
                    {
                        await SendStartMessage_Timeshift(_LastRes + 1, _NextCommentPullTiming);

                        _NextCommentPullTiming = _NextCommentPullTiming + _CommentPullingInterval;
                    }
                    else
                    {
                        StopCommentPullTimingTimer();
                    }
                }
            }
            , null
            , _CommentPullingInterval - TimeSpan.FromSeconds(10)
            , _CommentPullingInterval
            );
        }

        #endregion
    }
}
