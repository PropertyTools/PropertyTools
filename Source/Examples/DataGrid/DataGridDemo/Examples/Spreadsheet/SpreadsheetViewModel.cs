// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   View model for the SpreadsheetExample window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo.Spreadsheet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    using DataGridDemo.Spreadsheet.Model;
    using DataGridDemo.Spreadsheet.Model.Serialization;

    using PropertyTools.Wpf;

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
        /// to <see cref="DataGrid.CurrentCell" />.
        /// </summary>
        public CellRef CurrentCell
        {
            get => this.currentCell;
            set
            {
                if (this.currentCell.Equals(value))
                {
                    return;
                }

                this.currentCell = value;
                this.UpdateCurrentCellSubscription();
                this.OnPropertyChanged(nameof(this.CurrentCell));
                this.OnPropertyChanged(nameof(this.CurrentCellText));
                this.OnPropertyChanged(nameof(this.IsCurrentCellBold));
                this.OnPropertyChanged(nameof(this.IsCurrentCellItalic));
            }
        }

        /// <summary>
        /// Gets or sets the round-trippable edit text of the current cell — the formula bar's edit box.
        /// </summary>
        public string CurrentCellText
        {
            get => this.subscribedCell?.Text ?? string.Empty;
            set => this.sheet.SetCellText(this.currentCell.ToCellAddress(), value);
        }

        /// <summary>
        /// Gets or sets whether the current cell is bold — the Format menu's Bold toggle.
        /// </summary>
        public bool IsCurrentCellBold
        {
            get => this.subscribedCell?.Style.Bold ?? false;
            set => this.sheet.SetCellStyle(this.currentCell.ToCellAddress(), (this.subscribedCell?.Style ?? CellStyle.Default).WithBold(value));
        }

        /// <summary>
        /// Gets or sets whether the current cell is italic.
        /// </summary>
        public bool IsCurrentCellItalic
        {
            get => this.subscribedCell?.Style.Italic ?? false;
            set => this.sheet.SetCellStyle(this.currentCell.ToCellAddress(), (this.subscribedCell?.Style ?? CellStyle.Default).WithItalic(value));
        }

        /// <summary>
        /// Sets the horizontal alignment of the current cell — the Format menu's alignment commands.
        /// </summary>
        public void SetCurrentCellAlignment(CellHorizontalAlignment alignment)
        {
            var style = this.subscribedCell?.Style ?? CellStyle.Default;
            this.sheet.SetCellStyle(this.currentCell.ToCellAddress(), style.WithHorizontalAlignment(alignment));
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
        /// Searches for text in the sheet's displayed cell text, starting just after the current cell
        /// and wrapping around, and moves <see cref="CurrentCell" /> to the first match.
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
                var displayText = this.sheet.GetCell(address).DisplayText;
                if (displayText.IndexOf(text, comparison) >= 0)
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
            this.OnPropertyChanged(nameof(this.CurrentCellText));
            this.OnPropertyChanged(nameof(this.IsCurrentCellBold));
            this.OnPropertyChanged(nameof(this.IsCurrentCellItalic));
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
