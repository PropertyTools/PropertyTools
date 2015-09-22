// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window511.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window511.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;
    using System.Windows;

    /// <summary>
    /// Interaction logic for Window511.xaml
    /// </summary>
    public partial class Window511 : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window511" /> class.
        /// </summary>
        public Window511()
        {
            this.InitializeComponent();

            this.DataContext = this;
            this.ItemsSource = new ObservableCollection<Mass>
                                    {
                                        100 * Mass.Kilogram,
                                        101 * Mass.Kilogram,
                                        102 * Mass.Kilogram
                                    };
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        public ObservableCollection<Mass> ItemsSource { get; private set; }
    }
}
