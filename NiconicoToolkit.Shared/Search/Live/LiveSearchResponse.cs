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
        public Program Program { get; init; }

        [JsonPropertyName("statistics")]
        public Statistics Statistics { get; init; }

        [JsonPropertyName("taxonomy")]
        public Taxonomy Taxonomy { get; init; }

        [JsonPropertyName("socialGroup")]
        public SocialGroup SocialGroup { get; init; }

        [JsonPropertyName("features")]
        public Features Features { get; init; }

        [JsonPropertyName("thumbnail")]
        public Thumbnail Thumbnail { get; init; }

        [JsonPropertyName("programProvider")]
        public ProgramProvider ProgramProvider { get; init; }

        [JsonPropertyName("timeshiftSetting")]
        public TimeshiftSetting TimeshiftSetting { get; init; }
    }

    public sealed class Program
    {
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("provider")]
        public Provider Provider { get; init; }

        [JsonPropertyName("schedule")]
        public Schedule Schedule { get; init; }
    }

    public sealed class Schedule
    {
        [JsonPropertyName("status")]
        public Status Status { get; init; }

        [JsonPropertyName("openTime")]
        public DateTime OpenTime { get; init; }

        [JsonPropertyName("beginTime")]
        public DateTime BeginTime { get; init; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; init; }
    }

    public sealed class Statistics
    {
        [JsonPropertyName("viewers")]
        public int Viewers { get; init; }

        [JsonPropertyName("comments")]
        public int Comments { get; init; }

        [JsonPropertyName("timeshiftReservations")]
        public int? TimeshiftReservations { get; init; }
    }

    public sealed class Taxonomy
    {
        [JsonPropertyName("categories")]
        public CategoriesContainer Categories { get; init; }
    }

    public sealed class CategoriesContainer
    {
        [JsonPropertyName("main")]
        public Category[] Main { get; init; }

        [JsonPropertyName("sub")]
        public Category[] Sub { get; init; }
    }

    public sealed class Category
    {
        [JsonPropertyName("text")]
        public string Text { get; init; }
    }

    public sealed class SocialGroup {
        [JsonPropertyName("socialGroupId")]
        public string SocialGroupId { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("thumbnailSmall")]
        public Uri ThumbnailSmall { get; init; }
    }

    public sealed class Features
    {
        [JsonPropertyName("enabled")]
        public HashSet<Feature> Enabled { get; init; }
    }

    public sealed class Thumbnail
    {
        [JsonPropertyName("large")]
        public Uri Large { get; init; }

        [JsonPropertyName("small")]
        public Uri Small { get; init; }

        [JsonPropertyName("huge")]
        public Huge Huge { get; init; }

        [JsonPropertyName("screenshot")]
        public Screenshot Screenshot { get; init; }
    }

    public sealed class Screenshot
    {
        [JsonPropertyName("large")]
        public Uri Large { get; init; }

        [JsonPropertyName("small")]
        public Uri Small { get; init; }
    }

    public sealed class Huge
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

    public sealed class ProgramProvider
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("programProviderId")]
        public string ProgramProviderId { get; init; }

        [JsonPropertyName("large")]
        public Icons Icons { get; init; }
    }

    public sealed class Icons
    {
        [JsonPropertyName("uri150x150")]
        public Uri Uri150x150 { get; init; }

        [JsonPropertyName("uri50x50")]
        public Uri Uri50x50 { get; init; }
    }

    public sealed class TimeshiftSetting
    {
        [JsonPropertyName("watchLimit")]
        public WatchLimit WatchLimit { get; init; }

        [JsonPropertyName("programValidDuration")]
        public int? ProgramValidDuration { get; init; }

        [JsonPropertyName("requirement")]
        public Requirement Requirement { get; init; }

        [JsonPropertyName("status")]
        public TimeshiftStatus Status { get; init; }

        [JsonPropertyName("endTime")]
        public DateTimeOffset? EndTime { get; init; }

        [JsonPropertyName("reservationDeadline")]
        public DateTimeOffset ReservationDeadline { get; init; }
    }

    public enum SocialGroupType { COMMUNITY, CHANNEL }

    public enum Provider {
        [Description("community")]
        COMMUNITY,

        [Description("channel")]
        CHANNEL,

        [Description("official")]
        OFFICIAL
    }

    public enum Status {
        [Description("beforeReleased")]
        BEFORE_RELEASE,

        [Description("released")]
        RELEASED,

        [Description("onAir")]
        ON_AIR,

        [Description("ended")]
        ENDED
    }

    public enum WatchLimit { ONCE, UNLIMITED }

    public enum TimeshiftStatus { BEFORE_OPEN, OPENED, CLOSED }

    public enum Requirement { RESERVATION, PAYMENT, NONE }

    public enum Feature
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
