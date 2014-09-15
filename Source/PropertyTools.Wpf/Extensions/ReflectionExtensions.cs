// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="PropertyTools">
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
//   Provides reflection extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides reflection extensions.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Filters on the <see cref="System.ComponentModel.BrowsableAttribute" /> and <see cref="PropertyTools.DataAnnotations.BrowsableAttribute" />.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <param name="arr">The array.</param>
        /// <returns>
        /// The filtered values.
        /// </returns>
        public static List<object> FilterOnBrowsableAttribute<T>(this T arr) where T : IEnumerable
        {
            // Default empty list
            var res = new List<object>();

            // Loop each item in the enumerable
            foreach (var o in arr)
            {
                // Get the field information for the current field
                var field = o.GetType().GetField(
                    o.ToString(), BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public);
                if (field != null)
                {
                    // Get the Browsable attribute, if it is declared for this field
                    var browsable = field.GetCustomAttributes<System.ComponentModel.BrowsableAttribute>(true).FirstOrDefault();
                    if (browsable != null)
                    {
                        // It is declared, is it true or false?
                        if (browsable.Browsable)
                        {
                            // Is true - field is not hidden so add it to the result.
                            res.Add(o);
                        }

                        continue;
                    }

                    var browsable2 = field.GetCustomAttributes<DataAnnotations.BrowsableAttribute>(true).FirstOrDefault();
                    if (browsable2 != null)
                    {
                        // It is declared, is it true or false?
                        if (browsable2.Browsable)
                        {
                            // Is true - field is not hidden so add it to the result.
                            res.Add(o);
                        }

                        continue;
                    }

                    // Not declared so add it to the result
                    res.Add(o);
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
        /// Gets the attributes of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of attribute to get.</typeparam>
        /// <param name="fieldInfo">The field info.</param>
        /// <param name="inherit">The inherit flag.</param>
        /// <returns>
        /// The attributes enumeration.
        /// </returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this FieldInfo fieldInfo, bool inherit)
        {
            return fieldInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        /// <summary>
        /// Return the first attribute of a given type for the specified property descriptor.
        /// </summary>
        /// <typeparam name="T">An attribute type.</typeparam>
        /// <param name="descriptor">The property descriptor.</param>
        /// <returns>The first attribute of the specified type.</returns>
        public static T GetFirstAttributeOrDefault<T>(this System.ComponentModel.PropertyDescriptor descriptor) where T : Attribute
        {
            return descriptor.Attributes.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the first attribute of the specified type.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns>The first attribute of the specified type.</returns>
        public static Attribute GetFirstAttributeOrDefault(this System.ComponentModel.PropertyDescriptor descriptor, Type attributeType)
        {
            return descriptor.Attributes.Cast<Attribute>().FirstOrDefault(attribute => attribute.GetType().IsAssignableFrom(attributeType));
        }

        /// <summary>
        /// Determines whether the specified property is browsable.
        /// </summary>
        /// <param name="pd">The property descriptor.</param>
        /// <returns><c>true</c> if the specified property is browsable; otherwise, <c>false</c>.</returns>
        public static bool IsBrowsable(this System.ComponentModel.PropertyDescriptor pd)
        {
            var a = pd.GetFirstAttributeOrDefault<DataAnnotations.BrowsableAttribute>();
            if (a != null)
            {
                return a.Browsable;
            }

            return pd.IsBrowsable;
        }

        /// <summary>
        /// Determines whether the property is read-only.
        /// </summary>
        /// <param name="pd">The property descriptor.</param>
        /// <returns><c>true</c> if the property is read-only; otherwise, <c>false</c>.</returns>
        public static bool IsReadOnly(this System.ComponentModel.PropertyDescriptor pd)
        {
            var a = pd.GetFirstAttributeOrDefault<DataAnnotations.ReadOnlyAttribute>();
            if (a != null)
            {
                return a.IsReadOnly;
            }

            return pd.IsReadOnly;
        }

        /// <summary>
        /// Gets the category for the specified property.
        /// </summary>
        /// <param name="pd">The property descriptor.</param>
        /// <returns>The category.</returns>
        public static string GetCategory(this System.ComponentModel.PropertyDescriptor pd)
        {
            var a = pd.GetFirstAttributeOrDefault<DataAnnotations.CategoryAttribute>();
            if (a != null)
            {
                return a.Category;
            }

            return pd.Category;
        }

        /// <summary>
        /// Gets the description for the specified property.
        /// </summary>
        /// <param name="pd">The property descriptor.</param>
        /// <returns>The description.</returns>
        public static string GetDescription(this System.ComponentModel.PropertyDescriptor pd)
        {
            var a = pd.GetFirstAttributeOrDefault<DataAnnotations.DescriptionAttribute>();
            if (a != null)
            {
                return a.Description;
            }

            return pd.Description;
        }

        /// <summary>
        /// Gets the display name for the specified property.
        /// </summary>
        /// <param name="pd">The property descriptor.</param>
        /// <returns>The display name.</returns>
        public static string GetDisplayName(this System.ComponentModel.PropertyDescriptor pd)
        {
            var a = pd.GetFirstAttributeOrDefault<DataAnnotations.DisplayNameAttribute>();
            if (a != null)
            {
                return a.DisplayName;
            }

            return pd.DisplayName;
        }
    }
}