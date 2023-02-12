using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NiconicoToolkit.Search.List
{
    public enum SortKey
    {
        [Description("_hotTotalScore")]
        HotTotalScore,

        [Description("videoCount")]
        VideoCount,

        [Description("startTime")]
        StartTime
    }

    public enum SortOrder
    {
        [Description("asc")]
        Asc,

        [Description("desc")]
        Desc
    }
}
