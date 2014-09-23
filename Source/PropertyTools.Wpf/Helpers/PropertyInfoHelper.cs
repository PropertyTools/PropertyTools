// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyInfoHelper.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   The property info helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The property info helper.
    /// </summary>
    public class PropertyInfoHelper
    {
        /// <summary>
        /// Gets the value of the specified property of the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>
        /// The get property.
        /// </returns>
        public static object GetPropertyValue(object instance, string propertyName)
        {
            var pi = instance.GetType().GetProperty(propertyName);
            return pi.GetValue(instance, null);
        }

        /// <summary>
        /// The set property.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The value.</param>
        public static void SetPropertyValue(object instance, string propertyName, object value)
        {
            var pi = instance.GetType().GetProperty(propertyName);
            pi.SetValue(instance, value, null);
        }
    }
}