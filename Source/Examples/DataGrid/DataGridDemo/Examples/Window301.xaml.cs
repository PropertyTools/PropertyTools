// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window301.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window301.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for Window301.xaml
    /// </summary>
    public partial class Window301
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window301" /> class.
        /// </summary>
        public Window301()
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