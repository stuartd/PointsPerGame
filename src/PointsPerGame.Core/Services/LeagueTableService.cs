using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;

namespace PointsPerGame.Core.Services;

public interface ILeagueTableService
{
	Task<IReadOnlyList<TeamResultDisplaySet>> GetResultsAsync(League league);
}

public sealed class LeagueTableService(IResultsDataSource dataSource) : ILeagueTableService
{
	public async Task<IReadOnlyList<TeamResultDisplaySet>> GetResultsAsync(League league)
	{
		var leagues = GetSourceLeagues(league);
		var results = new List<TeamResultDisplaySet>();

		foreach (var sourceLeague in leagues)
		{
			results.AddRange(await dataSource.GetResultsAsync(sourceLeague));
		}

		return [.. results.SortTeams()];
	}

    private static League[] GetSourceLeagues(League league) => league switch
    {
        League.AllLeagues => LeagueLists.AllLeagues,
        League.AllTopDivisions => LeagueLists.AllTopDivisions,
        League.AllEnglishDivisions => LeagueLists.AllEnglishDivisions,
        _ => [league],
    };
}
