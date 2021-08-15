namespace NiconicoToolkit.SnapshotSearch.JsonFilters
{
    public interface IJsonSearchFilter : ISearchFilter
	{
		IJsonSearchFilterData GetJsonFilterData();
    }

    public interface IValueJsonSearchFilter : IJsonSearchFilter
    {
        SearchFieldType FieldType { get; }
    }
}
