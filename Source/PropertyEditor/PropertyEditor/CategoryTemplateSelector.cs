using System;
using System.Windows;
using System.Windows.Controls;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// The CategoryTemplateSelector is used to select a DataTemplate for the categories
    /// </summary>
    public class CategoryTemplateSelector : DataTemplateSelector
    {
        public FrameworkElement TemplateOwner { get; set; }
        public ShowCategoriesAs ShowAs { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var category = item as PropertyCategory;
            if (category == null)
            {
                throw new ArgumentException("item must be of type Property");
            }

            /*var element = container as FrameworkElement;
            if (element == null)
            {
                return base.SelectTemplate(category, container);
            }*/
            var key = ShowAs == ShowCategoriesAs.GroupBox ? "CategoryGroupBoxTemplate" : "CategoryExpanderTemplate";
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
