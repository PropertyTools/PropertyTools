// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnaryOperatorNode.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   A formula AST node applying a unary operator to an operand.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// A formula AST node applying a unary operator to an operand, e.g. <c>-A1</c> or <c>50%</c>.
    /// </summary>
    internal sealed class UnaryOperatorNode : FormulaNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnaryOperatorNode" /> class.
        /// </summary>
        public UnaryOperatorNode(UnaryOperator op, FormulaNode operand)
        {
            this.Operator = op;
            this.Operand = operand;
        }

        /// <summary>
        /// Gets the operator.
        /// </summary>
        public UnaryOperator Operator { get; }

        /// <summary>
        /// Gets the operand.
        /// </summary>
        public FormulaNode Operand { get; }
    }
}
