using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NeoSmart.AsyncLock;
using NiconicoToolkit.Live.WatchSession;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using NiconicoToolkit.Live.WatchSession.ToClientMessage;
using System.Net.WebSockets;

namespace NiconicoToolkit.Live.WatchSession
{
    public delegate void LiveStreamRecieveErrorHandler(ErrorMessageType errorMessageType);
    public delegate void LiveStreamRecieveServerTimeHandler(DateTime serverTime);
    public delegate void LiveStreamRecievePermitHandler(string parmit);
    public delegate void LiveStreamRecieveStreamHandler(Live2CurrentStreamEventArgs e);
    public delegate void LiveStreamRecieveRoomHandler(Live2CurrentRoomEventArgs e);
    public delegate void LiveStreamRecieveRoomsHandler(Live2RoomsEventArgs e);
    public delegate void LiveStreamRecieveStatisticsHandler(Live2StatisticsEventArgs e);
    public delegate void LiveStreamReceiveTagUpdatedHandler(Live2TagUpdatedEventArgs e);
    public delegate void LiveStreamRecieveScheduleHandler(Live2ScheduleEventArgs e);
    public delegate void LiveStreamReceivedisconnectHandler(Live2DisconnectEventArgs e);
    public delegate void LiveStreamRecieveResconnectHandler(Live2ReconnectEventArgs e);
    public delegate void LiveStreamElapsedTimeUpdateEventHandler(LiveStreamElapsedTimeUpdateEventArgs e);

    public sealed class LiveStreamElapsedTimeUpdateEventArgs
    {
        public TimeSpan Time { get; set; }
        public int VideoPosition { get; set; }
        public bool Seeked { get; set; }
    }

    public class Live2CurrentStreamEventArgs
    {
        public string SyncUri { get; set; }
        public string Uri { get; set; }
        public string Quality { get; set; }
        public string[] AvailableQualities { get; set; }
        public string MediaServerType { get; set; }
        public string MediaServerAuth { get; set; }
        public string Protocol { get; set; }
    }

    public class Live2RoomsEventArgs
    {
        public Live2CurrentRoomEventArgs[] Rooms { get; set; }
    }


    public class Live2CurrentRoomEventArgs
    {
        public Uri MessageServerUrl { get; set; }
        public string MessageServerType { get; set; }
        public string RoomName { get; set; }
        public string ThreadId { get; set; }
        public bool IsFirst { get; set; }
        public string WaybackKey { get; set; }

        public LiveCommentSession CreateCommentClientForTimeshift(string userAgent, string userId, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            return LiveCommentSession.CreateForTimeshift(MessageServerUrl.OriginalString, ThreadId, userId, userAgent, WaybackKey, startTime, endTime);
        }
        public LiveCommentSession CreateCommentClientForLiveStreaming(string userAgent, string userId)
        {
            return LiveCommentSession.CreateForLiveStream(MessageServerUrl.OriginalString, ThreadId, userId, userAgent);
        }
    }

    public class Live2StatisticsEventArgs
    {
        public long ViewCount { get; set; }
        public long CommentCount { get; set; }
        public long AdPoints { get; set; }
        public long GiftPoints { get; set; }
    }

    public class Live2ScheduleEventArgs
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class Live2DisconnectEventArgs
    {
        public DisconnectReasonType ReasonType { get; set; }
    }

    public class Live2ReconnectEventArgs
    {
        public string AudienceToken { get; set; }
        public TimeSpan WaitTime { get; set; }
    }

    public class Live2TagUpdatedEventArgs
    {
        public TagUpdated_TagItem[] TagItems { get; set; }

        public bool OwnerLocked { get; set; }
    }

    public sealed class Live2WatchSession : IDisposable
    {
        private readonly ClientWebSocket _ws;
        private readonly AsyncLock _WebSocketLock = new AsyncLock();
        public readonly bool IsWatchWithTimeshift;
        private readonly string _webSocketUrl;
        private readonly JsonSerializerOptions _SocketJsonDeserializerOptions;
        private readonly JsonSerializerOptions _SocketJsonSerializerOptions;
        private Timer _watchingHeartbaetTimer;
        private CancellationTokenSource _connectionCts;

