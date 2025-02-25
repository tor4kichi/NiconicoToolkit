using NiconicoToolkit.Channels;
using NiconicoToolkit.Live;
using NiconicoToolkit.Live.Timeshift;
using NiconicoToolkit.Live.WatchPageProp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Search.Live
{
    public sealed class LiveSearchResponse : ResponseWithMeta<LiveSearchMeta>
    {
        [JsonPropertyName("data")]
        public LiveSearchItem[] Items { get; init; }
    }

    public sealed class LiveSearchMeta : Meta
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; init; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; init; }
    }

    public sealed class LiveSearchItem
    {
        [JsonPropertyName("programId")]
        public LiveId ProgramId { get; init; }

        [JsonPropertyName("program")]
        public LiveProgram Program { get; init; }

        [JsonPropertyName("statistics")]
        public LiveStatistics Statistics { get; init; }

        [JsonPropertyName("taxonomy")]
        public LiveTaxonomy Taxonomy { get; init; }

        [JsonPropertyName("socialGroup")]
        public LiveSocialGroup SocialGroup { get; init; }

        [JsonPropertyName("features")]
        public LiveFeatures Features { get; init; }

        [JsonPropertyName("thumbnail")]
        public LiveThumbnail Thumbnail { get; init; }

        [JsonPropertyName("programProvider")]
        public LiveProgramProvider ProgramProvider { get; init; }

        [JsonPropertyName("timeshiftSetting")]
        public LiveTimeshiftSetting TimeshiftSetting { get; init; }
    }

    public sealed class LiveProgram
    {
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("provider")]
        public LiveProvider Provider { get; init; }

        [JsonPropertyName("schedule")]
        public LiveSchedule Schedule { get; init; }
    }

    public sealed class LiveSchedule
    {
        [JsonPropertyName("status")]
        public SearchLiveStatus Status { get; init; }

        [JsonPropertyName("openTime")]
        public DateTime OpenTime { get; init; }

        [JsonPropertyName("beginTime")]
        public DateTime BeginTime { get; init; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; init; }
    }

    public sealed class LiveStatistics
    {
        [JsonPropertyName("viewers")]
        public int Viewers { get; init; }

        [JsonPropertyName("comments")]
        public int Comments { get; init; }

        [JsonPropertyName("timeshiftReservations")]
        public int? TimeshiftReservations { get; init; }
    }

    public sealed class LiveTaxonomy
    {
        [JsonPropertyName("categories")]
        public LiveCategoriesContainer Categories { get; init; }
    }

    public sealed class LiveCategoriesContainer
    {
        [JsonPropertyName("main")]
        public LiveCategory[] Main { get; init; }

        [JsonPropertyName("sub")]
        public LiveCategory[] Sub { get; init; }
    }

    public sealed class LiveCategory
    {
        [JsonPropertyName("text")]
        public string Text { get; init; }
    }

    public sealed class LiveSocialGroup 
    {
        [JsonPropertyName("socialGroupId")]
        public string SocialGroupId { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("thumbnailSmall")]
        public Uri ThumbnailSmall { get; init; }
    }

    public sealed class LiveFeatures
    {
        [JsonPropertyName("enabled")]
        public HashSet<LiveFeature> Enabled { get; init; }
    }

    public sealed class LiveThumbnail
    {
        [JsonPropertyName("large")]
        public Uri Large { get; init; }

        [JsonPropertyName("small")]
        public Uri Small { get; init; }

        [JsonPropertyName("huge")]
        public LiveHugeThumbnail Huge { get; init; }

        [JsonPropertyName("screenshot")]
        public LiveScreenshot Screenshot { get; init; }
    }

    public sealed class LiveScreenshot
    {
        [JsonPropertyName("large")]
        public Uri Large { get; init; }

        [JsonPropertyName("small")]
        public Uri Small { get; init; }
    }

    public sealed class LiveHugeThumbnail
    {
        [JsonPropertyName("s1280x720")]
        public Uri S1280X720 { get; init; }

        [JsonPropertyName("s1920x1080")]
        public Uri S1920X1080 { get; init; }

        [JsonPropertyName("s352x198")]
        public Uri S352X198 { get; init; }

        [JsonPropertyName("s640x360")]
        public Uri S640X360 { get; init; }
    }

    public sealed class LiveProgramProvider
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("programProviderId")]
        public string ProgramProviderId { get; init; }

        [JsonPropertyName("large")]
        public LiveIcons Icons { get; init; }
    }

    public sealed class LiveIcons
    {
        [JsonPropertyName("uri150x150")]
        public Uri Uri150x150 { get; init; }

        [JsonPropertyName("uri50x50")]
        public Uri Uri50x50 { get; init; }
    }

    public sealed class LiveTimeshiftSetting
    {
        [JsonPropertyName("watchLimit")]
        public LiveWatchLimit WatchLimit { get; init; }

        [JsonPropertyName("programValidDuration")]
        public int? ProgramValidDuration { get; init; }

        [JsonPropertyName("requirement")]
        public LiveRequirement Requirement { get; init; }

        [JsonPropertyName("status")]
        public LiveTimeshiftStatus Status { get; init; }

        [JsonPropertyName("endTime")]
        public DateTimeOffset? EndTime { get; init; }

        [JsonPropertyName("reservationDeadline")]
        public DateTimeOffset ReservationDeadline { get; init; }
    }

    public enum SocialGroupType { COMMUNITY, CHANNEL }

    public enum LiveProvider {
        [Description("community")]
        COMMUNITY,

        [Description("channel")]
        CHANNEL,

        [Description("official")]
        OFFICIAL
    }

    public enum SearchLiveStatus {
        [Description("beforeReleased")]
        BEFORE_RELEASE,

        [Description("released")]
        RELEASED,

        [Description("onAir")]
        ON_AIR,

        [Description("ended")]
        ENDED
    }

    public enum LiveWatchLimit { ONCE, UNLIMITED }

    public enum LiveTimeshiftStatus { BEFORE_OPEN, OPENED, CLOSED }

    public enum LiveRequirement { RESERVATION, PAYMENT, NONE }

    public enum LiveFeature
    {
        OPERATORONLY,
        MEMBER_ONLY,
        ECONOMY_MODE,
        PAY_PROGRAM,
        ICHIBA_COUNTER,
        CHASE_PLAY,
        PROGRAM_REDIRECT,
        DOMESTIC,
        DIRECT_LINK_ONLY,
        CHANNEL_TEST_PROGRAM
    }
}
