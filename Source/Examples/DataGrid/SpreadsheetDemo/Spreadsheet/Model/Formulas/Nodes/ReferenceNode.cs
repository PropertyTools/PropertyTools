// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceNode.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   A formula AST node referencing a single cell.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// A formula AST node referencing a single cell, e.g. <c>A1</c>.
    /// </summary>
    internal sealed class ReferenceNode : FormulaNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceNode" /> class.
        /// </summary>
        public ReferenceNode(CellAddress address)
        {
            this.Address = address;
        }

        /// <summary>
        /// Gets the referenced cell address.
        /// </summary>
        public CellAddress Address { get; }
    }
}
