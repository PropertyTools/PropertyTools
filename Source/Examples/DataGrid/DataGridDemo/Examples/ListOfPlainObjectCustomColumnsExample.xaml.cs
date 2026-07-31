// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlainObjectCustomColumnsExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for PlainObjectCustomColumnsExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for PlainObjectCustomColumnsExample.
    /// </summary>
    public partial class ListOfPlainObjectCustomColumnsExample
    {
        /// <summary>
        /// The static items source.
        /// </summary>
        private static readonly List<PlainOldObject> itemsSource = new List<PlainOldObject>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfPlainObjectCustomColumnsExample" /> class.
        /// </summary>
        public ListOfPlainObjectCustomColumnsExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<PlainOldObject> ItemsSource => itemsSource;
    }
}