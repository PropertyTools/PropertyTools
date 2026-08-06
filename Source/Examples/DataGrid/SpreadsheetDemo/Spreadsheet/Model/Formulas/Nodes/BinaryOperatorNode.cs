// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryOperatorNode.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   A formula AST node applying a binary operator to two operands.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// A formula AST node applying a binary operator to two operands, e.g. <c>A1+B1</c>.
    /// </summary>
    internal sealed class BinaryOperatorNode : FormulaNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode" /> class.
        /// </summary>
        public BinaryOperatorNode(BinaryOperator op, FormulaNode left, FormulaNode right)
        {
            this.Operator = op;
            this.Left = left;
            this.Right = right;
        }

        /// <summary>
        /// Gets the operator.
        /// </summary>
        public BinaryOperator Operator { get; }

        /// <summary>
        /// Gets the left operand.
        /// </summary>
        public FormulaNode Left { get; }

        /// <summary>
        /// Gets the right operand.
        /// </summary>
        public FormulaNode Right { get; }
    }
}
