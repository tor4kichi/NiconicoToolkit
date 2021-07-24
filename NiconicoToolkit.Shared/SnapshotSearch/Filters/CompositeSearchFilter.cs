using Microsoft.Toolkit.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace NiconicoToolkit.SnapshotSearch.Filters
{
    public class CompositeSearchFilter : ISearchFilter
	{
		List<ISearchFilter> _filters;

		public CompositeSearchFilter(params ISearchFilter[] filters)
        {
			_filters = filters.ToList();
		}		

		public CompositeSearchFilter AddCompareFilter<T>(SearchFieldType filterFieldType, T value, SearchFilterCompareCondition condition)
        {
			Guard.IsTrue(filterFieldType.IsAcceptableTypeForFiled<T>(), nameof(filterFieldType));
			_filters.Add(new CompareSearchFilter<T>(filterFieldType, value, condition));
			return this;
		}

		public IEnumerable<KeyValuePair<string, string>> GetFilterKeyValues(FilterGetKeyValuesContext context)
        {
			return _filters.SelectMany(x => x.GetFilterKeyValues(context));
        }
    }

}
