using System;
using System.Diagnostics;

namespace PointsPerGame.Core.Extensions;

[DebuggerStepThrough]
public static class StringExtensions
{
	extension(string s)
	{
		public bool HasValue()
		{
			return !s.IsNullOrEmpty();
		}

		public bool IsNullOrEmpty()
		{
			return string.IsNullOrEmpty(s);
		}

		public bool IsNullOrWhiteSpace()
		{
			return string.IsNullOrWhiteSpace(s);
		}

		/// <summary>
		/// If the string is longer than the allowed length, trim it and add an ellipsis  as the last character
		/// </summary>
		public string Truncate(int length)
		{
			if (s.IsNullOrEmpty() || s.Length < length)
			{
				return s;
			}

			if (length < 1)
			{
				throw new ArgumentException("Length cannot be less than one", nameof(length));
			}

			return s[..(length - 1)] + "…";
		}
	}
}