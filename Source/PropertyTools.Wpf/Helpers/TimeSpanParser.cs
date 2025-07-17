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
            @"([0-9]*[,|.]?[0-9]*)\s*([d|h|m|s|'|""]?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
            // Examples
            // FormatString = MM:ss, value = "91:12" => 91 minutes 12seconds
            // FormatString = HH:mm, value = "91:12" => 91 hours 12minutes
            if (value.Contains(":"))
            {
                return TimeSpan.Parse(value, CultureInfo.InvariantCulture);
            }

            // otherwise support values as:
            // "12d"
            // "12d 5h"
            // "5m 3s"
            // "12.5d"
            var total = new TimeSpan();
            foreach (Match m in ParserExpression.Matches(value))
            {
                string number = m.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(number))
                {
                    continue;
                }

                double d = double.Parse(number.Replace(',', '.'), CultureInfo.InvariantCulture);
                string unit = m.Groups[2].Value;
                switch (unit.ToLower())
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
                }
            }

            return total;
        }
    }
}