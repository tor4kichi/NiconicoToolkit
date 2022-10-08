using System;
using System.Collections.Generic;
using System.Text;

namespace NiconicoToolkit
{
    public static class DateTimeHelper
    {
        public static TimeZoneInfo NiconicoTimeZone { get; } = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
    }
}
