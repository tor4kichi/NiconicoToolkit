using System.Collections.Generic;

namespace NiconicoToolkit.SnapshotSearch.Filters
{
    public class CompareSimpleSearchFilter<T> : ISimpleSearchFilter
	{
		public SearchFieldType FilterType { get; }
		public T Value { get; }
		public SimpleFilterComparison Condition { get; }

		public CompareSimpleSearchFilter(SearchFieldType filterType, T value, SimpleFilterComparison condition)
		{
			FilterType = filterType;
			Value = value;
			Condition = condition;
		}

		public IEnumerable<KeyValuePair<string, string>> GetFilterKeyValues(FilterGetKeyValuesContext context)
		{
			if (Condition == SimpleFilterComparison.Equal)
            {
				var index = context.GetNextIndex(FilterType);
				yield return new KeyValuePair<string, string>($"filters[{FilterType.GetDescription()}][{index}]", FilterValueHelper.ToStringFilterValue(Value));
			}
			else
            {
				yield return new KeyValuePair<string, string>($"filters[{FilterType.GetDescription()}][{Condition.GetDescription()}]", FilterValueHelper.ToStringFilterValue(Value));
			}			
		}
	}



}
