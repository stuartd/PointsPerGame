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
	public async Task CheckAllAsync_Checks_Every_Concrete_Table_Reports_Retrieval_Failures_And_Caches_The_Result()
	{
		var dataSource = new StubDataSource();
		dataSource.SetFailure(
			TableSelection.SerieA,
			new HttpRequestException("Not found.", inner: null, HttpStatusCode.NotFound));
		dataSource.SetFailure(
			TableSelection.Ligue1,
			new InvalidOperationException("The table could not be parsed."));
		var cache = new TableAvailabilityCache();
		var checker = new TableAvailabilityChecker(dataSource, cache);

		var result = await checker.CheckAllAsync();

		dataSource.RequestedTables.ShouldBe(LeagueLists.AllLeagues);
		result.ShouldBe([
			.. LeagueLists.AllLeagues.Select(table => new TableAvailability(
				table,
				IsAvailable: table is not TableSelection.SerieA and not TableSelection.Ligue1)),
		]);

		var secondDataSource = new StubDataSource();
		var cachedResult = await new TableAvailabilityChecker(secondDataSource, cache).CheckAllAsync();

		secondDataSource.RequestedTables.ShouldBeEmpty();
		cachedResult.ShouldBe(result);
	}

	[Test]
	public async Task CheckAllAsync_Retries_Unavailable_Tables_After_Five_Minutes()
	{
		var timeProvider = new StubTimeProvider(new DateTimeOffset(2026, 8, 21, 12, 0, 0, TimeSpan.Zero));
		var dataSource = new StubDataSource();
		dataSource.SetFailure(
			TableSelection.Ligue1,
			new HttpRequestException("Not found.", inner: null, HttpStatusCode.NotFound));
		var checker = new TableAvailabilityChecker(dataSource, new TableAvailabilityCache(timeProvider));

		var initialResult = await checker.CheckAllAsync();

		initialResult.Single(result => result.Table == TableSelection.Ligue1).IsAvailable.ShouldBeFalse();
		dataSource.RequestedTables.ShouldBe(LeagueLists.AllLeagues);

		dataSource.RequestedTables.Clear();
		dataSource.ClearFailure(TableSelection.Ligue1);
		timeProvider.Advance(TimeSpan.FromMinutes(4));

		var cachedResult = await checker.CheckAllAsync();

		cachedResult.Single(result => result.Table == TableSelection.Ligue1).IsAvailable.ShouldBeFalse();
		dataSource.RequestedTables.ShouldBeEmpty();

		timeProvider.Advance(TimeSpan.FromMinutes(1));

		var refreshedResult = await checker.CheckAllAsync();

		refreshedResult.All(result => result.IsAvailable).ShouldBeTrue();
		dataSource.RequestedTables.ShouldBe([TableSelection.Ligue1]);
	}

	private sealed class StubDataSource : IResultsDataSource
	{
		private readonly Dictionary<TableSelection, Exception> failures = [];

		public List<TableSelection> RequestedTables { get; } = [];

		public void SetFailure(TableSelection table, Exception exception) => failures[table] = exception;
		public void ClearFailure(TableSelection table) => failures.Remove(table);

		public ValueTask<IReadOnlyList<TeamResults>> GetResultsAsync(TableSelection tableSelection)
		{
			RequestedTables.Add(tableSelection);

			return failures.TryGetValue(tableSelection, out var exception)
				? ValueTask.FromException<IReadOnlyList<TeamResults>>(exception)
				: ValueTask.FromResult<IReadOnlyList<TeamResults>>([]);
		}
	}

	private sealed class StubTimeProvider(DateTimeOffset utcNow) : TimeProvider
	{
		private DateTimeOffset utcNow = utcNow;

		public override DateTimeOffset GetUtcNow() => utcNow;

		public void Advance(TimeSpan duration) => utcNow = utcNow.Add(duration);
	}
}
