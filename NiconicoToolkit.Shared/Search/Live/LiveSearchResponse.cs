using NiconicoToolkit.Channels;
using NiconicoToolkit.Live;
using NiconicoToolkit.Live.Timeshift;
using NiconicoToolkit.Live.WatchPageProp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.Search.Live
{
    public sealed class LiveSearchResponse
    {
        [JsonPropertyName("data")]
        public LiveSearchItem[] Items { get; set; }

        [JsonPropertyName("meta")]
        public LiveSearchMeta Meta { get; set; }
    }

    public sealed class LiveSearchMeta
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }

    public sealed class LiveSearchItem
    {
        [JsonPropertyName("programId")]
        public LiveId ProgramId { get; set; }

        [JsonPropertyName("program")]
        public Program Program { get; set; }

        [JsonPropertyName("statistics")]
        public Statistics Statistics { get; set; }

        [JsonPropertyName("taxonomy")]
        public Taxonomy Taxonomy { get; set; }

        [JsonPropertyName("socialGroup")]
        public SocialGroup SocialGroup { get; set; }

        [JsonPropertyName("features")]
        public Features Features { get; set; }

        [JsonPropertyName("thumbnail")]
        public Thumbnail Thumbnail { get; set; }

        [JsonPropertyName("programProvider")]
        public ProgramProvider ProgramProvider { get; set; }

        [JsonPropertyName("timeshiftSetting")]
        public TimeshiftSetting TimeshiftSetting { get; set; }
    }

    public sealed class Program
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("provider")]
        public Provider Provider { get; set; }

        [JsonPropertyName("schedule")]
        public Schedule Schedule { get; set; }
    }

    public sealed class Schedule
    {
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("openTime")]
        public DateTime OpenTime { get; set; }

        [JsonPropertyName("beginTime")]
        public DateTime BeginTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }
    }

    public sealed class Statistics
    {
        [JsonPropertyName("viewers")]
        public int Viewers { get; set; }

        [JsonPropertyName("comments")]
        public int Comments { get; set; }

        [JsonPropertyName("timeshiftReservations")]
        public int TimeshiftReservations { get; set; }
    }

    public sealed class Taxonomy
    {
        [JsonPropertyName("categories")]
        public CategoriesContainer Categories { get; set; }
    }

    public sealed class CategoriesContainer
    {
        [JsonPropertyName("main")]
        public Category[] Main { get; set; }

        [JsonPropertyName("sub")]
        public Category[] Sub { get; set; }
    }

    public sealed class Category
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public sealed class SocialGroup {
        [JsonPropertyName("socialGroupId")]
        public string SocialGroupId { get; set; }

        [JsonPropertyName("type")]
        public SocialGroupType Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnail")]
        public Uri Thumbnail { get; set; }

        [JsonPropertyName("thumbnailSmall")]
        public Uri ThumbnailSmall { get; set; }

        [JsonPropertyName("level")]
        public int? Level { get; set; }

        [JsonPropertyName("isFollowed")]
        public bool IsFollowed { get; set; }

        [JsonPropertyName("isJoined")]
        public bool IsJoined { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("isSafeThumbnail")]
        public bool IsSafeThumbnail { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [JsonPropertyName("isPayChannel")]
        public bool IsPayChannel { get; set; }
    }

    public sealed class Features
    {
        [JsonPropertyName("enabled")]
        public Feature[] Enabled { get; set; }
    }

    public sealed class Thumbnail
    {
        [JsonPropertyName("large")]
        public Uri Large { get; set; }

        [JsonPropertyName("small")]
        public Uri Small { get; set; }

        [JsonPropertyName("huge")]
        public Huge Huge { get; set; }

        [JsonPropertyName("screenshot")]
        public Screenshot Screenshot { get; set; }
    }

    public sealed class Screenshot
    {
        [JsonPropertyName("large")]
        public Uri Large { get; set; }

        [JsonPropertyName("small")]
        public Uri Small { get; set; }
    }

    public sealed class Huge
    {
        [JsonPropertyName("s1280x720")]
        public Uri S1280X720 { get; set; }

        [JsonPropertyName("s1920x1080")]
        public Uri S1920X1080 { get; set; }

        [JsonPropertyName("s352x198")]
        public Uri S352X198 { get; set; }

        [JsonPropertyName("s640x360")]
        public Uri S640X360 { get; set; }
    }

    public sealed class ProgramProvider
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("programProviderId")]
        public string ProgramProviderId { get; set; }

        [JsonPropertyName("large")]
        public Icons icons { get; set; }
    }

    public sealed class Icons
    {
        [JsonPropertyName("uri150x150")]
        public Uri Uri150x150 { get; set; }

        [JsonPropertyName("uri50x50")]
        public Uri Uri50x50 { get; set; }
    }

    public sealed class TimeshiftSetting
    {
        [JsonPropertyName("watchLimit")]
        public WatchLimit WatchLimit { get; set; }

        [JsonPropertyName("programValidDuration")]
        public int? ProgramValidDuration { get; set; }

        [JsonPropertyName("requirement")]
        public Requirement Requirement { get; set; }

        [JsonPropertyName("status")]
        public TimeshiftStatus Status { get; set; }

        [JsonPropertyName("endTime")]
        public DateTimeOffset EndTime { get; set; }

        [JsonPropertyName("reservationDeadline")]
        public DateTimeOffset ReservationDeadline { get; set; }
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
