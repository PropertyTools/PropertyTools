using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace CustomTypeDescriptorDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var d = new Dictionary<string, object>();
            d.Add("Int", 32);
            d.Add("Double", 31.4);
            d.Add("String", "test");
            d.Add("Color", Colors.Blue);
            d.Add("Null", null);
            pe1.SelectedObject = new DictionaryWrapper(d);
        }
    }

    [TypeDescriptionProvider(typeof(DictionaryTypeDescriptorProvider))]
    public class DictionaryWrapper
    {
        public DictionaryWrapper(Dictionary<string, object> dictionary)
        {
            Dictionary = dictionary;
        }

        public Dictionary<string, object> Dictionary { get; set; }
    }

    public class DictionaryTypeDescriptorProvider : TypeDescriptionProvider
    {
        public DictionaryTypeDescriptorProvider()
            : base(TypeDescriptor.GetProvider(typeof(DictionaryWrapper)))
        {
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return new DictionaryTypeDescriptor(instance);
        }
    }

    public class DictionaryTypeDescriptor : CustomTypeDescriptor
    {
        private readonly object instance;

        public DictionaryTypeDescriptor(object instance)
        {
            this.instance = instance;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            var w = instance as DictionaryWrapper;
            Dictionary<string, object> dict = w.Dictionary;
            if (dict.Count == 0)
                return PropertyDescriptorCollection.Empty;

            var result = new List<PropertyDescriptor>(dict.Count);
            foreach (var kvp in dict)
            {
                var pd = new DictionaryItemPropertyDescriptor(kvp.Key,
                                                              kvp.Value != null ? kvp.Value.GetType() : typeof(object));
                result.Add(pd);
            }
            return new PropertyDescriptorCollection(result.ToArray());
        }
    }

    public class DictionaryItemPropertyDescriptor : PropertyDescriptor
    {
        private readonly Type propertyType;

        public DictionaryItemPropertyDescriptor(string key, Type propertyType)
            : base(key, null)
        {
            this.propertyType = propertyType;
        }

        public override Type ComponentType
        {
            get { return typeof(DictionaryWrapper); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return propertyType; }
        }

        public override bool CanResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(object component)
        {
            var w = component as DictionaryWrapper;
            return w.Dictionary[Name];
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            var w = component as DictionaryWrapper;
            w.Dictionary[Name] = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            throw new NotImplementedException();
        }
    }
}