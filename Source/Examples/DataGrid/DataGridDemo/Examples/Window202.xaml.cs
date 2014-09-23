// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window202.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window202.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for Window202.xaml
    /// </summary>
    public partial class Window202
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window202" /> class.
        /// </summary>
        public Window202()
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