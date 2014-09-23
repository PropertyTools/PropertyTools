// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window101.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window101.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for Window101.xaml
    /// </summary>
    public partial class Window101
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window101" /> class.
        /// </summary>
        public Window101()
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
                return Window1.StaticItemsSource;
            }
        }
    }
}