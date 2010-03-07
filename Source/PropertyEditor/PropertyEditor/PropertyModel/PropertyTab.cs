using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace PropertyEditorLibrary
{
    public class PropertyTab : PropertyBase
    {
        public CategoryTemplateSelector CategoryTemplateSelector { get { return Owner.CategoryTemplateSelector; } }

        public List<PropertyCategory> Categories { get; private set; }

        public ImageSource Icon { get; set; }
        public Visibility IconVisibility { get { return Icon != null ? Visibility.Visible : Visibility.Collapsed; } }

        public PropertyTab(string tabName, PropertyEditor owner)
            : base(owner)
        {
            Name = Header = tabName;
            Categories = new List<PropertyCategory>();
        }

    }
}
