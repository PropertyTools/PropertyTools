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

        #region Descriptor properties

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

        #region Event Handlers

        void InstancePropertyChanged(object sender, EventArgs e)
        {
            // Sending notification when the instance has been changed
            NotifyPropertyChanged("Value");
        }

        #endregion

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Descriptor.RemoveValueChanged(Instance, InstancePropertyChanged);
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Set/Get
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
        #endregion

    }
}
