using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Globalization;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// The Property ViewModel
    /// </summary>
    public class PropertyViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        public string FormatString { get; set; }

        /// <summary>
        /// Gets or sets the height of the editor for the property.
        /// </summary>
        /// <value>The height.</value>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property should be edited as multiline.
        /// </summary>
        /// <value><c>true</c> if multiline; otherwise, <c>false</c>.</value>
        public bool AcceptsReturn { get; set; }

        /// <summary>
        /// Gets or sets the text wrapping for multiline strings.
        /// </summary>
        /// <value>The text wrapping mode.</value>
        public TextWrapping TextWrapping { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">The instance being edited</param>
        /// <param name="descriptor">The property descriptor</param>
        /// <param name="owner">The parent PropertyEditor</param>
        public PropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(owner)
        {
            Instance = instance;
            Descriptor = descriptor;

            Header = descriptor.DisplayName;
            ToolTip = descriptor.Description;

            Height = double.NaN;

            SubscribeValueChanged();
        }

        /// <summary>
        /// Subscribes to the ValueChanged event.
        /// </summary>
        public void SubscribeValueChanged()
        {
            var list = Instance as IEnumerable;
            if (list != null)
            {
                foreach (var item in list)
                    Descriptor.AddValueChanged(item, InstancePropertyChanged);
            }
            else
            {
                Descriptor.AddValueChanged(Instance, InstancePropertyChanged);
            }
        }

        /// <summary>
        /// Unsubscribes the value changed event.
        /// </summary>
        public void UnsubscribeValueChanged()
        {
            var list = Instance as IEnumerable;
            if (list != null)
            {
                foreach (var item in list)
                    Descriptor.RemoveValueChanged(item, InstancePropertyChanged);
            }
            else
            {
                Descriptor.RemoveValueChanged(Instance, InstancePropertyChanged);
            }
        }

        private void InstancePropertyChanged(object sender, EventArgs e)
        {
            // Sending notification when the instance has been changed
            // If the instance is a list
            NotifyPropertyChanged("Value");
        }

        public PropertyTemplateSelector PropertyTemplateSelector
        {
            get { return Owner.PropertyTemplateSelector; }
        }

        public object Instance { get; private set; }
        public PropertyDescriptor Descriptor { get; private set; }

        private bool isEnabled = true;
        /// <summary>
        /// Gets or sets a value indicating whether this property is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
                // return PropertyDescriptorAdapter.GetIsEnabled(Instance);
            }
            set
            {
                isEnabled = value;
                //PropertyDescriptorAdapter.SetIsEnabled(Instance,value);
                NotifyPropertyChanged("IsEnabled");
            }
        }

        /// <summary>
        /// Gets or sets the IsVisible flag of the property.
        /// </summary>
        /// <value>The visiblity flag.</value>
        /* public Visibility IsVisible
         {
             get
             {
                 return PropertyDescriptorAdapter.GetIsVisible(Instance) ? Visibility.Visible : Visibility.Collapsed;
             }
             set
             {
                 PropertyDescriptorAdapter.SetIsVisible(Instance,value == Visibility.Visible);
                 NotifyPropertyChanged("IsVisible");
             }
         }*/

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get
            {
                var list = Instance as IEnumerable;
                if (list != null)
                    return GetValueFromEnumerable(list);
                var value = GetValue(Instance);

                if (!String.IsNullOrEmpty(FormatString))
                {
                    //    value = String.Format(CultureInfo.InvariantCulture, "{0:" + FormatString + "}", value);
                    value = String.Format("{0:" + FormatString + "}", value);
                }

                return value;
            }
            set
            {
                var list = Instance as IEnumerable;
                if (list != null)
                {
                    foreach (object item in list)
                    {
                        SetValue(item, value);
                    }
                }
                else
                {
                    SetValue(Instance, value);
                }
                //  NotifyPropertyChanged("Value");
            }
        }

        /// <summary>
        /// The the current value from an IEnumerable instance
        /// </summary>
        /// <param name="componentList"></param>
        /// <returns></returns>
        protected object GetValueFromEnumerable(IEnumerable componentList)
        {
            object value = null;
            foreach (object component in componentList)
            {
                object v = GetValue(component);
                if (value == null)
                    value = v;
                if (value != null && v == null)
                    return null;
                if (v != null && !v.Equals(value))
                    return null;
            }
            return value;
        }

        private bool Convert(ref object value)
        {
            // Check if it neccessary to convert the value
            Type propertyType = Descriptor.PropertyType;
            if (propertyType == typeof(object) ||
                value == null && propertyType.IsClass ||
                value != null && propertyType.IsAssignableFrom(value.GetType()))
            {
                // no conversion neccessary
            }
            else
            {
                // try to convert the value
                var converter = Descriptor.Converter;
                if (value != null)
                {
                    if (converter.CanConvertFrom(value.GetType()))
                    {
                        try
                        {
                            if (value is string)
                                value = converter.ConvertFromInvariantString(value as string);
                            else
                                value = converter.ConvertFrom(value);
                        }
                        // Catch FormatExceptions
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (propertyType == typeof(int) && value is double)
                        {
                            double d = (double)value;
                            value = (int)d;
                            return true;
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsModified(object component, object value)
        {
            // Return if the value has not been modified
            object currentValue = Descriptor.GetValue(component);
            if (currentValue == null && value == null)
            {
                return false;
            }
            if (value != null && value.Equals(currentValue))
            {
                return false;
            }
            return true;
        }

        protected virtual void SetValue(object instance, object value)
        {
            if (!IsModified(instance, value))
                return;

            if (Convert(ref value))
                Descriptor.SetValue(instance, value);
            // PropertyDescriptorAdapter.SetValue(instance, value);
        }

        protected virtual object GetValue(object instance)
        {
            return Descriptor.GetValue(instance);
            // return PropertyDescriptorAdapter.GetValue(instance);
        }

        #region Descriptor properties

        public string Name
        {
            get { return Descriptor.Name; }
        }


        public bool IsWriteable
        {
            get { return !IsReadOnly; }
        }

        public bool IsReadOnly
        {
            get { return Descriptor.IsReadOnly; }
        }

        public Type PropertyType
        {
            get { return Descriptor.PropertyType; }
        }

        public string PropertyName
        {
            get { return Descriptor.Name; }
        }

        public string DisplayName
        {
            get { return Descriptor.DisplayName; }
        }

        public string Category
        {
            get { return Descriptor.Category; }
        }

        public string Description
        {
            get { return Descriptor.Description; }
        }

        #endregion
    }
}