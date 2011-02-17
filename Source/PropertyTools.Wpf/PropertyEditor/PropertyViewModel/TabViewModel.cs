using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// ViewModel for the tabs.
    /// </summary>
    public class TabViewModel : ViewModelBase
    {
        public TabViewModel(string tabName, PropertyEditor owner)
            : base(owner)
        {
            Name = Header = tabName;
            Categories = new List<CategoryViewModel>();
        }

        public CategoryTemplateSelector CategoryTemplateSelector
        {
            get { return Owner.CategoryTemplateSelector; }
        }

        public List<CategoryViewModel> Categories { get; private set; }

        public string Name { get; set; }
        public ImageSource Icon { get; set; }

        public Visibility IconVisibility
        {
            get { return Icon != null ? Visibility.Visible : Visibility.Collapsed; }
        }

        public bool HasErrors
        {
            get
            {
                foreach (CategoryViewModel cat in Categories)
                    foreach (PropertyViewModel prop in cat.Properties)
                        if (prop.PropertyError != null)
                            return true;
                return false;
            }
        }

        public bool HasWarnings
        {
            get
            {
                foreach (CategoryViewModel cat in Categories)
                    foreach (PropertyViewModel prop in cat.Properties)
                        if (prop.PropertyWarning != null)
                            return true;
                return false;
            }
        }

        public void Sort()
        {
            Categories = Categories.OrderBy(c => c.SortOrder).ToList();
        }

        public void UpdateErrorInfo()
        {
            NotifyPropertyChanged("HasErrors");
            NotifyPropertyChanged("HasWarnings");
        }
    }
}