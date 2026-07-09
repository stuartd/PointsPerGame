using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;

namespace PointsPerGame.Core.Services;

public interface ILeagueTableService
{
	Task<IReadOnlyList<TeamResultDisplaySet>> GetResultsAsync(League league);
}

public sealed class LeagueTableService(IDataSource dataSource) : ILeagueTableService
{
	public async Task<IReadOnlyList<TeamResultDisplaySet>> GetResultsAsync(League league)
	{
		var leagues = GetSourceLeagues(league);
		var results = new List<TeamResultDisplaySet>();

		foreach (var sourceLeague in leagues)
		{
			results.AddRange(await dataSource.GetResultsAsync(sourceLeague));
		}

		return results.SortTeams().ToList();
	}

	private static IEnumerable<League> GetSourceLeagues(League league)
	{
		return league switch
		{
			League.All => LeagueLists.AllLeagues,
			League.AllTopDivisions => LeagueLists.AllTopDivisions,
			_ => [league],
		};
	}
}
