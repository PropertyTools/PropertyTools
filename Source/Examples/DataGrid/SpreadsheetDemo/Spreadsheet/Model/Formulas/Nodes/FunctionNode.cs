// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionNode.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   A formula AST node representing a function call.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    using System.Collections.Generic;

    /// <summary>
    /// A formula AST node representing a function call, e.g. <c>SUM(A1:A10, 5)</c>.
    /// </summary>
    internal sealed class FunctionNode : FormulaNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionNode" /> class.
        /// </summary>
        public FunctionNode(string name, IReadOnlyList<FormulaNode> arguments)
        {
            this.Name = name;
            this.Arguments = arguments;
        }

        /// <summary>
        /// Gets the function name, as written in the formula (case as typed; lookup is case-insensitive).
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the argument expressions.
        /// </summary>
        public IReadOnlyList<FormulaNode> Arguments { get; }
    }
}
