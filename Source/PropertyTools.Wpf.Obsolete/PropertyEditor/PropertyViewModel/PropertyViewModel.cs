// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyViewModel.cs" company="PropertyTools">
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
//   The Property ViewModel
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// The Property ViewModel
    /// </summary>
    public class PropertyViewModel : ViewModelBase, IDataErrorInfo, IEditableObject
    {
        /// <summary>
        /// The time span formatter.
        /// </summary>
        private static readonly TimeSpanFormatter timeSpanFormatter = new TimeSpanFormatter();

        /// <summary>
        /// The is enabled.
        /// </summary>
        private bool isEnabled = true;

        /// <summary>
        /// The is visible.
        /// </summary>
        private bool isVisible = true;

        /// <summary>
        /// The setting values.
        /// </summary>
        private bool settingValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance being edited
        /// </param>
        /// <param name="descriptor">
        /// The property descriptor
        /// </param>
        /// <param name="owner">
        /// The parent PropertyEditor
        /// </param>
        public PropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(owner)
        {
            this.Instance = instance;
            this.Descriptor = descriptor;

            this.Header = descriptor.DisplayName;
            this.ToolTip = descriptor.Description;

            this.Height = double.NaN;
            this.SetByThis = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the property should be edited as multiline.
        /// </summary>
        /// <value><c>true</c> if multiline; otherwise, <c>false</c>.</value>
        public bool AcceptsReturn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this text property should use PropertyChanged as UpdateTrigger.
        /// </summary>
        public bool AutoUpdateText { get; set; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category
        {
            get
            {
                return this.Descriptor.Category;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return this.Descriptor.Description;
            }
        }

        /// <summary>
        /// Gets or sets the property descriptor.
        /// </summary>
        /// <value>The descriptor.</value>
        public PropertyDescriptor Descriptor { get; private set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get
            {
                return this.Descriptor.DisplayName;
            }
        }

        /// <summary>
        /// Gets the first instance if the Instance is an Enumerable, otherwise return the Instance.
        /// </summary>
        /// <value>The first instance.</value>
        public object FirstInstance
        {
            get
            {
                if (this.IsEnumerable)
                {
                    var list = this.Instance as IEnumerable;
                    if (list == null)
                    {
                        throw new InvalidOperationException("Instance should be a list.");
                    }

                    return list.Cast<object>().FirstOrDefault();
                }

                return this.Instance;
            }
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
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public object Instance { get; private set; }

        /// <summary>
        /// Gets the instances or a single Instance as an Enumerable.
        /// </summary>
        /// <value>The instances.</value>
        public IEnumerable Instances
        {
            get
            {
                if (this.IsEnumerable)
                {
                    var list = this.Instance as IEnumerable;
                    if (list == null)
                    {
                        throw new InvalidOperationException("Instance should be a list.");
                    }

                    foreach (var o in list)
                    {
                        yield return o;
                    }
                }
                else
                {
                    yield return this.Instance;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this property is enabled.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get
            {
                if (this.IsEnabledDescriptor != null)
                {
                    return (bool)this.IsEnabledDescriptor.GetValue(this.FirstInstance);
                }

                if (this.Owner.PropertyStateProvider != null)
                {
                    return this.isEnabled
                           && this.Owner.PropertyStateProvider.IsEnabled(this.FirstInstance, this.Descriptor);
                }

                return this.isEnabled;
            }

            set
            {
                if (this.IsEnabledDescriptor != null)
                {
                    this.IsEnabledDescriptor.SetValue(this.FirstInstance, value);
                }

                this.isEnabled = value;
                this.NotifyPropertyChanged("IsEnabled");
            }
        }

        /// <summary>
        /// Gets or sets the descriptor for the property's IsEnabled.
        /// </summary>
        /// <value>The is enabled descriptor.</value>
        public PropertyDescriptor IsEnabledDescriptor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enumerable.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is enumerable; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnumerable { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return this.Descriptor.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this property is Visible.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is Visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible
        {
            get
            {
                if (this.IsVisibleDescriptor != null)
                {
                    return (bool)this.IsVisibleDescriptor.GetValue(this.FirstInstance);
                }

                if (this.Owner.PropertyStateProvider != null)
                {
                    return this.isVisible
                           && this.Owner.PropertyStateProvider.IsVisible(this.FirstInstance, this.Descriptor);
                }

                return this.isVisible;
            }

            set
            {
                if (this.IsVisibleDescriptor != null)
                {
                    this.IsVisibleDescriptor.SetValue(this.FirstInstance, value);
                }

                this.isVisible = value;
                this.NotifyPropertyChanged("IsVisible");
                this.NotifyPropertyChanged("Visibility");
            }
        }

        /// <summary>
        /// Gets or sets the descriptor for the property's IsVisible.
        /// </summary>
        /// <value>The is Visible descriptor.</value>
        public PropertyDescriptor IsVisibleDescriptor { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is writeable.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is writeable; otherwise, <c>false</c>.
        /// </value>
        public bool IsWriteable
        {
            get
            {
                return !this.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets or sets the max length of text.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return this.Descriptor.Name;
            }
        }

        /// <summary>
        /// Gets or sets OldValue.
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// Gets the property error.
        /// </summary>
        /// <value>The property error.</value>
        public string PropertyError
        {
            get
            {
                return this.Owner.PropertyStateProvider != null
                           ? this.Owner.PropertyStateProvider.GetError(this.FirstInstance, this.Descriptor)
                           : null;
            }
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName
        {
            get
            {
                return this.Descriptor.Name;
            }
        }

        /// <summary>
        /// Gets the property template selector.
        /// </summary>
        /// <value>The property template selector.</value>
        public PropertyTemplateSelector PropertyTemplateSelector
        {
            get
            {
                return this.Owner.PropertyTemplateSelector;
            }
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public Type PropertyType
        {
            get
            {
                return this.Descriptor.PropertyType;
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
                return this.Owner.PropertyStateProvider != null
                           ? this.Owner.PropertyStateProvider.GetWarning(this.FirstInstance, this.Descriptor)
                           : null;
            }
        }

        /// <summary>
        /// Gets or sets whether Value was set by this ViewModel
        /// </summary>
        /// <value>True if this ViewModel set Value, false otherwise</value>
        public bool SetByThis { get; set; }

        /// <summary>
        /// Gets or sets the text wrapping for multiline strings.
        /// </summary>
        /// <value>The text wrapping mode.</value>
        public TextWrapping TextWrapping { get; set; }

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get
            {
                object value;
                if (this.IsEnumerable)
                {
                    var list = this.Instance as IEnumerable;
                    if (list == null)
                    {
                        throw new InvalidOperationException("Instance should be an enumerable.");
                    }

                    value = this.GetValueFromEnumerable(list);
                }
                else
                {
                    value = this.GetValue(this.Instance);
                }

                if (!string.IsNullOrEmpty(this.FormatString))
                {
                    return this.FormatValue(value);
                }

                return value;
            }

            set
            {
                if (this.IsEnumerable && !this.settingValues)
                {
                    var list = this.Instance as IEnumerable;
                    if (list == null)
                    {
                        throw new InvalidOperationException("Instance should be an enumerable.");
                    }

                    this.OldValue = null;
                    this.settingValues = true;
                    foreach (var item in list)
                    {
                        this.SetValue(item, value);
                    }

                    this.settingValues = false;
                }
                else
                {
                    this.SetByThis = true;
                    this.OldValue = this.Value;
                    this.SetValue(this.Instance, value);
                }
            }
        }

        /// <summary>
        /// Gets the visibility of the property.
        /// </summary>
        /// <value>The visibility.</value>
        public Visibility Visibility
        {
            get
            {
                return this.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets Error.
        /// </summary>
        string IDataErrorInfo.Error
        {
            get
            {
                var dei = this.Instance as IDataErrorInfo;
                if (dei != null)
                {
                    return dei.Error;
                }

                return null;
            }
        }

        /// <summary>
        /// The i data error info.this.
        /// </summary>
        /// <param name = "columnName">
        /// The column name.
        /// </param>
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                var dei = this.Instance as IDataErrorInfo;
                if (dei != null)
                {
                    return dei[this.Descriptor.Name];
                }

                return null;
            }
        }

        /// <summary>
        /// The begin edit.
        /// </summary>
        public void BeginEdit()
        {
            var eo = this.Instance as IEditableObject;
            if (eo != null)
            {
                eo.BeginEdit();
            }
        }

        /// <summary>
        /// The cancel edit.
        /// </summary>
        public void CancelEdit()
        {
            var eo = this.Instance as IEditableObject;
            if (eo != null)
            {
                eo.CancelEdit();
            }
        }

        /// <summary>
        /// The end edit.
        /// </summary>
        public void EndEdit()
        {
            var eo = this.Instance as IEditableObject;
            if (eo != null)
            {
                eo.EndEdit();
            }
        }

        /// <summary>
        /// Subscribes to the ValueChanged event.
        /// </summary>
        public virtual void SubscribeValueChanged()
        {
            this.SubscribeValueChanged(this.Descriptor, this.InstancePropertyChanged);

            if (this.IsEnabledDescriptor != null)
            {
                this.SubscribeValueChanged(this.IsEnabledDescriptor, this.IsEnabledChanged);
            }

            if (this.IsVisibleDescriptor != null)
            {
                this.SubscribeValueChanged(this.IsVisibleDescriptor, this.IsVisibleChanged);
            }
        }

        /// <summary>
        /// Unsubscribes the value changed event.
        /// </summary>
        public virtual void UnsubscribeValueChanged()
        {
            this.UnsubscribeValueChanged(this.Descriptor, this.InstancePropertyChanged);

            if (this.IsEnabledDescriptor != null)
            {
                this.UnsubscribeValueChanged(this.IsEnabledDescriptor, this.IsEnabledChanged);
            }

            if (this.IsVisibleDescriptor != null)
            {
                this.UnsubscribeValueChanged(this.IsVisibleDescriptor, this.IsVisibleChanged);
            }
        }

        /// <summary>
        /// Updates the error/warning properties.
        /// </summary>
        public void UpdateErrorInfo()
        {
            this.NotifyPropertyChanged("PropertyError");
            this.NotifyPropertyChanged("PropertyWarning");
        }

        /// <summary>
        /// The format value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The format value.
        /// </returns>
        protected string FormatValue(object value)
        {
            var f = this.FormatString;
            if (!f.Contains("{0"))
            {
                f = string.Format("{{0:{0}}}", f);
            }

            if (value is TimeSpan)
            {
                return string.Format(timeSpanFormatter, f, value);
            }

            return string.Format(f, value);
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// The get value.
        /// </returns>
        protected virtual object GetValue(object instance)
        {
            return this.Descriptor.GetValue(instance);
        }

        /// <summary>
        /// The the current value from an IEnumerable instance
        /// </summary>
        /// <param name="componentList">
        /// </param>
        /// <returns>
        /// If all components in the enumerable are equal, it returns the value.
        /// If values are different, it returns null.
        /// </returns>
        protected object GetValueFromEnumerable(IEnumerable componentList)
        {
            object value = null;
            foreach (var component in componentList)
            {
                object v = this.GetValue(component);
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

        /// <summary>
        /// The set value.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        protected virtual void SetValue(object instance, object value)
        {
            if (!this.Convert(ref value))
            {
                return;
            }

            if (!this.IsModified(instance, value))
            {
                return;
            }

            this.Descriptor.SetValue(instance, value);
        }

        /// <summary>
        /// Subscribes to the ValueChanged event.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        protected void SubscribeValueChanged(PropertyDescriptor descriptor, EventHandler handler)
        {
            if (this.IsEnumerable)
            {
                var list = this.Instance as IEnumerable;
                if (list == null)
                {
                    throw new InvalidOperationException("Instance should be a list.");
                }

                foreach (var item in list)
                {
                    descriptor.AddValueChanged(this.GetPropertyOwner(descriptor, item), handler);
                }
            }
            else
            {
                descriptor.AddValueChanged(this.GetPropertyOwner(descriptor, this.Instance), handler);
            }
        }

        /// <summary>
        /// Unsubscribes the value changed event.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        protected void UnsubscribeValueChanged(PropertyDescriptor descriptor, EventHandler handler)
        {
            if (this.IsEnumerable)
            {
                var list = this.Instance as IEnumerable;
                if (list == null)
                {
                    throw new InvalidOperationException("Instance should be a list.");
                }

                foreach (var item in list)
                {
                    descriptor.RemoveValueChanged(this.GetPropertyOwner(descriptor, item), handler);
                }
            }
            else
            {
                descriptor.RemoveValueChanged(this.GetPropertyOwner(descriptor, this.Instance), handler);
            }
        }

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The convert.
        /// </returns>
        private bool Convert(ref object value)
        {
            // Check if it neccessary to convert the value
            var propertyType = this.Descriptor.PropertyType;
            if (propertyType == typeof(object) || value == null && propertyType.IsClass
                || value != null && propertyType.IsAssignableFrom(value.GetType()))
            {
                // no conversion neccessary
            }
            else
            {
                // try to convert the value
                var converter = this.Descriptor.Converter;
                if (value != null)
                {
                    if (propertyType == typeof(TimeSpan) && value is string)
                    {
                        value = TimeSpanParser.Parse(value as string, this.FormatString);
                        return true;
                    }

                    if (converter.CanConvertFrom(value.GetType()))
                    {
                        try
                        {
                            // Change to '.' decimal separator, and use InvariantCulture when converting
                            if (propertyType == typeof(float) || propertyType == typeof(double))
                            {
                                if (value is string)
                                {
                                    value = ((string)value).Replace(',', '.');
                                }
                            }

                            if (this.IsHexFormatString(this.FormatString))
                            {
                                var hex = int.Parse(
                                    value as string, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                                value = hex.ToString(CultureInfo.InvariantCulture);
                            }

                            value = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
                        }

                            // Catch FormatExceptions
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (propertyType == typeof(float) && value is double)
                        {
                            var d = (double)value;
                            value = (float)d;
                            return true;
                        }

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

        /// <summary>
        /// The get property owner.
        /// </summary>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// The get property owner.
        /// </returns>
        private object GetPropertyOwner(PropertyDescriptor descriptor, object instance)
        {
            var tdp = TypeDescriptor.GetProvider(instance);
            var td = tdp.GetTypeDescriptor(instance);
            var c = td.GetPropertyOwner(descriptor);
            return c;
        }

        /// <summary>
        /// The instance property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void InstancePropertyChanged(object sender, EventArgs e)
        {
            // Sending notification when the instance has been changed
            this.NotifyPropertyChanged("Value");
            this.SetByThis = false;
        }

        /// <summary>
        /// The is enabled changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void IsEnabledChanged(object sender, EventArgs e)
        {
            this.NotifyPropertyChanged("IsEnabled");
        }

        /// <summary>
        /// The is hex format string.
        /// </summary>
        /// <param name="formatString">
        /// The format string.
        /// </param>
        /// <returns>
        /// The is hex format string.
        /// </returns>
        private bool IsHexFormatString(string formatString)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(formatString))
            {
                result = formatString.StartsWith("X") || formatString.Contains("{0:X");
            }

            return result;
        }

        /// <summary>
        /// The is modified.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The is modified.
        /// </returns>
        private bool IsModified(object component, object value)
        {
            // Return if the value has not been modified
            var currentValue = this.Descriptor.GetValue(component);
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

        /// <summary>
        /// The is visible changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void IsVisibleChanged(object sender, EventArgs e)
        {
            this.NotifyPropertyChanged("IsVisible");
            this.NotifyPropertyChanged("Visibility");
        }

    }
}