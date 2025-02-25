using NiconicoToolkit.Activity.VideoWatchHistory;
using NiconicoToolkit.Channels;
using NiconicoToolkit.Follow;
using NiconicoToolkit.FollowingsActivity;
using NiconicoToolkit.Ichiba;
using NiconicoToolkit.Likes;
using NiconicoToolkit.Live.Cas;
using NiconicoToolkit.Live.Notify;
using NiconicoToolkit.Live.Timeshift;
using NiconicoToolkit.Live.WatchPageProp;
using NiconicoToolkit.Mylist;
using NiconicoToolkit.Mylist.LoginUser;
using NiconicoToolkit.Ranking.Video;
using NiconicoToolkit.Recommend;
using NiconicoToolkit.Search.List;
using NiconicoToolkit.Search.Live;
using NiconicoToolkit.Search.User;
using NiconicoToolkit.Search.Video;
using NiconicoToolkit.Series;
using NiconicoToolkit.SnapshotSearch;
using NiconicoToolkit.User;
using NiconicoToolkit.Video;
using NiconicoToolkit.Video.Watch;
using NiconicoToolkit.Video.Watch.Domand;
using NiconicoToolkit.Video.Watch.NV_Comment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(NvapiVideoItem))]

// Channles
[JsonSerializable(typeof(ChannelAdmissionResponse))]

// Follow
[JsonSerializable(typeof(FollowTagsResponse))]
[JsonSerializable(typeof(FollowUsersResponse))]
[JsonSerializable(typeof(FollowMylistResponse))]
[JsonSerializable(typeof(FollowChannelResponse))]
[JsonSerializable(typeof(ChannelAuthorityResponse))]
[JsonSerializable(typeof(ChannelFollowResult))]
[JsonSerializable(typeof(FollowedResultResponce))]

// FollowingsActivity
[JsonSerializable(typeof(FollowingsActivityResponse))]
[JsonSerializable(typeof(FollowingsActivityClient.CodeOnly))]

// History
[JsonSerializable(typeof(VideoWatchHistory))]
[JsonSerializable(typeof(VideoWatchHistoryDeleteResult))]

// Ichiba
[JsonSerializable(typeof(IchibaResponse_Internal))]

// Like
[JsonSerializable(typeof(LikeActionResponse))]
[JsonSerializable(typeof(LikesListResponse))]

// Live.Cas
[JsonSerializable(typeof(LiveProgramResponse))]

// Notify
[JsonSerializable(typeof(LiveNotifyUnreadResponse))]
[JsonSerializable(typeof(LiveNotifyContentResponse))]

// Mylist
[JsonSerializable(typeof(LoginUserMylistsResponse))]
[JsonSerializable(typeof(CreateMylistResponse))]
[JsonSerializable(typeof(ChangeMylistGroupsOrderResponse))]
[JsonSerializable(typeof(WatchAfterItemsResponse))]
[JsonSerializable(typeof(MoveOrCopyMylistItemsResponse))]
[JsonSerializable(typeof(GetMylistItemsResponse))]
[JsonSerializable(typeof(GetUserMylistGroupsResponse))]

// Ranking/Video
[JsonSerializable(typeof(HotTopicResponse))]
[JsonSerializable(typeof(PopularTagResponse))]
[JsonSerializable(typeof(VideoRankingResponse))]

// Recommend
[JsonSerializable(typeof(VideoRecommendResponse))]
[JsonSerializable(typeof(LiveRecommendResponse))]

// SnapshotSearch
[JsonSerializable(typeof(SnapshotApiVersion))]
[JsonSerializable(typeof(SnapshotResponse))]


public sealed partial class GlobalJsonSourceGenerationContext : JsonSerializerContext
{
}

