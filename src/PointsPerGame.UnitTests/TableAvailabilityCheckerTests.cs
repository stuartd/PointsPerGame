using NUnit.Framework;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Services;
using Shouldly;
using System.Net;

namespace PointsPerGame.UnitTests;

public class TableAvailabilityCheckerTests
{
	[Test]
	public async Task CheckAllAsync_Checks_Every_Concrete_Table_And_Reports_Retrieval_Failures()
	{
		var dataSource = new StubDataSource();
		dataSource.SetFailure(
			TableSelection.SerieA,
			new HttpRequestException("Not found.", inner: null, HttpStatusCode.NotFound));
		dataSource.SetFailure(
			TableSelection.Ligue1,
			new InvalidOperationException("The table could not be parsed."));
		var checker = new TableAvailabilityChecker(dataSource);

		var result = await checker.CheckAllAsync();

		dataSource.RequestedTables.ShouldBe(LeagueLists.AllLeagues);
		result.ShouldBe([
			.. LeagueLists.AllLeagues.Select(table => new TableAvailability(
				table,
				IsAvailable: table is not TableSelection.SerieA and not TableSelection.Ligue1)),
		]);
	}

	private sealed class StubDataSource : IResultsDataSource
	{
		private readonly Dictionary<TableSelection, Exception> failures = [];

		public List<TableSelection> RequestedTables { get; } = [];

		public void SetFailure(TableSelection table, Exception exception) => failures[table] = exception;

		public ValueTask<IReadOnlyList<TeamResults>> GetResultsAsync(TableSelection tableSelection)
		{
			RequestedTables.Add(tableSelection);

			return failures.TryGetValue(tableSelection, out var exception)
				? ValueTask.FromException<IReadOnlyList<TeamResults>>(exception)
				: ValueTask.FromResult<IReadOnlyList<TeamResults>>([]);
		}
	}
}
