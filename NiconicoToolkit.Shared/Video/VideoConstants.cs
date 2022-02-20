using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NiconicoToolkit.Video
{
    public static class VideoConstants
    {
        public static readonly DateTimeOffset MostOldestVideoPostedAt = new DateTimeOffset(2007, 3, 6, 0, 33, 0, TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time").BaseUtcOffset);
    }
}
