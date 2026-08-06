// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEvaluationContext.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides the cell values a formula evaluation reads.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides the cell values a formula evaluation reads.
    /// </summary>
    internal interface IEvaluationContext
    {
        /// <summary>
        /// Gets the value of a single cell.
        /// </summary>
        CellValue GetValue(CellAddress address);

        /// <summary>
        /// Enumerates the values of every cell in a range, in row-major order.
        /// </summary>
        IEnumerable<CellValue> GetValues(CellRange range);
    }
}
