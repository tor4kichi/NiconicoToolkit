using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.SnapshotSearch.JsonFilters
{
    public sealed class AndJsonFilter : IJsonSearchFilter
	{
		public IList<IJsonSearchFilter> Filters { get; }

		public AndJsonFilter(IEnumerable<IJsonSearchFilter> filters)
		{
			Filters = filters.ToList();
		}

		public IEnumerable<KeyValuePair<string, string>> GetFilterKeyValues(FilterGetKeyValuesContext context)
		{
			var json = JsonSerializer.Serialize(GetJsonFilterData(), JsonFilterSerializeHelper.SerializerOptions);
			yield return new KeyValuePair<string, string>(SearchConstants.JsonFilterParameter, json);
		}

		public IJsonSearchFilterData GetJsonFilterData()
		{
			return new AndJsonFilterData()
			{
				Filters = Filters.Select(x => x.GetJsonFilterData()).ToList()
			};
		}

		public override string ToString()
		{
			return string.Join(" and ", Filters.Select(x => $"({x})"));
		}
	}


	public sealed class AndJsonFilterData : IJsonSearchFilterData
	{
		[JsonPropertyName("type")]
		public string Type => "and";

		[JsonPropertyName("filters")]
		public List<IJsonSearchFilterData> Filters { get; set; }
	}

}
