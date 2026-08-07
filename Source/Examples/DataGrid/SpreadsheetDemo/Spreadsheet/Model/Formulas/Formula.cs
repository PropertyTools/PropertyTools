// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Formula.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a parsed formula: its source text, its syntax tree and the cells it depends on.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a parsed formula: its source text, its abstract syntax tree, and the cells/ranges
    /// it depends on.
    /// </summary>
    public sealed class Formula
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Formula" /> class.
        /// </summary>
        /// <param name="text">The formula text, without the leading '='.</param>
        /// <param name="root">
        /// The root of the syntax tree, or <c>null</c> if <paramref name="text" /> failed to parse
        /// (the text is preserved so the user can edit and fix it).
        /// </param>
        /// <param name="precedents">The individual cells this formula reads.</param>
        /// <param name="rangePrecedents">The cell ranges this formula reads.</param>
        public Formula(
            string text,
            FormulaNode root,
            IReadOnlyList<CellAddress> precedents,
            IReadOnlyList<CellRange> rangePrecedents)
        {
            this.Text = text ?? string.Empty;
            this.Root = root;
            this.Precedents = precedents ?? Array.Empty<CellAddress>();
            this.RangePrecedents = rangePrecedents ?? Array.Empty<CellRange>();
        }

        /// <summary>
        /// Gets the formula text, without the leading '='.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the root of the syntax tree, or <c>null</c> if the formula text is not valid.
        /// </summary>
        public FormulaNode Root { get; }

        /// <summary>
        /// Gets the individual cells this formula reads.
        /// </summary>
        public IReadOnlyList<CellAddress> Precedents { get; }

        /// <summary>
        /// Gets the cell ranges this formula reads.
        /// </summary>
        public IReadOnlyList<CellRange> RangePrecedents { get; }

        /// <summary>
        /// Gets a value indicating whether the formula text failed to parse.
        /// </summary>
        public bool HasSyntaxError => this.Root == null;
    }
}
