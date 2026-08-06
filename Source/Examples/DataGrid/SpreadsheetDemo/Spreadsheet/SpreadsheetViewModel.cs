// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   View model for the SpreadsheetExample window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    using SpreadsheetDemo.Spreadsheet.Model;
    using SpreadsheetDemo.Spreadsheet.Model.Serialization;

    using PropertyTools.Wpf;

    using CellRange = SpreadsheetDemo.Spreadsheet.Model.CellRange;

    /// <summary>
    /// View model for the <see cref="SpreadsheetExample" /> window.
    /// </summary>
    public class SpreadsheetViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The default size for a new sheet.
        /// </summary>
        private const int DefaultRowCount = 100;

        private const int DefaultColumnCount = 26;

        private readonly Workbook workbook = new Workbook();

        private Sheet sheet;
        private SheetGridAdapter rows;
        private IList<string> rowHeaders;
        private IList<string> columnHeaders;
        private CellRef currentCell;
        private CellRef selectionCell;
        private Cell subscribedCell;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetViewModel" /> class with a new,
        /// empty 100x26 sheet.
        /// </summary>
        public SpreadsheetViewModel()
            : this(new Sheet("Sheet1", DefaultRowCount, DefaultColumnCount))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetViewModel" /> class with the given
        /// sheet (mainly for tests; the window's own constructor uses the parameterless overload).
        /// </summary>
        public SpreadsheetViewModel(Sheet sheet)
        {
            this.ControlFactory = new SpreadsheetControlFactory();
            this.SetSheet(sheet);
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the workbook (tracks the active sheet and unsaved-changes state).
        /// </summary>
        public Workbook Workbook => this.workbook;

        /// <summary>
        /// Gets the sheet being displayed.
        /// </summary>
        public Sheet Sheet => this.sheet;

        /// <summary>
        /// Gets the grid's items source: one <see cref="SheetRowAdapter" /> per row of <see cref="Sheet" />.
        /// </summary>
        public SheetGridAdapter Rows => this.rows;

        /// <summary>
        /// Gets the row header labels ("1", "2", ...).
        /// </summary>
        public IList<string> RowHeaders => this.rowHeaders;

        /// <summary>
        /// Gets the column header labels ("A", "B", ..., "Z", "AA", ...).
        /// </summary>
        public IList<string> ColumnHeaders => this.columnHeaders;

        /// <summary>
        /// Gets the control factory that renders cells.
        /// </summary>
        public IDataGridControlFactory ControlFactory { get; }

        /// <summary>
        /// Gets or sets the currently selected cell, in the grid's own reference type. Two-way bound
        /// to <see cref="DataGrid.CurrentCell" />. Setting this also collapses <see cref="SelectionCell" />
        /// to the same cell, so that a programmatic move (Find, the name box, the formula bar) selects
        /// a single cell instead of extending the previous range. A user drag-selecting a range in the
        /// grid does not go through this setter — <c>DataGrid</c> only re-assigns <c>CurrentCell</c> at
        /// the start of a selection, moving <c>SelectionCell</c> alone while dragging.
        /// </summary>
        public CellRef CurrentCell
        {
            get => this.currentCell;
            set => this.SelectRange(new CellRange(value.ToCellAddress()));
        }

        /// <summary>
        /// Gets or sets the cell defining the other corner of the selection range. Two-way bound to
        /// <see cref="DataGrid.SelectionCell" />; kept independent of <see cref="CurrentCell" /> so a
        /// user's drag-selected range survives round-tripping through this view model.
        /// </summary>
        public CellRef SelectionCell
        {
            get => this.selectionCell;
            set
            {
                if (this.selectionCell.Equals(value))
                {
                    return;
                }

                this.selectionCell = value;
                this.OnPropertyChanged(nameof(this.SelectionCell));
                this.OnPropertyChanged(nameof(this.SelectionReferenceText));
            }
        }

        /// <summary>
        /// Gets or sets the selection as A1-style reference text — "A1" for a single cell, "A1:B2" for
        /// a range — the name box's edit text. Setting an out-of-range or malformed reference throws
        /// (<see cref="FormatException" />/<see cref="ArgumentOutOfRangeException" />) rather than
        /// changing the selection, so <c>ValidatesOnExceptions</c> can surface it without corrupting
        /// state or crashing.
        /// </summary>
        public string SelectionReferenceText
        {
            get
            {
                var range = this.SelectionRange;
                return range.IsSingleCell ? range.TopLeft.ToString() : range.ToString();
            }

            set => this.SelectRange(this.ParseReference(value));
        }

        /// <summary>
        /// Gets the current selection as a range spanning <see cref="CurrentCell" /> and
        /// <see cref="SelectionCell" />.
        /// </summary>
        private CellRange SelectionRange => new CellRange(this.currentCell.ToCellAddress(), this.selectionCell.ToCellAddress());

        /// <summary>
        /// Gets or sets the round-trippable edit text of the current cell — the formula bar's edit box.
        /// </summary>
        /// <remarks>
        /// Setting this applies the same text to every cell in the selection (parsed independently per
        /// cell, so a formula's references are not adjusted per target), matching how typing into one
        /// cell of a multi-cell selection and committing already fills the whole selection when editing
        /// inline in the grid.
        /// </remarks>
        public string CurrentCellText
        {
            get => this.subscribedCell?.Text ?? string.Empty;
            set
            {
                foreach (var address in this.SelectionRange)
                {
                    this.sheet.SetCellText(address, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the current cell is bold — the Format menu's Bold toggle. Setting this
        /// applies to every cell in the selection (see <see cref="SelectionRange" />), each keeping its
        /// own style otherwise.
        /// </summary>
        public bool IsCurrentCellBold
        {
            get => this.subscribedCell?.Style.Bold ?? false;
            set => this.sheet.SetStyle(this.SelectionRange, style => style.WithBold(value));
        }

        /// <summary>
        /// Gets or sets whether the current cell is italic. Setting this applies to every cell in the
        /// selection, each keeping its own style otherwise.
        /// </summary>
        public bool IsCurrentCellItalic
        {
            get => this.subscribedCell?.Style.Italic ?? false;
            set => this.sheet.SetStyle(this.SelectionRange, style => style.WithItalic(value));
        }

        /// <summary>
        /// Sets the horizontal alignment of every cell in the selection — the Format menu's alignment
        /// commands.
        /// </summary>
        public void SetCurrentCellAlignment(CellHorizontalAlignment alignment)
        {
            this.sheet.SetStyle(this.SelectionRange, style => style.WithHorizontalAlignment(alignment));
        }

        /// <summary>
        /// Inserts <c>SUM</c> formulas totalling the current selection: one below each column of the
        /// selection (when it spans more than one row) and one to the right of each row (when it spans
        /// more than one column) — for a rectangular multi-row, multi-column selection, both are
        /// inserted. A column/row sum is skipped if there's no room for it. If exactly one sum ends up
        /// being inserted, <see cref="CurrentCell" /> moves there. Does nothing — and returns
        /// <c>false</c> — for a single-cell selection, or if nothing could be placed.
        /// </summary>
        public bool InsertSum()
        {
            var range = this.SelectionRange;
            if (range.IsSingleCell)
            {
                return false;
            }

            var insertedAddresses = new List<CellAddress>();

            if (range.RowCount > 1)
            {
                var targetRow = range.BottomRight.Row + 1;
                if (targetRow < this.sheet.RowCount)
                {
                    for (var column = range.TopLeft.Column; column <= range.BottomRight.Column; column++)
                    {
                        var columnRange = new CellRange(
                            new CellAddress(range.TopLeft.Row, column),
                            new CellAddress(range.BottomRight.Row, column));
                        var target = new CellAddress(targetRow, column);
                        this.sheet.SetCellText(target, "=SUM(" + columnRange + ")");
                        insertedAddresses.Add(target);
                    }
                }
            }

            if (range.ColumnCount > 1)
            {
                var targetColumn = range.BottomRight.Column + 1;
                if (targetColumn < this.sheet.ColumnCount)
                {
                    for (var row = range.TopLeft.Row; row <= range.BottomRight.Row; row++)
                    {
                        var rowRange = new CellRange(
                            new CellAddress(row, range.TopLeft.Column),
                            new CellAddress(row, range.BottomRight.Column));
                        var target = new CellAddress(row, targetColumn);
                        this.sheet.SetCellText(target, "=SUM(" + rowRange + ")");
                        insertedAddresses.Add(target);
                    }
                }
            }

            if (insertedAddresses.Count == 1)
            {
                this.CurrentCell = insertedAddresses[0].ToCellRef();
            }

            return insertedAddresses.Count > 0;
        }

        /// <summary>
        /// Increases the number of decimal digits shown for every cell in the selection by one
        /// (starting from "General" goes to one decimal place).
        /// </summary>
        public void IncreaseDecimalPlaces()
        {
            this.sheet.SetStyle(this.SelectionRange, style => style.WithFormat(AdjustDecimalPlaces(style.FormatString, 1)));
        }

        /// <summary>
        /// Decreases the number of decimal digits shown for every cell in the selection by one (floors
        /// at zero decimal places — it never returns to "General").
        /// </summary>
        public void DecreaseDecimalPlaces()
        {
            this.sheet.SetStyle(this.SelectionRange, style => style.WithFormat(AdjustDecimalPlaces(style.FormatString, -1)));
        }

        /// <summary>
        /// Sets the .NET format string (see <see cref="CellStyle.FormatString" />) of every cell in the
        /// selection — used for the Format menu's number and date/time format presets. Pass <c>null</c>
        /// to restore the general format.
        /// </summary>
        public void SetCurrentCellFormat(string formatString)
        {
            this.sheet.SetStyle(this.SelectionRange, style => style.WithFormat(formatString));
        }

        /// <summary>
        /// Sorts the current selection in place: a selection spanning more than one row sorts whole
        /// rows (every column moves together) keyed by the leftmost column's value; a single row
        /// spanning more than one column sorts that row's cells left-to-right. Does nothing for a
        /// single-cell selection.
        /// </summary>
        /// <remarks>
        /// Cell content moves as-is, including formula text — relative references inside a moved
        /// formula are not adjusted (the same simplification already made for row/column insert and
        /// delete, which this project's plan defers along with the reference fix-up they'd all need).
        /// </remarks>
        public void SortSelection(bool ascending)
        {
            var range = this.SelectionRange;
            if (range.IsSingleCell)
            {
                return;
            }

            if (range.RowCount == 1)
            {
                this.SortRowCells(range.TopLeft.Row, range.TopLeft.Column, range.BottomRight.Column, ascending);
            }
            else
            {
                this.SortRowsByLeftmostColumn(range, ascending);
            }
        }

        /// <summary>
        /// Sorts the cells of a single row, left to right.
        /// </summary>
        private void SortRowCells(int row, int fromColumn, int toColumn, bool ascending)
        {
            var count = toColumn - fromColumn + 1;
            var cells = new (CellValue Key, CellContent Content, CellStyle Style)[count];
            for (var i = 0; i < count; i++)
            {
                var cell = this.sheet.GetCell(new CellAddress(row, fromColumn + i));
                cells[i] = (cell.Value, cell.Content, cell.Style);
            }

            var ordered = ascending ? cells.OrderBy(c => c.Key) : cells.OrderByDescending(c => c.Key);
            var sorted = ordered.ToArray();

            for (var i = 0; i < count; i++)
            {
                var address = new CellAddress(row, fromColumn + i);
                this.sheet.SetContent(address, sorted[i].Content);
                this.sheet.SetCellStyle(address, sorted[i].Style);
            }
        }

        /// <summary>
        /// Sorts whole rows within <paramref name="range" />, keyed by the value in its leftmost
        /// column, keeping every column's cells in a row together.
        /// </summary>
        private void SortRowsByLeftmostColumn(CellRange range, bool ascending)
        {
            var columnCount = range.ColumnCount;
            var rows = new List<(CellValue Key, CellContent[] Contents, CellStyle[] Styles)>();

            for (var row = range.TopLeft.Row; row <= range.BottomRight.Row; row++)
            {
                var contents = new CellContent[columnCount];
                var styles = new CellStyle[columnCount];
                for (var i = 0; i < columnCount; i++)
                {
                    var cell = this.sheet.GetCell(new CellAddress(row, range.TopLeft.Column + i));
                    contents[i] = cell.Content;
                    styles[i] = cell.Style;
                }

                rows.Add((this.sheet.GetValue(new CellAddress(row, range.TopLeft.Column)), contents, styles));
            }

            var ordered = ascending ? rows.OrderBy(r => r.Key) : rows.OrderByDescending(r => r.Key);
            var sorted = ordered.ToList();

            for (var i = 0; i < sorted.Count; i++)
            {
                var row = range.TopLeft.Row + i;
                for (var column = 0; column < columnCount; column++)
                {
                    var address = new CellAddress(row, range.TopLeft.Column + column);
                    this.sheet.SetContent(address, sorted[i].Contents[column]);
                    this.sheet.SetCellStyle(address, sorted[i].Styles[column]);
                }
            }
        }

        /// <summary>
        /// Adjusts a "0.00"-style decimal format string's digit count by <paramref name="delta" />,
        /// treating a missing/empty format (the general format) as zero decimal places, and never going
        /// below zero.
        /// </summary>
        private static string AdjustDecimalPlaces(string currentFormat, int delta)
        {
            var digits = Math.Max(0, CountDecimalDigits(currentFormat) + delta);
            return digits == 0 ? "0" : "0." + new string('0', digits);
        }

        private static int CountDecimalDigits(string formatString)
        {
            if (string.IsNullOrEmpty(formatString))
            {
                return 0;
            }

            var dotIndex = formatString.IndexOf('.');
            if (dotIndex < 0)
            {
                return 0;
            }

            var digits = 0;
            for (var i = dotIndex + 1; i < formatString.Length && (formatString[i] == '0' || formatString[i] == '#'); i++)
            {
                digits++;
            }

            return digits;
        }

        /// <summary>
        /// Replaces the current sheet with a new, empty one.
        /// </summary>
        public void New()
        {
            this.SetSheet(new Sheet("Sheet1", DefaultRowCount, DefaultColumnCount));
        }

        /// <summary>
        /// Loads a sheet from the given file, replacing the current one.
        /// </summary>
        public void Open(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                this.SetSheet(SheetSerializer.Load(stream));
            }

            this.workbook.FilePath = path;
        }

        /// <summary>
        /// Saves the current sheet to the given file.
        /// </summary>
        public void Save(string path)
        {
            using (var stream = File.Create(path))
            {
                SheetSerializer.Save(this.sheet, stream);
            }

            this.workbook.FilePath = path;
            this.workbook.MarkSaved();
        }

        /// <summary>
        /// Replaces the current sheet with one imported from a CSV file, sized to fit the data. Unlike
        /// <see cref="Open" />, this does not set <see cref="Workbook.FilePath" /> — CSV is not this
        /// application's native format, so "Save" should still prompt for a <c>.ptsheet</c> file.
        /// </summary>
        public void ImportCsv(string path)
        {
            Sheet imported;
            using (var stream = File.OpenRead(path))
            {
                imported = CsvSerializer.Load(stream);
            }

            this.SetSheet(imported);
        }

        /// <summary>
        /// Exports the current sheet's used range to a CSV file. This does not affect
        /// <see cref="Workbook.FilePath" /> or <see cref="Workbook.IsModified" /> — it is a one-off
        /// export, not a save of the workbook's native format.
        /// </summary>
        public void ExportCsv(string path)
        {
            using (var stream = File.Create(path))
            {
                CsvSerializer.Save(this.sheet, stream);
            }
        }

        /// <summary>
        /// Searches for text in the sheet's cells, starting just after the current cell and wrapping
        /// around, and moves <see cref="CurrentCell" /> to the first match. Matches both a cell's
        /// displayed value and, for formula cells, the formula text itself (e.g. searching "A1" finds
        /// a cell containing <c>=A1+1</c> even though its displayed value doesn't mention "A1").
        /// </summary>
        /// <returns><c>true</c> if a match was found.</returns>
        public bool FindNext(string text, bool matchCase)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            var comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            var start = this.currentCell.ToCellAddress();
            var rowCount = this.sheet.RowCount;
            var columnCount = this.sheet.ColumnCount;
            var totalCells = rowCount * columnCount;
            var startIndex = (start.Row * columnCount) + start.Column;

            for (var offset = 1; offset <= totalCells; offset++)
            {
                var index = (startIndex + offset) % totalCells;
                var address = new CellAddress(index / columnCount, index % columnCount);
                var cell = this.sheet.GetCell(address);
                if (cell.DisplayText.IndexOf(text, comparison) >= 0 || cell.Text.IndexOf(text, comparison) >= 0)
                {
                    this.CurrentCell = address.ToCellRef();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Replaces every occurrence of <paramref name="find" /> in the current cell's literal text
        /// (formulas are left untouched) with <paramref name="replaceWith" />.
        /// </summary>
        /// <returns><c>true</c> if the current cell's text contained a match and was changed.</returns>
        public bool ReplaceInCurrentCell(string find, string replaceWith, bool matchCase)
        {
            if (string.IsNullOrEmpty(find) || this.subscribedCell == null || this.subscribedCell.Content.IsFormula)
            {
                return false;
            }

            var comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            var text = this.subscribedCell.Text;
            if (text.IndexOf(find, comparison) < 0)
            {
                return false;
            }

            this.subscribedCell.Text = ReplaceAllOccurrences(text, find, replaceWith, comparison);
            return true;
        }

        /// <summary>
        /// Replaces every occurrence of <paramref name="find" /> with <paramref name="replaceWith" />
        /// in every cell's literal text (formulas are left untouched).
        /// </summary>
        /// <returns>The number of cells changed.</returns>
        public int ReplaceAll(string find, string replaceWith, bool matchCase)
        {
            if (string.IsNullOrEmpty(find))
            {
                return 0;
            }

            var comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            var count = 0;

            for (var row = 0; row < this.sheet.RowCount; row++)
            {
                for (var column = 0; column < this.sheet.ColumnCount; column++)
                {
                    var address = new CellAddress(row, column);
                    if (!this.sheet.TryGetCell(address, out var cell) || cell.Content.IsFormula)
                    {
                        continue;
                    }

                    var text = cell.Text;
                    if (text.IndexOf(find, comparison) < 0)
                    {
                        continue;
                    }

                    cell.Text = ReplaceAllOccurrences(text, find, replaceWith, comparison);
                    count++;
                }
            }

            return count;
        }

        private static string ReplaceAllOccurrences(string text, string find, string replaceWith, StringComparison comparison)
        {
            var result = new System.Text.StringBuilder();
            var index = 0;
            while (true)
            {
                var matchIndex = text.IndexOf(find, index, comparison);
                if (matchIndex < 0)
                {
                    result.Append(text, index, text.Length - index);
                    break;
                }

                result.Append(text, index, matchIndex - index);
                result.Append(replaceWith);
                index = matchIndex + find.Length;
            }

            return result.ToString();
        }

        private void SetSheet(Sheet newSheet)
        {
            this.UnsubscribeCurrentCell();

            this.sheet = newSheet;
            this.rows = new SheetGridAdapter(newSheet);
            this.rowHeaders = BuildRowHeaders(newSheet.RowCount);
            this.columnHeaders = BuildColumnHeaders(newSheet.ColumnCount);
            this.currentCell = new CellRef(0, 0);
            this.selectionCell = new CellRef(0, 0);

            this.workbook.Sheets.Clear();
            this.workbook.Sheets.Add(newSheet);
            this.workbook.ActiveSheet = newSheet;
            this.workbook.MarkSaved();

            this.UpdateCurrentCellSubscription();

            this.OnPropertyChanged(nameof(this.Sheet));
            this.OnPropertyChanged(nameof(this.Rows));
            this.OnPropertyChanged(nameof(this.RowHeaders));
            this.OnPropertyChanged(nameof(this.ColumnHeaders));
            this.OnPropertyChanged(nameof(this.CurrentCell));
            this.OnPropertyChanged(nameof(this.SelectionCell));
            this.OnPropertyChanged(nameof(this.SelectionReferenceText));
            this.OnPropertyChanged(nameof(this.CurrentCellText));
            this.OnPropertyChanged(nameof(this.IsCurrentCellBold));
            this.OnPropertyChanged(nameof(this.IsCurrentCellItalic));
        }

        /// <summary>
        /// Sets <see cref="CurrentCell" /> to <paramref name="range" />'s top-left corner and
        /// <see cref="SelectionCell" /> to its bottom-right corner in one step, so a genuine two-corner
        /// range (from <see cref="SelectionReferenceText" /> or <see cref="InsertSum" />'s target) isn't
        /// immediately collapsed by <see cref="CurrentCell" />'s own single-cell-selecting setter.
        /// </summary>
        private void SelectRange(CellRange range)
        {
            var newCurrentCell = range.TopLeft.ToCellRef();
            var newSelectionCell = range.BottomRight.ToCellRef();

            if (this.currentCell.Equals(newCurrentCell) && this.selectionCell.Equals(newSelectionCell))
            {
                return;
            }

            this.currentCell = newCurrentCell;
            this.selectionCell = newSelectionCell;
            this.UpdateCurrentCellSubscription();
            this.OnPropertyChanged(nameof(this.CurrentCell));
            this.OnPropertyChanged(nameof(this.SelectionCell));
            this.OnPropertyChanged(nameof(this.SelectionReferenceText));
            this.OnPropertyChanged(nameof(this.CurrentCellText));
            this.OnPropertyChanged(nameof(this.IsCurrentCellBold));
            this.OnPropertyChanged(nameof(this.IsCurrentCellItalic));
        }

        /// <summary>
        /// Parses a name-box reference ("A1" or "A1:B2") into a range, throwing a
        /// <see cref="FormatException" /> for unparsable text or an
        /// <see cref="ArgumentOutOfRangeException" /> for a reference outside the sheet — both caught
        /// gracefully by the name box's <c>ValidatesOnExceptions</c> binding.
        /// </summary>
        private CellRange ParseReference(string value)
        {
            if (!CellRange.TryParse(value?.Trim(), out var range))
            {
                throw new FormatException($"\"{value}\" is not a valid cell reference. Use \"A1\" or \"A1:B2\".");
            }

            if (range.BottomRight.Row >= this.sheet.RowCount || range.BottomRight.Column >= this.sheet.ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"\"{value}\" is outside the sheet.");
            }

            return range;
        }

        private void UpdateCurrentCellSubscription()
        {
            this.UnsubscribeCurrentCell();

            var address = this.currentCell.ToCellAddress();
            if (address.Row >= this.sheet.RowCount || address.Column >= this.sheet.ColumnCount)
            {
                return;
            }

            this.subscribedCell = this.sheet.GetCell(address);
            this.subscribedCell.PropertyChanged += this.OnCurrentCellPropertyChanged;
        }

        private void UnsubscribeCurrentCell()
        {
            if (this.subscribedCell != null)
            {
                this.subscribedCell.PropertyChanged -= this.OnCurrentCellPropertyChanged;
                this.subscribedCell = null;
            }
        }

        private void OnCurrentCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Cell.Text):
                    this.OnPropertyChanged(nameof(this.CurrentCellText));
                    break;
                case nameof(Cell.Style):
                    this.OnPropertyChanged(nameof(this.IsCurrentCellBold));
                    this.OnPropertyChanged(nameof(this.IsCurrentCellItalic));
                    break;
            }
        }

        private static IList<string> BuildRowHeaders(int rowCount)
        {
            var headers = new List<string>(rowCount);
            for (var row = 0; row < rowCount; row++)
            {
                headers.Add((row + 1).ToString());
            }

            return headers;
        }

        private static IList<string> BuildColumnHeaders(int columnCount)
        {
            var headers = new List<string>(columnCount);
            for (var column = 0; column < columnCount; column++)
            {
                headers.Add(CellAddress.ToColumnName(column));
            }

            return headers;
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
