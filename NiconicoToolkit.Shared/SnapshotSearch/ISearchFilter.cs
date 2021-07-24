using System.Collections.Generic;

namespace NiconicoToolkit.SnapshotSearch
{
    public class FilterGetKeyValuesContext
    {
        Dictionary<SearchFieldType, int> _fieldContainsCount = new ();

        public int GetNextIndex(SearchFieldType searchFieldType)
        {
            _fieldContainsCount.TryAdd(searchFieldType, 0);
            var index = _fieldContainsCount[searchFieldType];
            _fieldContainsCount[searchFieldType] += 1;
            return index;
        }
    }
    public interface ISearchFilter
    {
		IEnumerable<KeyValuePair<string, string>> GetFilterKeyValues(FilterGetKeyValuesContext context);
    }

}
