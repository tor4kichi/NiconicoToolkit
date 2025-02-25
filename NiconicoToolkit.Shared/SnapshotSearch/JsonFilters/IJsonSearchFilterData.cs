using System.Text.Json.Serialization;

namespace NiconicoToolkit.SnapshotSearch.JsonFilters;

[JsonDerivedType(typeof(AndJsonFilterData), nameof(AndJsonFilterData))]
[JsonDerivedType(typeof(OrJsonFilterData), nameof(OrJsonFilterData))]
[JsonDerivedType(typeof(NotJsonFilterData), nameof(NotJsonFilterData))]
[JsonDerivedType(typeof(EqualJsonFilterData), nameof(EqualJsonFilterData))]
[JsonDerivedType(typeof(RangeJsonFilterData), nameof(RangeJsonFilterData))]
public interface IJsonSearchFilterData
{
    string Type { get; }
}
