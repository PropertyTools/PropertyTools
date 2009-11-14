using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace OpenControls
{
    /// <summary>
    /// The PropertyTemplateSelector is used to select a DataTemplate based on a type
    /// The dataTemplates must be defined in the BasicEditors/ExtendedEditors.xaml
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
                if (editor.EditedType.GUID == property.PropertyType.GUID)
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
            DataTemplate template = TryFindDataTemplate(element, propertyType);
            
            if (template==null && propertyType.BaseType==typeof(Enum))
            {
                if (ShowEnumAsComboBox)
                    template = TryFindDataTemplate(element, "enumComboBox");
                else
                    template = TryFindDataTemplate(element, "enumRadioButtons");                
            }
            // Find a template for the base type
            while (template == null && propertyType.BaseType != null)
            {
                propertyType = propertyType.BaseType;
                template = TryFindDataTemplate(element, propertyType);
            }

            // Use the default template (TextBox)
            if (template == null)
            {
                template = TryFindDataTemplate(element, "default");
            }
            return template;
        }

        private static DataTemplate TryFindDataTemplate(FrameworkElement element, object dataTemplateKey)
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
