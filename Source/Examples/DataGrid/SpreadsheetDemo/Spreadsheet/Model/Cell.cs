// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cell.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a single cell in a Sheet.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System.ComponentModel;

    /// <summary>
    /// Represents a single cell in a <see cref="Model.Sheet" />: its content (what was typed), its
    /// computed value, and its style.
    /// </summary>
    /// <remarks>
    /// Cells are created and owned by their <see cref="Sheet" /> (see <see cref="Sheet.GetCell" />)
    /// and live for the lifetime of the sheet, so bindings to a <see cref="Cell" /> stay valid across
    /// edits and recalculation.
    /// </remarks>
    [TypeConverter(typeof(CellConverter))]
    public sealed class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// The content, i.e. what was typed into the cell (a literal value or a formula).
        /// </summary>
        private CellContent content = CellContent.Empty;

        /// <summary>
        /// The computed value: equal to <see cref="CellContent.Value" /> for literals, or the result
        /// of evaluating <see cref="CellContent.Formula" /> for formulas.
        /// </summary>
        private CellValue cellValue = CellValue.Empty;

        /// <summary>
        /// The style.
        /// </summary>
        private CellStyle style = CellStyle.Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell" /> class.
        /// </summary>
        /// <param name="sheet">The owning sheet.</param>
        /// <param name="address">The address of this cell within the sheet.</param>
        internal Cell(Sheet sheet, CellAddress address)
        {
            this.Sheet = sheet;
            this.Address = address;
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the sheet that owns this cell.
        /// </summary>
        public Sheet Sheet { get; }

        /// <summary>
        /// Gets the address of this cell within its sheet.
        /// </summary>
        public CellAddress Address { get; }

        /// <summary>
        /// Gets the content of this cell: what was typed (a literal value or a formula).
        /// </summary>
        public CellContent Content => this.content;

        /// <summary>
        /// Gets the computed value of this cell.
        /// </summary>
        public CellValue Value => this.cellValue;

        /// <summary>
        /// Gets the style of this cell.
        /// </summary>
        public CellStyle Style => this.style;

        /// <summary>
        /// Gets a value indicating whether this cell has no content.
        /// </summary>
        public bool IsEmpty => this.content.IsEmpty;

        /// <summary>
        /// Gets or sets the round-trippable edit text for this cell, e.g. "=A1+2", "3.14", "Hello".
        /// Setting this text edits the cell through the owning <see cref="Model.Sheet" />, so parsing,
        /// dependency tracking, recalculation and undo all go through the same code path regardless of
        /// whether the edit came from the grid or from a formula bar.
        /// </summary>
        public string Text
        {
            get => CellFormatter.ToEditText(this.content, this.Sheet.Culture);
            set => this.Sheet.SetCellText(this.Address, value);
        }

        /// <summary>
        /// Gets the formatted text shown in the grid, applying this cell's style.
        /// </summary>
        public string DisplayText => CellFormatter.ToDisplayText(this.cellValue, this.style, this.Sheet.Culture);

        /// <summary>
        /// Sets <see cref="Content" />. Called by <see cref="Model.Sheet" /> only.
        /// </summary>
        internal void SetContentCore(CellContent newContent)
        {
            this.content = newContent;
            this.OnPropertyChanged(nameof(this.Content));
            this.OnPropertyChanged(nameof(this.Text));
            this.OnPropertyChanged(nameof(this.IsEmpty));
        }

        /// <summary>
        /// Sets <see cref="Value" />. Called by <see cref="Model.Sheet" /> only. Suppresses the change
        /// notification when the computed value did not actually change, so that recalculating a
        /// formula that produces the same result does not touch bound UI.
        /// </summary>
        internal void SetValueCore(CellValue newValue)
        {
            if (this.cellValue.Equals(newValue))
            {
                return;
            }

            this.cellValue = newValue;
            this.OnPropertyChanged(nameof(this.Value));
            this.OnPropertyChanged(nameof(this.DisplayText));
        }

        /// <summary>
        /// Sets <see cref="Style" />. Called by <see cref="Model.Sheet" /> only.
        /// </summary>
        internal void SetStyleCore(CellStyle newStyle)
        {
            newStyle = newStyle ?? CellStyle.Default;
            if (ReferenceEquals(this.style, newStyle))
            {
                return;
            }

            this.style = newStyle;
            this.OnPropertyChanged(nameof(this.Style));
            this.OnPropertyChanged(nameof(this.DisplayText));
        }

        /// <summary>
        /// Returns <see cref="DisplayText" />. Used by the grid when copying cells to the clipboard.
        /// </summary>
        public override string ToString()
        {
            return this.DisplayText;
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged" />.
        /// </summary>
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
