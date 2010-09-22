using System;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// The Property ViewModel
    /// </summary>
    public class PropertyViewModel : ViewModelBase, IDataErrorInfo
    {
        private bool isEnabled = true;

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
        }

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

        public bool IsEnumerable { get; set; }

        public PropertyTemplateSelector PropertyTemplateSelector
        {
            get { return Owner.PropertyTemplateSelector; }
        }

        public object Instance { get; private set; }
        public PropertyDescriptor Descriptor { get; private set; }
        public PropertyDescriptor IsEnabledDescriptor { get; set; }

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
                if (IsEnabledDescriptor != null)
                    return (bool)IsEnabledDescriptor.GetValue(FirstInstance);

                if (Owner.PropertyStateProvider != null)
                    return isEnabled && Owner.PropertyStateProvider.IsEnabled(FirstInstance, Descriptor);
                
                return isEnabled;
            }
            set
            {
                if (IsEnabledDescriptor != null)
                    IsEnabledDescriptor.SetValue(FirstInstance, value);

                isEnabled = value;
                NotifyPropertyChanged("IsEnabled");
            }
        }

        public string PropertyError
        {
            get
            {
                return Owner.PropertyStateProvider != null ? Owner.PropertyStateProvider.GetError(FirstInstance, this.Descriptor) : null;
            }
        }

        public string PropertyWarning
        {
            get { return Owner.PropertyStateProvider != null ? Owner.PropertyStateProvider.GetWarning(FirstInstance, this.Descriptor) : null; }
        }

        /// <summary>
        /// Gets the visibility of the property.
        /// </summary>
        /// <value>The visibility.</value>
        public Visibility Visibility { get { return Visibility.Visible; } }

        public object FirstInstance
        {
            get
            {
                if (IsEnumerable)
                {
                    var list = Instance as IEnumerable;
                    if (list == null)
                    {
                        throw new InvalidOperationException("Instance should be a list.");
                    }
                    return list.Cast<object>().FirstOrDefault();
                }
                return Instance;
            }
        }

        public IEnumerable Instances
        {
            get
            {
                if (IsEnumerable)
                {
                    var list = Instance as IEnumerable;
                    if (list == null)
                    {
                        throw new InvalidOperationException("Instance should be a list.");
                    }
                    foreach (var o in list)
                        yield return o;
                }
                else
                {
                    yield return Instance;
                }
            }
        }
        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get
            {
                object value;
                if (IsEnumerable)
                {
                    var list = Instance as IEnumerable;
                    if (list == null)
                    {
                        throw new InvalidOperationException("Instance should be a list.");
                    }
                    value = GetValueFromEnumerable(list);
                }
                else
                {
                    value = GetValue(Instance);
                }

                if (!String.IsNullOrEmpty(FormatString))
                {
                    if (value != null)
                        value = String.Format("{0:" + FormatString + "}", value);
                }

                return value;
            }
            set
            {
                if (IsEnumerable)
                {
                    var list = Instance as IEnumerable;
                    if (list == null)
                    {
                        throw new InvalidOperationException("Instance should be a list.");
                    }
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
        /// Subscribes to the ValueChanged event.
        /// </summary>
        public void SubscribeValueChanged()
        {
            SubscribeValueChanged(Descriptor, InstancePropertyChanged);

            if (IsEnabledDescriptor != null)
            {
                SubscribeValueChanged(IsEnabledDescriptor, IsEnabledChanged);
            }
        }

        private void IsEnabledChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("IsEnabled");
        }

        /// <summary>
        /// Unsubscribes the value changed event.
        /// </summary>
        public void UnsubscribeValueChanged()
        {
        }

        /// <summary>
        /// Subscribes to the ValueChanged event.
        /// </summary>
        public void SubscribeValueChanged(PropertyDescriptor descriptor, EventHandler handler)
        {
            if (IsEnumerable)
            {
                var list = Instance as IEnumerable;
                if (list == null)
                {
                    throw new InvalidOperationException("Instance should be a list.");
                }
                foreach (var item in list)
                    descriptor.AddValueChanged(item, handler);
            }
            else
            {
                descriptor.AddValueChanged(Instance, handler);
            }
        }

        /// <summary>
        /// Unsubscribes the value changed event.
        /// </summary>
        public void UnsubscribeValueChanged(PropertyDescriptor descriptor, EventHandler handler)
        {
            if (IsEnumerable)
            {
                var list = Instance as IEnumerable;
                if (list == null)
                {
                    throw new InvalidOperationException("Instance should be a list.");
                }
                foreach (var item in list)
                    descriptor.RemoveValueChanged(item, handler);
            }
            else
            {
                descriptor.RemoveValueChanged(Instance, handler);
            }
        }
        private void InstancePropertyChanged(object sender, EventArgs e)
        {
            // Sending notification when the instance has been changed
            // If the instance is a list
            NotifyPropertyChanged("Value");
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
                {
                    value = v;
                }
                if (value != null && v == null)
                {
                    return null;
                }
                if (v != null && !v.Equals(value))
                {
                    return null;
                }
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
                TypeConverter converter = Descriptor.Converter;
                if (value != null)
                {
                    if (converter.CanConvertFrom(value.GetType()))
                    {
                        try
                        {
                            //if (value is string)
                            //{
                            //    value = converter.ConvertFromInvariantString(value as string);
                            //}
                            //else
                            {
                                value = converter.ConvertFrom(value);
                            }
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
                            var d = (double)value;
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
            if (!Convert(ref value))
                return;

            if (!IsModified(instance, value))
                return;

            Descriptor.SetValue(instance, value);
        }

        protected virtual object GetValue(object instance)
        {
            return Descriptor.GetValue(instance);
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

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                var dei = Instance as IDataErrorInfo;
                if (dei != null)
                    return dei[Descriptor.Name];
                return null;
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                var dei = Instance as IDataErrorInfo;
                if (dei != null)
                    return dei.Error;
                return null;
            }
        }

        public void UpdateErrorInfo()
        {
            NotifyPropertyChanged("PropertyError");
            NotifyPropertyChanged("PropertyWarning");
        }
    }
}