using System;
using System.Collections.Generic;
using System.Text;

namespace NiconicoToolkit.Search.Video
{
    public enum Range
    {
        In1Hoour = 4,
        In24Hour = 1,
        In1Week = 2,
        InMonth = 3,
    }

    public static class RangeExtention
    {
        public static DateTime ToDateTime(this Range range)
        {
            DateTime now = DateTime.Now;
            switch (range)
            {
                case Range.In1Hoour:
                    return now.AddHours(-1);
                case Range.In24Hour:
                    return now.AddDays(-1);
                case Range.In1Week:
                    return now.AddDays(-7);
                case Range.InMonth:
                    return now.AddMonths(-1);
                default:
                    throw new NotSupportedException($"not support {nameof(Range)}.{range.ToString()}");
            }
        }
    }
}
