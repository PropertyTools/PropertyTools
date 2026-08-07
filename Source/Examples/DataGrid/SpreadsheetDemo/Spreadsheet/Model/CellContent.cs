// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellContent.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents what a user entered into a cell: either a literal value or a formula.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using SpreadsheetDemo.Spreadsheet.Model.Formulas;

    /// <summary>
    /// Represents what a user entered into a cell: either a literal value or a formula.
    /// </summary>
    public readonly struct CellContent
    {
        /// <summary>
        /// Gets the empty content.
        /// </summary>
        public static readonly CellContent Empty = default(CellContent);

        /// <summary>
        /// Initializes a new instance of the <see cref="CellContent" /> struct.
        /// </summary>
        private CellContent(CellValue value, Formula formula)
        {
            this.Value = value;
            this.Formula = formula;
        }

        /// <summary>
        /// Creates content from a literal value.
        /// </summary>
        public static CellContent FromValue(CellValue value)
        {
            return new CellContent(value, null);
        }

        /// <summary>
        /// Creates content from a formula.
        /// </summary>
        public static CellContent FromFormula(Formula formula)
        {
            return new CellContent(CellValue.Empty, formula);
        }

        /// <summary>
        /// Gets a value indicating whether this content is a formula.
        /// </summary>
        public bool IsFormula => this.Formula != null;

        /// <summary>
        /// Gets the formula, or <c>null</c> if this content is a literal value.
        /// </summary>
        public Formula Formula { get; }

        /// <summary>
        /// Gets the literal value. This is <see cref="CellValue.Empty" /> when this content is a
        /// formula; the formula's computed value lives on <see cref="Cell.Value" /> instead, since it
        /// depends on the rest of the sheet.
        /// </summary>
        public CellValue Value { get; }

        /// <summary>
        /// Gets a value indicating whether this content is empty (not a formula, and an empty literal value).
        /// </summary>
        public bool IsEmpty => !this.IsFormula && this.Value.IsEmpty;
    }
}
