using NUnit.Framework;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Services;
using Shouldly;

namespace PointsPerGame.UnitTests;

public class TableAvailabilityExtensionsTests
{
	private static readonly IReadOnlyList<TableAvailability> availability = [
		.. LeagueLists.AllLeagues.Select(table => new TableAvailability(
			table,
			IsAvailable: table is not TableSelection.EnglishChampionship and not TableSelection.SerieA)),
	];

	[Test]
	public void GetUnavailableTablesFor_Returns_Unavailable_Tables_Used_By_The_Requested_Selection()
	{
		availability.GetUnavailableTablesFor(TableSelection.AllLeagues)
			.ShouldBe([TableSelection.EnglishChampionship, TableSelection.SerieA]);
		availability.GetUnavailableTablesFor(TableSelection.AllTopDivisions)
			.ShouldBe([TableSelection.SerieA]);
		availability.GetUnavailableTablesFor(TableSelection.AllEnglishDivisions)
			.ShouldBe([TableSelection.EnglishChampionship]);
		availability.GetUnavailableTablesFor(TableSelection.SerieA)
			.ShouldBe([TableSelection.SerieA]);
		availability.GetUnavailableTablesFor(TableSelection.Ligue1)
			.ShouldBeEmpty();
		availability.GetUnavailableTablesFor(TableSelection.AllScottishDivisions)
			.ShouldBeEmpty();
	}
}
