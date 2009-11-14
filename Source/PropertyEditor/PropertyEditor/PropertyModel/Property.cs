using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Reflection;
using System.Collections;
using System.Diagnostics;

namespace OpenControls
{
    public class Property : PropertyBase
    {
        public PropertyTemplateSelector PropertyTemplateSelector { get; set; }

        #region Fields

        private object _instance;
        private PropertyDescriptor _descriptor;

        #endregion

        #region Optional
        public Visibility OptionVisibility
        {
            get { return IsOptional ? Visibility.Visible : Visibility.Collapsed; }
        }
        public Visibility LabelVisibility
        {
            get { return !IsOptional ? Visibility.Visible : Visibility.Collapsed; }
        }

        public bool IsOptional { get; set; }

        public bool IsOptionalChecked
        {
            get
            {
                if (IsOptional)
                    return (bool)GetProperty(OptionalProperty);
                else
                    return true; // default must be true (enable editor)
            }
            set
            {
                if (IsOptional)
                {
                    SetProperty(OptionalProperty, value);
                    NotifyPropertyChanged("IsOptionalChecked");
                }

            }
        }

        private void SetProperty(string name, object value)
        {
            PropertyInfo pi = _instance.GetType().GetProperty(name);
            pi.SetValue(_instance, value, null);
        }

        private object GetProperty(string name)
        {
            PropertyInfo pi = _instance.GetType().GetProperty(name);
            return pi.GetValue(_instance, null);
        }

        public string OptionalProperty { get; set; }

        public void InitializeOptional()
        {
            IsOptional = false;
            foreach (Attribute a in _descriptor.Attributes)
            {
                OptionalAttribute oa = a as OptionalAttribute;
                if (oa != null)
                {
                    IsOptional = true;
                    OptionalProperty = oa.PropertyName;

                }
            }
        }

        #endregion

        #region Initialization

        public Property(object instance, PropertyDescriptor descriptor, PropertyTemplateSelector pts)
        {
            _instance = instance;
            _descriptor = descriptor;
            PropertyTemplateSelector = pts;

            Header = descriptor.DisplayName;
            ToolTip = descriptor.Description;

            _descriptor.AddValueChanged(_instance, instance_PropertyChanged);

            InitializeOptional();
        }

        #endregion

        #region Properties

        /// <value>
        /// Initializes the reflected instance property
        /// </value>
        /// <exception cref="NotSupportedException">
        /// The conversion cannot be performed
        /// </exception>
        public object Value
        {
            get
            {
                Debug.IndentLevel++;
                Debug.WriteLine("Property.Value.Get("+PropertyName+")");
                object returnValue;
                if (_instance is IEnumerable)
                    returnValue = GetMultiValue(_instance as IEnumerable);
                else
                    returnValue = GetSingleValue(_instance);
                Debug.IndentLevel--;
                return returnValue;
            }
            set
            {
                Debug.IndentLevel++;
                Debug.WriteLine("Property.Value.Set(" + PropertyName + ")");
                object currentValue = _descriptor.GetValue(_instance);
                if (value != null && value.Equals(currentValue))
                {
                    return;
                }
                Type propertyType = _descriptor.PropertyType;
                if (propertyType == typeof(object) ||
                    value == null && propertyType.IsClass ||
                    value != null && propertyType.IsAssignableFrom(value.GetType()))
                {
                    _descriptor.SetValue(_instance, value);
                }
                else
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(_descriptor.PropertyType);
                    object convertedValue = converter.ConvertFrom(value);
                    _descriptor.SetValue(_instance, convertedValue);
                }
                Debug.IndentLevel--;
            }
        }

        private object GetSingleValue(object component)
        {
            return _descriptor.GetValue(component);
        }

        // WIP
        private object GetMultiValue(IEnumerable componentList)
        {
            object value = null;
            foreach (object component in componentList)
            {
                object v = _descriptor.GetValue(component);
                if (value == null)
                    value = v;
                if (value != null && !v.Equals(value))
                    return null; // undetermined (todo...)
            }
            return null; // no value
        }

        public bool IsWriteable
        {
            get { return !IsReadOnly; }
        }

        public bool IsReadOnly
        {
            get { return _descriptor.IsReadOnly; }
        }

        public Type PropertyType
        {
            get { return _descriptor.PropertyType; }
        }

        public string PropertyName
        {
            get { return _descriptor.Name; }
        }

        public string DisplayName
        {
            get { return _descriptor.DisplayName; }
        }

        public string Category
        {
            get { return _descriptor.Category; }
        }

        public string Description
        {
            get { return _descriptor.Description; }
        }

        #endregion

        #region Event Handlers

        void instance_PropertyChanged(object sender, EventArgs e)
        {
            Debug.IndentLevel++;
            Debug.WriteLine("instance_PropertyChanged");
            NotifyPropertyChanged("Value");
            Debug.IndentLevel--;
        }

        #endregion

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                _descriptor.RemoveValueChanged(_instance, instance_PropertyChanged);
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
