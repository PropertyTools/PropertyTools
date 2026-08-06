// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayOfBoolExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ArrayOfBoolExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for ArrayOfBoolExample.
    /// </summary>
    public partial class ArrayOfBoolExample
    {
        /// <summary>
        /// The items source.
        /// </summary>
        private static readonly bool[] itemsSource = { true, false, true, false, true };

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayOfBoolExample" /> class.
        /// </summary>
        public ArrayOfBoolExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public bool[] ItemsSource => itemsSource;
    }
}
