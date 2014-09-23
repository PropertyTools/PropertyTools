// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window303.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for Window303.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// Interaction logic for Window303.xaml
    /// </summary>
    public partial class Window303
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window303" /> class.
        /// </summary>
        public Window303()
        {
            this.InitializeComponent();
            this.ItemsSource = new List<Vector3D> { new Vector3D(1, 0, 0), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1) };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<Vector3D> ItemsSource { get; set; }
    }
}