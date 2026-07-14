using PointsPerGame.Core.Mappings;
using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Services;
using System.ComponentModel;
using System.Reflection;
using PointsPerGame.Core.Extensions;

namespace PointsPerGame.UI.Blazor.Services;

public class TablesService(ILeagueTableService leagueTableService)
{
    // As this actually isn't doing anything async, using a ValueTask
    // to avoid the compiler spinning up a state machine that won't be used
    public static ValueTask<IReadOnlyDictionary<int, string>> GetLeagueLinksAsync()
    {
        IReadOnlyDictionary<int, string> links = Enum.GetValues<TableSelection>()
            .ToDictionary(league => (int)league, GetLeagueDescription);

        return ValueTask.FromResult(links);
    }

    public static string? GetLeagueName(int leagueId) => IsKnownLeague(leagueId) ? GetLeagueDescription((TableSelection)leagueId) : null;

    public static string? GetLeagueSourceUrl(int leagueId)
    {
        if (!IsKnownLeague(leagueId))
        {
            return null;
        }

        var league = (TableSelection)leagueId;

        return league.IsMultiLeague()
            ? null
            : GuardianLeagueMappings.GetUriForLeague(league);
    }

    public async Task<IReadOnlyList<TeamResults>> GetLeagueTableAsync(int leagueId)
    {
        if (!IsKnownLeague(leagueId))
        {
            return [];
        }

        var league = (TableSelection)leagueId;
        return await leagueTableService.GetResultsAsync(league);
    }

    private static bool IsKnownLeague(int leagueId) => Enum.IsDefined(typeof(TableSelection), leagueId);

    private static string GetLeagueDescription(TableSelection tableSelection)
    {
        var member = typeof(TableSelection).GetMember(tableSelection.ToString()).Single();
        var attr = member.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? tableSelection.ToString();
    }
}
