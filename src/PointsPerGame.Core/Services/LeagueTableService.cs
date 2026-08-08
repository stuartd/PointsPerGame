using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Extensions;

namespace PointsPerGame.Core.Services;

public interface ILeagueTableService
{
	ValueTask<IReadOnlyList<TeamResults>> GetResultsAsync(TableSelection tableSelection);
}

public sealed class LeagueTableService(IResultsDataSource dataSource) : ILeagueTableService
{
	private const int PointsForWin = 3;

	public async ValueTask<IReadOnlyList<TeamResults>> GetResultsAsync(TableSelection tableSelection)
	{
		var leagues = tableSelection.GetConcreteTables();
		var results = new List<TeamResults>();

		foreach (var sourceLeague in leagues)
		{
			results.AddRange(await dataSource.GetResultsAsync(sourceLeague));
		}

		return [.. results.SortTeams(pointsForWin: PointsForWin)];
	}
}
