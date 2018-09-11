// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomTypeDescriptorExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for CustomTypeDescriptorExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Interaction logic for CustomTypeDescriptorExample.xaml
    /// </summary>
    public partial class CustomTypeDescriptorExample
    {
        static CustomTypeDescriptorExample()
        {
            var m = new CustomModel { Properties = { ["Name"] = "a", ["Value"] = Math.PI } };
            items.Add(m);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomColumnsExample" /> class.
        /// </summary>
        public CustomTypeDescriptorExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private static ObservableCollection<CustomModel> items = new ObservableCollection<CustomModel>();

        public ObservableCollection<CustomModel> Items => items;
    }

    public class CustomModel : ICustomTypeDescriptor, INotifyPropertyChanged
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the properties of the model.
        /// </summary>
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        /// <inheritdoc />
        public AttributeCollection GetAttributes() => throw new NotImplementedException();

        /// <inheritdoc />
        public string GetClassName() => throw new NotImplementedException();

        /// <inheritdoc />
        public string GetComponentName() => throw new NotImplementedException();

        /// <inheritdoc />
        public TypeConverter GetConverter() => throw new NotImplementedException();

        /// <inheritdoc />
        public EventDescriptor GetDefaultEvent() => throw new NotImplementedException();

        /// <inheritdoc />
        public PropertyDescriptor GetDefaultProperty() => throw new NotImplementedException();

        /// <inheritdoc />
        public object GetEditor(Type editorBaseType) => throw new NotImplementedException();

        /// <inheritdoc />
        public EventDescriptorCollection GetEvents() => throw new NotImplementedException();

        /// <inheritdoc />
        public EventDescriptorCollection GetEvents(Attribute[] attributes) => throw new NotImplementedException();

        /// <inheritdoc />
        public PropertyDescriptorCollection GetProperties()
        {
            var propertyDescriptors = new List<PropertyDescriptor>();
            propertyDescriptors.AddRange(this.Properties.Keys.Select(key => new CustomPropertyDescriptor(key, this.Properties[key].GetType())));

            return new PropertyDescriptorCollection(propertyDescriptors.ToArray());
        }

        /// <inheritdoc />
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => throw new NotImplementedException();

        /// <inheritdoc />
        public object GetPropertyOwner(PropertyDescriptor pd) => throw new NotImplementedException();

        public void SetValue(string name, object value)
        {
            this.Properties[name] = value;
            this.OnPropertyChanged(name);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CustomPropertyDescriptor : PropertyDescriptor
    {
        public CustomPropertyDescriptor(string name, Type propertyType)
            : base(name, null)
        {
            this.PropertyType = propertyType;
            this.IsReadOnly = false;
            this.ComponentType = typeof(CustomModel);
        }

        /// <inheritdoc />
        public override bool CanResetValue(object component) => throw new NotImplementedException();

        /// <inheritdoc />
        public override object GetValue(object component) => ((CustomModel)component).Properties[this.Name];

        /// <inheritdoc />
        public override void ResetValue(object component) => throw new NotImplementedException();

        /// <inheritdoc />
        public override void SetValue(object component, object value)
        {
            ((CustomModel)component).SetValue(this.Name, value);
        }

        /// <inheritdoc />
        public override bool ShouldSerializeValue(object component) => throw new NotImplementedException();

        /// <inheritdoc />
        public override Type ComponentType { get; }

        /// <inheritdoc />
        public override bool IsReadOnly { get; }

        /// <inheritdoc />
        public override Type PropertyType { get; }
    }
}