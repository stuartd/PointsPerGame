using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace PointsPerGame.Core.Utilities
{
    public static class CheckParameter
    {
        /// <summary>
        /// Requires the specified condition returns true, otherwise throws ArgumentException.
        /// </summary>
        /// <example>
        /// <code>
        /// Check.Require(() => param, p => p != string.Empty)
        /// </code>
        /// <code>
        /// Check.Require(() => myobject.Property, p => p > 0)
        /// </code>
        /// </example>
        public static void Require<T>(Expression<Func<T>> expression, Predicate<T> condition, string message = null)
        {
            string name;
            T value;
            GetParamNameAndValue(expression, out name, out value);

            if (!condition.Invoke(value))
            {
                var method = FindMethod();
                var exceptionMessage = message
                    ?? string.Format("The parameter '{0}' of type '{1}' from method '{2}' failed the predicate.", name, typeof(T).Name, method);

                throw new ArgumentException(exceptionMessage, name);
            }
        }

        /// <summary>
        /// Requires the specified parameter is not null, otherwise throws ArgumentException.
        /// </summary>
        /// <example>
        /// <code>
        /// Check.RequireNotNull(() => parameterName)
        /// </code>
        /// </example>
        public static void RequireNotNull<T>(Expression<Func<T>> expression) where T : class
        {
            string name;
            T value;
            GetParamNameAndValue(expression, out name, out value);

            if (value == null)
            {
                var method = FindMethod();
                var message = string.Format("The parameter '{0}' of type '{1}' from method '{2}' is null.", name, typeof(T).Name, method);

                throw new ArgumentNullException(name, message);
            }
        }

        /// <summary>
        /// Requires the specified string parameter is not null or empty, otherwise throws ArgumentException.
        /// </summary>
        /// <example>
        /// <code>
        /// Check.RequireNotNullOrEmpty(() => parameterName)
        /// </code>
        /// </example>
        public static void RequireNotNullOrEmpty(Expression<Func<string>> expression)
        {
            string name;
            string value;
            GetParamNameAndValue(expression, out name, out value);

            if (string.IsNullOrEmpty(value))
            {
                var method = FindMethod();
                var message = string.Format("The string parameter '{0}' from method '{1}' is null or empty.", name, method);

                throw new ArgumentException(message, name);
            }
        }

        private static void GetParamNameAndValue<T>(Expression<Func<T>> expression, out string name, out T value)
        {
            var memberExpr = expression.Body as MemberExpression;

            if (memberExpr == null)
            {
                throw new InvalidOperationException("Unable to find the expression.");
            }

            name = memberExpr.Member.Name;
            var expr = expression.Compile();
            value = expr.Invoke();
        }

        private static string FindMethod()
        {
            var trace = new StackTrace();
            var method = trace.GetFrames().ElementAt(2);
            return method.GetMethod().Name;
        }
    }
}
