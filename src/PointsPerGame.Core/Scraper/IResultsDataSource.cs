using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;

public interface IResultsDataSource
{
    Task<IReadOnlyList<TeamResultDisplaySet>> GetResultsAsync(League league);
}