using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Services;

public readonly record struct TableAvailability(TableSelection Table, bool IsAvailable);

public sealed class TableAvailabilityChecker(IResultsDataSource dataSource)
{
	private readonly IResultsDataSource dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

	public async ValueTask<IReadOnlyList<TableAvailability>> CheckAllAsync()
	{
		var availability = new List<TableAvailability>();

		foreach (var table in LeagueLists.AllLeagues)
		{
			availability.Add(await CheckAsync(table));
		}

		return availability;
	}

	private async ValueTask<TableAvailability> CheckAsync(TableSelection table)
	{
		try
		{
			await dataSource.GetResultsAsync(table);
			return new(table, IsAvailable: true);
		}
		catch (Exception exception) when (exception is HttpRequestException or InvalidOperationException or OperationCanceledException)
		{
			return new(table, IsAvailable: false);
		}
	}
}