        public event LiveStreamRecieveErrorHandler RecieveError;
        public event LiveStreamRecieveServerTimeHandler RecieveServerTime;
        public event LiveStreamRecieveStreamHandler RecieveStream;
        public event LiveStreamRecieveRoomHandler RecieveRoom;
        public event LiveStreamRecieveRoomsHandler RecieveRooms;
        public event LiveStreamRecieveStatisticsHandler RecieveStatistics;
        public event LiveStreamReceiveTagUpdatedHandler RecieveTagUpdated;
        public event LiveStreamRecieveScheduleHandler RecieveSchedule;
        public event LiveStreamReceivedisconnectHandler ReceiveDisconnect;
        public event LiveStreamRecieveResconnectHandler RecieveReconnect;
        public event LiveStreamElapsedTimeUpdateEventHandler ElapsedTimeUpdated;

        internal Live2WatchSession(string webSocketUrl, bool isTimeshift, string userAgent)
        {
            IsWatchWithTimeshift = isTimeshift;
            _webSocketUrl = webSocketUrl;

            _ws = new ClientWebSocket();
            _ws.Options.AddSubProtocol("msg.nicovideo.jp#json");

            (string, string)[] customHeaderPairs = new[]
            {
                ("host", "a.live2.nicovideo.jp"),
                ("Origin", "https://live.nicovideo.jp"),
                ("User-Agent", userAgent),
            };

            foreach (var header in customHeaderPairs)
            {
                _ws.Options.SetRequestHeader(header.Item1, header.Item2);
            }


            _SocketJsonDeserializerOptions = new System.Text.Json.JsonSerializerOptions()
            {
                Converters =
                {
                    new JsonStringEnumMemberConverter(JsonSnakeCaseNamingPolicy.Instance),
                    new WatchSessionToClientMessageJsonConverter(),
                },
            };

            _SocketJsonSerializerOptions = new System.Text.Json.JsonSerializerOptions()
            {
                Converters =
                {
                    new JsonStringEnumMemberConverter(JsonSnakeCaseNamingPolicy.Instance),
                    new WatchSessionToServerMessageJsonConverter(),
                },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
        }

        private async Task<bool> OpenAsync(CancellationToken ct)
        {
            await CloseAsync();

            using (var releaser = await _WebSocketLock.LockAsync())
            {
                _connectionCts = new CancellationTokenSource();
                await Task.Run(async () => 
                {
                    await _ws.ConnectAsync(new Uri(_webSocketUrl), ct);
                });
            }

            _ = Task.Run(() => WatchSessionCommandRecieveLoopAsync(ct), ct);

            async Task WatchSessionCommandRecieveLoopAsync(CancellationToken ct)
            {
                using var linkCt = CancellationTokenSource.CreateLinkedTokenSource(ct, _connectionCts.Token);
                try
                {
                    byte[] buffer = new byte[256 * 8];
                    while (await _ws.ReceiveAsync(buffer, linkCt.Token) is not null and var result)
                    {
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            _ws_MessageReceived(new ReadOnlySpan<byte>(buffer, 0, result.Count));
                        }
                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            _ws_MessageReceived(new ReadOnlySpan<byte>(buffer, 0, result.Count));
                            break;
                        }
                    }
                }
                catch (OperationCanceledException) { }
            }

            return _ws.State == WebSocketState.Open;
        }

