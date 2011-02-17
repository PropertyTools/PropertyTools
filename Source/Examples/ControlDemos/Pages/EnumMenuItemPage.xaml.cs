using System.ComponentModel;
using System.Windows.Controls;

namespace ControlsDemo
{
    /// <summary>
    /// Interaction logic for EnumMenuItemPage.xaml
    /// </summary>
    public partial class EnumMenuItemPage : Page
    {
        public EnumMenuItemPage()
        {
            InitializeComponent();
            DataContext = new EnumMenuItemDemoViewModel();
        }
    }

    public enum Fruit
    {
        Apple,
        Pear,
        Banana
    }

    public class EnumMenuItemDemoViewModel : INotifyPropertyChanged
    {
        private Fruit favouriteFruit;

        public Fruit FavouriteFruit
        {
            get { return favouriteFruit; }
            set
            {
                if (favouriteFruit != value)
                {
                    favouriteFruit = value;
                    RaisePropertyChanged("FavouriteFruit");
                }
            }
        }

        #region PropertyChanged Block

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}