using System;
using System.ComponentModel;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Properties that are nullable or marked [Optional(...)] are enabled/disabled by a checkbox
    /// </summary>
    public class OptionalPropertyViewModel : PropertyViewModel
    {
        private bool enabledButHasNoValue;
        private string optionalPropertyName;
        private object previousValue;

        public OptionalPropertyViewModel(object instance, PropertyDescriptor descriptor, string optionalPropertyName,
                                         PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
            OptionalPropertyName = optionalPropertyName;

            // http://msdn.microsoft.com/en-us/library/ms366789.aspx
            IsPropertyNullable = descriptor.PropertyType.IsGenericType &&
                                 descriptor.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>);
        }

        public string OptionalPropertyName
        {
            get { return optionalPropertyName; }
            set
            {
                if (optionalPropertyName != value)
                {
                    optionalPropertyName = value;
                    NotifyPropertyChanged("OptionalPropertyName");
                }
            }
        }

        public bool IsPropertyNullable { get; private set; }

        public bool IsOptionalChecked
        {
            get
            {
                if (IsPropertyNullable || OptionalPropertyName == null)
                {
                    return enabledButHasNoValue || Value != null;
                }

                if (!string.IsNullOrEmpty(OptionalPropertyName))
                {
                    PropertyDescriptor desc = TypeDescriptor.GetProperties(Instance).Find(OptionalPropertyName, false);
                    return (bool) desc.GetValue(Instance);
                }
                return true; // default must be true (enable editor)
            }
            set
            {
                if (IsPropertyNullable || OptionalPropertyName == null)
                {
                    if (value)
                    {
                        Value = previousValue;
                        enabledButHasNoValue = true;
                    }
                    else
                    {
                        previousValue = Value;
                        Value = null;
                        enabledButHasNoValue = false;
                    }
                    NotifyPropertyChanged("IsOptionalChecked");
                    return;
                }

                if (!string.IsNullOrEmpty(OptionalPropertyName))
                {
                    PropertyDescriptor desc = TypeDescriptor.GetProperties(Instance).Find(OptionalPropertyName, false);
                    desc.SetValue(Instance, value);
                    NotifyPropertyChanged("IsOptionalChecked");
                }
            }
        }
    }
}