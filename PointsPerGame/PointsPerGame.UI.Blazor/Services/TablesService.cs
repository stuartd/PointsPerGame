using PointsPerGame.Core.DTOs;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Services;
using System.ComponentModel;
using System.Reflection;

namespace PointsPerGame.UI.Blazor.Services;

public class TablesService(ILeagueTableService leagueTableService)
{
    public static Task<IReadOnlyDictionary<int, string>> GetLeagueLinksAsync()
    {
        IReadOnlyDictionary<int, string> links = Enum.GetValues<League>()
            .ToDictionary(league => (int)league, GetLeagueDescription);

        return Task.FromResult(links);
    }

    public string? GetLeagueName(int leagueId)
    {
        return IsKnownLeague(leagueId) ? GetLeagueDescription((League)leagueId) : null;
    }

    public async Task<IReadOnlyList<TableRowDto>> GetLeagueTableAsync(int leagueId)
    {
        if (!IsKnownLeague(leagueId))
        {
            return [];
        }

        var league = (League)leagueId;
        var resultData = await leagueTableService.GetResultsAsync(league);

        return [.. resultData.Select(TableRowDto.FromResultSet)];
    }

    private static bool IsKnownLeague(int leagueId)
    {
        return Enum.IsDefined(typeof(League), leagueId);
    }

    private static string GetLeagueDescription(League league)
    {
        var member = typeof(League).GetMember(league.ToString()).Single();
        var attr = member.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? league.ToString();
    }
}
