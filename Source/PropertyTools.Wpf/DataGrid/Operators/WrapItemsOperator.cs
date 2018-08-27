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
        /// Initializes a new instance of the <see cref="WrapItemsOperator"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public WrapItemsOperator(DataGrid owner) : base(owner)
        {
        }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <returns>
        /// The number.
        /// </returns>
        public override int GetRowCount()
        {
            var m = this.Owner.PropertyDefinitions.Count;
            var n = this.Owner.ItemsSource.Count / m;
            return this.Owner.ItemsInRows ? n : m;
        }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <returns>
        /// The number.
        /// </returns>
        public override int GetColumnCount()
        {
            var m = this.Owner.PropertyDefinitions.Count;
            var n = this.Owner.ItemsSource.Count / m;
            return this.Owner.ItemsInRows ? m : n;
        }

        /// <summary>
        /// Determines whether items can be sorted by the specified column/row index.
        /// </summary>
        /// <param name="index">The column index if items are in rows, otherwise the row index.</param>
        /// <returns>
        ///   <c>true</c> if the items can be sorted; <c>false</c> otherwise.
        /// </returns>
        public override bool CanSort(int index)
        {
            return false;
        }

        /// <summary>
        /// Gets the item index for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The get item index.
        /// </returns>
        protected override int GetItemIndex(CellRef cell)
        {
            return this.Owner.ItemsInRows ? (cell.Row * this.Owner.Columns) + cell.Column : (cell.Column * this.Owner.Rows) + cell.Row;
        }
    }
}