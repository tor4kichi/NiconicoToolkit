using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NiconicoToolkit.Search.Live
{
    public enum Sort
    {
        [Description("recentAsc")]
        RecentAsc,

        [Description("recentDesc")]
        RecentDesc,

        [Description("popular")]
        Popular,

        [Description("timeshiftReservationsDesc")]
        TimeshiftReservationsDesc,

        [Description("timeshiftReservationsAsc")]
        TimeshiftReservationsAsc,

        [Description("viewersDesc")]
        ViewersDesc,

        [Description("viewersAsc")]
        ViewersAsc,

        [Description("commentsDesc")]
        CommentsDesc,

        [Description("commentsAsc")]
        CommentsAsc,

        [Description("userLevelDesc")]
        UserLevelDesc,

        [Description("userLevelAsc")]
        UserLevelAsc,

        [Description("communityCreatedDesc")]
        CommunityCreatedDesc,

        [Description("communityCreatedAsc")]
        CommunityCreatedAsc
    }
}
