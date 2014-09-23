// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window103.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window103.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for Window103.xaml
    /// </summary>
    public partial class Window103
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window103" /> class.
        /// </summary>
        public Window103()
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