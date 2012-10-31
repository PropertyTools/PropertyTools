// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
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
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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