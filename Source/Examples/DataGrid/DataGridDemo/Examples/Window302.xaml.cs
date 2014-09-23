// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window302.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window302.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for Window302.xaml
    /// </summary>
    public partial class Window302
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window302" /> class.
        /// </summary>
        public Window302()
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