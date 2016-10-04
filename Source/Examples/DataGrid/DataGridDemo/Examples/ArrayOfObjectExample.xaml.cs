// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayOfObjectExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ArrayOfObjectExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;

    /// <summary>
    /// Interaction logic for ArrayOfObjectExample.xaml
    /// </summary>
    public partial class ArrayOfObjectExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayOfObjectExample" /> class.
        /// </summary>
        public ArrayOfObjectExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new object[] { 1, true, "Hello", Fruit.Apple };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public object[] ItemsSource { get; set; }
    }
}