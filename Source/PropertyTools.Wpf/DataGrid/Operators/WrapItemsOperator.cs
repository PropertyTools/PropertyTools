// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WrapItemsOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents an operator for DataGrid when its WrapItems property is true.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Represents an operator for <see cref="DataGrid" /> when its WrapItems property is true.
    /// </summary>
    public class WrapItemsOperator : ListOperator
    {
        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <returns>
        /// The number.
        /// </returns>
        public override int GetRowCount(DataGrid owner)
        {
            var m = owner.PropertyDefinitions.Count;
            var n = owner.ItemsSource.Count / m;
            return owner.ItemsInRows ? n : m;
        }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <returns>
        /// The number.
        /// </returns>
        public override int GetColumnCount(DataGrid owner)
        {
            var m = owner.PropertyDefinitions.Count;
            var n = owner.ItemsSource.Count / m;
            return owner.ItemsInRows ? m : n;
        }

        /// <summary>
        /// Gets the item index for the specified cell.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The get item index.
        /// </returns>
        protected override int GetItemIndex(DataGrid owner, CellRef cell)
        {
            return owner.ItemsInRows ? (cell.Row * owner.Columns) + cell.Column : (cell.Column * owner.Rows) + cell.Row;
        }

        /// <summary>
        /// Determines whether items can be sorted by the specified column/row index.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <param name="index">The column index if items are in rows, otherwise the row index.</param>
        /// <returns>
        ///   <c>true</c> if the items can be sorted; <c>false</c> otherwise.
        /// </returns>
        public override bool CanSort(DataGrid owner, int index)
        {
            return false;
        }
    }
}