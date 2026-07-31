// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormattedTimeSpanParser.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   TimeSpan parser that use a format string to evaluate the input string.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// TimeSpan parser that use a format string to evaluate the input string.
    /// </summary>
    /// <remarks>Supports the following format codes: D, DD, H, HH, M, MM, S, SS, d, dd, h, hh, m, mm, s, ss, f, ff, fff</remarks>
    public class FormattedTimeSpanParser
    {
        /// <summary>
        /// The special characters.
        /// </summary>
        private const string specialCharacters = @"\?$^.+*|{}[]()";

        /// <summary>
        /// The conversion expression.
        /// </summary>
        private readonly Regex conversionExpression =
            new Regex("D{1,2}|H{1,2}|M{1,2}|S{1,2}|d{1,2}|h{1,2}|m{1,2}|s{1,2}|f{1,7}", RegexOptions.Compiled);

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedTimeSpanParser" /> class.
        /// </summary>
        /// <param name="formatString">The format string.</param>
        public FormattedTimeSpanParser(string formatString)
        {
            // escape special characters
            foreach (char specialChar in specialCharacters)
            {
                formatString = formatString.Replace(
                    specialChar.ToString(CultureInfo.InvariantCulture), @"\" + specialChar);
            }

            // replace all format codes by regular expression groups
            // the name of the groups is the same as the format code ("hh", "mm" etc.)
            formatString = this.conversionExpression.Replace(
                formatString,
                m =>
                    {
                        string length = "{1," + m.Value.Length + "}";

                        if (m.Value.Length == 1)
                        {
                            length = "{1,2}";
                        }

                        return @"(?<" + m.Value + @">\d" + length + ")";
                    });

            // escape tabs and whitespaces
            formatString = formatString.Replace("\t", @"\t");
            formatString = formatString.Replace(" ", @"\s");

            this.Expression = new Regex(formatString, RegexOptions.Compiled);
        }

        /// <summary>
        /// Gets or sets the regular expression.
        /// </summary>
        private Regex Expression { get; set; }

        /// <summary>
        /// Parses the specified time span string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A time span.
        /// </returns>
        public TimeSpan Parse(string value)
        {
            TimeSpan result;
            if (this.TryParse(value, out result))
            {
                return result;
            }

            throw new FormatException(string.Format("Invalid TimeSpan value '{0}'.", value));
        }

        /// <summary>
        /// Tries to parse the specified time span string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The parsed time span.</param>
        /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
        public bool TryParse(string value, out TimeSpan result)
        {
            result = default(TimeSpan);
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            try
            {
                var r = this.Expression.Match(value);
                if (!r.Success)
                {
                    return false;
                }

                int days = 0;
                int hours = 0;
                int minutes = 0;
                int seconds = 0;
                int milliseconds = 0;

                for (int groupNumber = 1; groupNumber < r.Groups.Count; groupNumber++)
                {
                    var group = r.Groups[groupNumber];
                    var name = this.Expression.GroupNameFromNumber(groupNumber);

                    if (group.Success)
                    {
                        // Console.WriteLine("Match on " + name + ": '" + group.Value + "'");
                        int v = int.Parse(group.Value);
                        switch (name)
                        {
                            case "DD":
                            case "D":
                            case "dd":
                            case "d":
                                days = v;
                                break;
                            case "HH":
                            case "H":
                            case "hh":
                            case "h":
                                hours = v;
                                break;
                            case "MM":
                            case "M":
                            case "mm":
                            case "m":
                                minutes = v;
                                break;
                            case "SS":
                            case "S":
                            case "ss":
                            case "s":
                                seconds = v;
                                break;
                            case "fff":
                            case "ff":
                            case "f":
                                milliseconds = v;
                                break;
                        }
                    }
                }

                result = new TimeSpan(days, hours, minutes, seconds, milliseconds);
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
    }
}