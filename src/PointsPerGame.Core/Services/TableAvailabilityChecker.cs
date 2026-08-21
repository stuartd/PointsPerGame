using PointsPerGame.Core.Names;
using System.Collections.Concurrent;

namespace PointsPerGame.Core.Services;

public readonly record struct TableAvailability(TableSelection Table, bool IsAvailable);

public sealed class TableAvailabilityCache
{
	private static readonly TimeSpan availableCacheDuration = TimeSpan.FromDays(1);
	private static readonly TimeSpan unavailableCacheDuration = TimeSpan.FromMinutes(5);
	private readonly ConcurrentDictionary<TableSelection, CacheEntry> entries = [];
	private readonly SemaphoreSlim cacheLock = new(1, 1);
	private readonly TimeProvider timeProvider;

	public TableAvailabilityCache() : this(TimeProvider.System)
	{
	}

	public TableAvailabilityCache(TimeProvider timeProvider)
	{
		this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
	}

	internal async ValueTask<TableAvailability> GetOrCreateAsync(
		TableSelection table,
		Func<ValueTask<TableAvailability>> createAvailability)
	{
		if (TryGet(table, out var cachedAvailability))
		{
			return cachedAvailability;
		}

		await cacheLock.WaitAsync();

		try
		{
			if (TryGet(table, out cachedAvailability))
			{
				return cachedAvailability;
			}

			var availability = await createAvailability();
			var cacheDuration = availability.IsAvailable ? availableCacheDuration : unavailableCacheDuration;
			entries[table] = new(availability, timeProvider.GetUtcNow().Add(cacheDuration));

			return availability;
		}
		finally
		{
			cacheLock.Release();
		}
	}

	private bool TryGet(TableSelection table, out TableAvailability availability)
	{
		if (entries.TryGetValue(table, out var entry) && entry.ExpiresAt > timeProvider.GetUtcNow())
		{
			availability = entry.Availability;
			return true;
		}

		availability = default;
		return false;
	}

	private readonly record struct CacheEntry(TableAvailability Availability, DateTimeOffset ExpiresAt);
}

public sealed class TableAvailabilityChecker(
	IResultsDataSource dataSource,
	TableAvailabilityCache cache)
{
	private readonly IResultsDataSource dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
	private readonly TableAvailabilityCache cache = cache ?? throw new ArgumentNullException(nameof(cache));

	public async ValueTask<IReadOnlyList<TableAvailability>> CheckAllAsync()
	{
		var availability = new List<TableAvailability>();

		foreach (var table in LeagueLists.AllLeagues)
		{
			availability.Add(await cache.GetOrCreateAsync(table, () => CheckAsync(table)));
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
}
