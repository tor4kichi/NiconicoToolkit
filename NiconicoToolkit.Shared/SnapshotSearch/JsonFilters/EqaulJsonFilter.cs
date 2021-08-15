using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.SnapshotSearch.JsonFilters
{
    public sealed class EqualJsonFilter : IValueJsonSearchFilter
	{
		public SearchFieldType FieldType { get; }
		public object Value { get; }
        
        public EqualJsonFilter(SearchFieldType filterType, object value)
		{
			FieldType = filterType;
			Value = value;
		}

		public IEnumerable<KeyValuePair<string, string>> GetFilterKeyValues(FilterGetKeyValuesContext context)
		{
			var json = JsonSerializer.Serialize(GetJsonFilterData(), JsonFilterSerializeHelper.SerializerOptions);
			yield return new KeyValuePair<string, string>(SearchConstants.JsonFilterParameter, json);
		}

        public IJsonSearchFilterData GetJsonFilterData()
        {
			return new EqualJsonFilterData()
			{
				Field = FieldType.GetDescription(),
				Value = FilterValueHelper.ToStringFilterValue(Value)
			};
		}

        public override string ToString()
        {
			return $"{FieldType} = {Value}";
        }
    }


	public sealed class EqualJsonFilterData : IJsonSearchFilterData
	{
		[JsonPropertyName("type")]
		public string Type => "equal";

		[JsonPropertyName("field")]
		public string Field { get; set; }

		[JsonPropertyName("value")]
		public string Value { get; set; }
	}


}
