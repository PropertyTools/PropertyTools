// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
//   Provides reflection extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides reflection extensions.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Filters on the browsable attribute.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="arr">
        /// The arr.
        /// </param>
        /// <returns>
        /// </returns>
        public static List<object> FilterOnBrowsableAttribute<T>(this T arr) where T : IEnumerable
        {
            // Default empty list
            var res = new List<object>();

            // Loop each item in the enumerable
            foreach (var o in arr)
            {
                // Get the field information for the current field
                FieldInfo field = o.GetType().GetField(
                    o.ToString(), BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public);
                if (field != null)
                {
                    // Get the Browsable attribute, if it is declared for this field
                    var browsable = field.GetCustomAttributes<BrowsableAttribute>(true).FirstOrDefault();
                    if (browsable != null)
                    {
                        // It is declared, is it true or false?
                        if (browsable.Browsable)
                        {
                            // Is true - field is not hidden so add it to the result.
                            res.Add(o);
                        }
                    }
                    else
                    {
                        // Not declared so add it to the result
                        res.Add(o);
                    }
                }
                else
                {
                    // Can't evaluate, include it.
                    res.Add(o);
                }
            }

            return res;
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="fieldInfo">
        /// The field info.
        /// </param>
        /// <param name="inherit">
        /// The inherit flag.
        /// </param>
        /// <typeparam name="T">
        /// The type of attribute to get.
        /// </typeparam>
        /// <returns>
        /// The attributes enumeration.
        /// </returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this FieldInfo fieldInfo, bool inherit)
        {
            return fieldInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

    }
}