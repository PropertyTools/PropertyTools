// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataViewOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents an operator for DataGrid when its ItemsSource is a DataView.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// Represents an operator for <see cref="DataGrid" /> when its ItemsSource is a <see cref="DataView" />.
    /// </summary>
    public class DataViewOperator : DataGridOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataViewOperator"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public DataViewOperator(DataGrid owner) : base(owner)
        {
        }

        /// <summary>
        /// Gets the <see cref="DataView" /> from the owner's ItemsSource.
        /// </summary>
        private DataView DataView => this.Owner.ItemsSource as DataView;

        /// <summary>
        /// Determines whether rows can be deleted.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if rows can be deleted; otherwise <c>false</c>.
        /// </returns>
        public override bool CanDeleteRows()
        {
            var dataView = this.DataView;
            return this.Owner.CanDelete && this.Owner.ItemsInRows && dataView != null && dataView.AllowDelete;
        }

        /// <summary>
        /// Determines whether rows can be inserted.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if rows can be inserted; otherwise <c>false</c>.
        /// </returns>
        public override bool CanInsertRows()
        {
            var dataView = this.DataView;
            return this.Owner.CanInsert && this.Owner.ItemsInRows && dataView != null && dataView.AllowNew;
        }

        /// <summary>
        /// Determines whether columns can be deleted.
        /// </summary>
        /// <returns>
        ///   <c>false</c> - column deletion is not supported for DataView.
        /// </returns>
        public override bool CanDeleteColumns()
        {
            return false;
        }

        /// <summary>
        /// Determines whether columns can be inserted.
        /// </summary>
        /// <returns>
        ///   <c>false</c> - column insertion is not supported for DataView.
        /// </returns>
        public override bool CanInsertColumns()
        {
            return false;
        }

        /// <summary>
        /// Gets the item in the specified cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The <see cref="DataRowView" /> at the row index.
        /// </returns>
        public override object GetItem(CellRef cell)
        {
            var dataView = this.DataView;
            if (dataView == null)
            {
                return null;
            }

            var index = this.Owner.ItemsInRows ? cell.Row : cell.Column;
            index = this.GetItemsSourceIndex(index);

            if (index < 0 || index >= dataView.Count)
            {
                return null;
            }

            return dataView[index];
        }

        /// <summary>
        /// Inserts a new row into the <see cref="DataView" />.
        /// </summary>
        /// <param name="index">The index (not used; new rows are always appended).</param>
        /// <returns>
        /// The index of the inserted row if insertion succeeded, <c>-1</c> otherwise.
        /// </returns>
        public override int InsertItem(int index)
        {
            var dataView = this.DataView;
            if (dataView == null || !dataView.AllowNew)
            {
                return -1;
            }

            try
            {
                var newRow = dataView.AddNew();
                newRow.EndEdit();
                return dataView.Count - 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Sets value to item in cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <param name="value">The value.</param>
        public override void SetValue(CellRef cell, object value)
        {
            var dataView = this.DataView;
            if (dataView == null)
            {
                return;
            }

            var index = this.Owner.ItemsInRows ? cell.Row : cell.Column;
            index = this.GetItemsSourceIndex(index);

            if (index < 0 || index >= dataView.Count)
            {
                return;
            }

            var pd = this.GetPropertyDefinition(cell);
            if (pd == null || string.IsNullOrEmpty(pd.PropertyName))
            {
                return;
            }

            dataView[index][pd.PropertyName] = value;
        }

        /// <summary>
        /// Gets the binding path for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The column name used as the binding path.
        /// </returns>
        public override string GetBindingPath(CellRef cell)
        {
            var pd = this.GetPropertyDefinition(cell);
            if (!string.IsNullOrEmpty(pd?.PropertyName))
            {
                return pd.PropertyName;
            }

            var columnIndex = this.Owner.ItemsInRows ? cell.Column : cell.Row;
            var dataView = this.DataView;
            if (dataView != null && columnIndex >= 0 && columnIndex < dataView.Table.Columns.Count)
            {
                return dataView.Table.Columns[columnIndex].ColumnName;
            }

            return $"[{columnIndex}]";
        }

        /// <summary>
        /// Updates the property definitions using the DataTable column descriptors.
        /// </summary>
        public override void UpdatePropertyDefinitions()
        {
            var dataView = this.DataView;
            if (dataView == null || dataView.Count == 0)
            {
                return;
            }

            // Get property descriptors from a DataRowView instance (DataRowView implements ICustomTypeDescriptor)
            var sampleRow = dataView[0];
            var properties = TypeDescriptor.GetProperties(sampleRow);

            foreach (var pd in this.Owner.PropertyDefinitions)
            {
                if (!string.IsNullOrEmpty(pd.PropertyName))
                {
                    var descriptor = properties[pd.PropertyName];
                    this.SetPropertiesFromDescriptor(pd, descriptor);
                }
            }
        }

        /// <summary>
        /// Deletes the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if the item was deleted; otherwise <c>false</c>.
        /// </returns>
        protected override bool DeleteItem(int index)
        {
            var dataView = this.DataView;
            if (dataView == null || !dataView.AllowDelete)
            {
                return false;
            }

            index = this.GetItemsSourceIndex(index);

            if (index < 0 || index >= dataView.Count)
            {
                return false;
            }

            try
            {
                dataView.Delete(index);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Generates column definitions from the <see cref="DataTable" /> columns.
        /// </summary>
        /// <param name="list">The items source (must be a <see cref="DataView" />).</param>
        /// <returns>A sequence of column definitions.</returns>
        protected override IEnumerable<ColumnDefinition> GenerateColumnDefinitions(IList list)
        {
            var dataView = list as DataView;
            if (dataView?.Table == null)
            {
                yield break;
            }

            foreach (DataColumn column in dataView.Table.Columns)
            {
                yield return new ColumnDefinition
                {
                    PropertyName = column.ColumnName,
                    Header = column.Caption ?? column.ColumnName,
                    HorizontalAlignment = this.DefaultHorizontalAlignment,
                    Width = this.DefaultColumnWidth
                };
            }
        }
    }
}
