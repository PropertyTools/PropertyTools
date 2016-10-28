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

        private static void CreateObjects(ICollection<ExampleObject> list, int n = 10)
        {
            var r = new Random(0);
            for (int i = 0; i < n; i++)
            {
                list.Add(
                    new ExampleObject
                    {
                        Boolean = r.Next(2) == 0,
                        DateTime = DateTime.Now.AddDays(r.Next(365)),
                        Color = Color.FromArgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255)),
                        Number = r.Next(),
                        Fruit = (Fruit)r.Next(5),
                        Integer = r.Next(1024),
                        Selector = null,
                        String = StandardCollections.GenerateName()
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