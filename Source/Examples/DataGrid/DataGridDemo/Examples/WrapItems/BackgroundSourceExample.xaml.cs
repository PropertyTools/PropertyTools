// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundSourceExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for BackgroundSourceExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for BackgroundSourceExample.
    /// </summary>
    public partial class BackgroundSourceExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundSourceExample" /> class.
        /// </summary>
        public BackgroundSourceExample()
        {
            this.InitializeComponent();
            this.ItemsSource = new[] { 11d, 0, 0, 0, 22, 0, 0, 0, 33 };
            this.BackgroundSource = new[]
                                  {
                                      Brushes.LightBlue, Brushes.LightGray, Brushes.LightGray, Brushes.LightGray,
                                      Brushes.LightBlue, Brushes.LightGray, Brushes.LightGray, Brushes.LightGray,
                                      Brushes.LightBlue,
                                  };
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public double[] ItemsSource { get; }

        public Brush[] BackgroundSource { get; }
    }
}