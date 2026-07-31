// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectCellsAndToggleCheckBoxExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Demonstrates selecting a row and toggling a boolean value through a check box cell.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    using PropertyTools;

    /// <summary>
    /// Demonstrates selecting a row and toggling a boolean value through a check box cell.
    /// </summary>
    public partial class SelectCellsAndToggleCheckBoxExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCellsAndToggleCheckBoxExample" /> class.
        /// </summary>
        public SelectCellsAndToggleCheckBoxExample()
        {
            this.ItemsSource = new ObservableCollection<RowModel>
            {
                new RowModel { Boolean = true },
                new RowModel { Boolean = false },
                new RowModel { Boolean = true }
            };

            this.InitializeComponent();
            this.DataContext = this;
        }

        public ObservableCollection<RowModel> ItemsSource { get; }

        public class RowModel : Observable
        {
            private bool boolean;

            public bool Boolean
            {
                get => this.boolean;
                set
                {
                    if (this.SetValue(ref this.boolean, value))
                    {
                        this.RaisePropertyChanged(nameof(this.String));
                    }
                }
            }

            public string String => this.boolean ? "true" : "false";
        }
    }
}
