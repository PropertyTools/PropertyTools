// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanParser.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Parses a string to a TimeSpan.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Parses a string to a TimeSpan.
    /// </summary>
    public class TimeSpanParser
    {
        /// <summary>
        /// The parser expression.
        /// </summary>
        private static readonly Regex ParserExpression = new Regex(
            @"([0-9]+(?:[,.][0-9]+)?)\s*([dhms'""]?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatString">The format string.</param>
        /// <returns>
        /// A TimeSpan.
        /// </returns>
        public static TimeSpan Parse(string value, string formatString = null)
        {
            TimeSpan result;
            if (TryParse(value, formatString, out result))
            {
                return result;
            }

            throw new FormatException(string.Format("Invalid TimeSpan value '{0}'.", value));
        }

        /// <summary>
        /// Tries to parse the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The parsed time span.</param>
        /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string value, out TimeSpan result)
        {
            return TryParse(value, null, out result);
        }

        /// <summary>
        /// Tries to parse the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatString">The format string.</param>
        /// <param name="result">The parsed time span.</param>
        /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string value, string formatString, out TimeSpan result)
        {
            result = default(TimeSpan);
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(formatString))
                {
                    return new FormattedTimeSpanParser(formatString).TryParse(value, out result);
                }

                if (value.Contains(":"))
                {
                    return TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out result);
                }

                // otherwise support values as:
                // "12d"
                // "12d 5h"
                // "5m 3s"
                // "12.5d"
                var total = new TimeSpan();
                var position = 0;
                var success = false;
                foreach (Match m in ParserExpression.Matches(value))
                {
                    if (!m.Success || m.Length == 0)
                    {
                        continue;
                    }

                    if (!HasValidSeparator(value, position, m.Index))
                    {
                        return false;
                    }

                    var number = m.Groups[1].Value;
                    double d;
                    if (!double.TryParse(number.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out d))
                    {
                        return false;
                    }

                    switch (m.Groups[2].Value.ToLowerInvariant())
                    {
                        case "":
                        case "d":
                            total = total.Add(TimeSpan.FromDays(d));
                            break;
                        case "h":
                            total = total.Add(TimeSpan.FromHours(d));
                            break;
                        case "m":
                        case "'":
                            total = total.Add(TimeSpan.FromMinutes(d));
                            break;
                        case "\"":
                        case "s":
                            total = total.Add(TimeSpan.FromSeconds(d));
                            break;
                        default:
                            return false;
                    }

                    success = true;
                    position = m.Index + m.Length;
                }

                if (!success || !HasValidTrailingSeparator(value, position))
                {
                    return false;
                }

                result = total;
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the text between two matches contains only supported separators.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">Start index.</param>
        /// <param name="endIndex">End index.</param>
        /// <returns><c>true</c> if the separator is valid; otherwise, <c>false</c>.</returns>
        private static bool HasValidSeparator(string value, int startIndex, int endIndex)
        {
            return IsSeparator(value.Substring(startIndex, endIndex - startIndex), allowEmpty: true);
        }

        /// <summary>
        /// Determines whether the trailing text contains only whitespace.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns><c>true</c> if the trailing text is valid; otherwise, <c>false</c>.</returns>
        private static bool HasValidTrailingSeparator(string value, int startIndex)
        {
            return IsSeparator(value.Substring(startIndex), allowEmpty: true) && !value.Substring(startIndex).Contains("+");
        }

        /// <summary>
        /// Determines whether the specified separator is valid.
        /// </summary>
        /// <param name="separator">The separator text.</param>
        /// <param name="allowEmpty">if set to <c>true</c> empty separators are allowed.</param>
        /// <returns><c>true</c> if the separator is valid; otherwise, <c>false</c>.</returns>
        private static bool IsSeparator(string separator, bool allowEmpty)
        {
            if (string.IsNullOrWhiteSpace(separator))
            {
                return allowEmpty;
            }

            return string.Equals(separator.Trim(), "+", StringComparison.Ordinal);
        }
    }
}