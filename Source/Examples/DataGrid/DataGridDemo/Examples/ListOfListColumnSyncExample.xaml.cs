// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfListColumnSyncExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfListColumnSyncExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using PropertyTools.Wpf;

    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ListOfListColumnSyncExample.
    /// Demonstrates that the number of columns in the <see cref="DataGrid" /> updates
    /// when items are added to or removed from an inner row collection.
    /// </summary>
    public partial class ListOfListColumnSyncExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfListColumnSyncExample" /> class.
        /// </summary>
        public ListOfListColumnSyncExample()
        {
            this.InitializeComponent();

            this.AddItemToFirstRowCommand = new DelegateCommand(this.AddItemToFirstRow);
            this.RemoveItemFromFirstRowCommand = new DelegateCommand(this.RemoveItemFromFirstRow);

            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ObservableCollection<int>> ItemsSource { get; } =
            new ObservableCollection<ObservableCollection<int>>
            {
                new ObservableCollection<int> { 1, 2, 3 },
                new ObservableCollection<int> { 4, 5, 6 },
            };

        /// <summary>
        /// Gets the command that adds an item to the first row.
        /// </summary>
        public ICommand AddItemToFirstRowCommand { get; }

        /// <summary>
        /// Gets the command that removes the last item from the first row.
        /// </summary>
        public ICommand RemoveItemFromFirstRowCommand { get; }

        /// <summary>
        /// Adds an item to the first row, increasing the number of columns in the grid.
        /// </summary>
        private void AddItemToFirstRow()
        {
            var firstRow = this.ItemsSource[0];
            firstRow.Add(firstRow.Count + 1);
        }

        /// <summary>
        /// Removes the last item from the first row, decreasing the number of columns in the grid.
        /// </summary>
        private void RemoveItemFromFirstRow()
        {
            var firstRow = this.ItemsSource[0];
            if (firstRow.Count > 0)
            {
                firstRow.RemoveAt(firstRow.Count - 1);
            }
        }
    }
}
