// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mass.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a mass.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SimpleDemo
{
    /// <summary>
    /// Represents a mass.
    /// </summary>
    [TypeConverter(typeof(MassConverter))]
    public class Mass
    {
        /// <summary>
        /// Gets or sets the mass.
        /// </summary>
        /// <value>The mass.</value>
        public double Value { get; set; }

        /// <summary>
        /// Parses the specified string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// 
        /// </returns>
        public static Mass Parse(string s)
        {
            s = s.Replace(',', '.').Trim();
            var r = new Regex(@"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?");
            var m = r.Match(s);
            if (!m.Success) return null;
            var value = double.Parse(m.Groups[0].Value, CultureInfo.InvariantCulture);
            // string unit = m.Groups[1].Value;
            return new Mass { Value = value };
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0:N0} kg", this.Value, CultureInfo.InvariantCulture);
        }
    }
}