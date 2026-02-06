// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicBackgroundExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for DynamicBackgroundExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for DynamicBackgroundExample.
    /// Demonstrates using BackgroundProperty to bind cell backgrounds to data properties.
    /// </summary>
    public partial class DynamicBackgroundExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicBackgroundExample" /> class.
        /// </summary>
        public DynamicBackgroundExample()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the close button click to dismiss the explanation panel.
        /// </summary>
        private void CloseExplanation(object sender, RoutedEventArgs e)
        {
            this.ExplanationPanel.Visibility = Visibility.Collapsed;
        }
    }
}
