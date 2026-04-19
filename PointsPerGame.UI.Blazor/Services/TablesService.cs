using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;
using PointsPerGame.Core.Models;

namespace PointsPerGame.UI.Blazor.Services
{
    public class TablesService
    {
        public Task<Dictionary<int, string>> GetLeagueLinksAsync()
        {
            var values = Enum.GetValues(typeof(League)).Cast<League>();
            var dict = values.ToDictionary(l => (int)l, l =>
            {
                var member = l.GetType().GetMember(l.ToString()).Single();
                var attr = member.GetCustomAttribute<DescriptionAttribute>();
                return attr?.Description ?? l.ToString();
            });

            return Task.FromResult(dict);
        }

        // DTO used by the Blazor UI
        public record TableRowDto(
            string Team,
            int Played,
            int Won,
            int Drawn,
            int Lost,
            int GoalsScored,
            int GoalsConceded,
            int GoalDifference,
            int Points,
            string TeamUrl,
            string Crest
        );

        // Fetches table rows for the requested league by delegating to the Core GuardianScraper
        public async Task<List<TableRowDto>> GetLeagueTableAsync(int leagueId)
        {
            var league = (League)leagueId;
            // GuardianScraper is implemented in PointsPerGame.Core and exposes GetResults
            var results = await GuardianScraper.GetResults(league);

            return results.Select(r => new TableRowDto(
                r.Team,
                r.Played,
                r.Won,
                r.Drawn,
                r.Lost,
                r.GoalsScored,
                r.GoalsConceded,
                r.GoalDifference,
                r.Points,
                r.Url ?? string.Empty,
                r.Crest ?? string.Empty
            )).ToList();
        }
    }
}
