// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeHelper.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;

    /// <summary>
    /// The type helper.
    /// </summary>
    public static class TypeHelper
    {
        #region Public Methods

        /// <summary>
        /// Finds the biggest common type of items in the list.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// The biggest common type.
        /// </returns>
        public static Type FindBiggestCommonType(IEnumerable list)
        {
            Type type = null;
            foreach (var item in list)
            {
                Type itemType = item.GetType();
                if (type == null)
                {
                    type = itemType;
                    continue;
                }

                while (type != null && type.BaseType != null && !type.IsAssignableFrom(itemType))
                {
                    type = type.BaseType;
                }
            }

            return type;
        }

        /// <summary>
        /// Gets the underlying enum type of the specified type.
        /// </summary>
        /// <param name="propertyType">
        /// The type.
        /// </param>
        /// <returns>
        /// The type of the underlying enum.
        /// </returns>
        public static Type GetEnumType(Type propertyType)
        {
            if (propertyType == null)
            {
                return null;
            }

            var ult = Nullable.GetUnderlyingType(propertyType);
            if (ult != null)
            {
                propertyType = ult;
            }

            if (typeof(Enum).IsAssignableFrom(propertyType))
            {
                return propertyType;
            }

            return null;
        }

        #endregion
    }
}