// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   View model for the SpreadsheetExample window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo.Spreadsheet
{
    using System.Collections.Generic;

    using DataGridDemo.Spreadsheet.Model;

    using PropertyTools.Wpf;

    /// <summary>
    /// View model for the <see cref="SpreadsheetExample" /> window.
    /// </summary>
    public class SpreadsheetViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetViewModel" /> class with a new,
        /// empty 100x26 sheet.
        /// </summary>
        public SpreadsheetViewModel()
            : this(new Sheet("Sheet1", 100, 26))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetViewModel" /> class.
        /// </summary>
        /// <param name="sheet">The sheet to display.</param>
        public SpreadsheetViewModel(Sheet sheet)
        {
            this.Sheet = sheet;
            this.Rows = new SheetGridAdapter(sheet);
            this.RowHeaders = BuildRowHeaders(sheet.RowCount);
            this.ColumnHeaders = BuildColumnHeaders(sheet.ColumnCount);
            this.ControlFactory = new SpreadsheetControlFactory();
        }

        /// <summary>
        /// Gets the sheet being displayed.
        /// </summary>
        public Sheet Sheet { get; }

        /// <summary>
        /// Gets the grid's items source: one <see cref="SheetRowAdapter" /> per row of <see cref="Sheet" />.
        /// </summary>
        public SheetGridAdapter Rows { get; }

        /// <summary>
        /// Gets the row header labels ("1", "2", ...).
        /// </summary>
        public IList<string> RowHeaders { get; }

        /// <summary>
        /// Gets the column header labels ("A", "B", ..., "Z", "AA", ...).
        /// </summary>
        public IList<string> ColumnHeaders { get; }

        /// <summary>
        /// Gets the control factory that renders cells.
        /// </summary>
        public IDataGridControlFactory ControlFactory { get; }

        private static IList<string> BuildRowHeaders(int rowCount)
        {
            var headers = new List<string>(rowCount);
            for (var row = 0; row < rowCount; row++)
            {
                headers.Add((row + 1).ToString());
            }

            return headers;
        }

        private static IList<string> BuildColumnHeaders(int columnCount)
        {
            var headers = new List<string>(columnCount);
            for (var column = 0; column < columnCount; column++)
            {
                headers.Add(CellAddress.ToColumnName(column));
            }

            return headers;
        }
    }
}
