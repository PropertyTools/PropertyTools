// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsInColumnsExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ItemsInColumnsExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ItemsInColumnsExample.
    /// </summary>
    public partial class ItemsInColumnsExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsInColumnsExample" /> class.
        /// </summary>
        public ItemsInColumnsExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<ExampleObject> ItemsSource
        {
            get
            {
                return WpfDataGridExample.StaticItemsSource;
            }
        }
    }
}