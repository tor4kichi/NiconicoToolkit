namespace NiconicoToolkit.Live.Timeshift;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

public partial class TimeshiftReservationsResponse
{
    [JsonPropertyName("user")]
    public User User { get; set; }

    [JsonPropertyName("reservations")]
    public Reservations Reservations { get; set; }
}


public partial class Reservations
{
    [JsonPropertyName("reservations")]
    public Reservation[] Items { get; set; }
}

public enum LiveProgramFeature
{
    ADVERTISEMENT,
    CHASE_PLAY,
    USER_ADVERTISEMENT,
    ICHIBA_COUNTER,
    RECOMMEND,
    CONTENT_TREE,
    PREMIUM_APPEAL,
    COMMENT_COLOR_CODE,
    PAY_PROGRAM,
    ECONOMY_MODE,
    MEMBER_ONLY,
    GIFT,
    DOMESTIC,
}

public partial class Features
{
    [JsonPropertyName("enabled")]
    public HashSet<string> Enabled { get; set; }

    [JsonIgnore]
    public bool IsMemberOnly => Enabled.Contains("MEMBER_ONLY");

    [JsonIgnore]
    public bool IsPayProgram => Enabled.Contains("PAY_PROGRAM");
}

public partial class Program
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("provider")]
    public ProviderType Provider { get; set; }

    [JsonPropertyName("schedule")]
    public Schedule Schedule { get; set; }
}

public enum ScheduleStatus
{
    RELEASED,
    ON_AIR,
    ENDED,
};

public partial class Schedule
{
    [JsonPropertyName("status")]
    public ScheduleStatus Status { get; set; }

    [JsonPropertyName("openTime")]
    public DateTimeOffset OpenTime { get; set; }

    [JsonPropertyName("beginTime")]
    public DateTimeOffset BeginTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTimeOffset EndTime { get; set; }
}

public partial class ProgramProvider
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("profileUrl")]
    public Uri ProfileUrl { get; set; }

    [JsonPropertyName("type")]
    public ProviderType Type { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; } = null;

    [JsonPropertyName("nicopediaArticleUrl")]
    public string? NicopediaArticleUrl { get; set; } = null;

    [JsonPropertyName("programProviderId")]
    public string? ProgramProviderId { get; set; } = null;

    [JsonPropertyName("userLevel")]
    public int? UserLevel { get; set; } = null;

    [JsonPropertyName("icons")]
    public ProgramProviderIcons? Icons { get; set; } = null;
}

public sealed class ProgramProviderIcons
{
    [JsonPropertyName("uri150x150")]
    public Uri Uri150x150 { get; set; }

    [JsonPropertyName("uri50x50")]
    public Uri Uri50x50 { get; set; }
}

public partial class Statistics
{
    [JsonPropertyName("comments")]
    public long Comments { get; set; }

    [JsonPropertyName("id")]
    public LiveId Id { get; set; }

    [JsonPropertyName("timeshiftReservations")]
    public long TimeshiftReservations { get; set; }

    [JsonPropertyName("viewers")]
    public long Viewers { get; set; }
}

public partial class Thumbnail
{
    [JsonPropertyName("large")]
    public Uri Large { get; set; }

    [JsonPropertyName("huge")]
    public Huge Huge { get; set; }

    [JsonPropertyName("small")]
    public Uri Small { get; set; }
}

public partial class Huge
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

public enum TimeshiftStatus
{
    BEFORE_OPEN,
    OPENED,
    CLOSED,
    ENDED,
    RELEASED
}

public enum TimeshiftWatchLimit
{
    UNLIMITED,
}

public enum TimeshiftRequirement
{
    NONE,
    PAYMENT,
    RESERVATION,
}

public partial class TimeshiftSetting
{
    [JsonPropertyName("watchLimit")]
    public string WatchLimitText { get; set; }

    [JsonPropertyName("requirement")]
    public string RequirementText { get; set; }

    [JsonPropertyName("status")]
    public string StatusText { get; set; }

    [JsonPropertyName("endTime")]
    public DateTimeOffset EndTime { get; set; }

    [JsonPropertyName("reservationDeadline")]
    public DateTimeOffset ReservationDeadline { get; set; }
}

public partial class TimeshiftTicket
{
    [JsonPropertyName("reserveTime")]
    public DateTimeOffset ReserveTime { get; set; }

