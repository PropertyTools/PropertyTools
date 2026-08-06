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
        /// The items source.
        /// </summary>
        private static readonly string[] itemsSource = { "Peugeot", "Volvo", "Tesla", "Renault", "Audi" };

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayOfStringExample" /> class.
        /// </summary>
        public ArrayOfStringExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public string[] ItemsSource => itemsSource;
    }
}
