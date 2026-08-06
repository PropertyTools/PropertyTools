// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SheetEvaluationContext.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   An evaluation context backed by a Sheet.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    using System.Collections.Generic;

    /// <summary>
    /// An <see cref="IEvaluationContext" /> backed by a <see cref="Sheet" />.
    /// </summary>
    internal sealed class SheetEvaluationContext : IEvaluationContext
    {
        private readonly Sheet sheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="SheetEvaluationContext" /> class.
        /// </summary>
        public SheetEvaluationContext(Sheet sheet)
        {
            this.sheet = sheet;
        }

        /// <inheritdoc />
        public CellValue GetValue(CellAddress address)
        {
            return this.sheet.GetValue(address);
        }

        /// <inheritdoc />
        public IEnumerable<CellValue> GetValues(CellRange range)
        {
            foreach (var address in range)
            {
                yield return this.sheet.GetValue(address);
            }
        }
    }
}