        public async Task CloseAsync()
        {
            using var releaser = await _WebSocketLock.LockAsync();
            
            _connectionCts?.Cancel();
            _connectionCts?.Dispose();
            _connectionCts = null;
            _watchingHeartbaetTimer?.Dispose();
            _watchingHeartbaetTimer = null;
            if (_ws.State == WebSocketState.Open)
            {
                try
                {
                    await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, CancellationToken.None);
                }
                catch { }
            }
        }

        public async void Dispose()
        {
            _ws.Dispose();
            await CloseAsync();
        }

        #region Message Received

        private void _ws_MessageReceived(ReadOnlySpan<byte> message)
        {
#if WINDOWS_UWP
            var data = JsonDeserializeHelper.Deserialize<WatchServerToClientMessage>(message, _SocketJsonDeserializerOptions);
#else
            var data = JsonDeserializeHelper.Deserialize<WatchServerToClientMessage>(message.ToArray(), _SocketJsonDeserializerOptions);
#endif
            _ = data switch
            {
                Error_WatchSessionToClientMessage error => ProcessErrorData(error),
                Seat_WatchSessionToClientMessage seat => ProcessSeatData(seat),
                Akashic_WatchSessionToClientMessage akashic => ProcessAkashicData(akashic),
                Stream_WatchSessionToClientMessage stream => ProcessStreamData(stream),
                Room_WatchSessionToClientMessage room => ProcessRoomData(room),
                Rooms_WatchSessionToClientMessage rooms => ProcessRoomsData(rooms),
                ServerTime_WatchSessionToClientMessage serverTime => ProcessServerTimeData(serverTime),
                Statistics_WatchSessionToClientMessage statistics => ProcessStatisticsData(statistics),
                Schedule_WatchSessionToClientMessage schedule => ProcessScheduleData(schedule),
                Ping_WatchSessionToClientMessage ping => ProcessPingData(ping),
                Disconnect_WatchSessionToClientMessage disconnect => ProcessDisconnectData(disconnect),
                Reconnect_WatchSessionToClientMessage reconnect => ProcessReconnectData(reconnect),
                PostCommentResult_WatchSessionToClientMessage postCommentResult => ProcessPostCommentResultData(postCommentResult),
                TagUpdated_WatchSessionToClientMessage tagUpdate => ProcessTagUpdated(tagUpdate),
                _ => true
            };
        }


        private bool ProcessErrorData(Error_WatchSessionToClientMessage error)
        {
            RecieveError?.Invoke(error.Code);
            return true;
        }

        private readonly byte[] _keepSeatMessageBytes = System.Text.Encoding.Default.GetBytes(@"{""type"":""keepSeat""}");
        private bool ProcessSeatData(Seat_WatchSessionToClientMessage seat)
        {
            _watchingHeartbaetTimer?.Dispose();
            _watchingHeartbaetTimer = new Timer((state) =>
            {
                _ = (state as Live2WatchSession).SendMessageAsync(_keepSeatMessageBytes);
            }
            , this, TimeSpan.Zero, TimeSpan.FromSeconds(seat.KeepIntervalSec)
            );

            return true;
        }

        private bool ProcessAkashicData(Akashic_WatchSessionToClientMessage akashic)
        {
            return true;
        }

        private bool ProcessStreamData(Stream_WatchSessionToClientMessage stream)
        {
            RecieveStream?.Invoke(new Live2CurrentStreamEventArgs()
            {
                Uri = stream.Uri,
                SyncUri = stream.SyncUri,
                Quality = stream.Quality,
                AvailableQualities = stream.AvailableQualities,
                Protocol = stream.Protocol
            });
            return true;
        }

        private bool ProcessRoomData(Room_WatchSessionToClientMessage room)
        {
            RecieveRoom?.Invoke(new Live2CurrentRoomEventArgs()
            {
                MessageServerUrl = room.MessageServer.Uri,
                MessageServerType = room.MessageServer.Type,
                RoomName = room.Name,
                ThreadId = room.ThreadId,
                IsFirst = room.IsFirst,
                WaybackKey = room.waybackkey
            });
            return true;
        }

        private bool ProcessRoomsData(Rooms_WatchSessionToClientMessage rooms)
        {
            RecieveRooms?.Invoke(new Live2RoomsEventArgs()
            {
                Rooms = rooms.Rooms.Select(x => new Live2CurrentRoomEventArgs() 
                {
                    MessageServerUrl = x.MessageServer.Uri,
                    MessageServerType = x.MessageServer.Type,
                    RoomName = x.Name,
                    ThreadId = x.ThreadId,
                }).ToArray()   
            }) ;
            return true;
        }

        private bool ProcessServerTimeData(ServerTime_WatchSessionToClientMessage serverTime)
        {
            RecieveServerTime?.Invoke(serverTime.CurrentTime);
            return true;
        }

        private bool ProcessStatisticsData(Statistics_WatchSessionToClientMessage statistics)
        {
            RecieveStatistics?.Invoke(new Live2StatisticsEventArgs()
            {
                ViewCount = statistics.Viewers ?? 0,
                CommentCount = statistics.comments ?? 0,
                AdPoints = statistics.adPoints ?? 0,
                GiftPoints = statistics.giftPoints ?? 0,
            });
            return true;
        }

        private bool ProcessTagUpdated(TagUpdated_WatchSessionToClientMessage tagUpdate)
        {
            RecieveTagUpdated?.Invoke(new Live2TagUpdatedEventArgs() { TagItems = tagUpdate.Tags.Items, OwnerLocked = tagUpdate.Tags.OwnerLocked });
            return true;
        }

        private bool ProcessScheduleData(Schedule_WatchSessionToClientMessage schedule)
        {
            RecieveSchedule?.Invoke(new Live2ScheduleEventArgs()
            {
                BeginTime = schedule.Begin,
                EndTime = schedule.End,
            });
            return true;
        }

        private readonly byte[] _pongMessageBytes = System.Text.Encoding.Default.GetBytes("{\"type\":\"pong\"}");
        private bool ProcessPingData(Ping_WatchSessionToClientMessage ping)
        {
            _ = SendMessageAsync(_pongMessageBytes);
            return true;
        }

        private bool ProcessDisconnectData(Disconnect_WatchSessionToClientMessage disconnect)
        {
            ReceiveDisconnect?.Invoke(new Live2DisconnectEventArgs()
            {
                ReasonType = disconnect.Reason
            });
            return true;
        }

        private bool ProcessReconnectData(Reconnect_WatchSessionToClientMessage reconnect)
        {
            RecieveReconnect?.Invoke(new Live2ReconnectEventArgs() 
            {
                AudienceToken = reconnect.AudienceToken,
                WaitTime = TimeSpan.FromSeconds(reconnect.WaitTimeSec)
            });

            return true;
        }

        private bool ProcessPostCommentResultData(PostCommentResult_WatchSessionToClientMessage postCommentResult)
        {
            _PostCommentResultTaskCompletionSource.SetResult(postCommentResult.Chat);
            return true;
        }


