// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfIntExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfIntExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for ListOfIntExample.
    /// </summary>
    public partial class ListOfIntExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfIntExample" /> class.
        /// </summary>
        public ListOfIntExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new List<int> { 3, 7, 9 };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<int> ItemsSource { get; set; }
    }
}