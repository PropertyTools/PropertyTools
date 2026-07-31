// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOfBoolExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ListOfBoolExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for ListOfBoolExample.
    /// </summary>
    public partial class ListOfBoolExample
    {
        /// <summary>
        /// The items source
        /// </summary>
        private static readonly List<bool> itemsSource = new List<bool> { true, false, true, false, true };

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfBoolExample" /> class.
        /// </summary>
        public ListOfBoolExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IList<bool> ItemsSource => itemsSource;
    }
}
