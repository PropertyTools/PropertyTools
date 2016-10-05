// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WrapItemsIsEnabledExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for WrapItemsIsEnabledExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for WrapItemsIsEnabledExample.
    /// </summary>
    public partial class WrapItemsIsEnabledExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrapItemsIsEnabledExample" /> class.
        /// </summary>
        public WrapItemsIsEnabledExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new[] { 11d, 0, 0, 0, 22, 0, 0, 0, 33 };
            this.IsItemEnabled = new[] { true, false, false, false, true, false, false, false, true };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public double[] ItemsSource { get; }

        public bool[] IsItemEnabled { get; }
    }
}