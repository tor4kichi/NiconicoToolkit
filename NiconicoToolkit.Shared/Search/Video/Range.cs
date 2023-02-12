using System;
using System.Collections.Generic;
using System.Text;

namespace NiconicoToolkit.Search.Video
{
    public enum Range
    {
        InHour = 4,
        In24Hours = 1,
        InWeek = 2,
        InMonth = 3,
    }

    public static class RangeExtention
    {
        public static DateTime ToDateTime(this Range range)
        {
            DateTime now = DateTime.Now;
            switch (range)
            {
                case Range.InHour:
                    return now.AddHours(-1);
                case Range.In24Hours:
                    return now.AddDays(-1);
                case Range.InWeek:
                    return now.AddDays(-7);
                case Range.InMonth:
                    return now.AddMonths(-1);
                default:
                    throw new NotSupportedException($"not support {nameof(Range)}.{range.ToString()}");
            }
        }
    }
}
