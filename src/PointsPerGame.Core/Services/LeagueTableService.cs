using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;

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
		var leagues = GetSourceLeagues(tableSelection);
		var results = new List<TeamResults>();

		foreach (var sourceLeague in leagues)
		{
			results.AddRange(await dataSource.GetResultsAsync(sourceLeague));
		}

		return [.. results.SortTeams(pointsForWin: PointsForWin)];
	}

/// <summary>
/// A 'league' here can be a single league or a grouping of leagues (e.g. AllLeagues)
/// so return the underlying league(s) for the league value.
/// </summary>
    private static TableSelection[] GetSourceLeagues(TableSelection tableSelection) => tableSelection switch
    {
        TableSelection.AllLeagues => LeagueLists.AllLeagues,
        TableSelection.AllTopDivisions => LeagueLists.AllTopDivisions,
        TableSelection.AllEnglishDivisions => LeagueLists.AllEnglishDivisions,
        _ => [tableSelection],
    };
}
