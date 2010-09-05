using System;
using System.Collections;

namespace PropertyEditorLibrary
{
    public static class TypeHelper
    {
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