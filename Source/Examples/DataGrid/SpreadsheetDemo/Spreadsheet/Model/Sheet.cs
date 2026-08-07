// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sheet.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a sheet of cells: sparse storage, editing and (eventually) recalculation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet.Model.Calculation;
    using SpreadsheetDemo.Spreadsheet.Model.Formulas;

    /// <summary>
    /// Represents a sheet of cells. Storage is sparse: only cells that have been read or written are
    /// materialized as <see cref="Cell" /> objects; a missing cell reads as <see cref="CellValue.Empty" />.
    /// </summary>
    public sealed class Sheet
    {
        /// <summary>
        /// The materialized cells, keyed by address.
        /// </summary>
        private readonly Dictionary<CellAddress, Cell> cells = new Dictionary<CellAddress, Cell>();

        /// <summary>
        /// Tracks which cells each formula reads, so a changed cell can find its dependents.
        /// </summary>
        private readonly DependencyGraph dependencyGraph = new DependencyGraph();

        /// <summary>
        /// Recomputes formula values in dependency order after an edit.
        /// </summary>
        private readonly RecalculationEngine recalculationEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sheet" /> class.
        /// </summary>
        /// <param name="name">The sheet name.</param>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        public Sheet(string name, int rowCount, int columnCount)
        {
            if (rowCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            }

            if (columnCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount));
            }

            this.Name = name;
            this.RowCount = rowCount;
            this.ColumnCount = columnCount;
            this.recalculationEngine = new RecalculationEngine(this, this.dependencyGraph);
        }

        /// <summary>
        /// Raised after a cell's content, value or style changes.
        /// </summary>
        public event EventHandler<CellChangedEventArgs> CellChanged;

        /// <summary>
        /// Gets or sets the sheet name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        public int RowCount { get; private set; }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int ColumnCount { get; private set; }

        /// <summary>
        /// Gets or sets the culture used to parse and format cell text.
        /// </summary>
        public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

        /// <summary>
        /// Gets or sets the parser used for cell input starting with '='. Defaults to
        /// <see cref="Formulas.FormulaParser" />; set to <c>null</c> to preserve formula input as
        /// unparsed text (evaluating to <see cref="CellError.Name" />) without a working formula engine.
        /// </summary>
        public IFormulaParser FormulaParser { get; set; } = new FormulaParser();

        /// <summary>
        /// Gets or sets the evaluator used to compute formula values. Defaults to
        /// <see cref="Formulas.FormulaEvaluator" />; set to <c>null</c> to make every formula evaluate
        /// to <see cref="CellError.NotAvailable" /> without a working formula engine.
        /// </summary>
        public IFormulaEvaluator FormulaEvaluator { get; set; } = new FormulaEvaluator();

        /// <summary>
        /// Gets the cell at the specified address, creating and caching it if this is the first time
        /// it has been accessed.
        /// </summary>
        public Cell GetCell(CellAddress address)
        {
            this.ValidateAddress(address);
            if (!this.cells.TryGetValue(address, out var cell))
            {
                cell = new Cell(this, address);
                this.cells[address] = cell;
            }

            return cell;
        }

        /// <summary>
        /// Tries to get the cell at the specified address without materializing it.
        /// </summary>
        /// <returns><c>true</c> if the cell has been materialized (i.e. has been read or written before).</returns>
        public bool TryGetCell(CellAddress address, out Cell cell)
        {
            return this.cells.TryGetValue(address, out cell);
        }

        /// <summary>
        /// Gets the value at the specified address. Cells that have never been written return
        /// <see cref="CellValue.Empty" /> without being materialized.
        /// </summary>
        public CellValue GetValue(CellAddress address)
        {
            return this.cells.TryGetValue(address, out var cell) ? cell.Value : CellValue.Empty;
        }

        /// <summary>
        /// Parses <paramref name="input" /> with <see cref="CellInputParser" /> and sets it as the
        /// content of the cell at the specified address. This is the single entry point used by both
        /// the grid editor and the formula bar.
        /// </summary>
        public void SetCellText(CellAddress address, string input)
        {
            var content = CellInputParser.Parse(input ?? string.Empty, this.FormulaParser, this.Culture);
            this.SetContent(address, content);
        }

        /// <summary>
        /// Sets the content of the cell at the specified address and recomputes its value together
        /// with everything that depends on it.
        /// </summary>
        public void SetContent(CellAddress address, CellContent content)
        {
            this.ApplyContent(address, content);
            this.OnCellChanged(address);
        }

        /// <summary>
        /// Copies the content and style of <paramref name="source" /> onto the cell at
        /// <paramref name="targetAddress" />. Used when the grid propagates an edit from one selected
        /// cell to the rest of the selection; the source cell is never assigned directly, since that
        /// would make two addresses share one <see cref="Cell" /> identity.
        /// </summary>
        public void CopyCell(CellAddress targetAddress, Cell source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            this.ApplyContent(targetAddress, source.Content);
            this.GetCell(targetAddress).SetStyleCore(source.Style);
            this.OnCellChanged(targetAddress);
        }

        /// <summary>
        /// Defers recalculation until the returned scope is disposed, so that many edits (paste, a
        /// bulk clear, loading a file) trigger one recalculation pass instead of one per cell.
        /// </summary>
        /// <remarks>
        /// Within the scope, <see cref="CellChanged" /> still fires per edited cell as usual, but the
        /// recomputed <see cref="Cell.Value" /> for cells affected only indirectly (as a dependent, not
        /// edited directly) does not settle until the scope is disposed. Callers that need every
        /// touched cell's value to be current the moment <see cref="CellChanged" /> fires should not
        /// use this for edits with formula dependents; it is intended for bulk operations (paste, load)
        /// where only the end-of-batch state matters.
        /// </remarks>
        public IDisposable DeferRecalculation()
        {
            return this.recalculationEngine.Defer();
        }

        /// <summary>
        /// Sets a cell's content, updates its formula dependencies, and marks it (and everything
        /// depending on it) for recalculation.
        /// </summary>
        private void ApplyContent(CellAddress address, CellContent content)
        {
            var cell = this.GetCell(address);
            cell.SetContentCore(content);

            var hasValidFormula = content.IsFormula && !content.Formula.HasSyntaxError;
            this.dependencyGraph.SetPrecedents(
                address,
                hasValidFormula ? content.Formula.Precedents : Array.Empty<CellAddress>(),
                hasValidFormula ? content.Formula.RangePrecedents : Array.Empty<CellRange>());

            this.recalculationEngine.MarkDirty(address);
        }

        /// <summary>
        /// Applies a style transform to every cell in the range.
        /// </summary>
        public void SetStyle(CellRange range, Func<CellStyle, CellStyle> transform)
        {
            if (transform == null)
            {
                throw new ArgumentNullException(nameof(transform));
            }

            foreach (var address in range)
            {
                var cell = this.GetCell(address);
                this.SetCellStyle(address, transform(cell.Style));
            }
        }

        /// <summary>
        /// Sets the style of a single cell directly.
        /// </summary>
        public void SetCellStyle(CellAddress address, CellStyle style)
        {
            var cell = this.GetCell(address);
            cell.SetStyleCore(style);
            this.OnCellChanged(address);
        }

        /// <summary>
        /// Clears the content (but not the style) of every cell in the range.
        /// </summary>
        public void ClearContents(CellRange range)
        {
            foreach (var address in range)
            {
                if (this.cells.TryGetValue(address, out var cell) && !cell.IsEmpty)
                {
                    this.SetContent(address, CellContent.Empty);
                }
            }
        }

        /// <summary>
        /// Evaluates the given content to a value: pass-through for literals; for formulas, an error
        /// while no <see cref="FormulaParser" />/<see cref="FormulaEvaluator" /> is configured, or
        /// while the formula text has a syntax error.
        /// </summary>
        internal CellValue Evaluate(CellContent content)
        {
            if (!content.IsFormula)
            {
                return content.Value;
            }

            var formula = content.Formula;
            if (formula.HasSyntaxError)
            {
                return CellValue.FromError(CellError.Name);
            }

            return this.FormulaEvaluator != null
                ? this.FormulaEvaluator.Evaluate(formula, this)
                : CellValue.FromError(CellError.NotAvailable);
        }

        /// <summary>
        /// Throws if the address is outside <see cref="RowCount" />/<see cref="ColumnCount" />.
        /// </summary>
        private void ValidateAddress(CellAddress address)
        {
            if (address.Row >= this.RowCount || address.Column >= this.ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(address));
            }
        }

        /// <summary>
        /// Raises <see cref="CellChanged" />.
        /// </summary>
        private void OnCellChanged(CellAddress address)
        {
            this.CellChanged?.Invoke(this, new CellChangedEventArgs(address));
        }
    }
}
