using NUnit.Framework;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Services;
using Shouldly;

namespace PointsPerGame.UnitTests;

public class TableAvailabilityExtensionsTests
{
	private static readonly IReadOnlyList<TableAvailability> Availability = [
		.. LeagueLists.AllLeagues.Select(table => new TableAvailability(
			table,
			IsAvailable: table is not TableSelection.EnglishChampionship and not TableSelection.SerieA)),
	];

	[Test]
	public void GetUnavailableTablesFor_Returns_Unavailable_Tables_Used_By_The_Requested_Selection()
	{
		Availability.GetUnavailableTablesFor(TableSelection.AllLeagues)
			.ShouldBe([TableSelection.EnglishChampionship, TableSelection.SerieA]);
		Availability.GetUnavailableTablesFor(TableSelection.AllTopDivisions)
			.ShouldBe([TableSelection.SerieA]);
		Availability.GetUnavailableTablesFor(TableSelection.AllEnglishDivisions)
			.ShouldBe([TableSelection.EnglishChampionship]);
		Availability.GetUnavailableTablesFor(TableSelection.SerieA)
			.ShouldBe([TableSelection.SerieA]);
		Availability.GetUnavailableTablesFor(TableSelection.Ligue1)
			.ShouldBeEmpty();
		Availability.GetUnavailableTablesFor(TableSelection.AllScottishDivisions)
			.ShouldBeEmpty();
	}
}
