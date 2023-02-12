using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NiconicoToolkit.Search.Video
{
    public enum SortKey
    {
        [Description("hot")]
        Hot,

        [Description("personalized")]
        Personalized,

        [Description("registeredAt")]
        RegisteredAt,

        [Description("viewCount")]
        ViewCount,

        [Description("likeCount")]
        LikeCount,

        [Description("mylistCount")]
        MylistCount,

        [Description("lastCommentTime")]
        LastCommentTime,

        [Description("commentCount")]
        CommentCount,

        [Description("duration")]
        Duration
    }

    public enum SortOrder
    {
        [Description("asc")]
        Asc,

        [Description("desc")]
        Desc,

        [Description("none")]
        None
    }
}
