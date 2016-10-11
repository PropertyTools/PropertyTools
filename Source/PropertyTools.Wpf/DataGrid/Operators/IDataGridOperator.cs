// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGridOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies DataGrid functionality dependent on the type of items source.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Specifies DataGrid functionality dependent on the type of items source.
    /// </summary>
    public interface IDataGridOperator
    {
        /// <summary>
        /// Auto-generates the columns.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        void AutoGenerateColumns(DataGrid owner);

        /// <summary>
        /// Updates the property definitions.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        void UpdatePropertyDefinitions(DataGrid owner);

        /// <summary>
        /// Gets the type of the property in the specified cell.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>
        /// The type of the property.
        /// </returns>
        Type GetPropertyType(PropertyDefinition definition, CellRef cell, object currentValue);

        /// <summary>
        /// Gets the property descriptor.
        /// </summary>
        /// <param name="pd">The property definition.</param>
        /// <returns>The property descriptor.</returns>
        PropertyDescriptor GetPropertyDescriptor(PropertyDefinition pd);

        /// <summary>
        /// Gets the binding path for the specified cell.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The binding path
        /// </returns>
        string GetBindingPath(DataGrid owner, CellRef cell);

        /// <summary>
        /// Gets the value in the specified cell.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>The value</returns>
        object GetCellValue(DataGrid owner, CellRef cell);

        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if insertion is successful, <c>false</c> otherwise.
        /// </returns>
        bool InsertItem(DataGrid owner, int index);

        /// <summary>
        /// Gets the item in the specified cell.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The <see cref="object" />.
        /// </returns>
        object GetItem(DataGrid owner, CellRef cell);

        /// <summary>
        /// Sets value of the specified cell to the specified value.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <param name="cell">The cell to change.</param>
        /// <param name="value">The value.</param>
        void SetValue(DataGrid owner, CellRef cell, object value);

        /// <summary>
        /// Tries to set cell value in the specified cell.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the cell value was set.</returns>
        bool TrySetCellValue(DataGrid owner, CellRef cell, object value);

        /// <summary>
        /// Gets the data context for the specified cell.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>The context object.</returns>
        object GetDataContext(DataGrid owner, CellRef cell);

        /// <summary>
        /// Determines whether columns can be deleted.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <returns><c>true</c> if columns can be deleted; otherwise <c>false</c>.</returns>
        bool CanDeleteColumns(DataGrid owner);

        /// <summary>
        /// Determines whether rows can be deleted.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <returns><c>true</c> if rows can be deleted; otherwise <c>false</c>.</returns>
        bool CanDeleteRows(DataGrid owner);

        /// <summary>
        /// Determines whether columns can be inserted.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <returns><c>true</c> if columns can be inserted; otherwise <c>false</c>.</returns>
        bool CanInsertColumns(DataGrid owner);

        /// <summary>
        /// Determines whether rows can be inserted.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <returns><c>true</c> if rows can be inserted; otherwise <c>false</c>.</returns>
        bool CanInsertRows(DataGrid owner);

        /// <summary>
        /// Deletes the item at the specified index.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        /// <param name="index">The index.</param>
        /// <returns><c>true</c> if rows can be inserted; otherwise <c>false</c>.</returns>
        bool DeleteItem(DataGrid owner, int index);

        /// <summary>
        /// Deletes the columns.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        void DeleteColumns(DataGrid owner);

        /// <summary>
        /// Deletes the rows.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        void DeleteRows(DataGrid owner);

        /// <summary>
        /// Inserts the columns.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        void InsertColumns(DataGrid owner);

        /// <summary>
        /// Inserts the rows.
        /// </summary>
        /// <param name="owner">The data grid.</param>
        void InsertRows(DataGrid owner);
    }
}