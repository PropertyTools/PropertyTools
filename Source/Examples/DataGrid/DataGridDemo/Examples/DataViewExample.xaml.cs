// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataViewExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Demonstrates using a DataView as the ItemsSource for PropertyTools DataGrid.
//   Shows auto-column generation, row insert/delete, RowFilter and Sort.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.ComponentModel;
    using System.Data;
    using System.Windows.Input;

    using PropertyTools.Wpf;

    /// <summary>
    /// Demonstrates binding a <see cref="PropertyTools.Wpf.DataGrid"/> to a <see cref="DataView"/>.
    /// </summary>
    public partial class DataViewExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataViewExample"/> class.
        /// </summary>
        public DataViewExample()
        {
            this.InitializeComponent();
            this.DataContext = new DataViewExampleViewModel();
        }
    }

    /// <summary>
    /// ViewModel for the DataView example.
    /// </summary>
    public class DataViewExampleViewModel : INotifyPropertyChanged
    {
        private readonly DataView view;
        private string rowFilter = string.Empty;
        private string sort = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataViewExampleViewModel"/> class.
        /// </summary>
        public DataViewExampleViewModel()
        {
            var table = new DataTable("Products");
            table.Columns.Add(new DataColumn("Name", typeof(string)));
            table.Columns.Add(new DataColumn("Category", typeof(string)));
            table.Columns.Add(new DataColumn("Value", typeof(int)));
            table.Columns.Add(new DataColumn("InStock", typeof(bool)));

            table.Rows.Add("Apple", "A", 10, true);
            table.Rows.Add("Banana", "B", 25, true);
            table.Rows.Add("Cherry", "A", 15, false);
            table.Rows.Add("Date", "B", 40, true);
            table.Rows.Add("Elderberry", "A", 5, false);
            table.Rows.Add("Fig", "B", 30, true);
            table.Rows.Add("Grape", "A", 20, true);

            this.view = table.DefaultView;
            this.ClearCommand = new DelegateCommand(this.Clear);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Gets the <see cref="DataView"/> bound to the DataGrid.</summary>
        public DataView View => this.view;

        /// <summary>Gets or sets the DataView RowFilter expression.</summary>
        public string RowFilter
        {
            get => this.rowFilter;
            set
            {
                if (this.rowFilter == value) return;
                this.rowFilter = value;
                this.ApplyFilter();
                this.OnPropertyChanged(nameof(this.RowFilter));
            }
        }

        /// <summary>Gets or sets the DataView Sort expression.</summary>
        public string Sort
        {
            get => this.sort;
            set
            {
                if (this.sort == value) return;
                this.sort = value;
                this.ApplySort();
                this.OnPropertyChanged(nameof(this.Sort));
            }
        }

        /// <summary>Gets the command that clears the filter and sort.</summary>
        public ICommand ClearCommand { get; }

        private void ApplyFilter()
        {
            try { this.view.RowFilter = this.rowFilter; }
            catch { /* ignore invalid filter expressions */ }
        }

        private void ApplySort()
        {
            try { this.view.Sort = this.sort; }
            catch { /* ignore invalid sort expressions */ }
        }

        private void Clear()
        {
            this.RowFilter = string.Empty;
            this.Sort = string.Empty;
        }

        private void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
