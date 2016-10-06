namespace DataGridDemo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System.Windows.Media;

    using PropertyTools.Wpf;

    public class ExampleViewModel
    {
        static ExampleViewModel()
        {
            StaticItemsSource = new ObservableCollection<ExampleObject>();
            CreateObjects(StaticItemsSource);
        }

        public ExampleViewModel()
        {
            this.ClearCommand = new DelegateCommand(this.Clear);
            this.ResetCommand = new DelegateCommand(this.Reset);
        }

        public static ObservableCollection<ExampleObject> StaticItemsSource { get; }

        public ObservableCollection<ExampleObject> ItemsSource => StaticItemsSource;

        public ICommand ClearCommand { get; }

        public ICommand ResetCommand { get; }

        private static void CreateObjects(ICollection<ExampleObject> list, int n = 50)
        {
            for (int i = 0; i < n; i++)
            {
                list.Add(
                    new ExampleObject
                    {
                        Boolean = true,
                        DateTime = DateTime.Now,
                        Color = Colors.Blue,
                        Number = Math.PI,
                        Fruit = Fruit.Apple,
                        Integer = 7,
                        Selector = null,
                        String = "Hello"
                    });
                list.Add(
                    new ExampleObject
                    {
                        Boolean = false,
                        DateTime = DateTime.Now.AddDays(-1),
                        Color = Colors.Gold,
                        Number = Math.E,
                        Fruit = Fruit.Pear,
                        Integer = -1,
                        Selector = null,
                        String = "World"
                    });
            }
        }


        private void Reset()
        {
            this.ItemsSource.Clear();
            CreateObjects(this.ItemsSource);
        }

        private void Clear()
        {
            this.ItemsSource.Clear();
        }
    }
}