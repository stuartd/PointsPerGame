using System.ComponentModel;
using System.Reflection;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;

namespace PointsPerGame.UI.Blazor.Services
{
    public class TablesService(IDataSource dataSource) {
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
            double PointsPerGame,
            string TeamUrl,
            string Crest
        );

        public async Task<List<TableRowDto>> GetLeagueTableAsync(int leagueId)
        {
            var league = (League)leagueId;
			var results = await dataSource.GetResultsAsync(league);

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
                r.PointsPerGame,
                r.Url ?? string.Empty,
                r.Crest ?? string.Empty
            )).ToList();
        }
    }
}
