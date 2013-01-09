// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Property.cs" company="PropertyTools">
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
//   The Property model
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Collections;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// The Property model
    /// </summary>
    public class Property : PropertyBase
    {
        public PropertyTemplateSelector PropertyTemplateSelector { get { return Owner.PropertyTemplateSelector; } }

        public object Instance { get; private set; }
        public PropertyDescriptor Descriptor { get; private set; }

        public string FormatString { get; set; }
        public double Height { get; set; }
        public bool AcceptsReturn { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="descriptor"></param>
        /// <param name="owner"></param>
        public Property(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(owner)
        {
            Instance = instance;
            Descriptor = descriptor;

            Name = descriptor.Name;
            Header = descriptor.DisplayName;
            ToolTip = descriptor.Description;

            // todo: is this ok? could it be a weak event?
            Descriptor.AddValueChanged(Instance, InstancePropertyChanged);

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
        void InstancePropertyChanged(object sender, EventArgs e)
        {
            // Sending notification when the instance has been changed
            NotifyPropertyChanged("Value");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Descriptor.RemoveValueChanged(Instance, InstancePropertyChanged);
            }
            base.Dispose(disposing);
        }

        /// <value>
        /// Initializes the reflected Instance property
        /// </value>
        /// <exception cref="NotSupportedException">
        /// The conversion cannot be performed
        /// </exception>
        public object Value
        {
            get
            {
                return OnGetProperty();
            }
            set
            {
                OnSetProperty(value);
            }
        }

        protected virtual object OnGetProperty()
        {
            object value;
            if (Instance is IEnumerable)
                value = GetValueFromEnumerable(Instance as IEnumerable);
            else
                value = Descriptor.GetValue(Instance);
            return value;
        }

        protected virtual void OnSetProperty(object value)
        {
            // Return if the value has not been modified
            object currentValue = Descriptor.GetValue(Instance);
            if (currentValue == null && value == null)
            {
                return;
            }
            if (value != null && value.Equals(currentValue))
            {
                return;
            }

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
                // convert the value
                var converter = TypeDescriptor.GetConverter(Descriptor.PropertyType);
                value = converter.ConvertFrom(value);
            }

            var list = Instance as IEnumerable;
            if (list != null)
            {
                foreach (object item in list)
                {
                    OnSetProperty(item, Descriptor, value);
                }
            }
            else
            {
                OnSetProperty(Instance, Descriptor, value);
            }

        }

        protected void OnSetProperty(object instance, PropertyDescriptor descriptor, object value)
        {
            // Use the PropertySetter service, if available
            if (Owner.PropertySetter != null)
            {
                Owner.PropertySetter.SetProperty(instance, descriptor, value);
                return;
            }

            Descriptor.SetValue(Instance, value);
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
                object v = Descriptor.GetValue(component);
                if (value == null)
                    value = v;
                if (value != null && !v.Equals(value))
                    return null;
            }
            return null; // no value
        }
    }
}