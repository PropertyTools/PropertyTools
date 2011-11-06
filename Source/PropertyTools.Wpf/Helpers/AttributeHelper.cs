// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeHelper.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// The attribute helper.
    /// </summary>
    public static class AttributeHelper
    {
        #region Public Methods

        /// <summary>
        /// Check if an attribute collection contains an attribute of the given type
        /// </summary>
        /// <param name="attributes">
        /// </param>
        /// <param name="attributeType">
        /// </param>
        /// <returns>
        /// The contains attribute of type.
        /// </returns>
        public static bool ContainsAttributeOfType(AttributeCollection attributes, Type attributeType)
        {
            // return attributes.Cast<object>().Any(a => attributeType.IsAssignableFrom(a.GetType()));))))
            foreach (var a in attributes)
            {
                if (attributeType.IsAssignableFrom(a.GetType()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return the first attribute of a given type
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="descriptor">
        /// </param>
        /// <returns>
        /// </returns>
        public static T GetFirstAttribute<T>(PropertyDescriptor descriptor) where T : Attribute
        {
            return descriptor.Attributes.OfType<T>().FirstOrDefault();
        }

        #endregion
    }

    /// <summary>
    /// The property info helper.
    /// </summary>
    public class PropertyInfoHelper
    {
        #region Public Methods

        /// <summary>
        /// The get property.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The get property.
        /// </returns>
        public static object GetProperty(object instance, string propertyName)
        {
            var pi = instance.GetType().GetProperty(propertyName);
            return pi.GetValue(instance, null);
        }

        /// <summary>
        /// The set property.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            var pi = instance.GetType().GetProperty(propertyName);
            pi.SetValue(instance, value, null);
        }

        #endregion
    }
}