// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayOfDoubleExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ArrayOfDoubleExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for ArrayOfDoubleExample.xaml
    /// </summary>
    public partial class ArrayOfDoubleExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayOfDoubleExample" /> class.
        /// </summary>
        public ArrayOfDoubleExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new[] { 3.0, 7, 9 };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public double[] ItemsSource { get; set; }
    }
}