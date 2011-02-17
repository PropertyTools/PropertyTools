using System;
using System.Windows;
using System.Windows.Controls;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The CategoryTemplateSelector is used to select a DataTemplate for the categories
    /// </summary>
    public class CategoryTemplateSelector : DataTemplateSelector
    {
        public FrameworkElement TemplateOwner { get; set; }
        public PropertyEditor Owner { get; private set; }

        public CategoryTemplateSelector(PropertyEditor owner)
        {
            Owner = owner;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var category = item as CategoryViewModel;
            if (category == null)
            {
                throw new ArgumentException("item must be of type CategoryViewModel");
            }
          
            var key = "CategoryGroupBoxTemplate";
            if (Owner.ShowCategoriesAs == ShowCategoriesAs.Expander) key = "CategoryExpanderTemplate";
            if (Owner.ShowCategoriesAs == ShowCategoriesAs.Header) key = "CategoryHeaderTemplate";

            var template = TryToFindDataTemplate(TemplateOwner, key);

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
