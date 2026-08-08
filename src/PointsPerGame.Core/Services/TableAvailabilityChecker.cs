using PointsPerGame.Core.Names;
using System.Runtime.Caching;

namespace PointsPerGame.Core.Services;

public readonly record struct TableAvailability(TableSelection Table, bool IsAvailable);

public sealed class TableAvailabilityChecker(IResultsDataSource dataSource)
{
	private const string CacheKey = "AllTables";
	private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(1);
	private static readonly MemoryCache cache = new(nameof(TableAvailabilityChecker));
	private static readonly SemaphoreSlim cacheLock = new(1, 1);
	private readonly IResultsDataSource dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

	public async ValueTask<IReadOnlyList<TableAvailability>> CheckAllAsync()
	{
		if (TryGetCachedAvailability(out var cachedAvailability))
		{
			return cachedAvailability;
		}

		await cacheLock.WaitAsync();

		try
		{
			if (TryGetCachedAvailability(out cachedAvailability))
			{
				return cachedAvailability;
			}

			var availability = await CheckAllUncachedAsync();
			cache.Set(CacheKey, availability, DateTimeOffset.Now.Add(CacheDuration));

			return availability;
		}
		finally
		{
			cacheLock.Release();
		}
	}

	private async ValueTask<IReadOnlyList<TableAvailability>> CheckAllUncachedAsync()
	{
		var availability = new List<TableAvailability>();

		foreach (var table in LeagueLists.AllLeagues)
		{
			availability.Add(await CheckAsync(table));
		}

		return availability.AsReadOnly();
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

	private static bool TryGetCachedAvailability(out IReadOnlyList<TableAvailability> availability)
	{
		if (cache.Get(CacheKey) is IReadOnlyList<TableAvailability> cachedAvailability)
		{
			availability = cachedAvailability;
			return true;
		}

		availability = [];
		return false;
	}
}
