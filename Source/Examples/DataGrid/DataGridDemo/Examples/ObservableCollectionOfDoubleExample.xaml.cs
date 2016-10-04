// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableCollectionOfDoubleExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ObservableCollectionOfDoubleExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for ObservableCollectionOfDoubleExample.
    /// </summary>
    public partial class ObservableCollectionOfDoubleExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionOfDoubleExample" /> class.
        /// </summary>
        public ObservableCollectionOfDoubleExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new List<double> { 3, 7, 9 };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<double> ItemsSource { get; set; }
    }
}