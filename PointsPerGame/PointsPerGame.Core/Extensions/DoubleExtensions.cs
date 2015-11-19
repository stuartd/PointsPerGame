using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointsPerGame.Core.Extensions
{
    public static class DoubleExtensions
    {
        public static bool IsEqualTo(this double value, double other)
        {
            return Math.Abs(value - other) < Double.Epsilon;
        }

        public static bool IsNotEqualTo(this double value, double other)
        {
            return Math.Abs(value - other) > Double.Epsilon;
        }
    }
}
