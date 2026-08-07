// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormulaEvaluator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Computes the value of a parsed formula.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    using System;

    using SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions;

    /// <summary>
    /// Computes the value of a parsed formula by walking its syntax tree.
    /// </summary>
    public sealed class FormulaEvaluator : IFormulaEvaluator
    {
        private readonly FunctionRegistry functions;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaEvaluator" /> class.
        /// </summary>
        /// <param name="functions">
        /// The function registry, or <c>null</c> to use <see cref="FunctionRegistry.CreateDefault" />.
        /// </param>
        public FormulaEvaluator(FunctionRegistry functions = null)
        {
            this.functions = functions ?? FunctionRegistry.CreateDefault();
        }

        /// <summary>
        /// Gets the function registry used by this evaluator. Register additional functions here.
        /// </summary>
        public FunctionRegistry Functions => this.functions;

        /// <inheritdoc />
        public CellValue Evaluate(Formula formula, Sheet sheet)
        {
            return this.Evaluate(formula.Root, new SheetEvaluationContext(sheet));
        }

        /// <summary>
        /// Evaluates a syntax tree node against the given context.
        /// </summary>
        internal CellValue Evaluate(FormulaNode node, IEvaluationContext context)
        {
            switch (node)
            {
                case LiteralNode literal:
                    return literal.Value;
                case ReferenceNode reference:
                    return context.GetValue(reference.Address);
                case RangeNode range:
                    // A bare range used outside a function (e.g. "=A1:A3") behaves like its top-left cell.
                    return context.GetValue(range.Range.TopLeft);
                case UnaryOperatorNode unary:
                    return this.EvaluateUnary(unary, context);
                case BinaryOperatorNode binary:
                    return this.EvaluateBinary(binary, context);
                case FunctionNode function:
                    return this.EvaluateFunction(function, context);
                default:
                    return CellValue.FromError(CellError.Value);
            }
        }

        private CellValue EvaluateUnary(UnaryOperatorNode node, IEvaluationContext context)
        {
            var operand = this.Evaluate(node.Operand, context);
            if (operand.IsError)
            {
                return operand;
            }

            if (!FunctionHelpers.TryGetNumber(operand, out var number, out var error))
            {
                return error;
            }

            switch (node.Operator)
            {
                case UnaryOperator.Negate:
                    return NumberResult(-number);
                case UnaryOperator.Plus:
                    return NumberResult(number);
                case UnaryOperator.Percent:
                    return NumberResult(number / 100.0);
                default:
                    return CellValue.FromError(CellError.Value);
            }
        }

        private CellValue EvaluateBinary(BinaryOperatorNode node, IEvaluationContext context)
        {
            var left = this.Evaluate(node.Left, context);
            if (left.IsError)
            {
                return left;
            }

            var right = this.Evaluate(node.Right, context);
            if (right.IsError)
            {
                return right;
            }

            switch (node.Operator)
            {
                case BinaryOperator.Add:
                    return Arithmetic(left, right, (a, b) => a + b);
                case BinaryOperator.Subtract:
                    return Arithmetic(left, right, (a, b) => a - b);
                case BinaryOperator.Multiply:
                    return Arithmetic(left, right, (a, b) => a * b);
                case BinaryOperator.Divide:
                    return Divide(left, right);
                case BinaryOperator.Power:
                    return Arithmetic(left, right, Math.Pow);
                case BinaryOperator.Concat:
                    return Concat(left, right);
                case BinaryOperator.Equal:
                    return Compare(left, right, r => r == 0);
                case BinaryOperator.NotEqual:
                    return Compare(left, right, r => r != 0);
                case BinaryOperator.LessThan:
                    return Compare(left, right, r => r < 0);
                case BinaryOperator.LessThanOrEqual:
                    return Compare(left, right, r => r <= 0);
                case BinaryOperator.GreaterThan:
                    return Compare(left, right, r => r > 0);
                case BinaryOperator.GreaterThanOrEqual:
                    return Compare(left, right, r => r >= 0);
                default:
                    return CellValue.FromError(CellError.Value);
            }
        }

        private static CellValue Arithmetic(CellValue left, CellValue right, Func<double, double, double> operation)
        {
            if (!FunctionHelpers.TryGetNumber(left, out var a, out var error))
            {
                return error;
            }

            if (!FunctionHelpers.TryGetNumber(right, out var b, out error))
            {
                return error;
            }

            return NumberResult(operation(a, b));
        }

        private static CellValue Divide(CellValue left, CellValue right)
        {
            if (!FunctionHelpers.TryGetNumber(left, out var a, out var error))
            {
                return error;
            }

            if (!FunctionHelpers.TryGetNumber(right, out var b, out error))
            {
                return error;
            }

            return b == 0 ? CellValue.FromError(CellError.DivideByZero) : NumberResult(a / b);
        }

        private static CellValue Concat(CellValue left, CellValue right)
        {
            if (!FunctionHelpers.TryGetText(left, out var a, out var error))
            {
                return error;
            }

            if (!FunctionHelpers.TryGetText(right, out var b, out error))
            {
                return error;
            }

            return CellValue.FromText(a + b);
        }

        private static CellValue Compare(CellValue left, CellValue right, Func<int, bool> predicate)
        {
            return CellValue.FromBoolean(predicate(left.CompareTo(right)));
        }

        private CellValue EvaluateFunction(FunctionNode node, IEvaluationContext context)
        {
            if (!this.functions.TryGet(node.Name, out var function))
            {
                return CellValue.FromError(CellError.Name);
            }

            if (node.Arguments.Count < function.MinArgumentCount
                || (function.MaxArgumentCount >= 0 && node.Arguments.Count > function.MaxArgumentCount))
            {
                return CellValue.FromError(CellError.Value);
            }

            var callContext = new FunctionCallContext(node.Arguments, this, context);
            try
            {
                return function.Invoke(callContext);
            }
            catch (Exception)
            {
                // A function threw an unexpected CLR exception (e.g. overflow); surface it as a
                // formula error instead of crashing recalculation.
                return CellValue.FromError(CellError.Value);
            }
        }

        private static CellValue NumberResult(double value)
        {
            return double.IsNaN(value) || double.IsInfinity(value) ? CellValue.FromError(CellError.Number) : CellValue.FromNumber(value);
        }
    }
}
