using System.Diagnostics;

namespace PointsPerGame.Core.Extensions;

[DebuggerStepThrough]
public static class StringExtensions
{
    public static bool HasValue(this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }

    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// If the string is longer than the allowed length, trim it and add an ellipsis as the last character.
    /// </summary>
    public static string? Truncate(this string? value, int length)
    {
        if (string.IsNullOrEmpty(value) || value.Length < length)
        {
            return value;
        }

        if (length < 1)
        {
            throw new ArgumentException("Length cannot be less than one", nameof(length));
        }

        return value[..(length - 1)] + "...";
    }
}

