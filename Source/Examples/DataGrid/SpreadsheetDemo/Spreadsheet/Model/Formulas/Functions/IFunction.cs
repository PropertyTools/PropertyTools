// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFunction.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a formula function, e.g. SUM.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions
{
    /// <summary>
    /// Represents a formula function, e.g. <c>SUM</c>.
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// Gets the function name, as used in formulas (lookup is case-insensitive).
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the minimum number of arguments.
        /// </summary>
        int MinArgumentCount { get; }

        /// <summary>
        /// Gets the maximum number of arguments, or -1 for unlimited.
        /// </summary>
        int MaxArgumentCount { get; }

        /// <summary>
        /// Invokes the function.
        /// </summary>
        CellValue Invoke(FunctionCallContext context);
    }
}
