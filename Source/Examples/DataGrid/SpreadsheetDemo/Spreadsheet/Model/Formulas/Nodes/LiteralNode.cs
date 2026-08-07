// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteralNode.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   A formula AST node holding a literal value.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// A formula AST node holding a literal value (number, string or boolean).
    /// </summary>
    internal sealed class LiteralNode : FormulaNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralNode" /> class.
        /// </summary>
        public LiteralNode(CellValue value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the literal value.
        /// </summary>
        public CellValue Value { get; }
    }
}
