﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// The Property ViewModel
    /// </summary>
    public class PropertyViewModel : ViewModelBase, IDataErrorInfo
    {
        private bool isEnabled = true;
        private bool isVisible = true;

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

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enumerable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enumerable; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnumerable { get; set; }

        /// <summary>
        /// Gets the property template selector.
        /// </summary>
        /// <value>The property template selector.</value>
        public PropertyTemplateSelector PropertyTemplateSelector
        {
            get { return Owner.PropertyTemplateSelector; }
        }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public object Instance { get; private set; }

        /// <summary>
        /// Gets or sets the property descriptor.
        /// </summary>
        /// <value>The descriptor.</value>
        public PropertyDescriptor Descriptor { get; private set; }

        /// <summary>
        /// Gets or sets the descriptor for the property's IsEnabled.
        /// </summary>
        /// <value>The is enabled descriptor.</value>
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
                    return (bool) IsEnabledDescriptor.GetValue(FirstInstance);

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

        /// <summary>
        /// Gets or sets the descriptor for the property's IsVisible.
        /// </summary>
        /// <value>The is Visible descriptor.</value>
        public PropertyDescriptor IsVisibleDescriptor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property is Visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is Visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible
        {
            get
            {
                if (IsVisibleDescriptor != null)
                    return (bool)IsVisibleDescriptor.GetValue(FirstInstance);

                if (Owner.PropertyStateProvider != null)
                    return isVisible && Owner.PropertyStateProvider.IsVisible(FirstInstance, Descriptor);

                return isVisible;
            }
            set
            {
                if (IsVisibleDescriptor != null)
                    IsVisibleDescriptor.SetValue(FirstInstance, value);

                isVisible = value;
                NotifyPropertyChanged("IsVisible");
                NotifyPropertyChanged("Visibility");
            }
        }

        /// <summary>
        /// Gets the property error.
        /// </summary>
        /// <value>The property error.</value>
        public string PropertyError
        {
            get
            {
                return Owner.PropertyStateProvider != null
                           ? Owner.PropertyStateProvider.GetError(FirstInstance, Descriptor)
                           : null;
            }
        }

        /// <summary>
        /// Gets the property warning.
        /// </summary>
        /// <value>The property warning.</value>
        public string PropertyWarning
        {
            get
            {
                return Owner.PropertyStateProvider != null
                           ? Owner.PropertyStateProvider.GetWarning(FirstInstance, Descriptor)
                           : null;
            }
        }

        /// <summary>
        /// Gets the visibility of the property.
        /// </summary>
        /// <value>The visibility.</value>
        public Visibility Visibility
        {
            get { return IsVisible ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Gets the first instance if the Instance is an Enumerable, otherwise return the Instance.
        /// </summary>
        /// <value>The first instance.</value>
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

        /// <summary>
        /// Gets the instances or a single Instance as an Enumerable.
        /// </summary>
        /// <value>The instances.</value>
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
                    foreach (object o in list)
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

        #region IDataErrorInfo Members

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

        #endregion

        /// <summary>
        /// Subscribes to the ValueChanged event.
        /// </summary>
        public virtual void SubscribeValueChanged()
        {
            SubscribeValueChanged(Descriptor, InstancePropertyChanged);

            if (IsEnabledDescriptor != null)
            {
                SubscribeValueChanged(IsEnabledDescriptor, IsEnabledChanged);
            }
            if (IsVisibleDescriptor != null)
            {
                SubscribeValueChanged(IsVisibleDescriptor, IsVisibleChanged);
            }
        }

        private void IsEnabledChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("IsEnabled");
        }

        private void IsVisibleChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("IsVisible");
            NotifyPropertyChanged("Visibility");
        }

        /// <summary>
        /// Unsubscribes the value changed event.
        /// </summary>
        public virtual void UnsubscribeValueChanged()
        {
            UnsubscribeValueChanged(Descriptor, InstancePropertyChanged);

            if (IsEnabledDescriptor != null)
            {
                UnsubscribeValueChanged(IsEnabledDescriptor, IsEnabledChanged);
            }
            if (IsVisibleDescriptor != null)
            {
                UnsubscribeValueChanged(IsVisibleDescriptor, IsVisibleChanged);
            }
        }

        /// <summary>
        /// Subscribes to the ValueChanged event.
        /// </summary>
        protected void SubscribeValueChanged(PropertyDescriptor descriptor, EventHandler handler)
        {
            if (IsEnumerable)
            {
                var list = Instance as IEnumerable;
                if (list == null)
                {
                    throw new InvalidOperationException("Instance should be a list.");
                }
                foreach (object item in list)
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
        protected void UnsubscribeValueChanged(PropertyDescriptor descriptor, EventHandler handler)
        {
            if (IsEnumerable)
            {
                var list = Instance as IEnumerable;
                if (list == null)
                {
                    throw new InvalidOperationException("Instance should be a list.");
                }
                foreach (object item in list)
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
            NotifyPropertyChanged("Value");
        }

        /// <summary>
        /// The the current value from an IEnumerable instance
        /// </summary>
        /// <param name="componentList"></param>
        /// <returns>If all components in the enumerable are equal, it returns the value.
        /// If values are different, it returns null.</returns>
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
            var propertyType = Descriptor.PropertyType;
            if (propertyType == typeof (object) ||
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
                        if (propertyType == typeof (int) && value is double)
                        {
                            var d = (double) value;
                            value = (int) d;
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
            var currentValue = Descriptor.GetValue(component);
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

        /// <summary>
        /// Updates the error/warning properties.
        /// </summary>
        public void UpdateErrorInfo()
        {
            NotifyPropertyChanged("PropertyError");
            NotifyPropertyChanged("PropertyWarning");
        }

        #region Descriptor properties

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return Descriptor.Name; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is writeable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is writeable; otherwise, <c>false</c>.
        /// </value>
        public bool IsWriteable
        {
            get { return !IsReadOnly; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return Descriptor.IsReadOnly; }
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public Type PropertyType
        {
            get { return Descriptor.PropertyType; }
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName
        {
            get { return Descriptor.Name; }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return Descriptor.DisplayName; }
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category
        {
            get { return Descriptor.Category; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return Descriptor.Description; }
        }

        #endregion
    }
}