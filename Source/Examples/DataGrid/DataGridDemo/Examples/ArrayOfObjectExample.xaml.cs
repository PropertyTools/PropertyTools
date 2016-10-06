// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayOfObjectExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ArrayOfObjectExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for ArrayOfObjectExample.xaml
    /// </summary>
    public partial class ArrayOfObjectExample
    {
        /// <summary>
        /// The static items.
        /// </summary>
        internal static readonly object[] StaticItems = { 1, Math.PI, true, (bool?)true, "Hello", null, Fruit.Apple, (Fruit?)Fruit.Banana };

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayOfObjectExample" /> class.
        /// </summary>
        public ArrayOfObjectExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public object[] Items => StaticItems;
    }
}