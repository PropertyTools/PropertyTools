// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayOfStringExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ArrayOfStringExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for ArrayOfStringExample.
    /// </summary>
    public partial class ArrayOfStringExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayOfStringExample" /> class.
        /// </summary>
        public ArrayOfStringExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new[] { "Peugeot", "Volvo", "Tesla", "Renault", "Audi" };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public string[] ItemsSource { get; set; }
    }
}
