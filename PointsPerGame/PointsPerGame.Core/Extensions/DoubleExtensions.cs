using System;

namespace PointsPerGame.Core.Extensions
{
    public static class DoubleExtensions
    {
        public static bool IsEqualTo(this double value, double other)
        {
            return Math.Abs(value - other) < double.Epsilon;
        }

        public static bool IsNotEqualTo(this double value, double other)
        {
            return Math.Abs(value - other) > double.Epsilon;
        }
    }
}
