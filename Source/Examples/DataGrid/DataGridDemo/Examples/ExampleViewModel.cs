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
            this.CellDefinitionFactory.RegisterValueConverter(typeof(Mass), new MassValueConverter());

            this.ClearCommand = new DelegateCommand(this.Clear);
            this.ResetCommand = new DelegateCommand(this.Reset);
        }

        public static ObservableCollection<ExampleObject> StaticItemsSource { get; }

        public ObservableCollection<ExampleObject> ItemsSource => StaticItemsSource;

        public CellDefinitionFactory CellDefinitionFactory { get; } = new CellDefinitionFactory();

        public ICommand ClearCommand { get; }

        public ICommand ResetCommand { get; }

        private static void CreateObjects(ICollection<ExampleObject> list, int n = 10)
        {
            for (int i = 0; i < n; i++)
            {
                list.Add(ExampleObject.CreateRandom());
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