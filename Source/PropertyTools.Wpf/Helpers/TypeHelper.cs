// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeHelper.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   The type helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The type helper.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Finds the biggest common type of items in the list.
        /// </summary>
        /// <param name="items">The list.</param>
        /// <returns>
        /// The biggest common type.
        /// </returns>
        public static Type FindBiggestCommonType(IEnumerable items)
        {
            if (items == null)
            {
                return null;
            }

            Type type = null;
            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

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

            if (type == null && items is IList)
            {
                type = items.AsQueryable().ElementType;
            }

            return type;
        }

        /// <summary>
        /// Gets the underlying enum type of the specified type, if the specified type is a nullable type.
        /// </summary>
        /// <param name="propertyType">The type.</param>
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

        /// <summary>
        /// Gets the type of the items in the specified enumeration.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        /// The type of the items.
        /// </returns>
        public static Type GetItemType(IEnumerable enumerable)
        {
            return enumerable != null ? enumerable.AsQueryable().ElementType : null;
        }

        /// <summary>
        /// Gets the item type from a list type.
        /// </summary>
        /// <param name="listType">The list type.</param>
        /// <returns>
        /// The <see cref="Type" /> of the elements.
        /// </returns>
        public static Type GetListElementType(Type listType)
        {
            // http://stackoverflow.com/questions/1043755/c-generic-list-t-how-to-get-the-type-of-t
            foreach (var interfaceType in listType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    var args = interfaceType.GetGenericArguments();
                    if (args.Length > 0)
                    {
                        return args[0];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the first type is assignable from the specified second type.
        /// </summary>
        /// <param name="firstType">Type of the first type.</param>
        /// <param name="secondType">The type of the second type.</param>
        /// <returns>
        /// True if it is assignable.
        /// </returns>
        public static bool Is(this Type firstType, Type secondType)
        {
            if (firstType.IsGenericType && secondType == firstType.GetGenericTypeDefinition())
            {
                return true;
            }

            // checking generic interfaces
            foreach (var @interface in firstType.GetInterfaces())
            {
                if (@interface.IsGenericType)
                {
                    if (secondType == @interface.GetGenericTypeDefinition())
                    {
                        return true;
                    }
                }

                if (secondType == @interface)
                {
                    return true;
                }
            }

            var ult = Nullable.GetUnderlyingType(firstType);
            if (ult != null)
            {
                if (secondType.IsAssignableFrom(ult))
                {
                    return true;
                }
            }

            if (secondType.IsAssignableFrom(firstType))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets inner generic type of an IList&gt;IList&lt;
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// The <see cref="Type" />.
        /// </returns>
        public static Type GetInnerMostGenericType(IList list)
        {
            var genericArguments = list.GetType().GetGenericArguments();
            var innerType = genericArguments.Length > 0 ? genericArguments[0] : null;

            if (innerType != null && innerType.IsGenericType)
            {
                var innerGenericArguments = innerType.GetGenericArguments();
                var innerMostType = genericArguments.Length > 0 ? innerGenericArguments[0] : null;
                return innerMostType;
            }

            return innerType;
        }

        /// <summary>
        /// Gets the type of the inner list of a IList&gt;IList&lt;
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// The type of the inner list. Return <c>null</c> if only interface type can be retrieved.
        /// </returns>
        public static Type GetInnerTypeOfList(IList list)
        {
            var innerType = TypeHelper.GetInnerMostGenericType(list);
            if (innerType.IsInterface)
            {
                if (list.Count > 0)
                {
                    var row = list[0] as IList;
                    if (row != null && row.Count > 0)
                    {
                        // Get the type from the [0][0]. The assumption is all the elements in the ItemsSource are of the same type.
                        innerType = row[0].GetType();
                    }
                }
                else
                {
                    innerType = null;
                }
            }

            return innerType;
        }

        /// <summary>
        /// Determines whether the type is IList{IList}.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if the type is IList{IList}; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIListIList(Type type)
        {
            if (!typeof(IList).IsAssignableFrom(type))
            {
                return false;
            }

            var listElementType = GetListElementType(type);
            if (listElementType == null)
            {
                return false;
            }

            if (listElementType.IsGenericType && listElementType.GetGenericTypeDefinition() == typeof(IList<>))
            {
                return true;
            }

            return GetListElementType(listElementType) != null;
        }
    }
}