using System;
using System.Collections;

namespace PropertyTools.Wpf
{
    public static class TypeHelper
    {
        /// <summary>
        /// Finds the biggest common type of items in the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static Type FindBiggestCommonType(IEnumerable list)
        {
            Type type = null;
            foreach (object item in list)
            {
                Type itemType = item.GetType();
                if (type == null)
                {
                    type = itemType;
                    continue;
                }
                while (type.BaseType != null && !type.IsAssignableFrom(itemType))
                {
                    type = type.BaseType;
                }
            }
            return type;
        }

    }
}