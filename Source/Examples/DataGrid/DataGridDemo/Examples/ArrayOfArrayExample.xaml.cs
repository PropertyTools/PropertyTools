// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayOfArrayExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ArrayOfArrayExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for ArrayOfArrayExample.
    /// </summary>
    public partial class ArrayOfArrayExample
    {
        /// <summary>
        /// The items source
        /// </summary>
        private static readonly int[][] itemsSource = { new[] { 1, 2, 3 }, new[] { 10, 20, 30 } };

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayOfArrayExample" /> class.
        /// </summary>
        public ArrayOfArrayExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public int[][] ItemsSource => itemsSource;
    }
}