// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeHelper.cs" company="PropertyTools">
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

            return type;
        }

        /// <summary>
        /// Gets the underlying enum type of the specified type, if the specified type is a nullable type.
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

        /// <summary>
        /// Gets the type of the items in the specified enumeration.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>The type of the items.</returns>
        public static Type GetItemType(IEnumerable enumerable)
        {
            return enumerable != null ? enumerable.AsQueryable().ElementType : null;
        }

        /// <summary>
        /// The get list item type.
        /// </summary>
        /// <param name="listType">
        /// The list type.
        /// </param>
        /// <returns>
        /// </returns>
        [Obsolete]
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
        /// True if ok.
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
    }
}