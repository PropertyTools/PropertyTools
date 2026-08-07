// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetDataGrid.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   A DataGrid that plugs in an operator correctly reporting column insert/delete as unsupported.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet
{
    using PropertyTools.Wpf;

    /// <summary>
    /// A <see cref="DataGrid" /> bound to a <see cref="SheetGridAdapter" />.
    /// </summary>
    /// <remarks>
    /// <see cref="ListListOperator.CanInsertColumns" /> and <see cref="ListListOperator.CanDeleteColumns" />
    /// unconditionally return <c>true</c>, regardless of <see cref="DataGrid.CanInsert" />/
    /// <see cref="DataGrid.CanDelete" /> or whether the items source reports a fixed size (only row
    /// insert/delete respects those). Row/column insert and delete for a
    /// <see cref="Model.Sheet" /> - which would also need to fix up formula references - are not
    /// implemented yet, so this subclass plugs in an operator that correctly reports both as
    /// unsupported instead of throwing <see cref="System.NotSupportedException" /> from the adapter.
    /// </remarks>
    public class SpreadsheetDataGrid : DataGrid
    {
        /// <inheritdoc />
        protected override IDataGridOperator CreateOperator()
        {
            return this.ItemsSource is SheetGridAdapter ? new SpreadsheetDataGridOperator(this) : base.CreateOperator();
        }
    }
}
