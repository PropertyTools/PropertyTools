using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// ViewModel for categories.
    /// The categories can be shown as GroupBox, Expander or by the Header.
    /// </summary>
    public class CategoryViewModel : ViewModelBase
    {
        private bool isEnabled = true;

        public CategoryViewModel(string categoryName, PropertyEditor owner)
            : base(owner)
        {
            Name = Header = categoryName;
            Properties = new List<PropertyViewModel>();
        }

        public List<PropertyViewModel> Properties { get; private set; }
        public string Name { get; set; }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                NotifyPropertyChanged("IsEnabled");
            }
        }

        public Visibility Visibility
        {
            get { return Visibility.Visible; }
        }

        public void Sort()
        {
            Properties = Properties.OrderBy(p => p.SortOrder).ToList();
        }
    }
}