using System;

namespace NiconicoToolkit.SnapshotSearch
{
    public static class FilterValueHelper
    {
		public static string ToStringFilterValue<T>(T value)
        {
			if (value is DateTimeOffset dateTimeOffset)
            {
				var dateTime = dateTimeOffset;
				return dateTime.ToString("o");
            }
			else
            {
				return value.ToString();
            }
        }
    }

}
