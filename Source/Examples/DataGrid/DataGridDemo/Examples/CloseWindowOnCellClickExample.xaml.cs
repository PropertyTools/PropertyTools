// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloseWindowOnCellClickExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for CloseWindowOnCellClickExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for CloseWindowOnCellClickExample.
    /// Demonstrates that clicking a cell no longer throws an <see cref="System.InvalidOperationException" />
    /// when the click handler closes the window (disconnecting the <see cref="PropertyTools.Wpf.DataGrid" />
    /// from its <see cref="System.Windows.PresentationSource" />) while the mouse down event is being processed (#510).
    /// </summary>
    public partial class CloseWindowOnCellClickExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloseWindowOnCellClickExample" /> class.
        /// </summary>
        public CloseWindowOnCellClickExample()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the <see cref="UIElement.PreviewMouseLeftButtonDown" /> event of the data grid by closing the window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
