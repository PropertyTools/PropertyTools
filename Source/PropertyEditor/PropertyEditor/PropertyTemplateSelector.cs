using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace PropertyEditorLibrary
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
            var property = item as Property;
            if (property == null)
            {
                throw new ArgumentException("item must be of type Property");
            }
            // Debug.WriteLine("Select template for " + property.PropertyName);

            // Check if an editor is defined for the given type
            foreach (TypeEditor editor in Editors)
            {
                if (editor.EditedType.IsAssignableFrom(property.PropertyType))
                    return editor.EditorTemplate;
            }

            var element = container as FrameworkElement;
            if (element == null)
            {
                return base.SelectTemplate(property.Value, container);
            }

            // todo: this returns sometimes warnings like
            // System.Windows.ResourceDictionary Warning: 9 : Resource not found; ResourceKey='TargetType=PropertyEditorLibrary.PropertyEditor ID=System.String'; ResourceKey.HashCode='2100452176'; ResourceKey.Type='System.Windows.ComponentResourceKey'
            // System.Windows.ResourceDictionary Warning: 9 : Resource not found; ResourceKey='TargetType=PropertyEditorLibrary.PropertyEditor ID=System.Object'; ResourceKey.HashCode='2100451148'; ResourceKey.Type='System.Windows.ComponentResourceKey'

            var template = FindDataTemplate(property, TemplateOwner);
            // Debug.WriteLine("  Returning " + template);
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
            if (property is SlidableProperty)
            {
                template = TryToFindDataTemplate(element, "SliderBoxTemplate");
                if (template != null)
                    return template;
            }

            // Use the default template (TextBox)
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
            return element.TryFindResource(key) as DataTemplate;
        }
    }
}
