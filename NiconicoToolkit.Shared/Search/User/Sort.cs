using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NiconicoToolkit.Search.User
{
    public enum SortKey
    {
        [Description("_personalized")]
        Personalized,

        [Description("followerCount")]
        FollowerCount,

        [Description("videoCount")]
        VideoCount,

        [Description("liveCount")]
        LiveCount
    }
}
