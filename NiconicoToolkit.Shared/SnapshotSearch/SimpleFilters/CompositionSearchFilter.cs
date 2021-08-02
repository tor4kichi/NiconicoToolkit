using System.Collections.Generic;
using System.Linq;

namespace NiconicoToolkit.SnapshotSearch.Filters
{
    public class CompositionSearchFilter : ISimpleSearchFilter
	{
		private readonly IEnumerable<ISearchFilter> _filters;

		public CompositionSearchFilter(IEnumerable<ISimpleSearchFilter> filters)
		{
			_filters = filters;
		}

		public IEnumerable<KeyValuePair<string, string>> GetFilterKeyValues(FilterGetKeyValuesContext context)
		{
			return _filters.SelectMany(x => x.GetFilterKeyValues(context));
		}
	}



}
