// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window102.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window102.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for Window102.xaml
    /// </summary>
    public partial class Window102
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window102" /> class.
        /// </summary>
        public Window102()
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