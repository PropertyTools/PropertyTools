using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PropertyEditorLibrary
{
    public class TabViewModel : ViewModelBase
    {
        public CategoryTemplateSelector CategoryTemplateSelector { get { return Owner.CategoryTemplateSelector; } }

        public List<CategoryViewModel> Categories { get; private set; }

        public string Name { get; set; }
        public ImageSource Icon { get; set; }
        public Visibility IconVisibility { get { return Icon != null ? Visibility.Visible : Visibility.Collapsed; } }

        public TabViewModel(string tabName, PropertyEditor owner)
            : base(owner)
        {
            Name = Header = tabName;
            Categories = new List<CategoryViewModel>();
        }

        public void Sort()
        {
            Categories=Categories.OrderBy(c => c.SortOrder).ToList();
        }
    }
}
