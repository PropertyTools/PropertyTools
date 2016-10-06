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
        /// The static items.
        /// </summary>
        internal static readonly double[] StaticItems = { 3.0, 7, 9 };

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayOfDoubleExample" /> class.
        /// </summary>
        public ArrayOfDoubleExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public double[] Items => StaticItems;
    }
}