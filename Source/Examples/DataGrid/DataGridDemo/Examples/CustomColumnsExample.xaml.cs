// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomColumnsExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for CustomColumnsExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for CustomColumnsExample.xaml
    /// </summary>
    public partial class CustomColumnsExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomColumnsExample" /> class.
        /// </summary>
        public CustomColumnsExample()
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