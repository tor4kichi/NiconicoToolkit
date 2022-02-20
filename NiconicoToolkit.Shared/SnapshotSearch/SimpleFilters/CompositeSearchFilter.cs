using Microsoft.Toolkit.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace NiconicoToolkit.SnapshotSearch.Filters
{
    public class CompositeSearchFilter : ISimpleSearchFilter
	{
		public List<ISimpleSearchFilter> Filters { get; }
		public CompositeSearchFilter(params ISimpleSearchFilter[] filters)
        {
			Filters = filters.ToList();
		}		

		public CompositeSearchFilter AddCompareFilter<T>(SearchFieldType filterFieldType, T value, SimpleFilterComparison condition)
        {
			Guard.IsTrue(filterFieldType.IsAcceptableTypeForFiled(value.GetType()), nameof(filterFieldType));

			Filters.Add(new CompareSimpleSearchFilter<T>(filterFieldType, value, condition));
			return this;
		}

		public IEnumerable<KeyValuePair<string, string>> GetFilterKeyValues(FilterGetKeyValuesContext context)
        {
			return Filters.SelectMany(x => x.GetFilterKeyValues(context));
        }
    }

}
