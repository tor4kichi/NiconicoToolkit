using System;

namespace NiconicoToolkit.SnapshotSearch
{
    public static class FilterValueHelper
    {
		public static string ToStringFilterValue(object value)
        {
			if (value is DateTimeOffset dateTimeOffset)
            {
				var dateTime = dateTimeOffset;
				return dateTime.ToString("o");
            }
			else
            {
				return value?.ToString() ?? string.Empty;
            }
        }
    }

}
