// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WrapItemsExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for WrapItemsExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for WrapItemsExample.xaml
    /// </summary>
    public partial class WrapItemsExample
    {
        /// <summary>
        /// The static items source.
        /// </summary>
        private static readonly double[] itemsSource = { 11.0, 12, 13, 21, 22, 23, 31, 32, 33 };

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapItemsExample" /> class.
        /// </summary>
        public WrapItemsExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public double[] ItemsSource => itemsSource;
    }
}