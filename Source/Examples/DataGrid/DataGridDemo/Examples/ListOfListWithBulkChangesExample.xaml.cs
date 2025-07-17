// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfListWithBulkChangesExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfListWithBulkChangesExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using PropertyTools.Wpf;

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ListOfListWithBulkChangesExample.
    /// </summary>
    public partial class ListOfListWithBulkChangesExample : INotifyPropertyChanged
    {
        private static List<ObservableCollection<string>> itemsSource = [];
        private static readonly ObservableList<ObservableCollection<string>> observableItemsSource = new(itemsSource);
        // private static readonly ObservableCollection<ObservableCollection<string>> observableItemsSource = new();

        private int rowCount = 1000;

        public ListOfListWithBulkChangesExample()
        {
            this.InitializeComponent();

            this.CreateRowsCommand1 = new DelegateCommand(CreateRows1);
            this.CreateRowsCommand2 = new DelegateCommand(CreateRows2);
            this.CreateRowsCommand3 = new DelegateCommand(CreateRows3);

            this.DataContext = this;
        }

        public ICommand CreateRowsCommand1 { get; }
        public ICommand CreateRowsCommand2 { get; }
        public ICommand CreateRowsCommand3 { get; }

        public int RowCount
        {
            get => this.rowCount;
            set
            {
                this.rowCount = value;
                this.OnPropertyChanged(nameof(RowCount));
            }
        }

        public ICollection<ObservableCollection<string>> ObservableItemsSource
        {
            get => observableItemsSource;
        }

        private void CreateRows1()
        {
            observableItemsSource.BeginEdit();

            var itemsToAdd = new List<ObservableCollection<string>>();
            for (var i = 0; i < RowCount; i++)
            {
                observableItemsSource.Add(CreateRow(observableItemsSource.Count+1));
            }

            observableItemsSource.EndEdit();
        }

        private static ObservableCollection<string> CreateRow(int i) => new ObservableCollection<string> { $"Row {i + 1} - A", $"Row {i + 1} - B", $"Row {i + 1} - C" };

        private void CreateRows2()
        {
            var itemsToAdd = new List<ObservableCollection<string>>();
            for (var i = 0; i < RowCount; i++)
            {
                itemsToAdd.Add(CreateRow(observableItemsSource.Count + 1));
            }

            observableItemsSource.AddRange(itemsToAdd);
        }

        private void CreateRows3()
        {
            observableItemsSource.BeginEdit();

            var itemsToAdd = new List<ObservableCollection<string>>();
            for (var i = 0; i < RowCount; i++)
            {
                itemsSource.Add(CreateRow(observableItemsSource.Count + 1));
            }

            observableItemsSource.EndEdit();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}