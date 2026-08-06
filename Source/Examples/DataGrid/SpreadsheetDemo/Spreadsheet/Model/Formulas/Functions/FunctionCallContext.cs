// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionCallContext.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides a function with access to its arguments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a function with access to its arguments.
    /// </summary>
    public sealed class FunctionCallContext
    {
        private readonly IReadOnlyList<FormulaNode> arguments;
        private readonly FormulaEvaluator evaluator;
        private readonly IEvaluationContext evaluationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionCallContext" /> class.
        /// </summary>
        internal FunctionCallContext(
            IReadOnlyList<FormulaNode> arguments,
            FormulaEvaluator evaluator,
            IEvaluationContext evaluationContext)
        {
            this.arguments = arguments;
            this.evaluator = evaluator;
            this.evaluationContext = evaluationContext;
        }

        /// <summary>
        /// Gets the number of arguments the function was called with.
        /// </summary>
        public int ArgumentCount => this.arguments.Count;

        /// <summary>
        /// Evaluates the argument at the given index to a single value. If the argument is a range,
        /// this returns the range's top-left cell; functions that should operate on every cell in a
        /// range argument should use <see cref="GetArgumentValues" /> instead.
        /// </summary>
        public CellValue GetArgument(int index)
        {
            return this.evaluator.Evaluate(this.arguments[index], this.evaluationContext);
        }

        /// <summary>
        /// Evaluates the argument at the given index, expanding it to every cell it covers: every
        /// cell in a range, or the single evaluated value otherwise.
        /// </summary>
        public IEnumerable<CellValue> GetArgumentValues(int index)
        {
            var node = this.arguments[index];
            if (node is RangeNode rangeNode)
            {
                foreach (var value in this.evaluationContext.GetValues(rangeNode.Range))
                {
                    yield return value;
                }
            }
            else
            {
                yield return this.evaluator.Evaluate(node, this.evaluationContext);
            }
        }

        /// <summary>
        /// Evaluates every argument, expanding ranges, in order. Used by functions such as
        /// <c>SUM</c>/<c>AVERAGE</c> that operate over an arbitrary mix of scalars and ranges.
        /// </summary>
        public IEnumerable<CellValue> GetAllArgumentValues()
        {
            for (var i = 0; i < this.arguments.Count; i++)
            {
                foreach (var value in this.GetArgumentValues(i))
                {
                    yield return value;
                }
            }
        }
    }
}
