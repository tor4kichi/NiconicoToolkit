using Microsoft.Toolkit.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace NiconicoToolkit.SnapshotSearch.Filters
{
    public class CompositeSearchFilter : ISimpleSearchFilter
	{
		List<ISimpleSearchFilter> _filters;

		public IReadOnlyList<ISimpleSearchFilter> Filters { get; }
		public CompositeSearchFilter(params ISimpleSearchFilter[] filters)
        {
			_filters = filters.ToList();
			Filters = _filters.AsReadOnly();
		}		

		public CompositeSearchFilter AddCompareFilter<T>(SearchFieldType filterFieldType, T value, SimpleFilterComparison condition)
        {
			Guard.IsTrue(filterFieldType.IsAcceptableTypeForFiled(value.GetType()), nameof(filterFieldType));

			_filters.Add(new CompareSimpleSearchFilter<T>(filterFieldType, value, condition));
			return this;
		}

		public IEnumerable<KeyValuePair<string, string>> GetFilterKeyValues(FilterGetKeyValuesContext context)
        {
			return _filters.SelectMany(x => x.GetFilterKeyValues(context));
        }
    }

}
