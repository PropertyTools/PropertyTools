// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringUtilities.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides utilities related to strings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Provides utilities related to <see cref="string" />s.
    /// </summary>
    public class StringUtilities
    {
        /// <summary>
        /// Converts a string from camel case to a string where space is inserted before each capital letter.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns>
        /// A string.
        /// </returns>
        public static string FromCamelCase(string variableName)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < variableName.Length; i++)
            {
                if (i > 0 && char.IsUpper(variableName[i]) && !char.IsUpper(variableName[i - 1]))
                {
                    sb.Append(" ");
                    if (i == variableName.Length - 1 || char.IsUpper(variableName[i + 1]))
                    {
                        sb.Append(variableName[i]);
                    }
                    else
                    {
                        sb.Append(variableName[i].ToString(CultureInfo.InvariantCulture).ToLower());
                    }

                    continue;
                }

                sb.Append(variableName[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts the specified string to horizontal alignment.
        /// </summary>
        /// <param name="a">The string to convert.</param>
        /// <returns>A <see cref="System.Windows.HorizontalAlignment" /> value.</returns>
        public static System.Windows.HorizontalAlignment ToHorizontalAlignment(string a)
        {
            switch ((a ?? string.Empty).ToUpper())
            {
                case "L":
                    return System.Windows.HorizontalAlignment.Left;
                case "R":
                    return System.Windows.HorizontalAlignment.Right;
                default:
                    return System.Windows.HorizontalAlignment.Center;
            }
        }
    }
}