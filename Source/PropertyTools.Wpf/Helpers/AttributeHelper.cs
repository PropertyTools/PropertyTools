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
        /// The attributes.
        /// </param>
        /// <param name="attributeType">
        /// The type to check for.
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
        /// Return the first attribute of a given type for the specified property descriptor.
        /// </summary>
        /// <typeparam name="T">
        /// An attribute type.
        /// </typeparam>
        /// <param name="descriptor">
        /// The property descriptor.
        /// </param>
        /// <returns>
        /// The first attribute of the specified type.
        /// </returns>
        public static T GetFirstAttribute<T>(PropertyDescriptor descriptor) where T : Attribute
        {
            return descriptor.Attributes.OfType<T>().FirstOrDefault();
        }

        #endregion
    }
}