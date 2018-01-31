// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScrollViewerExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ScrollViewerExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for ScrollViewerExample.
    /// </summary>
    public partial class ScrollViewerExample
    {
        /// <summary>
        /// The items source
        /// </summary>
        private static readonly List<int> itemsSource = new List<int> { 3, 7, 9 };

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOfIntExample" /> class.
        /// </summary>
        public ScrollViewerExample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IList<int> ItemsSource => itemsSource;
    }
}