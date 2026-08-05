// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetDataGridOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   A ListListOperator that correctly reports column insert/delete as unsupported.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo.Spreadsheet
{
    using PropertyTools.Wpf;

    /// <summary>
    /// A <see cref="ListListOperator" /> for <see cref="SheetGridAdapter" /> that reports column
    /// insert/delete as unsupported (see <see cref="SpreadsheetDataGrid" /> for why the base
    /// behaviour is unsafe for this items source).
    /// </summary>
    internal sealed class SpreadsheetDataGridOperator : ListListOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetDataGridOperator" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public SpreadsheetDataGridOperator(DataGrid owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        public override bool CanInsertColumns()
        {
            return false;
        }

        /// <inheritdoc />
        public override bool CanDeleteColumns()
        {
            return false;
        }
    }
}
