using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;
using System.ComponentModel;
using System.Reflection;
using PointsPerGame.Core.DTOs;

namespace PointsPerGame.UI.Blazor.Services
{
    public class TablesService(IDataSource dataSource) {
        public static Task<Dictionary<int, string>> GetLeagueLinksAsync()
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

        public async Task<IReadOnlyList<TableRowDto>> GetLeagueTableAsync(int leagueId)
        {
            var league = (League)leagueId;
			var resultData = await dataSource.GetResultsAsync(league);
            
			return [ ..resultData.Select(TableRowDto.FromResultSet)];
		}
    }
}