    [JsonPropertyName("expireTime")]
    public DateTimeOffset? ExpireTime { get; set; }
}

//public partial class Restriction
//{
//    [JsonPropertyName("developmentFeatures")]
//    public object[] DevelopmentFeatures { get; set; }
//}


public partial class Superichiba
{
    [JsonPropertyName("deletable")]
    public bool Deletable { get; set; }

    [JsonPropertyName("hasBroadcasterRole")]
    public bool HasBroadcasterRole { get; set; }
}


public class PayProgram
{
    [JsonPropertyName("isMemberFree")]
    public bool IsMemberFree { get; set; }

    [JsonPropertyName("isOnSale")]
    public bool IsOnSale { get; set; }

    [JsonPropertyName("isTrialStreamEnabled")]
    public bool IsTrialStreamEnabled { get; set; }

    [JsonPropertyName("isTrialStreamShown")]
    public bool IsTrialStreamShown { get; set; }
}

public class Reservation
{
    [JsonPropertyName("features")]
    public Features Features { get; set; }

    /// <summary>
    /// True = 視聴可能 / False = 視聴不可
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("payProgram")]
    public PayProgram PayProgram { get; set; }

    [JsonPropertyName("program")]
    public Program Program { get; set; }

    [JsonPropertyName("programId")]
    public LiveId ProgramId { get; set; }

    [JsonPropertyName("programProvider")]
    public ProgramProvider ProgramProvider { get; set; }

    [JsonPropertyName("socialGroup")]
    public SocialGroup SocialGroup { get; set; }

    [JsonPropertyName("statistics")]
    public Statistics Statistics { get; set; }

    [JsonPropertyName("thumbnail")]
    public Thumbnail Thumbnail { get; set; }

    [JsonPropertyName("timeshiftSetting")]
    public TimeshiftSetting TimeshiftSetting { get; set; }

    [JsonPropertyName("timeshiftTicket")]
    public TimeshiftTicket TimeshiftTicket { get; set; }
}

public class SocialGroup
{
    [JsonPropertyName("companyName")]
    public string CompanyName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("isClosed")]
    public bool? IsClosed { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool? IsDeleted { get; set; }

    [JsonPropertyName("isFollowed")]
    public bool? IsFollowed { get; set; }

    [JsonPropertyName("isJoined")]
    public bool? IsJoined { get; set; }

    [JsonPropertyName("isPayChannel")]
    public bool? IsPayChannel { get; set; }

    [JsonPropertyName("isSafeThumbnail")]
    public bool? IsSafeThumbnail { get; set; }

    [JsonPropertyName("level")]
    public int? Level { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("socialGroupId")]
    public string SocialGroupId { get; set; }

    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; }

    [JsonPropertyName("thumbnailSmall")]
    public string ThumbnailSmall { get; set; }

    [JsonPropertyName("type")]
    public ProviderType Type { get; set; }
}

public class TrackingParams
{
    [JsonPropertyName("siteId")]
    public string SiteId { get; set; }

    [JsonPropertyName("pageId")]
    public string PageId { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; }
}

public class User
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("nickname")]
    public string Nickname { get; set; }

    [JsonPropertyName("isLoggedIn")]
    public bool IsLoggedIn { get; set; }

    [JsonPropertyName("accountType")]
    public string AccountType { get; set; }

    [JsonPropertyName("isOperator")]
    public bool IsOperator { get; set; }

    [JsonPropertyName("isBroadcaster")]
    public bool IsBroadcaster { get; set; }

    [JsonPropertyName("premiumOrigin")]
    public string PremiumOrigin { get; set; }

    //[JsonPropertyName("permissions")]
    //public List<object> Permissions { get; set; }

    [JsonPropertyName("isMailRegistered")]
    public bool IsMailRegistered { get; set; }

    [JsonPropertyName("isProfileRegistered")]
    public bool IsProfileRegistered { get; set; }

    [JsonPropertyName("isMobileMailAddressRegistered")]
    public bool IsMobileMailAddressRegistered { get; set; }

    [JsonPropertyName("isExplicitlyLoginable")]
    public bool IsExplicitlyLoginable { get; set; }

    [JsonPropertyName("nicosid")]
    public string Nicosid { get; set; }

    [JsonPropertyName("superichiba")]
    public Superichiba Superichiba { get; set; }

    [JsonPropertyName("iconUrl")]
    public Uri IconUrl { get; set; }
}
