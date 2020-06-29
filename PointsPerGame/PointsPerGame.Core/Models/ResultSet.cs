namespace PointsPerGame.Core.Models
{
    public class HomeResultSet : ResultSet
    {
        public HomeResultSet()
            : base(ResultSetType.Home)
        {
        }
    }

    public class AwayResultSet : ResultSet
    {
        public AwayResultSet()
            : base(ResultSetType.Away)
        {
        }
    }

    public class CombinedResultSet : ResultSet
    {
        public CombinedResultSet() : base(ResultSetType.Composite)
        {
        }
    }

    public abstract class ResultSet
    {
        protected ResultSet(ResultSetType resultType)
        {
            Type = resultType;
        }

        public ResultSetType Type;

        public int Won;
        public int Drawn;
        public int Lost;
        public int GoalsScored;
        public int GoalsConceded;

        public int Played => Won + Drawn + Lost;

        public int GoalDifference => GoalsScored - GoalsConceded;

        public int Points => ((Won * 3) + Drawn);
    }
}