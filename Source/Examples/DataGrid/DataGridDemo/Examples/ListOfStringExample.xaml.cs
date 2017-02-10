// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfStringExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfStringExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for ListOfStringExample.
    /// </summary>
    public partial class ListOfStringExample
    {
        /// <summary>
        /// The static items source
        /// </summary>
        private static readonly List<string> itemsSource = new List<string> { "Peugeot", "Volvo", "Tesla", "Renault", "Audi" };

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfStringExample" /> class.
        /// </summary>
        public ListOfStringExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IList<string> ItemsSource => itemsSource;
    }
}