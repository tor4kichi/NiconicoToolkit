using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NiconicoToolkit.SnapshotSearch.JsonFilters
{
    public sealed class RangeJsonFilter : IValueJsonSearchFilter
	{
        private RangeJsonFilterData _rangeJsonFilterData;
        
        public RangeJsonFilter(SearchFieldType filterType, object from, object to, bool include_lower = true, bool include_upper = true)
        {
			_rangeJsonFilterData = new RangeJsonFilterData()
			{
				Field = filterType.GetDescription(),
				From = from is not null ? FilterValueHelper.ToStringFilterValue(from) : null,
				To = to is not null ? FilterValueHelper.ToStringFilterValue(to) : null,
				IncludeLower = include_lower == false ? false : default(bool?),
				IncludeUpper = include_upper == false ? false : default(bool?),
			};

            FieldType = filterType;
            From = from;
            To = to;
            IncludeLower = include_lower;
            IncludeUpper = include_upper;
        }

        public SearchFieldType FieldType { get; }
        public object From { get; }
        public object To { get; }
        public bool IncludeLower { get; }
        public bool IncludeUpper { get; }

        public IEnumerable<KeyValuePair<string, string>> GetFilterKeyValues(FilterGetKeyValuesContext context)
        {
			var json = JsonSerializer.Serialize(GetJsonFilterData(), JsonFilterSerializeHelper.SerializerOptions);
			yield return new KeyValuePair<string, string>(SearchConstants.JsonFilterParameter, json);
		}

        public IJsonSearchFilterData GetJsonFilterData()
        {
            return _rangeJsonFilterData;
		}

        public override string ToString()
        {
			var d = _rangeJsonFilterData;
			return $"{d.From} {(d.IncludeLower == true ? "<=" : "<")} {d.Field} {(d.IncludeUpper == true ? "<=" : "<")} {d.To}}}";
        }
    }

	public sealed class RangeJsonFilterData : IJsonSearchFilterData
	{
		[JsonPropertyName("type")]
		public string Type => "range";

		[JsonPropertyName("field")]
		public string Field { get; set; }

		[JsonPropertyName("from")]
		public string From { get; set; }

		[JsonPropertyName("to")]
		public string To { get; set; }

		[JsonPropertyName("include_lower")]
		public bool? IncludeLower { get; set; }

		[JsonPropertyName("include_upper")]
		public bool? IncludeUpper { get; set; }
	}
}
