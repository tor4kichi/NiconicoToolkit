using CommunityToolkit.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NiconicoToolkit.SnapshotSearch;
using NiconicoToolkit.SnapshotSearch.JsonFilters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace NiconicoToolkit.Tests
{
    [TestClass]
    public class SnapshotFilterSearializeTest
    {

        [TestMethod]
        public void JsonDeserializeTest()
        {
            var jsonFilter = "{\"type\":\"and\",\"filters\":[{\"type\":\"equal\",\"field\":\"genre\",\"value\":\"アニメ\"},{\"type\":\"range\",\"field\":\"lengthSeconds\",\"from\":\"1200\"}]}";
            var deserialized = JsonSerializer.Deserialize<IJsonSearchFilterData>(jsonFilter, new JsonSerializerOptions()
            {
                Converters =
                {
                    new JsonFilterDataJsonConverter()
                }
            });

            Guard.IsOfType<AndJsonFilterData>(deserialized, nameof(deserialized));
            var andFilter = deserialized as AndJsonFilterData;

            Guard.IsOfType<EqualJsonFilterData>(andFilter.Filters[0], nameof(andFilter.Filters));
            var eqaulFilter = andFilter.Filters[0] as EqualJsonFilterData;
            Guard.IsEqualTo(SearchFieldType.Genre.GetDescription(), eqaulFilter.Field, nameof(eqaulFilter.Field));
            Guard.IsEqualTo("アニメ", eqaulFilter.Value, nameof(eqaulFilter.Value));

            Guard.IsOfType<RangeJsonFilterData>(andFilter.Filters[1], nameof(andFilter.Filters));
            var rangeFilter = andFilter.Filters[1] as RangeJsonFilterData;
            Guard.IsEqualTo(SearchFieldType.LengthSeconds.GetDescription(), rangeFilter.Field, nameof(rangeFilter.Field));
            Guard.IsEqualTo(1200, int.Parse(rangeFilter.From), nameof(rangeFilter.From));
        }
    }
}
