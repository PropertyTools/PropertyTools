// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Workbook.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a collection of sheets, together with file and modification state.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;

    /// <summary>
    /// Represents a collection of sheets, together with file and modification state.
    /// </summary>
    public sealed class Workbook : INotifyPropertyChanged
    {
        /// <summary>
        /// The active sheet.
        /// </summary>
        private Sheet activeSheet;

        /// <summary>
        /// The path of the file this workbook was loaded from or last saved to.
        /// </summary>
        private string filePath;

        /// <summary>
        /// A value indicating whether the workbook has unsaved changes.
        /// </summary>
        private bool isModified;

        /// <summary>
        /// Initializes a new instance of the <see cref="Workbook" /> class.
        /// </summary>
        public Workbook()
        {
            this.Sheets = new ObservableCollection<Sheet>();
            this.Sheets.CollectionChanged += this.OnSheetsCollectionChanged;
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the sheets in this workbook.
        /// </summary>
        public ObservableCollection<Sheet> Sheets { get; }

        /// <summary>
        /// Gets or sets the active sheet.
        /// </summary>
        public Sheet ActiveSheet
        {
            get => this.activeSheet;
            set
            {
                if (ReferenceEquals(this.activeSheet, value))
                {
                    return;
                }

                this.activeSheet = value;
                this.OnPropertyChanged(nameof(this.ActiveSheet));
            }
        }

        /// <summary>
        /// Gets or sets the path of the file this workbook was loaded from or last saved to, or
        /// <c>null</c> for a new, unsaved workbook.
        /// </summary>
        public string FilePath
        {
            get => this.filePath;
            set
            {
                if (this.filePath == value)
                {
                    return;
                }

                this.filePath = value;
                this.OnPropertyChanged(nameof(this.FilePath));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the workbook has unsaved changes. Set automatically
        /// whenever a cell in any of <see cref="Sheets" /> changes; cleared by <see cref="MarkSaved" />.
        /// </summary>
        public bool IsModified
        {
            get => this.isModified;
            private set
            {
                if (this.isModified == value)
                {
                    return;
                }

                this.isModified = value;
                this.OnPropertyChanged(nameof(this.IsModified));
            }
        }

        /// <summary>
        /// Clears <see cref="IsModified" />. Call after successfully saving or loading the workbook.
        /// </summary>
        public void MarkSaved()
        {
            this.IsModified = false;
        }

        /// <summary>
        /// Subscribes to/unsubscribes from <see cref="Sheet.CellChanged" /> as sheets are added to or
        /// removed from <see cref="Sheets" />, so that any edit on any sheet sets <see cref="IsModified" />.
        /// </summary>
        private void OnSheetsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Sheet sheet in e.OldItems)
                {
                    sheet.CellChanged -= this.OnSheetCellChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (Sheet sheet in e.NewItems)
                {
                    sheet.CellChanged += this.OnSheetCellChanged;
                }
            }
        }

        private void OnSheetCellChanged(object sender, CellChangedEventArgs e)
        {
            this.IsModified = true;
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
