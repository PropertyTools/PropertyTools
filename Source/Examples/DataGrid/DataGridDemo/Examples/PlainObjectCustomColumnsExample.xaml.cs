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
    public partial class PlainObjectCustomColumnsExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlainObjectCustomColumnsExample" /> class.
        /// </summary>
        public PlainObjectCustomColumnsExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new List<PlainOldObject>();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<PlainOldObject> ItemsSource { get; set; }
    }
}