using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The PropertyTemplateSelector is used to select a DataTemplate given an PropertyViewModel instance.
    /// The DataTemplates should be defined in the BasicEditors.xaml/ExtendedEditors.xaml
    /// or in the Editors collection of the PropertyEditor.
    /// This Selector can also be overriden if you want to provide custom selecting implementation.
    /// </summary>
    public class PropertyTemplateSelector : DataTemplateSelector
    {
        public Collection<TypeEditor> Editors { get; set; }

        public FrameworkElement TemplateOwner { get; set; }

        public PropertyEditor Owner { get; private set; }

        public PropertyTemplateSelector(PropertyEditor owner)
        {
            Editors = new Collection<TypeEditor>();
            Owner = owner;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var property = item as PropertyViewModel;
            if (property == null)
            {
                throw new ArgumentException("item must be of type Property");
            }
            //         Debug.WriteLine("Select template for " + property.PropertyName);

            // Check if an editor is defined for the given type
            foreach (var editor in Editors)
            {
                // checking generic type
                if (property.PropertyType.IsGenericType && editor.EditedType == property.PropertyType.GetGenericTypeDefinition())
                    return editor.EditorTemplate;

                // checking generic interfaces
                foreach (var @interface in property.PropertyType.GetInterfaces())
                {
                    if (@interface.IsGenericType)
                    {
                        if (editor.EditedType==@interface.GetGenericTypeDefinition())
                            return editor.EditorTemplate;
                    }
                    if (editor.EditedType==@interface)
                        return editor.EditorTemplate;
                }

                if (editor.EditedType.IsAssignableFrom(property.PropertyType))
                    return editor.EditorTemplate;
            }

            var element = container as FrameworkElement;
            if (element == null)
            {
                return base.SelectTemplate(property.Value, container);
            }

            var template = FindDataTemplate(property, TemplateOwner);
            //       Debug.WriteLine("  Returning " + template);
            return template;
        }

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
                return template;

            // DataTemplates for enum types
            if (propertyType.BaseType == typeof(Enum))
            {
                var rba = propertyViewModel.Descriptor.Attributes[typeof(RadioButtonsAttribute)] as RadioButtonsAttribute;
				int nValues = Enum.GetValues( propertyType ).FilterOnBrowsableAttribute().Count;
                if (rba != null)
                {
                    if (rba.UseRadioButtons)
                        nValues = Owner.EnumAsRadioButtonsLimit;
                    else
                        nValues = Owner.EnumAsRadioButtonsLimit + 1;
                }
                if (nValues>Owner.EnumAsRadioButtonsLimit)
                    template = TryToFindDataTemplate(element, "ComboBoxEnumTemplate");
                else
                    template = TryToFindDataTemplate(element, "RadioButtonsEnumTemplate");
                if (template != null)
                    return template;
            }

            // Try to find a template for the base type
            while (propertyType.BaseType != null)
            {
                propertyType = propertyType.BaseType;
                template = TryToFindDataTemplate(element, propertyType);
                if (template != null)
                    return template;
            }

            // If the Slidable attribute is set, use the 'sliderBox' template
            if (propertyViewModel is SlidablePropertyViewModel)
            {
                template = TryToFindDataTemplate(element, "SliderBoxTemplate");
                if (template != null)
                    return template;
            }

            if (propertyViewModel is FilePathPropertyViewModel)
            {
                template = TryToFindDataTemplate(element, "FilePathTemplate");
                if (template != null)
                    return template;
            }

            if (propertyViewModel is DirectoryPathPropertyViewModel)
            {
                template = TryToFindDataTemplate(element, "DirectoryPathTemplate");
                if (template != null)
                    return template;
            }

            if (propertyViewModel is PasswordPropertyViewModel)
            {
                template = TryToFindDataTemplate(element, "PasswordTemplate");
                if (template != null)
                    return template;
            }

            // Use the default template (TextBox)
            if (propertyViewModel.AutoUpdateText)
                template = TryToFindDataTemplate(element, "DefaultTemplateAutoUpdate");
            else
                template = TryToFindDataTemplate(element, "DefaultTemplate");

            return template;
        }

        private static DataTemplate TryToFindDataTemplate(FrameworkElement element, string key)
        {
            var resource = element.TryFindResource(key);
            return resource as DataTemplate;
        }

        private static DataTemplate TryToFindDataTemplate(FrameworkElement element, Type type)
        {
            var key = new ComponentResourceKey(typeof(PropertyEditor), type);
            // todo: this command throws an exception
            return element.TryFindResource(key) as DataTemplate;
        }
    }
}
