using PointsPerGame.Core.Extensions;
using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Services;

public static class TableAvailabilityExtensions
{
	public static IReadOnlyList<TableSelection> GetUnavailableTablesFor(
		this IEnumerable<TableAvailability> availability,
		TableSelection requestedTable)
	{
		ArgumentNullException.ThrowIfNull(availability);

		var unavailableTables = availability
			.Where(tableAvailability => tableAvailability.IsAvailable == false)
			.Select(tableAvailability => tableAvailability.Table)
			.ToHashSet();

		return [.. requestedTable.GetConcreteTables().Where(unavailableTables.Contains)];
	}
}
