using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ControlsDemo
{
    /// <summary>
    /// Interaction logic for RadioButtonListPage.xaml
    /// </summary>
    public partial class RadioButtonListPage : Page
    {
        private readonly RadioButtonListViewModel vm = new RadioButtonListViewModel();

        public RadioButtonListPage()
        {
            InitializeComponent();
            vm.SelectedFruit = Fruits.Orange;
            DataContext = vm;
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            vm.SelectedFruit = Fruits.Pear;
        }
    }

    public class RadioButtonListViewModel : INotifyPropertyChanged
    {
        private Fruits selectedFruit;

        public Fruits SelectedFruit
        {
            get { return selectedFruit; }
            set
            {
                selectedFruit = value;
                RaisePropertyChanged("SelectedFruit");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void RaisePropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    public enum Fruits
    {
        Apple,
        Pear,
        Orange
    } ;
}