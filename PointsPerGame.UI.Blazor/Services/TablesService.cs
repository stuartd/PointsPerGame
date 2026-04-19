using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PointsPerGame.Core.Names;

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
    }
}
