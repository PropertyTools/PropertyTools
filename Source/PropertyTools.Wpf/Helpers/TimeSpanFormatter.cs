namespace PropertyTools.Wpf
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Formats the TimeSpan to a string
    /// </summary>
    /// <remarks>
    /// http://www.java2s.com/Open-Source/CSharp/Sound-Mp3/stamp/Microsoft/Office/PowerPoint/STAMP/Core/TimeSpanFormatter.cs.htm
    /// </remarks>
    public class TimeSpanFormatter : IFormatProvider, ICustomFormatter
    {
        private readonly Regex formatParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanFormatter"/> class.
        /// </summary>
        public TimeSpanFormatter()
        {
            this.formatParser = new Regex("D{1,2}|H{1,2}|M{1,2}|S{1,2}|d{1,2}|h{1,2}|m{1,2}|s{1,2}|f{1,7}", RegexOptions.Compiled);
        }

        #region IFormatProvider Members

        /// <summary>
        /// Returns an object that provides formatting services for the specified type.
        /// </summary>
        /// <param name="formatType">An object that specifies the type of format object to return.</param>
        /// <returns>
        /// An instance of the object specified by <paramref name="formatType"/>, if the <see cref="T:System.IFormatProvider"/> implementation can supply that type of object; otherwise, null.
        /// </returns>
        public object GetFormat(Type formatType)
        {
            if (typeof(ICustomFormatter).Equals(formatType))
            {
                return this;
            }

            return null;
        }

        #endregion

        #region ICustomFormatter Members

        /// <summary>
        /// Converts the value of a specified timespan to an equivalent string representation using specified format and culture-specific formatting information.
        /// </summary>
        /// <param name="format">A format string containing formatting specifications.</param>
        /// <param name="arg">An object to format.</param>
        /// <param name="formatProvider">An object that supplies format information about the current instance.</param>
        /// <returns>
        /// The string representation of the value of <paramref name="arg"/>, formatted as specified by <paramref name="format"/> and <paramref name="formatProvider"/>.
        /// </returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is TimeSpan)
            {
                var timeSpan = (TimeSpan)arg;
                return this.formatParser.Replace(format, GetMatchEvaluator(timeSpan));
            }
            else
            {
                var formattable = arg as IFormattable;
                if (formattable != null)
                {
                    return formattable.ToString(format, formatProvider);
                }

                return arg != null ? arg.ToString() : string.Empty;
            }
        }

        #endregion

        private MatchEvaluator GetMatchEvaluator(TimeSpan timeSpan)
        {
            return m => EvaluateMatch(m, timeSpan);
        }

        private string EvaluateMatch(Match match, TimeSpan timeSpan)
        {
            switch (match.Value)
            {
                case "DD":
                    return timeSpan.TotalDays.ToString("00");
                case "D":
                    return timeSpan.TotalDays.ToString("0");
                case "dd":
                    return timeSpan.Days.ToString("00");
                case "d":
                    return timeSpan.Days.ToString("0");
                case "HH":
                    return ((int)timeSpan.TotalHours).ToString("00");
                case "H":
                    return ((int)timeSpan.TotalHours).ToString("0");
                case "hh":
                    return timeSpan.Hours.ToString("00");
                case "h":
                    return timeSpan.Hours.ToString("0");
                case "MM":
                    return ((int)timeSpan.TotalMinutes).ToString("00");
                case "M":
                    return ((int)timeSpan.TotalMinutes).ToString("0");
                case "mm":
                    return timeSpan.Minutes.ToString("00");
                case "m":
                    return timeSpan.Minutes.ToString("0");
                case "SS":
                    return ((int)timeSpan.TotalSeconds).ToString("00");
                case "S":
                    return ((int)timeSpan.TotalSeconds).ToString("0");
                case "ss":
                    return timeSpan.Seconds.ToString("00");
                case "s":
                    return timeSpan.Seconds.ToString("0");
                case "fffffff":
                    return (timeSpan.Milliseconds * 10000).ToString("0000000");
                case "ffffff":
                    return (timeSpan.Milliseconds * 1000).ToString("000000");
                case "fffff":
                    return (timeSpan.Milliseconds * 100).ToString("00000");
                case "ffff":
                    return (timeSpan.Milliseconds * 10).ToString("0000");
                case "fff":
                    return (timeSpan.Milliseconds).ToString("000");
                case "ff":
                    return (timeSpan.Milliseconds / 10).ToString("00");
                case "f":
                    return (timeSpan.Milliseconds / 100).ToString("0");
                default:
                    return match.Value;
            }
        }
    }
}
