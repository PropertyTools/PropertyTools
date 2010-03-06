using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace OpenControls
{
    /// <summary>
    /// The PropertyTemplateSelector is used to select a DataTemplate based on a type
    /// The DataTemplates must be defined in the BasicEditors.xaml/ExtendedEditors.xaml
    /// or in the Editors collection of the PropertyEditor.
    /// </summary>
    public class PropertyTemplateSelector : DataTemplateSelector
    {
        public Collection<TypeEditor> Editors { get; set; }

        public FrameworkElement TemplateOwner { get; set; }
        public bool ShowEnumAsComboBox { get; set; }

        public PropertyTemplateSelector()
        {
            Editors = new Collection<TypeEditor>();
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Property property = item as Property;
            if (property == null)
            {
                throw new ArgumentException("item must be of type Property");
            }

            // Check if an editor is defined for the given type
            foreach (TypeEditor editor in Editors)
            {
                // todo: how to check if type is the same?
                bool r1 = Object.Equals(editor.EditedType, property.PropertyType);

                bool r2 = editor.EditedType.GUID == property.PropertyType.GUID;
                if (r1 != r2)
                    Console.WriteLine("something wrong");
                if (r2)
                    return editor.EditorTemplate;
            }

            var element = container as FrameworkElement;
            if (element == null)
            {
                return base.SelectTemplate(property.Value, container);
            }

            DataTemplate template = FindDataTemplate(property, TemplateOwner);
            return template;
        }

        private DataTemplate FindDataTemplate(Property property, FrameworkElement element)
        {
            Type propertyType = property.PropertyType;

            // Try to find a template for the given type
            var template = TryToFindDataTemplate(element, propertyType);
            if (template != null)
                return template;

            // DataTemplates for enum types
            if (propertyType.BaseType == typeof(Enum))
            {
                if (ShowEnumAsComboBox)
                    template = TryToFindDataTemplate(element, "enumComboBox");
                else
                    template = TryToFindDataTemplate(element, "enumRadioButtons");
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
            if (property.IsSlidable)
            {
                template = TryToFindDataTemplate(element, "sliderBox");
                if (template != null)
                    return template;
            }

            // Use the 'default' template (TextBox)
            template = TryToFindDataTemplate(element, "default");

            return template;
        }

        private static DataTemplate TryToFindDataTemplate(FrameworkElement element, object dataTemplateKey)
        {
            object dataTemplate = element.TryFindResource(dataTemplateKey);
            if (dataTemplate == null)
            {
                dataTemplateKey = new ComponentResourceKey(typeof(PropertyEditor), dataTemplateKey);
                dataTemplate = element.TryFindResource(dataTemplateKey);
            }
            return dataTemplate as DataTemplate;
        }
    }
}
