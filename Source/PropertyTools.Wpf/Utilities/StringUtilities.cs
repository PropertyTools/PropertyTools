// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringUtilities.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides utilities related to <see cref="string" />s.
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
    }
}