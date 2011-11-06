// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The reflection extensions.
    /// </summary>
    public static class ReflectionExtensions
    {
        #region Public Methods

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
        /// The get custom attributes.
        /// </summary>
        /// <param name="fieldInfo">
        /// The field info.
        /// </param>
        /// <param name="inherit">
        /// The inherit.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this FieldInfo fieldInfo, bool inherit)
        {
            return fieldInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        #endregion
    }
}