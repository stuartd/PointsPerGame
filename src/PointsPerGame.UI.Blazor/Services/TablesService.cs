using PointsPerGame.Core.DTOs;
using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Services;
using System.ComponentModel;
using System.Reflection;

namespace PointsPerGame.UI.Blazor.Services;

public class TablesService(ILeagueTableService leagueTableService)
{
    // As this actually isn't doing anything async, using a ValueTask
    // to avoid the compiler spinning up a state machine that won't be used
    public static ValueTask<IReadOnlyDictionary<int, string>> GetLeagueLinksAsync()
    {
        IReadOnlyDictionary<int, string> links = Enum.GetValues<League>()
            .ToDictionary(league => (int)league, GetLeagueDescription);

        return ValueTask.FromResult(links);
    }

    public static string? GetLeagueName(int leagueId) => IsKnownLeague(leagueId) ? GetLeagueDescription((League)leagueId) : null;

    public static string? GetLeagueSourceUrl(int leagueId)
    {
        if (!IsKnownLeague(leagueId))
        {
            return null;
        }

        var league = (League)leagueId;

        return league is League.AllLeagues or League.AllTopDivisions
            ? null
            : GuardianLeagueMappings.GetUriForLeague(league);
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

    private static bool IsKnownLeague(int leagueId) => Enum.IsDefined(typeof(League), leagueId);

    private static string GetLeagueDescription(League league)
    {
        var member = typeof(League).GetMember(league.ToString()).Single();
        var attr = member.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? league.ToString();
    }
}
