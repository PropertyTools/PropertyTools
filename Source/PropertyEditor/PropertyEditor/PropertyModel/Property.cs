using System;
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

        #region Optional

        private string _OptionalProperty;
        public string OptionalProperty
        {
            get
            {
                return _OptionalProperty;
            }
            set
            {
                if (_OptionalProperty != value)
                {
                    _OptionalProperty = value;
                    NotifyPropertyChanged("OptionalProperty");
                }
            }
        }

        public Visibility OptionVisibility
        {
            get { return IsOptional ? Visibility.Visible : Visibility.Collapsed; }
        }
        public Visibility LabelVisibility
        {
            get { return !IsOptional ? Visibility.Visible : Visibility.Collapsed; }
        }

        public bool IsOptional { get; set; }

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
        public bool IsOptionalChecked
        {
            get
            {
                if (!string.IsNullOrEmpty(OptionalProperty))
                    return (bool)GetProperty(OptionalProperty);
                return true; // default must be true (enable editor)
            }
            set
            {
                if (!string.IsNullOrEmpty(OptionalProperty))
                {
                    SetProperty(OptionalProperty, value);
                    NotifyPropertyChanged("IsOptionalChecked");
                }

            }
        }

        #endregion

        #region FormatString
        public string FormatString { get; set; }

        public void InitializeFormatString()
        {
            FormatString = null;
            foreach (Attribute a in _descriptor.Attributes)
            {
                var fsa = a as FormatStringAttribute;
                if (fsa != null)
                {
                    FormatString = fsa.FormatString;
                }
            }
        }
        #endregion

        #region Slidable
        public Visibility SliderVisibility
        {
            get { return IsSlidable ? Visibility.Visible : Visibility.Collapsed; }
        }

        public bool IsSlidable { get; set; }

        public double SliderMaximum { get; set; }
        public double SliderMinimum { get; set; }
        public double SliderSmallChange { get; set; }
        public double SliderLargeChange { get; set; }

        public void InitializeSlidable()
        {
            IsSlidable = false;
            foreach (Attribute a in _descriptor.Attributes)
            {
                SlidableAttribute sa = a as SlidableAttribute;
                if (sa != null)
                {
                    IsSlidable = true;
                    SliderMaximum = sa.Maximum;
                    SliderMinimum = sa.Minimum;
                    SliderLargeChange = sa.LargeChange;
                    SliderSmallChange = sa.SmallChange;
                }
            }
        }

        #endregion

        #region Constructor
        private object _instance;

        public Property(object instance, PropertyDescriptor descriptor, PropertyTemplateSelector pts)
        {
            _instance = instance;
            _descriptor = descriptor;
            PropertyTemplateSelector = pts;

            Header = descriptor.DisplayName;
            ToolTip = descriptor.Description;

            _descriptor.AddValueChanged(_instance, instance_PropertyChanged);

            InitializeOptional();
            InitializeSlidable();
            InitializeFormatString();
        }
        #endregion

        #region Descriptor properties
        private PropertyDescriptor _descriptor;

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
            // Debug.IndentLevel++;
            // Debug.WriteLine("instance_PropertyChanged");
            NotifyPropertyChanged("Value");
            // Debug.IndentLevel--;
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

        #region Set/Get
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
                // Debug.IndentLevel++;
                // Debug.WriteLine("Property.Value.Get("+PropertyName+")");
                object value;
                if (_instance is IEnumerable)
                    value = GetMultiValue(_instance as IEnumerable);
                else
                    value = _descriptor.GetValue(_instance);
                Debug.IndentLevel--;
                return value;
            }
            set
            {
                // Debug.IndentLevel++;
                // Debug.WriteLine("Property.Value.Set(" + PropertyName + ")");
                object currentValue = _descriptor.GetValue(_instance);
                if (value != null && value.Equals(currentValue))
                {
                    return;
                }
                Type propertyType = _descriptor.PropertyType;
                bool multi = _instance is IEnumerable;

                if (propertyType == typeof(object) ||
                    value == null && propertyType.IsClass ||
                    value != null && propertyType.IsAssignableFrom(value.GetType()))
                {
                    if (multi)
                        SetMultiValue(_instance as IEnumerable, value);
                    else
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

        private void SetMultiValue(IEnumerable list, object value)
        {
            foreach (object item in list)
            {
                _descriptor.SetValue(item, value);
            }
        }

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
        #endregion

    }
}
