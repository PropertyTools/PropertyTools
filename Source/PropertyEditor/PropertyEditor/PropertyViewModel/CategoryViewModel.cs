using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// ViewModel for categories.
    /// The categories can be shown as GroupBox, Expander or by the Header.
    /// </summary>
    public class CategoryViewModel : ViewModelBase
    {
        public List<PropertyViewModel> Properties { get; set; }
        public string Name { get; set; }

        #region Enabled/Visible

        private bool isEnabled = true;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; NotifyPropertyChanged("IsEnabled"); }
        }

        public Visibility Visibility { get { return Visibility.Visible; } }
        #endregion

        public CategoryViewModel(string categoryName, PropertyEditor owner)
            : base(owner)
        {
            Name = Header = categoryName;
            Properties = new List<PropertyViewModel>();
        }

        public void Sort()
        {
            Properties = Properties.OrderBy(p => p.SortOrder).ToList();
        }
    }
}