#endregion


#region Send Message

        private async Task SendMessageAsync(byte[] message)
        {            
            using (var releaser = await _WebSocketLock.LockAsync())
            {
                if (_ws.State == WebSocketState.Open)
                {
                    await _ws.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        private async Task SendMessageAsync(WatchClientToServerMessageDataBase messageData)
        {
            await SendMessageAsync(
                JsonSerializer.SerializeToUtf8Bytes(
                    new WatchClientToServerMessagePayload(messageData), 
                    _SocketJsonSerializerOptions
                    )
                );
        }

        public async Task<bool> StartWachingAsync(LiveQualityType requestQuality, bool isLowLatency, LiveQualityLimitType? limit = null, bool? chasePlay = false)
        {
            if (await OpenAsync(CancellationToken.None) is false) { return false; }

            await SendMessageAsync(new StartWatching_ToServerMessageData()
            {
                Reconnect = false,
                Room = new Room()
                {
                    Protocol = "webSocket",
                    Commentable = !IsWatchWithTimeshift
                },
                Stream = new StartWatchingStream()
                {
                    Quality = requestQuality,
                    Limit = limit,
                    Latency = isLowLatency ? LiveLatencyType.Low : LiveLatencyType.High,
                    ChasePlay = chasePlay
                },
            });

            return true;
        }

        public async Task ChangeStreamAsync(LiveQualityType requestQuality, bool isLowLatency, LiveQualityLimitType? limit = null, bool? chasePlay = false)
        {
            await SendMessageAsync(new ChangeStream_ToServerMessageData()
            {
                Quality = requestQuality,
                Limit = limit,
                Latency = isLowLatency ? LiveLatencyType.Low : LiveLatencyType.High,
                ChasePlay = chasePlay
            });
        }

        TaskCompletionSource<PostCommentResultChat> _PostCommentResultTaskCompletionSource;

        /// <remarks>[Require Login]</remarks>
        [RequireLogin]
        public async Task<PostCommentResultChat> PostCommentAsync(string text, TimeSpan videoPosition, bool isAnonymous, string size = null, string position = null, string color = null, string font = null)
        {
            _PostCommentResultTaskCompletionSource = new TaskCompletionSource<PostCommentResultChat>();
            try
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    await SendMessageAsync(new PostComment_ToServerMessageData()
                    {
                        Text = text,
                        Vpos = (int)(videoPosition.TotalSeconds * 100),
                        IsAnonymous = isAnonymous,
                        Size = size,
                        Position = position,
                        Color = color,
                        Font = font
                    });
                }
            }
            catch (TaskCanceledException)
            {
                _PostCommentResultTaskCompletionSource.SetCanceled();
            }

            return await _PostCommentResultTaskCompletionSource.Task;
        }

#endregion
    }
}
