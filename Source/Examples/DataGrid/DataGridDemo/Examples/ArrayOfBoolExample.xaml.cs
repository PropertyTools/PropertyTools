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
        /// Initializes a new instance of the <see cref="ArrayOfBoolExample" /> class.
        /// </summary>
        public ArrayOfBoolExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new[] { true, false, true, false, true };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public bool[] ItemsSource { get; set; }
    }
}
