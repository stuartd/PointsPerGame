using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;

public interface IResultsDataSource
{
    ValueTask<IReadOnlyList<TeamResultDisplaySet>> GetResultsAsync(League league);
}