// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGridOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies DataGrid functionality that depends on the type of items source.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Specifies DataGrid functionality that depends on the type of items source.
    /// </summary>
    public interface IDataGridOperator
    {
        /// <summary>
        /// Auto-generates the columns.
        /// </summary>
        void AutoGenerateColumns();

        /// <summary>
        /// Updates the property definitions.
        /// </summary>
        void UpdatePropertyDefinitions();

        /// <summary>
        /// Gets the type of the property in the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The type of the property.
        /// </returns>
        Type GetPropertyType(CellRef cell);

        /// <summary>
        /// Creates the cell descriptor for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>A cell descriptor.</returns>
        CellDescriptor CreateCellDescriptor(CellRef cell);
 
        /// <summary>
        /// Gets the binding path for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The binding path
        /// </returns>
        string GetBindingPath(CellRef cell);

        /// <summary>
        /// Gets the value in the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>The value</returns>
        object GetCellValue(CellRef cell);

        /// <summary>
        /// Converts the items source index to a collection view index.
        /// </summary>
        /// <param name="index">The index in the items source.</param>
        /// <returns>The index in the collection view</returns>
        int GetCollectionViewIndex(int index);

        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// The index of the inserted item if insertion is successful, <c>-1</c> otherwise.
        /// </returns>
        int InsertItem(int index);

        /// <summary>
        /// Gets the item in the specified cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The <see cref="object" />.
        /// </returns>
        object GetItem(CellRef cell);

        /// <summary>
        /// Tries to set cell value in the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the cell value was set.</returns>
        bool TrySetCellValue(CellRef cell, object value);

        /// <summary>
        /// Gets the data context for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>The context object.</returns>
        object GetDataContext(CellRef cell);

        /// <summary>
        /// Determines whether columns can be deleted.
        /// </summary>
        /// <returns><c>true</c> if columns can be deleted; otherwise <c>false</c>.</returns>
        bool CanDeleteColumns();

        /// <summary>
        /// Determines whether rows can be deleted.
        /// </summary>
        /// <returns><c>true</c> if rows can be deleted; otherwise <c>false</c>.</returns>
        bool CanDeleteRows();

        /// <summary>
        /// Determines whether columns can be inserted.
        /// </summary>
        /// <returns><c>true</c> if columns can be inserted; otherwise <c>false</c>.</returns>
        bool CanInsertColumns();

        /// <summary>
        /// Determines whether rows can be inserted.
        /// </summary>
        /// <returns><c>true</c> if rows can be inserted; otherwise <c>false</c>.</returns>
        bool CanInsertRows();

        /// <summary>
        /// Deletes columns at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="n">The number of columns to delete.</param>
        void DeleteColumns(int index, int n);

        /// <summary>
        /// Deletes rows at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="n">The number of rows to delete.</param>
        void DeleteRows(int index, int n);

        /// <summary>
        /// Inserts columns at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="n">The number of columns to insert.</param>
        void InsertColumns(int index, int n);

        /// <summary>
        /// Inserts rows at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="n">The number of rows to insert.</param>
        void InsertRows(int index, int n);

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <returns>The number.</returns>
        int GetRowCount();

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <returns>The number.</returns>
        int GetColumnCount();

        /// <summary>
        /// Determines whether items can be sorted by the specified column/row index.
        /// </summary>
        /// <param name="index">The column index if items are in rows, otherwise the row index.</param>
        /// <returns>
        ///   <c>true</c> if the items can be sorted; <c>false</c> otherwise.
        /// </returns>
        bool CanSort(int index);
    }
}