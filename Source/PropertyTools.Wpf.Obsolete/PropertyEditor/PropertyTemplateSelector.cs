// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyTemplateSelector.cs" company="PropertyTools">
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
//   The PropertyTemplateSelector is used to select a DataTemplate given an PropertyViewModel instance.
//   The DataTemplates should be defined in the BasicEditors.xaml/ExtendedEditors.xaml
//   or in the Editors collection of the PropertyEditor.
//   This Selector can also be overriden if you want to provide custom selecting implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// The PropertyTemplateSelector is used to select a DataTemplate given an PropertyViewModel instance.
    /// The DataTemplates should be defined in the BasicEditors.xaml/ExtendedEditors.xaml
    /// or in the Editors collection of the PropertyEditor.
    /// This Selector can also be overriden if you want to provide custom selecting implementation.
    /// </summary>
    public class PropertyTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTemplateSelector"/> class.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public PropertyTemplateSelector(PropertyEditor owner)
        {
            this.Editors = new Collection<TypeEditor>();
            this.Owner = owner;
        }

        /// <summary>
        /// Gets or sets Editors.
        /// </summary>
        public Collection<TypeEditor> Editors { get; set; }

        /// <summary>
        /// Gets Owner.
        /// </summary>
        public PropertyEditor Owner { get; private set; }

        /// <summary>
        /// Gets or sets TemplateOwner.
        /// </summary>
        public FrameworkElement TemplateOwner { get; set; }

        /// <summary>
        /// The select template.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var property = item as PropertyViewModel;
            if (property == null)
            {
                throw new ArgumentException("item must be of type Property");
            }

            // Debug.WriteLine("Select template for " + property.PropertyName);

            // Check if an editor is defined for the given type
            foreach (var editor in this.Editors)
            {
                // checking generic type
                if (property.PropertyType.IsGenericType
                    && editor.EditedType == property.PropertyType.GetGenericTypeDefinition())
                {
                    return editor.EditorTemplate;
                }

                // checking generic interfaces
                foreach (var @interface in property.PropertyType.GetInterfaces())
                {
                    if (@interface.IsGenericType)
                    {
                        if (editor.EditedType == @interface.GetGenericTypeDefinition())
                        {
                            return editor.EditorTemplate;
                        }
                    }

                    if (editor.EditedType == @interface)
                    {
                        return editor.EditorTemplate;
                    }
                }

                if (editor.EditedType.IsAssignableFrom(property.PropertyType))
                {
                    return editor.EditorTemplate;
                }
            }

            var element = container as FrameworkElement;
            if (element == null)
            {
                return base.SelectTemplate(property.Value, container);
            }

            var template = this.FindDataTemplate(property, this.TemplateOwner);

            // Debug.WriteLine("  Returning " + template);
            return template;
        }

        /// <summary>
        /// The try to find data template.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// </returns>
        private static DataTemplate TryToFindDataTemplate(FrameworkElement element, string key)
        {
            var resource = element.TryFindResource(key);
            return resource as DataTemplate;
        }

        /// <summary>
        /// The try to find data template.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// </returns>
        private static DataTemplate TryToFindDataTemplate(FrameworkElement element, Type type)
        {
            var key = new ComponentResourceKey(typeof(PropertyEditor), type);

            // todo: this command throws an exception
            return element.TryFindResource(key) as DataTemplate;
        }

        /// <summary>
        /// The find data template.
        /// </summary>
        /// <param name="propertyViewModel">
        /// The property view model.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <returns>
        /// </returns>
        private DataTemplate FindDataTemplate(PropertyViewModel propertyViewModel, FrameworkElement element)
        {
            Type propertyType = propertyViewModel.PropertyType;

            if (Nullable.GetUnderlyingType(propertyType) != null)
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
            }

            // Try to find a template for the given type
            var template = TryToFindDataTemplate(element, propertyType);
            if (template != null)
            {
                return template;
            }

            // DataTemplates for enum types
            if (propertyType.BaseType == typeof(Enum))
            {
                var rba =
                    propertyViewModel.Descriptor.Attributes[typeof(RadioButtonsAttribute)] as RadioButtonsAttribute;
                int nValues = Enum.GetValues(propertyType).FilterOnBrowsableAttribute().Count;
                if (rba != null)
                {
                    nValues = this.Owner.EnumAsRadioButtonsLimit;
                }

                if (nValues > this.Owner.EnumAsRadioButtonsLimit)
                {
                    template = TryToFindDataTemplate(element, "ComboBoxEnumTemplate");
                }
                else
                {
                    template = TryToFindDataTemplate(element, "RadioButtonsEnumTemplate");
                }

                if (template != null)
                {
                    return template;
                }
            }

            // Try to find a template for the base type
            while (propertyType.BaseType != null)
            {
                propertyType = propertyType.BaseType;
                template = TryToFindDataTemplate(element, propertyType);
                if (template != null)
                {
                    return template;
                }
            }

            // If the Slidable attribute is set, use the 'sliderBox' template
            if (propertyViewModel is SlidablePropertyViewModel)
            {
                template = TryToFindDataTemplate(element, "SliderBoxTemplate");
                if (template != null)
                {
                    return template;
                }
            }

            if (propertyViewModel is FilePathPropertyViewModel)
            {
                template = TryToFindDataTemplate(element, "FilePathTemplate");
                if (template != null)
                {
                    return template;
                }
            }

            if (propertyViewModel is DirectoryPathPropertyViewModel)
            {
                template = TryToFindDataTemplate(element, "DirectoryPathTemplate");
                if (template != null)
                {
                    return template;
                }
            }

            if (propertyViewModel is PasswordPropertyViewModel)
            {
                template = TryToFindDataTemplate(element, "PasswordTemplate");
                if (template != null)
                {
                    return template;
                }
            }

            // Use the default template (TextBox)
            if (propertyViewModel.AutoUpdateText)
            {
                template = TryToFindDataTemplate(element, "DefaultTemplateAutoUpdate");
            }
            else
            {
                template = TryToFindDataTemplate(element, "DefaultTemplate");
            }

            return template;
        }

    }
}