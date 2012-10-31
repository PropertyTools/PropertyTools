// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultPropertyViewModelFactory.cs" company="PropertyTools">
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
//   Initializes a new instance of the <see cref="DefaultPropertyViewModelFactory"/> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Windows;

namespace PropertyEditorLibrary
{
    public class DefaultPropertyViewModelFactory : IPropertyViewModelFactory
    {
        private readonly PropertyEditor owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPropertyViewModelFactory"/> class.
        /// </summary>
        /// <param name="owner">The owner PropertyEditor of the factory.
        /// This is neccessary in order to get the PropertyTemplateSelector to work.</param>
        public DefaultPropertyViewModelFactory(PropertyEditor owner)
        {
            this.owner = owner;
        }

        public virtual PropertyViewModel CreateViewModel(object instance, PropertyDescriptor descriptor)
        {
            PropertyViewModel propertyViewModel = null;

            // Optional by Nullable type
            var nullable = descriptor.PropertyType.IsGenericType &&
                           descriptor.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (nullable)
                propertyViewModel = new OptionalPropertyViewModel(instance, descriptor, null, owner);

            // Optional by Attribute
            var oa = AttributeHelper.GetAttribute<OptionalAttribute>(descriptor);
            if (oa != null)
                propertyViewModel = new OptionalPropertyViewModel(instance, descriptor, oa.PropertyName, owner);

            // Wide
            var wa = AttributeHelper.GetAttribute<WidePropertyAttribute>(descriptor);
            if (wa != null)
                propertyViewModel = new WidePropertyViewModel(instance, descriptor, wa.ShowHeader, owner);

            // If bool properties should be shown as checkbox only (no header label), we create
            // a CheckBoxPropertyViewModel
            if (descriptor.PropertyType == typeof(bool) && owner != null && !owner.ShowBoolHeader)
                propertyViewModel = new CheckBoxPropertyViewModel(instance, descriptor, owner);

            // Properties with the Slidable attribute set
            var sa = AttributeHelper.GetAttribute<SlidableAttribute>(descriptor);
            if (sa != null)
                propertyViewModel = new SlidablePropertyViewModel(instance, descriptor, owner)
                                        {
                                            SliderMinimum = sa.Minimum,
                                            SliderMaximum = sa.Maximum,
                                            SliderLargeChange = sa.LargeChange,
                                            SliderSmallChange = sa.SmallChange
                                        };

            // FilePath
            var fpa = AttributeHelper.GetAttribute<FilePathAttribute>(descriptor);
            if (fpa != null)
                propertyViewModel = new FilePathPropertyViewModel(instance, descriptor, owner) { Filter = fpa.Filter, DefaultExtension = fpa.DefaultExtension };

            // DirectoryPath
            var dpa = AttributeHelper.GetAttribute<DirectoryPathAttribute>(descriptor);
            if (dpa != null)
                propertyViewModel = new DirectoryPathPropertyViewModel(instance, descriptor, owner);

            // Default text property
            if (propertyViewModel == null)
            {
                var tp = new PropertyViewModel(instance, descriptor, owner);

                propertyViewModel = tp;
            }

            var fsa = AttributeHelper.GetAttribute<FormatStringAttribute>(descriptor);
            if (fsa != null)
                propertyViewModel.FormatString = fsa.FormatString;

            var ha = AttributeHelper.GetAttribute<HeightAttribute>(descriptor);
            if (ha != null)
                propertyViewModel.Height = ha.Height;
            if (propertyViewModel.Height>0)
            {
                propertyViewModel.AcceptsReturn = true;
                propertyViewModel.TextWrapping = TextWrapping.Wrap;
            }

            var soa = AttributeHelper.GetAttribute<SortOrderAttribute>(descriptor);
            if (soa != null)
                propertyViewModel.SortOrder = soa.SortOrder;

            return propertyViewModel;
        }
    }

}