using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.SnapshotSearch.JsonFilters
{
    public sealed class NotJsonFilter : IJsonSearchFilter
	{
		private readonly IJsonSearchFilter _filter;

		public NotJsonFilter(IJsonSearchFilter filter)
		{
			_filter = filter;
		}

		public IEnumerable<KeyValuePair<string, string>> GetFilterKeyValues(FilterGetKeyValuesContext context)
		{
			var json = JsonSerializer.Serialize(GetJsonFilterData(), JsonFilterSerializeHelper.SerializerOptions);
			yield return new KeyValuePair<string, string>(SearchConstants.JsonFilterParameter, json);
		}

		public IJsonSearchFilterData GetJsonFilterData()
		{
			return new NotJsonFilterData()
			{
				Filter = _filter.GetJsonFilterData()
			};
		}
	}

	public sealed class NotJsonFilterData : IJsonSearchFilterData
	{
		[JsonPropertyName("type")]
		public string Type => "not";

		[JsonPropertyName("filter")]
		public IJsonSearchFilterData Filter { get; set; }
	}

}
