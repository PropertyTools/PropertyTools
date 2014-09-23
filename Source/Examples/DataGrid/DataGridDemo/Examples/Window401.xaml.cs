// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window401.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window401.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for Window401.xaml
    /// </summary>
    public partial class Window401
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window401" /> class.
        /// </summary>
        public Window401()
        {
            this.InitializeComponent();
            this.ItemsSource = new[] { 3.0, 7, 9 };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public double[] ItemsSource { get; set; }
    }
}