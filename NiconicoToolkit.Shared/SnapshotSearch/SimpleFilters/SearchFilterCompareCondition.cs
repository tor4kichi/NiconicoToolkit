using System.ComponentModel;

namespace NiconicoToolkit.SnapshotSearch.Filters
{
    public enum SimpleFilterComparison

	{
		[Description("0")]
		Equal,

		[Description("gt")]
		GreaterThan,

		[Description("gte")]
		GreaterThanOrEqual,

		[Description("lt")]
		LessThan,

		[Description("lte")]
		LessThenOrEqual,
	}



}
