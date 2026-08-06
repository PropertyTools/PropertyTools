// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetDataGridOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   A ListListOperator that correctly reports column insert/delete as unsupported.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet
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
        /// <remarks>
        /// <see cref="DataGridOperator.DefaultColumnWidth" /> is a separate property from
        /// <see cref="DataGrid.DefaultColumnWidth" /> — nothing in the base library ever copies the
        /// owner's value into it, so it stays at its own default (star-sized) forever, and
        /// <see cref="ListListOperator.GenerateColumnDefinitions" /> reads the operator's copy. Without
        /// this assignment, setting <see cref="DataGrid.DefaultColumnWidth" /> on the grid has no
        /// effect at all for a list-of-lists items source.
        /// </remarks>
        public SpreadsheetDataGridOperator(DataGrid owner)
            : base(owner)
        {
            this.DefaultColumnWidth = owner.DefaultColumnWidth;
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
