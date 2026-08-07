// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateFunction.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   An IFunction implemented by a delegate, used for the built-in functions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions
{
    using System;

    /// <summary>
    /// An <see cref="IFunction" /> implemented by a delegate.
    /// </summary>
    internal sealed class DelegateFunction : IFunction
    {
        private readonly Func<FunctionCallContext, CellValue> invoke;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateFunction" /> class.
        /// </summary>
        public DelegateFunction(string name, int minArgumentCount, int maxArgumentCount, Func<FunctionCallContext, CellValue> invoke)
        {
            this.Name = name;
            this.MinArgumentCount = minArgumentCount;
            this.MaxArgumentCount = maxArgumentCount;
            this.invoke = invoke;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public int MinArgumentCount { get; }

        /// <inheritdoc />
        public int MaxArgumentCount { get; }

        /// <inheritdoc />
        public CellValue Invoke(FunctionCallContext context)
        {
            return this.invoke(context);
        }
    }
}
