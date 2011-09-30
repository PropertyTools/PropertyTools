using System.Collections;
using System.ComponentModel;
using System;

namespace PropertyTools.Wpf
{
    class EnumPropertyViewModel : PropertyViewModel
    {
        public EnumPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        { }

        public object EnumValues
        {
            get
            {
                object value;
                if (IsEnumerable)
                {
                    var list = Instance as IEnumerable;
                    if (list == null)
                    {
                        throw new InvalidOperationException("Instance should be an enumerable.");
                    }
                    return GetValue(FirstInstance);
                }
                else
                {
                    value = GetValue(Instance);
                }

                if (!String.IsNullOrEmpty(FormatString))
                    return FormatValue(value);
                return value;
            }
        }
    }
}
