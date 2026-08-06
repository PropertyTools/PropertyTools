// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RangeNode.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   A formula AST node referencing a range of cells.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// A formula AST node referencing a range of cells, e.g. <c>A1:B10</c>.
    /// </summary>
    internal sealed class RangeNode : FormulaNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeNode" /> class.
        /// </summary>
        public RangeNode(CellRange range)
        {
            this.Range = range;
        }

        /// <summary>
        /// Gets the referenced range.
        /// </summary>
        public CellRange Range { get; }
    }
}
