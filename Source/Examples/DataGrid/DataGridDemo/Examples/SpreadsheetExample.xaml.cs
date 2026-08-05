// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for SpreadsheetExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using DataGridDemo.Spreadsheet;

    /// <summary>
    /// Interaction logic for SpreadsheetExample.
    /// </summary>
    public partial class SpreadsheetExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetExample" /> class.
        /// </summary>
        public SpreadsheetExample()
        {
            this.InitializeComponent();
            this.DataContext = new SpreadsheetViewModel();
        }
    }
}
