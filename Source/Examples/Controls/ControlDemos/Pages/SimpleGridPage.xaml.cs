using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ControlsDemo
{
    /// <summary>
    /// Interaction logic for SimpleGridPage.xaml
    /// </summary>
    public partial class SimpleGridPage : Page
    {
        private readonly SimpleGridViewModel vm = new SimpleGridViewModel();

        public SimpleGridPage()
        {
            InitializeComponent();
            DataContext = vm;
        }
    }

    public class SimpleGridViewModel : INotifyPropertyChanged
    {
        public Collection<Item> Items { get; set; }

        public SimpleGridViewModel()
        {
            Items = new Collection<Item>();
            for (int i = 0; i < 10; i++)
                Items.Add(new Item() { String = "String" + i, Double = i * 0.1, Int = i * 10 });
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

    public class Item
    {
        public string String { get; set; }
        public double Double { get; set; }
        public int Int { get; set; }
        public bool Bool { get; set; }
        public Fruit Enum { get; set; }
    }
}