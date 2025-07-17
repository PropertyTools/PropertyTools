using PropertyTools.Wpf;

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DataGridDemo
{
    public partial class LargeCollectionExample
    {
        public LargeCollectionExample()
        {
            this.InitializeComponent();
        }
    }

    public class LargeCollectionViewModel
    {
        static LargeCollectionViewModel()
        {
            StaticItemsSource = new ObservableCollection<ExampleObject>();
            CreateObjects(StaticItemsSource);
        }

        public LargeCollectionViewModel()
        {
            this.AddCommand = new DelegateCommand(this.Add);
            this.ResetCommand = new DelegateCommand(this.Reset);
        }

        public static ObservableCollection<ExampleObject> StaticItemsSource { get; }

        public ObservableCollection<ExampleObject> ItemsSource => StaticItemsSource;

        public ICommand AddCommand { get; }
        public ICommand ResetCommand { get; }

        private static void CreateObjects(ObservableCollection<ExampleObject> list, int n = 128)
        {
            for (int i = 0; i < n; i++)
            {
                var o = ExampleObject.CreateRandom();
                o.Integer = list.Count + 1;
                list.Add(o);
            }
        }


        private void Add()
        {
            CreateObjects(this.ItemsSource, this.ItemsSource.Count);
        }

        private void Reset()
        {
            this.ItemsSource.Clear();
            CreateObjects(this.ItemsSource);
        }
    }
}