// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClipboardSeparatorExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ClipboardSeparatorExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for ClipboardSeparatorExample.
    /// Demonstrates setting a custom <see cref="PropertyTools.Wpf.DataGrid.ClipboardSeparator"/>
    /// so that <c>Ctrl+Alt+C</c> copies data using a comma (<c>,</c>) instead of the
    /// default culture list separator.
    /// </summary>
    public partial class ClipboardSeparatorExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardSeparatorExample" /> class.
        /// </summary>
        public ClipboardSeparatorExample()
        {
            this.InitializeComponent();
            this.DataContext = new ExampleViewModel();
        }
    }
}
