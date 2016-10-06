// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WrapItemsOfMassExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for WrapItemsOfMassExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for WrapItemsOfMassExample.xaml
    /// </summary>
    public partial class WrapItemsOfMassExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrapItemsExample" /> class.
        /// </summary>
        public WrapItemsOfMassExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new[]
                                {
                                    11.0 * Mass.Kilogram, 12 * Mass.Kilogram, 13 * Mass.Kilogram, 21 * Mass.Kilogram,
                                    22 * Mass.Kilogram, 23 * Mass.Kilogram, 31 * Mass.Kilogram, 32 * Mass.Kilogram,
                                    33 * Mass.Kilogram
                                };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public Mass[] ItemsSource { get; set; }
    }
}