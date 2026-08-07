// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DurationParser.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Parses and formats durations (elapsed time) in "h:mm:ss" / "m:ss" notation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Parses and formats durations (elapsed time) in "h:mm:ss" / "m:ss" notation. Shared by
    /// <see cref="CellInputParser" /> (parsing what the user typed) and <see cref="CellFormatter" />
    /// (formatting for display/editing, and recognizing literal text that needs an apostrophe to
    /// round-trip instead of being reparsed as a duration).
    /// </summary>
    /// <remarks>
    /// This intentionally does not use <see cref="TimeSpan" />'s own parser/formatter: .NET's
    /// <c>TimeSpan.Parse("5:30")</c> reads a single colon as <c>h:mm</c>, but this project follows the
    /// user-facing convention of a single colon meaning <c>m:ss</c> instead (a stopwatch/lap-time
    /// reading), and its custom-format <c>h</c>/<c>hh</c> specifiers wrap at 24 hours, which is wrong
    /// for elapsed-time totals like "30:00:00" (30 hours).
    /// </remarks>
    internal static class DurationParser
    {
        /// <summary>
        /// Tries to parse "[-]h:mm:ss" or "[-]m:ss" (optionally with fractional seconds, e.g. "1:30.5").
        /// </summary>
        public static bool TryParse(string input, out TimeSpan value)
        {
            value = default;
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            var text = input.Trim();
            if (text.Length == 0)
            {
                return false;
            }

            var negative = text[0] == '-';
            if (negative || text[0] == '+')
            {
                text = text.Substring(1);
            }

            var parts = text.Split(':');
            if (parts.Length != 2 && parts.Length != 3)
            {
                return false;
            }

            foreach (var part in parts)
            {
                if (part.Length == 0)
                {
                    return false;
                }
            }

            if (!double.TryParse(parts[parts.Length - 1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var seconds)
                || seconds < 0)
            {
                return false;
            }

            var hours = 0;
            int minutes;
            if (parts.Length == 3)
            {
                if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out hours)
                    || !int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out minutes)
                    || hours < 0 || minutes < 0)
                {
                    return false;
                }
            }
            else if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out minutes) || minutes < 0)
            {
                return false;
            }

            value = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);
            if (negative)
            {
                value = value.Negate();
            }

            return true;
        }

        /// <summary>
        /// Formats a duration according to <paramref name="formatString" /> ("h:mm:ss", "hh:mm:ss",
        /// "m:ss" or "mm:ss"), falling back to unpadded "h:mm:ss" for <c>null</c>/empty/unrecognized
        /// formats. Hours/minutes show the true total (not wrapped at 24/60), so e.g. 30 hours displays
        /// as "30:00:00", not "6:00:00".
        /// </summary>
        public static string Format(TimeSpan duration, string formatString)
        {
            switch (formatString)
            {
                case "hh:mm:ss":
                    return FormatHms(duration, padHours: true);
                case "m:ss":
                    return FormatMs(duration, padMinutes: false);
                case "mm:ss":
                    return FormatMs(duration, padMinutes: true);
                default:
                    return FormatHms(duration, padHours: false);
            }
        }

        private static string FormatHms(TimeSpan duration, bool padHours)
        {
            var negative = duration < TimeSpan.Zero;
            if (negative)
            {
                duration = duration.Negate();
            }

            var totalHours = (long)duration.TotalHours;
            var hoursText = padHours
                ? totalHours.ToString("D2", CultureInfo.InvariantCulture)
                : totalHours.ToString(CultureInfo.InvariantCulture);
            var text = $"{hoursText}:{duration.Minutes:D2}:{duration.Seconds:D2}";
            return negative ? "-" + text : text;
        }

        private static string FormatMs(TimeSpan duration, bool padMinutes)
        {
            var negative = duration < TimeSpan.Zero;
            if (negative)
            {
                duration = duration.Negate();
            }

            var totalMinutes = (long)duration.TotalMinutes;
            var minutesText = padMinutes
                ? totalMinutes.ToString("D2", CultureInfo.InvariantCulture)
                : totalMinutes.ToString(CultureInfo.InvariantCulture);
            var text = $"{minutesText}:{duration.Seconds:D2}";
            return negative ? "-" + text : text;
        }
    }
}
