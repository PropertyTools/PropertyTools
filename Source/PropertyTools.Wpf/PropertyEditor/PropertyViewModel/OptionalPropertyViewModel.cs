using System;
using System.ComponentModel;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Properties that are nullable or marked [Optional(...)] are enabled/disabled by a checkbox
    /// </summary>
    public class OptionalPropertyViewModel : PropertyViewModel
    {
        private bool enabledButHasNoValue;
        private object previousValue;
        private PropertyDescriptor optionalDescriptor;

        public OptionalPropertyViewModel(object instance, PropertyDescriptor descriptor, string optionalPropertyName,
                                         PropertyEditor owner)
            : this(instance,descriptor, GetDescriptor(instance,optionalPropertyName),owner)
        {
        }

        public string OptionalPropertyName
        {
            get
            {
                if (optionalDescriptor != null)
                    return optionalDescriptor.Name;
                return null;
            }
        }

        private static PropertyDescriptor GetDescriptor(object instance, string propertyName)
        {
            if (instance == null || propertyName == null)
                return null;
            return TypeDescriptor.GetProperties(instance).Find(propertyName, false);
        }

        public OptionalPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyDescriptor optionalDescriptor,
                                         PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
            this.optionalDescriptor = optionalDescriptor;

            // http://msdn.microsoft.com/en-us/library/ms366789.aspx
            IsPropertyNullable = descriptor.PropertyType.IsGenericType &&
                                 descriptor.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            IsPropertyNullable = true;
        }

        public bool IsPropertyNullable { get; private set; }

        public bool IsOptionalChecked
        {
            get
            {
                if (optionalDescriptor != null)
                    return (bool)optionalDescriptor.GetValue(FirstInstance);

                if (IsPropertyNullable)
                {
                    return enabledButHasNoValue || Value != null;
                }

                return true; // default value is true (enable editor)
            }
            set
            {
                if (optionalDescriptor != null)
                {
                    optionalDescriptor.SetValue(FirstInstance, value);
                    NotifyPropertyChanged("IsOptionalChecked");
                    return;
                }

                if (IsPropertyNullable)
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
            }
        }

        public override void SubscribeValueChanged()
        {
            base.SubscribeValueChanged();
            if (optionalDescriptor != null)
            {
                SubscribeValueChanged(optionalDescriptor, IsOptionalChanged);
            }
        }

        private void IsOptionalChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("IsOptionalChecked");
        }

        public override void UnsubscribeValueChanged()
        {
            base.UnsubscribeValueChanged();
            if (optionalDescriptor != null)
            {
                UnsubscribeValueChanged(optionalDescriptor, IsOptionalChanged);
            }
        }
    }
}