using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;

public interface IResultsDataSource
{
    ValueTask<IReadOnlyList<TeamResults>> GetResultsAsync(TableSelection tableSelection);
}
