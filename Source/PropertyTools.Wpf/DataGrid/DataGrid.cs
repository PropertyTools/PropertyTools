// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGrid.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Displays enumerable data in a customizable grid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Displays enumerable data in a customizable grid.
    /// </summary>
    [ContentProperty("ItemsSource")]
    [DefaultProperty("ItemsSource")]
    [TemplatePart(Name = PartGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PartSheetScrollViewer, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PartSheetGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PartColumnScrollViewer, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PartColumnGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PartRowScrollViewer, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PartRowGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PartSelectionBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartRowSelectionBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartColumnSelectionBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartCurrentBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartSelection, Type = typeof(Border))]
    [TemplatePart(Name = PartAutoFillSelection, Type = typeof(Border))]
    [TemplatePart(Name = PartAutoFillBox, Type = typeof(Border))]
    [TemplatePart(Name = PartTopLeft, Type = typeof(Border))]
    public partial class DataGrid : Control
    {
        /// <summary>
        /// The auto fill box.
        /// </summary>
        private const string PartAutoFillBox = "PART_AutoFillBox";

        /// <summary>
        /// The auto fill selection.
        /// </summary>
        private const string PartAutoFillSelection = "PART_AutoFillSelection";

        /// <summary>
        /// The column grid.
        /// </summary>
        private const string PartColumnGrid = "PART_ColumnGrid";

        /// <summary>
        /// The column scroll viewer.
        /// </summary>
        private const string PartColumnScrollViewer = "PART_ColumnScrollViewer";

        /// <summary>
        /// The column selection background.
        /// </summary>
        private const string PartColumnSelectionBackground = "PART_ColumnSelectionBackground";

        /// <summary>
        /// The current background.
        /// </summary>
        private const string PartCurrentBackground = "PART_CurrentBackground";

        /// <summary>
        /// The grid.
        /// </summary>
        private const string PartGrid = "PART_Grid";

        /// <summary>
        /// The row grid.
        /// </summary>
        private const string PartRowGrid = "PART_RowGrid";

        /// <summary>
        /// The row scroll viewer.
        /// </summary>
        private const string PartRowScrollViewer = "PART_RowScrollViewer";

        /// <summary>
        /// The row selection background.
        /// </summary>
        private const string PartRowSelectionBackground = "PART_RowSelectionBackground";

        /// <summary>
        /// The selection.
        /// </summary>
        private const string PartSelection = "PART_Selection";

        /// <summary>
        /// The selection background.
        /// </summary>
        private const string PartSelectionBackground = "PART_SelectionBackground";

        /// <summary>
        /// The sheet grid.
        /// </summary>
        private const string PartSheetGrid = "PART_SheetGrid";

        /// <summary>
        /// The sheet scroll viewer.
        /// </summary>
        private const string PartSheetScrollViewer = "PART_SheetScrollViewer";

        /// <summary>
        /// The top left cell.
        /// </summary>
        private const string PartTopLeft = "PART_TopLeft";

        /// <summary>
        /// The cell map.
        /// </summary>
        private readonly Dictionary<int, UIElement> cellMap = new Dictionary<int, UIElement>();

        /// <summary>
        /// The column header map.
        /// </summary>
        private readonly Dictionary<int, FrameworkElement> columnHeaderMap = new Dictionary<int, FrameworkElement>();

        /// <summary>
        /// The auto fill box.
        /// </summary>
        private Border autoFillBox;

        /// <summary>
        /// The auto fill cell.
        /// </summary>
        private CellRef autoFillCell;

        /// <summary>
        /// The auto fill selection.
        /// </summary>
        private Border autoFillSelection;

        /// <summary>
        /// The auto fill tool tip.
        /// </summary>
        private ToolTip autoFillToolTip;

        /// <summary>
        /// The auto filler.
        /// </summary>
        private AutoFiller autoFiller;

        /// <summary>
        /// The index in the sheet grid where new cells can be inserted.
        /// </summary>
        /// <remarks>The selection and auto fill controls should always be at the end of the sheetGrid children collection.</remarks>
        private int cellInsertionIndex;

        /// <summary>
        /// The column grid control.
        /// </summary>
        private Grid columnGrid;

        /// <summary>
        /// The is selecting rows.
        /// </summary>
        private bool isSelectingRows;

        /// <summary>
        /// The row grid control.
        /// </summary>
        private Grid rowGrid;

        /// <summary>
        /// The column scroll view control.
        /// </summary>
        private ScrollViewer columnScrollViewer;

        /// <summary>
        /// The column selection background control.
        /// </summary>
        private Border columnSelectionBackground;

        /// <summary>
        /// The current background control.
        /// </summary>
        private Border currentBackground;

        /// <summary>
        /// The current editor.
        /// </summary>
        private FrameworkElement currentEditor;

        /// <summary>
        /// The editing cells.
        /// </summary>
        private IEnumerable<CellRef> editingCells;

        /// <summary>
        /// The end pressed.
        /// </summary>
        private bool endPressed;

        /// <summary>
        /// The is capturing.
        /// </summary>
        private bool isCapturing;

        /// <summary>
        /// The is selecting columns.
        /// </summary>
        private bool isSelectingColumns;

        /// <summary>
        /// The sheet grid control.
        /// </summary>
        private Grid sheetGrid;

        /// <summary>
        /// The row scroll viewer control.
        /// </summary>
        private ScrollViewer rowScrollViewer;

        /// <summary>
        /// The row selection background control.
        /// </summary>
        private Border rowSelectionBackground;

        /// <summary>
        /// The selection control.
        /// </summary>
        private Border selection;

        /// <summary>
        /// The selection background control.
        /// </summary>
        private Border selectionBackground;

        /// <summary>
        /// The sheet scroll viewer control.
        /// </summary>
        private ScrollViewer sheetScrollViewer;

        /// <summary>
        /// Reference to the collection that has subscribed to the INotifyCollectionChanged event.
        /// </summary>
        private object subcribedCollection;

        /// <summary>
        /// The top/left control.
        /// </summary>
        private Border topLeft;

        /// <summary>
        /// Initializes static members of the <see cref="DataGrid" /> class.
        /// </summary>
        static DataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DataGrid),
                new FrameworkPropertyMetadata(typeof(DataGrid)));

            InsertRowsCommand = new RoutedCommand("InsertRows", typeof(DataGrid));
            DeleteRowsCommand = new RoutedCommand("DeleteRows", typeof(DataGrid));
            InsertColumnsCommand = new RoutedCommand("InsertColumns", typeof(DataGrid));
            DeleteColumnsCommand = new RoutedCommand("DeleteColumns", typeof(DataGrid));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGrid" /> class.
        /// </summary>
        public DataGrid()
        {
            this.CommandBindings.Add(
                new CommandBinding(
                    InsertRowsCommand,
                    (s, e) => this.InsertRows(),
                    (s, e) => e.CanExecute = this.CanInsertRows));
            this.CommandBindings.Add(
                new CommandBinding(
                    DeleteRowsCommand,
                    (s, e) => this.DeleteRows(),
                    (s, e) => e.CanExecute = this.CanDeleteRows));
            this.CommandBindings.Add(
                new CommandBinding(
                    InsertColumnsCommand,
                    (s, e) => this.InsertColumns(),
                    (s, e) => e.CanExecute = this.CanInsertColumns));
            this.CommandBindings.Add(
                new CommandBinding(
                    DeleteColumnsCommand,
                    (s, e) => this.DeleteColumns(),
                    (s, e) => e.CanExecute = this.CanDeleteColumns));
        }

        /// <summary>
        /// Gets the delete columns command.
        /// </summary>
        /// <value>The delete columns command.</value>
        public static ICommand DeleteColumnsCommand { get; private set; }

        /// <summary>
        /// Gets the delete rows command.
        /// </summary>
        /// <value>The delete rows command.</value>
        public static ICommand DeleteRowsCommand { get; private set; }

        /// <summary>
        /// Gets the insert columns command.
        /// </summary>
        /// <value>The insert columns command.</value>
        public static ICommand InsertColumnsCommand { get; private set; }

        /// <summary>
        /// Gets the insert rows command.
        /// </summary>
        /// <value>The insert rows command.</value>
        public static ICommand InsertRowsCommand { get; private set; }

        /// <summary>
        /// The auto size all columns.
        /// </summary>
        public void AutoSizeAllColumns()
        {
            this.sheetGrid.UpdateLayout();
            this.columnGrid.UpdateLayout();
            for (int i = 0; i < this.Columns; i++)
            {
                this.AutoSizeColumn(i);
            }
        }

        /// <summary>
        /// Auto-size the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        public void AutoSizeColumn(int column)
        {
            // Initialize with the width of the header element
            var headerElement = this.GetColumnElement(column);
            double maximumWidth = headerElement.ActualWidth + headerElement.Margin.Left + headerElement.Margin.Right;

            // Compare with the widths of the cell elements
            for (int i = 0; i < this.sheetGrid.RowDefinitions.Count; i++)
            {
                var c = this.GetCellElement(new CellRef(i, column)) as FrameworkElement;
                if (c != null)
                {
                    maximumWidth = Math.Max(maximumWidth, c.ActualWidth + c.Margin.Left + c.Margin.Right);
                }
            }

            this.sheetGrid.ColumnDefinitions[column].Width = new GridLength((int)maximumWidth + 2);
        }

        /// <summary>
        /// Copies items to the clipboard.
        /// </summary>
        public void Copy()
        {
            this.Copy("\t");
        }

        /// <summary>
        /// Cuts the selected items.
        /// </summary>
        public void Cut()
        {
            this.Copy();
            this.Delete();
        }

        /// <summary>
        /// Copies the selected cells.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public void Copy(string separator)
        {
            var text = this.SelectionToString(separator);
            var array = this.SelectionToArray();

            var dataObject = new DataObject();
            dataObject.SetText(text);

            if (AreAllElementsSerializable(array))
            {
                try
                {
                    dataObject.SetData(typeof(DataGrid), array);
                }
                catch (Exception e)
                {
                    // nonserializable values?
                    Debug.WriteLine(e);
                }
            }

            Clipboard.SetDataObject(dataObject);
        }

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="updateGrid">The update grid.</param>
        /// <returns>
        /// The delete item.
        /// </returns>
        public bool DeleteItem(int index, bool updateGrid)
        {
            var list = this.ItemsSource;
            if (list == null)
            {
                return false;
            }

            if (index < 0 || index >= list.Count)
            {
                return false;
            }

            if (this.IsIListIList() && this.ItemsInColumns)
            {
                foreach (var row in this.ItemsSource.OfType<IList>().Where(row => index < row.Count))
                {
                    row.RemoveAt(index);
                }
            }
            else
            {
                list.RemoveAt(index);
            }

            if (updateGrid)
            {
                this.UpdateGridContent();
            }

            return true;
        }

        /// <summary>
        /// The end text edit.
        /// </summary>
        /// <param name="commit">The commit.</param>
        public void EndTextEdit(bool commit = true)
        {
            var textEditor = this.currentEditor as TextBox;
            if (commit && textEditor != null)
            {
                foreach (var cell in this.editingCells)
                {
                    this.TrySetCellValue(cell, textEditor.Text);
                }
            }

            this.HideEditor();
        }

        /// <summary>
        /// Gets the cell reference for the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="isInAutoFillMode">if set to <c>true</c> [is in auto fill mode].</param>
        /// <param name="relativeTo">The relative to.</param>
        /// <returns>
        /// The cell reference.
        /// </returns>
        public CellRef GetCell(Point position, bool isInAutoFillMode = false, CellRef relativeTo = default(CellRef))
        {
            double w = 0;
            int column = -1;
            int row = -1;
            for (int j = 0; j < this.sheetGrid.ColumnDefinitions.Count; j++)
            {
                double aw0 = j - 1 >= 0 ? this.sheetGrid.ColumnDefinitions[j - 1].ActualWidth : 0;
                double aw1 = this.sheetGrid.ColumnDefinitions[j].ActualWidth;
                double aw2 = j + 1 < this.sheetGrid.ColumnDefinitions.Count
                                 ? this.sheetGrid.ColumnDefinitions[j + 1].ActualWidth
                                 : 0;
                if (isInAutoFillMode)
                {
                    if (relativeTo.Column <= j)
                    {
                        aw0 = 0;
                        aw2 *= 0.5;
                    }
                    else
                    {
                        aw0 *= 0.5;
                        aw2 = 0;
                    }
                }
                else
                {
                    aw0 = 0;
                    aw2 = 0;
                }

                if (position.X > w - aw0 && position.X < w + aw1 + aw2)
                {
                    column = j;
                    break;
                }

                w += aw1;
            }

            if (w > 0 && column == -1)
            {
                column = this.sheetGrid.ColumnDefinitions.Count - 1;
            }

            double h = 0;
            for (int i = 0; i < this.sheetGrid.RowDefinitions.Count; i++)
            {
                double ah = this.sheetGrid.RowDefinitions[i].ActualHeight;
                if (position.Y < h + ah)
                {
                    row = i;
                    break;
                }

                h += ah;
            }

            if (h > 0 && row == -1)
            {
                row = this.sheetGrid.RowDefinitions.Count - 1;
            }

            if (column == -1 || row == -1)
            {
                return new CellRef(-1, -1);
            }

            return new CellRef(row, column);
        }

        /// <summary>
        /// Gets the element at the specified cell.
        /// </summary>
        /// <param name="cellRef">The cell reference.</param>
        /// <returns>
        /// The element, or <c>null</c> if the cell was not found.
        /// </returns>
        public UIElement GetCellElement(CellRef cellRef)
        {
            UIElement e;
            return this.cellMap.TryGetValue(cellRef.GetHashCode(), out e) ? e : null;
        }

        /// <summary>
        /// Gets the formatted string value for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The cell string.
        /// </returns>
        public string GetCellString(CellRef cell)
        {
            var value = this.GetCellValue(cell);
            if (value == null)
            {
                return null;
            }

            var formatString = this.GetFormatString(cell);
            return FormatValue(value, formatString);
        }

        /// <summary>
        /// Gets the cell value from the Content property for the specified cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The get cell value.
        /// </returns>
        public object GetCellValue(CellRef cell)
        {
            if (cell.Column < 0 || cell.Column >= this.Columns || cell.Row < 0 || cell.Row >= this.Rows)
            {
                return null;
            }

            var item = this.GetItem(cell);
            if (item != null)
            {
                var pd = this.GetPropertyDefinition(cell);
                if (pd != null)
                {
                    if (pd.Descriptor != null)
                    {
                        return pd.Descriptor.GetValue(item);
                    }

                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the position of the specified cell.
        /// </summary>
        /// <param name="cellRef">The cell reference.</param>
        /// <returns>
        /// The upper-left position of the cell.
        /// </returns>
        public Point GetPosition(CellRef cellRef)
        {
            double x = 0;
            double y = 0;
            for (int j = 0; j < cellRef.Column && j < this.sheetGrid.ColumnDefinitions.Count; j++)
            {
                x += this.sheetGrid.ColumnDefinitions[j].ActualWidth;
            }

            for (int i = 0; i < cellRef.Row && i < this.sheetGrid.RowDefinitions.Count; i++)
            {
                y += this.sheetGrid.RowDefinitions[i].ActualHeight;
            }

            return new Point(x, y);
        }

        /// <summary>
        /// Gets the visible cells.
        /// </summary>
        /// <param name="topLeftCell">The top left cell.</param>
        /// <param name="bottomRightCell">The bottom right cell.</param>
        public void GetVisibleCells(out CellRef topLeftCell, out CellRef bottomRightCell)
        {
            double left = this.sheetScrollViewer.HorizontalOffset;
            double right = left + this.sheetScrollViewer.ActualWidth;
            double top = this.sheetScrollViewer.VerticalOffset;
            double bottom = top + this.sheetScrollViewer.ActualHeight;

            topLeftCell = this.GetCell(new Point(left, top));
            bottomRightCell = this.GetCell(new Point(right, bottom));
        }

        /// <summary>
        /// Hides the current editor control.
        /// </summary>
        public void HideEditor()
        {
            if (this.currentEditor != null && this.currentEditor.Visibility == Visibility.Visible)
            {
                var textEditor = this.currentEditor as TextBox;
                if (textEditor != null)
                {
                    textEditor.PreviewKeyDown -= this.TextEditorPreviewKeyDown;
                    textEditor.LostFocus -= this.TextEditorLostFocus;
                    textEditor.Loaded -= this.TextEditorLoaded;
                }

                this.sheetGrid.Children.Remove(this.currentEditor);
                this.currentEditor = null;
                this.Focus();
            }
        }

        /// <summary>
        /// Inserts an item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="updateGrid">Determines whether the grid should be updated.</param>
        /// <returns>
        /// <c>true</c> if an item was inserted, <c>false</c> otherwise.
        /// </returns>
        public bool InsertItem(int index, bool updateGrid = true)
        {
            var success = this.Operator.InsertItem(index);
            if (success)
            {
                if (updateGrid)
                {
                    this.UpdateGridContent();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Changes the current cell with the specified delta.
        /// </summary>
        /// <param name="deltaRows">The change in rows.</param>
        /// <param name="deltaColumns">The change in columns.</param>
        public void ChangeCurrentCell(int deltaRows, int deltaColumns)
        {
            int row = this.CurrentCell.Row;
            int column = this.CurrentCell.Column;
            row += deltaRows;
            column += deltaColumns;
            if (row < 0)
            {
                row = this.Rows - 1;
                column--;
            }

            if (row >= this.Rows && (!this.CanInsertRows || !this.EasyInsert))
            {
                column++;
                row = 0;
                if (column >= this.Columns)
                {
                    column = 0;
                }
            }

            if (column < 0)
            {
                column = 0;
                row--;
                if (row < 0)
                {
                    row = this.Rows - 1;
                }
            }

            if (column >= this.Columns && (!this.CanInsertColumns || !this.EasyInsert))
            {
                column = 0;
                row++;
                if (row >= this.Rows)
                {
                    row = 0;
                }
            }

            this.CurrentCell = new CellRef(row, column);
            this.SelectionCell = new CellRef(row, column);
            this.ScrollIntoView(this.CurrentCell);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see
        /// cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> .
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.sheetScrollViewer = this.Template.FindName(PartSheetScrollViewer, this) as ScrollViewer;
            this.sheetGrid = this.Template.FindName(PartSheetGrid, this) as Grid;
            this.columnScrollViewer = this.Template.FindName(PartColumnScrollViewer, this) as ScrollViewer;
            this.columnGrid = this.Template.FindName(PartColumnGrid, this) as Grid;
            this.rowScrollViewer = this.Template.FindName(PartRowScrollViewer, this) as ScrollViewer;
            this.rowGrid = this.Template.FindName(PartRowGrid, this) as Grid;
            this.rowSelectionBackground = this.Template.FindName(PartRowSelectionBackground, this) as Border;
            this.columnSelectionBackground = this.Template.FindName(PartColumnSelectionBackground, this) as Border;
            this.selectionBackground = this.Template.FindName(PartSelectionBackground, this) as Border;
            this.currentBackground = this.Template.FindName(PartCurrentBackground, this) as Border;
            this.selection = this.Template.FindName(PartSelection, this) as Border;
            this.autoFillSelection = this.Template.FindName(PartAutoFillSelection, this) as Border;
            this.autoFillBox = this.Template.FindName(PartAutoFillBox, this) as Border;
            this.topLeft = this.Template.FindName(PartTopLeft, this) as Border;

            if (this.sheetScrollViewer == null)
            {
                throw new Exception(PartSheetScrollViewer + " not found.");
            }

            if (this.rowScrollViewer == null)
            {
                throw new Exception(PartRowScrollViewer + " not found.");
            }

            if (this.columnScrollViewer == null)
            {
                throw new Exception(PartColumnScrollViewer + " not found.");
            }

            if (this.topLeft == null)
            {
                throw new Exception(PartTopLeft + " not found.");
            }

            if (this.autoFillBox == null)
            {
                throw new Exception(PartAutoFillBox + " not found.");
            }

            if (this.columnGrid == null)
            {
                throw new Exception(PartColumnGrid + " not found.");
            }

            if (this.rowGrid == null)
            {
                throw new Exception(PartRowGrid + " not found.");
            }

            if (this.sheetGrid == null)
            {
                throw new Exception(PartSheetGrid + " not found.");
            }

            this.sheetScrollViewer.ScrollChanged += this.ScrollViewerScrollChanged;
            this.rowScrollViewer.ScrollChanged += this.RowScrollViewerScrollChanged;
            this.columnScrollViewer.ScrollChanged += this.ColumnScrollViewerScrollChanged;

            this.topLeft.MouseLeftButtonDown += this.TopLeftMouseLeftButtonDown;
            this.autoFillBox.MouseLeftButtonDown += this.AutoFillBoxMouseLeftButtonDown;
            this.columnGrid.MouseLeftButtonDown += this.ColumnGridMouseLeftButtonDown;
            this.columnGrid.MouseMove += this.ColumnGridMouseMove;
            this.columnGrid.MouseLeftButtonUp += this.ColumnGridMouseLeftButtonUp;
            this.rowGrid.MouseLeftButtonDown += this.RowGridMouseLeftButtonDown;
            this.rowGrid.MouseMove += this.RowGridMouseMove;
            this.rowGrid.MouseLeftButtonUp += this.RowGridMouseLeftButtonUp;

            this.columnGrid.Loaded += this.ColumnGridLoaded;

            this.sheetGrid.SizeChanged += this.ColumnGridSizeChanged;
            this.sheetGrid.MouseLeftButtonDown += this.SheetGridMouseLeftButtonDown;

            this.autoFiller = new AutoFiller(this.GetCellValue, this.TrySetCellValue);

            this.autoFillToolTip = new ToolTip
                                       {
                                           Placement = PlacementMode.Bottom,
                                           PlacementTarget = this.autoFillSelection
                                       };

            this.UpdateGridContent();
            this.SelectedCellsChanged();

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, (s, e) => this.Copy()));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, (s, e) => this.Cut()));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (s, e) => this.Paste()));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, (s, e) => this.Delete()));
        }

        /// <summary>
        /// Pastes the content from the clipboard to the current selection.
        /// </summary>
        public void Paste()
        {
            object[,] values = null;

            var dataObject = Clipboard.GetDataObject();
            if (dataObject != null)
            {
                var data = dataObject.GetData(typeof(DataGrid));
                values = data as object[,];
            }

            if (values == null && Clipboard.ContainsText())
            {
                string text = Clipboard.GetText().Trim();
                values = TextToArray(text);
            }

            if (values == null)
            {
                return;
            }

            int rowMin = Math.Min(this.CurrentCell.Row, this.SelectionCell.Row);
            int columnMin = Math.Min(this.CurrentCell.Column, this.SelectionCell.Column);
            int rowMax = Math.Max(this.CurrentCell.Row, this.SelectionCell.Row);
            int columnMax = Math.Max(this.CurrentCell.Column, this.SelectionCell.Column);

            int rows = values.GetUpperBound(0) + 1;
            int columns = values.GetUpperBound(1) + 1;

            for (int i = rowMin; i <= rowMax || i < rowMin + rows; i++)
            {
                if (i >= this.Rows)
                {
                    if (!this.InsertItem(-1))
                    {
                        break;
                    }
                }

                for (int j = columnMin; j <= columnMax || j < columnMin + columns; j++)
                {
                    object value = values[(i - rowMin) % rows, (j - columnMin) % columns];
                    this.TrySetCellValue(new CellRef(i, j), value);
                }
            }

            this.CurrentCell = new CellRef(rowMin, columnMin);
            this.SelectionCell = new CellRef(
                Math.Max(rowMax, rowMin + rows - 1),
                Math.Max(columnMax, columnMin + columns - 1));
        }

        /// <summary>
        /// Scroll the specified cell into view.
        /// </summary>
        /// <param name="cellRef">The cell reference.</param>
        public void ScrollIntoView(CellRef cellRef)
        {
            var pos0 = this.GetPosition(cellRef);
            var pos1 = this.GetPosition(new CellRef(cellRef.Row + 1, cellRef.Column + 1));

            // todo: get correct size
            const double ScrollBarWidth = 20;
            const double ScrollBarHeight = 20;

            if (pos0.X < this.sheetScrollViewer.HorizontalOffset)
            {
                this.sheetScrollViewer.ScrollToHorizontalOffset(pos0.X);
            }

            if (pos1.X > this.sheetScrollViewer.HorizontalOffset + this.sheetScrollViewer.ActualWidth - ScrollBarWidth)
            {
                this.sheetScrollViewer.ScrollToHorizontalOffset(
                    Math.Max(pos1.X - this.sheetScrollViewer.ActualWidth + ScrollBarWidth, 0));
            }

            if (pos0.Y < this.sheetScrollViewer.VerticalOffset)
            {
                this.sheetScrollViewer.ScrollToVerticalOffset(pos0.Y);
            }

            if (pos1.Y > this.sheetScrollViewer.VerticalOffset + this.sheetScrollViewer.ActualHeight - ScrollBarHeight)
            {
                this.sheetScrollViewer.ScrollToVerticalOffset(
                    Math.Max(pos1.Y - this.sheetScrollViewer.ActualHeight + ScrollBarHeight, 0));
            }
        }

        /// <summary>
        /// Shows the edit control for the current cell.
        /// </summary>
        /// <returns>
        /// True if an edit control is shown.
        /// </returns>
        public bool ShowEditControl()
        {
            this.HideEditor();
            var pd = this.GetPropertyDefinition(this.CurrentCell);
            if (pd == null || pd.IsReadOnly)
            {
                return false;
            }

            if (this.CurrentCell.Row >= this.Rows || this.CurrentCell.Column >= this.Columns)
            {
                return false;
            }

            var item = this.GetItem(this.CurrentCell);
            this.currentEditor = this.CreateEditControl(this.CurrentCell, pd);

            if (this.currentEditor == null)
            {
                return false;
            }

            this.currentEditor.SourceUpdated += this.CurrentCellSourceUpdated;

            this.SetElementDataContext(this.currentEditor, pd, item);

            var textEditor = this.currentEditor as TextBox;
            if (textEditor != null)
            {
                this.currentEditor.Visibility = Visibility.Hidden;
                textEditor.PreviewKeyDown += this.TextEditorPreviewKeyDown;
                textEditor.LostFocus += this.TextEditorLostFocus;
                textEditor.Loaded += this.TextEditorLoaded;
            }

            this.editingCells = this.SelectedCells.ToList();

            Grid.SetColumn(this.currentEditor, this.CurrentCell.Column);
            Grid.SetRow(this.currentEditor, this.CurrentCell.Row);

            this.sheetGrid.Children.Add(this.currentEditor);

            if (this.currentEditor.Visibility == Visibility.Visible)
            {
                this.currentEditor.Focus();
            }

            return true;
        }

        /// <summary>
        /// Shows the text box editor.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the text editor was shown, <c>false</c> otherwise.
        /// </returns>
        public bool ShowTextBoxEditControl()
        {
            var textEditor = this.currentEditor as TextBox;
            if (textEditor != null)
            {
                textEditor.Visibility = Visibility.Visible;
                textEditor.Focus();
                textEditor.CaretIndex = textEditor.Text.Length;
                textEditor.SelectAll();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Opens the combo box control.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the combo box was shown, <c>false</c> otherwise.
        /// </returns>
        public bool OpenComboBoxControl()
        {
            var comboBox = this.currentEditor as ComboBox;
            if (comboBox != null)
            {
                comboBox.IsDropDownOpen = true;
                comboBox.Focus();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Exports the grid to comma separated values.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <returns>
        /// The comma separated values string.
        /// </returns>
        public string ToCsv(string separator = ";")
        {
            var sb = new StringBuilder();
            for (int j = 0; j < this.Columns; j++)
            {
                var h = this.GetColumnHeader(j).ToString();
                h = CsvEncodeString(h);
                if (sb.Length > 0)
                {
                    sb.Append(separator);
                }

                sb.Append(h);
            }

            sb.AppendLine();
            sb.Append(this.SheetToString(";", true));
            return sb.ToString();
        }

        /// <summary>
        /// Tries to set the value in the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True if the value was set.
        /// </returns>
        public virtual bool TrySetCellValue(CellRef cell, object value)
        {
            if (this.ItemsSource != null)
            {
                var current = this.GetItem(cell);

                var pd = this.GetPropertyDefinition(cell);
                if (pd == null)
                {
                    return false;
                }

                object convertedValue;
                if (current != null && !pd.IsReadOnly && TryConvert(value, pd.PropertyType, out convertedValue))
                {
                    if (pd.Descriptor != null)
                    {
                        pd.Descriptor.SetValue(current, convertedValue);
                    }
                    else
                    {
                        this.SetValue(cell, convertedValue);

                        if (!(this.ItemsSource is INotifyCollectionChanged))
                        {
                            this.UpdateCellContent(cell);
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates the display control for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="pd">The property definition.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        /// The display control.
        /// </returns>
        protected virtual UIElement CreateDisplayControl(CellRef cell, PropertyDefinition pd, object item)
        {
            FrameworkElement element = null;

            if (item == null)
            {
                item = this.GetItem(cell);
            }

            if (pd == null)
            {
                pd = this.GetPropertyDefinition(cell);
            }

            if (pd != null && item != null)
            {
                element = this.CreateDisplayControl(cell, pd);

                this.SetElementDataContext(element, pd, item);

                element.SourceUpdated += this.CurrentCellSourceUpdated;

                element.VerticalAlignment = VerticalAlignment.Center;
                element.HorizontalAlignment = pd.HorizontalAlignment;
            }

            return element;
        }

        /// <summary>
        /// Creates a new instance of the specified type.
        /// </summary>
        /// <param name="itemType">The type.</param>
        /// <returns>
        /// The new instance.
        /// </returns>
        protected virtual object CreateInstance(Type itemType)
        {
            if (this.CreateItem != null)
            {
                return this.CreateItem();
            }

            // TODO: the item type may not have a parameterless constructor!
            return Activator.CreateInstance(itemType);
        }

        /// <summary>
        /// Generates column definitions based on ItemsSource
        /// </summary>
        protected virtual void GenerateColumnDefinitions()
        {
            this.Operator.GenerateColumnDefinitions();
        }

        /// <summary>
        /// Handles KeyDown events on the grid.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            bool control = (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None;
            bool shift = (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None;
            bool alt = (Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None;

            int row = shift ? this.SelectionCell.Row : this.CurrentCell.Row;
            int column = shift ? this.SelectionCell.Column : this.CurrentCell.Column;

            switch (e.Key)
            {
                case Key.Enter:
                    if (this.InputDirection == InputDirection.Vertical)
                    {
                        this.ChangeCurrentCell(shift ? -1 : 1, 0);
                    }
                    else
                    {
                        this.ChangeCurrentCell(0, shift ? -1 : 1);
                    }

                    e.Handled = true;
                    return;

                case Key.Up:
                    if (row > 0)
                    {
                        row--;
                    }

                    if (this.endPressed)
                    {
                        row = this.FindNextRow(row, column, -1);
                    }

                    if (control)
                    {
                        row = 0;
                    }

                    break;
                case Key.Down:
                    if (row < this.Rows - 1 || (this.CanInsertRows && this.EasyInsert))
                    {
                        row++;
                    }

                    if (this.endPressed)
                    {
                        row = this.FindNextRow(row, column, 1);
                    }

                    if (control)
                    {
                        row = this.Rows - 1;
                    }

                    break;
                case Key.Left:
                    if (column > 0)
                    {
                        column--;
                    }

                    if (this.endPressed)
                    {
                        column = this.FindNextColumn(row, column, -1);
                    }

                    if (control)
                    {
                        column = 0;
                    }

                    break;
                case Key.Right:
                    if (column < this.Columns - 1 || (this.CanInsertColumns && this.EasyInsert))
                    {
                        column++;
                    }

                    if (this.endPressed)
                    {
                        column = this.FindNextColumn(row, column, 1);
                    }

                    if (control)
                    {
                        column = this.Columns - 1;
                    }

                    break;
                case Key.End:

                    // Flag that the next key should be handled differently
                    this.endPressed = true;
                    e.Handled = true;
                    return;
                case Key.Home:
                    column = 0;
                    row = 0;
                    break;
                case Key.Back:
                case Key.Delete:
                    this.Delete();
                    e.Handled = true;
                    return;
                case Key.F2:
                    if (this.ShowTextBoxEditControl())
                    {
                        e.Handled = true;
                    }

                    return;
                case Key.F4:
                    if (this.OpenComboBoxControl())
                    {
                        e.Handled = true;
                    }

                    return;
                case Key.Space:
                    if (this.ToggleCheck())
                    {
                        e.Handled = true;
                    }

                    return;
                case Key.A:
                    if (control)
                    {
                        this.SelectAll();
                        e.Handled = true;
                    }

                    return;
                case Key.C:
                    if (control && alt)
                    {
                        Clipboard.SetText(this.ToCsv());
                        e.Handled = true;
                    }

                    return;
                default:
                    return;
            }

            if (e.Key != Key.End)
            {
                // Turn of special handling after the first key after End was pressed.
                this.endPressed = false;
            }

            if (shift)
            {
                this.SelectionCell = new CellRef(row, column);
            }
            else
            {
                this.CurrentCell = new CellRef(row, column);
                this.SelectionCell = new CellRef(row, column);
            }

            this.ScrollIntoView(new CellRef(row, column));

            e.Handled = true;
        }

        /// <summary>
        /// Handles mouse left button down events.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.Focus();
            base.OnMouseLeftButtonDown(e);
            this.HandleButtonDown(e);
            e.Handled = true;
        }

        /// <summary>
        /// Handles mouse left button up events.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            this.OnMouseUp(e);

            this.isCapturing = false;
            this.ReleaseMouseCapture();
            Mouse.OverrideCursor = null;

            if (this.autoFillSelection.Visibility == Visibility.Visible)
            {
                this.autoFiller.AutoFill(this.CurrentCell, this.SelectionCell, this.AutoFillCell);

                this.autoFillSelection.Visibility = Visibility.Hidden;
                this.autoFillToolTip.IsOpen = false;
            }
        }

        /// <summary>
        /// Handles mouse move events.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!this.isCapturing)
            {
                return;
            }

            bool isInAutoFillMode = this.autoFillSelection.Visibility == Visibility.Visible;

            var pos = e.GetPosition(this.sheetGrid);
            var cellRef = this.GetCell(pos, isInAutoFillMode, this.CurrentCell);

            if (cellRef.Column == -1 || cellRef.Row == -1)
            {
                return;
            }

            if (cellRef.Row >= this.Rows && (!this.CanInsertRows || !this.EasyInsert))
            {
                return;
            }

            if (cellRef.Column > this.Columns && (!this.CanInsertColumns || !this.EasyInsert))
            {
                return;
            }

            if (isInAutoFillMode)
            {
                this.AutoFillCell = cellRef;
                object result;
                if (this.autoFiller.TryExtrapolate(
                    cellRef,
                    this.CurrentCell,
                    this.SelectionCell,
                    this.AutoFillCell,
                    out result))
                {
                    var formatString = this.GetFormatString(cellRef);
                    this.autoFillToolTip.Content = FormatValue(result, formatString);
                }

                this.autoFillToolTip.Placement = PlacementMode.Relative;
                var p = e.GetPosition(this.autoFillSelection);
                this.autoFillToolTip.HorizontalOffset = p.X + 8;
                this.autoFillToolTip.VerticalOffset = p.Y + 8;
            }
            else
            {
                this.SelectionCell = cellRef;
            }

            this.ScrollIntoView(cellRef);
        }

        /// <summary>
        /// Handles mouse wheel preview events.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            if (control)
            {
                double s = 1 + (e.Delta * 0.0004);
                var tg = new TransformGroup();
                if (this.LayoutTransform != null)
                {
                    tg.Children.Add(this.LayoutTransform);
                }

                tg.Children.Add(new ScaleTransform(s, s));
                this.LayoutTransform = tg;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles text input events.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);

            // do not allow special characters (including backspace, tab, enter)
            // it is particularly bad to add backspace characters to the cell, since this may not be XML serialized...
            if (e.Text.Length == 0 || e.Text[0] < 32)
            {
                return;
            }

            if (this.currentEditor == null)
            {
                this.ShowEditControl();
            }

            var textEditor = this.currentEditor as TextBox;
            if (textEditor != null)
            {
                this.ShowTextBoxEditControl();
                textEditor.Text = e.Text;
                textEditor.CaretIndex = textEditor.Text.Length;
            }

            e.Handled = true;
        }

        /// <summary>
        /// Updates all cells.
        /// </summary>
        protected void UpdateAllCells()
        {
            foreach (var element in this.cellMap.Values)
            {
                var cell = this.GetCellRefFromUIElement(element);
                this.UpdateCellContent(cell);
            }
        }

        /// <summary>
        /// Updates the content of the specified cell.
        /// </summary>
        /// <param name="cellRef">The cell reference.</param>
        protected void UpdateCellContent(CellRef cellRef)
        {
            var c = this.GetCellElement(cellRef);
            if (c != null)
            {
                this.sheetGrid.Children.Remove(c);
                this.cellInsertionIndex--;
                this.cellMap.Remove(cellRef.GetHashCode());
            }

            this.InsertDisplayControl(cellRef);
        }

        /// <summary>
        /// Determines whether all elements in the specified array are serializable.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>
        /// <c>true</c> if all elements of the array are serializable, <c>false</c> otherwise.
        /// </returns>
        private static bool AreAllElementsSerializable(object[,] array)
        {
            int m = array.GetLength(0);
            int n = array.GetLength(1);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (array[i, j] == null)
                    {
                        continue;
                    }

                    var type = array[i, j].GetType();
                    if (!type.IsSerializable)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Clamps a value between a minimum and maximum limit.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        private static int Clamp(int value, int min, int max)
        {
            int result = value;
            if (result > max)
            {
                result = max;
            }

            if (result < min)
            {
                result = min;
            }

            return result;
        }

        /// <summary>
        /// Encodes the specified string for use in a comma separated value file.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        /// The encoded string.
        /// </returns>
        private static string CsvEncodeString(string input)
        {
            input = input.Replace("\"", "\"\"");
            if (input.Contains(";") || input.Contains("\""))
            {
                input = "\"" + input + "\"";
            }

            return input;
        }

        /// <summary>
        /// Formats value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatString">The format string.</param>
        /// <returns>
        /// The format value.
        /// </returns>
        private static string FormatValue(object value, string formatString)
        {
            if (string.IsNullOrEmpty(formatString))
            {
                return value != null ? value.ToString() : null;
            }

            if (!formatString.Contains("{0"))
            {
                formatString = "{0:" + formatString + "}";
            }

            return string.Format(formatString, value);
        }

        /// <summary>
        /// Gets property descriptors from one instance.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="itemType">The target item type.</param>
        /// <returns>
        /// The <see cref="PropertyDescriptorCollection" />.
        /// </returns>
        private static PropertyDescriptorCollection GetPropertiesFromInstance(IList items, Type itemType)
        {
            foreach (var item in items)
            {
                if (item != null && item.GetType() == itemType)
                {
                    return TypeDescriptor.GetProperties(item);
                }
            }

            return new PropertyDescriptorCollection(new PropertyDescriptor[0]);
        }

        /// <summary>
        /// The set element position.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="cell">The cell.</param>
        private static void SetElementPosition(UIElement element, CellRef cell)
        {
            Grid.SetColumn(element, cell.Column);
            Grid.SetRow(element, cell.Row);
        }

        /// <summary>
        /// Splits a string separated by \n and \t into an array.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// An 2-dimensional array of strings.
        /// </returns>
        private static object[,] TextToArray(string text)
        {
            int rows = 0;
            int columns = 0;
            var lines = text.Split('\n');
            foreach (string line in lines)
            {
                rows++;
                var fields = line.Split('\t');
                if (fields.Length > columns)
                {
                    columns = fields.Length;
                }
            }

            if (rows == 0 || columns == 0)
            {
                return null;
            }

            var result = new object[rows, columns];
            int row = 0;
            foreach (string line in lines)
            {
                var fields = line.Split('\t');

                int column = 0;
                foreach (string field in fields)
                {
                    result[row, column] = field.Trim(" \r\n\t".ToCharArray());
                    column++;
                }

                row++;
            }

            return result;
        }

        /// <summary>
        /// Tries to convert an object to the specified type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="convertedValue">The converted value.</param>
        /// <returns>
        /// True if conversion was successful.
        /// </returns>
        private static bool TryConvert(object value, Type targetType, out object convertedValue)
        {
            try
            {
                if (value != null && targetType == value.GetType())
                {
                    convertedValue = value;
                    return true;
                }

                if (targetType == typeof(string))
                {
                    convertedValue = value != null ? value.ToString() : null;
                    return true;
                }

                if (targetType == typeof(double))
                {
                    convertedValue = Convert.ToDouble(value);
                    return true;
                }

                if (targetType == typeof(int))
                {
                    convertedValue = Convert.ToInt32(value);
                    return true;
                }

                var converter = TypeDescriptor.GetConverter(targetType);
                if (value != null && converter.CanConvertFrom(value.GetType()))
                {
                    convertedValue = converter.ConvertFrom(value);
                    return true;
                }

                if (value != null)
                {
                    var parseMethod = targetType.GetMethod("Parse", new[] { value.GetType(), typeof(IFormatProvider) });
                    if (parseMethod != null)
                    {
                        convertedValue = parseMethod.Invoke(null, new[] { value, CultureInfo.CurrentCulture });
                        return true;
                    }
                }

                convertedValue = null;
                return false;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                convertedValue = null;
                return false;
            }
        }

        /// <summary>
        /// Handles changes in the currents cell.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataTransferEventArgs" /> instance containing the event data.</param>
        private void CurrentCellSourceUpdated(object sender, DataTransferEventArgs e)
        {
            // The source of the binding for the current cell was updated
            // (e.g. check box (display control) was changed or a combo box (edit control) was changed
            var value = this.GetCellValue(this.CurrentCell);

            // Set the same value in all selected cells.
            foreach (var cell in this.SelectedCells)
            {
                if (this.CurrentCell.Equals(cell))
                {
                    // this value should already be set by the binding
                    continue;
                }

                this.TrySetCellValue(cell, value);
            }
        }

        /// <summary>
        /// Adds the display control for the specified cell.
        /// </summary>
        /// <param name="cellRef">The cell reference.</param>
        private void AddDisplayControl(CellRef cellRef)
        {
            var e = this.CreateDisplayControl(cellRef, null, null);
            if (e == null)
            {
                return;
            }

            SetElementPosition(e, cellRef);
            this.sheetGrid.Children.Add(e);
            this.cellMap.Add(cellRef.GetHashCode(), e);
        }

        /// <summary>
        /// The add item cell mouse left button down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void AddItemCellMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            this.InsertItem(-1);
            e.Handled = true;
        }

        /// <summary>
        /// The auto fill box mouse left button down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void AutoFillBoxMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Show the auto-fill selection border
            this.autoFillSelection.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// The column grid loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ColumnGridLoaded(object sender, RoutedEventArgs e)
        {
            this.UpdateColumnWidths();
        }

        /// <summary>
        /// The column grid mouse left button down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ColumnGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            int column = this.GetCell(e.GetPosition(this.columnGrid)).Column;
            if (column >= 0)
            {
                bool shift = (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None;
                this.CurrentCell = shift ? new CellRef(0, this.CurrentCell.Column) : new CellRef(0, column);

                this.SelectionCell = new CellRef(this.Rows - 1, column);
            }

            this.isSelectingColumns = true;
            e.Handled = true;
        }

        /// <summary>
        /// The column grid mouse left button up.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ColumnGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.columnGrid.ReleaseMouseCapture();
            this.isSelectingColumns = false;
        }

        /// <summary>
        /// The column grid mouse move.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ColumnGridMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isSelectingColumns)
            {
                return;
            }

            int column = this.GetCell(e.GetPosition(this.columnGrid)).Column;
            if (column >= 0)
            {
                this.SelectionCell = new CellRef(this.Rows - 1, column);

                // e.Handled = true;
            }
        }

        /// <summary>
        /// The column grid size changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ColumnGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateColumnWidths();
        }

        /// <summary>
        /// Handles changes in the column scroll viewer.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ColumnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.sheetScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        /// <summary>
        /// Handles the column splitter change completed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dragCompletedEventArgs">The drag completed event args.</param>
        private void ColumnSplitterChangeCompleted(object sender, DragCompletedEventArgs dragCompletedEventArgs)
        {
            var gs = (GridSplitter)sender;
            var tt = gs.ToolTip as ToolTip;
            if (tt != null)
            {
                tt.IsOpen = false;
                gs.ToolTip = null;
            }
        }

        /// <summary>
        /// Handles the column splitter change delta event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ColumnSplitterChangeDelta(object sender, DragDeltaEventArgs e)
        {
            var gs = (GridSplitter)sender;
            var tt = gs.ToolTip as ToolTip;

            if (tt == null)
            {
                tt = new ToolTip();
                gs.ToolTip = tt;
                tt.IsOpen = true;
            }

            int column = Grid.GetColumn(gs);
            var width = this.columnGrid.ColumnDefinitions[column].ActualWidth;
            tt.Content = string.Format("Width: {0:0.#}", width); // device-independent units

            tt.PlacementTarget = this.columnGrid;
            tt.Placement = PlacementMode.Relative;
            var p = Mouse.GetPosition(this.columnGrid);
            tt.HorizontalOffset = p.X;
            tt.VerticalOffset = gs.ActualHeight + 4;
        }

        /// <summary>
        /// The column splitter change started.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dragStartedEventArgs">The drag started event args.</param>
        private void ColumnSplitterChangeStarted(object sender, DragStartedEventArgs dragStartedEventArgs)
        {
            this.ColumnSplitterChangeDelta(sender, null);
        }

        /// <summary>
        /// The column splitter double click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ColumnSplitterDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int column = Grid.GetColumn((GridSplitter)sender);
            this.AutoSizeColumn(column);
        }

        /// <summary>
        /// Handles the delete command.
        /// </summary>
        private void Delete()
        {
            foreach (var cell in this.SelectedCells)
            {
                this.TrySetCellValue(cell, null);
            }
        }

        /// <summary>
        /// Deletes the selected columns.
        /// </summary>
        private void DeleteColumns()
        {
            if (this.IsIListIList() && this.ColumnHeadersSource != null)
            {
                int from = Math.Min(this.CurrentCell.Column, this.SelectionCell.Column);
                int to = Math.Max(this.CurrentCell.Column, this.SelectionCell.Column);
                for (int i = to; i >= from; i--)
                {
                    this.ColumnHeadersSource.RemoveAt(i);
                }
            }

            if (this.ItemsInColumns)
            {
                int from = Math.Min(this.CurrentCell.Column, this.SelectionCell.Column);
                int to = Math.Max(this.CurrentCell.Column, this.SelectionCell.Column);
                for (int i = to; i >= from; i--)
                {
                    this.DeleteItem(i, false);
                }
            }

            this.UpdateGridContent();

            int maxColumn = this.Columns > 0 ? this.Columns - 1 : 0;
            if (this.CurrentCell.Column > maxColumn)
            {
                this.CurrentCell = new CellRef(maxColumn, this.CurrentCell.Column);
            }

            if (this.SelectionCell.Column > maxColumn)
            {
                this.SelectionCell = new CellRef(maxColumn, this.SelectionCell.Column);
            }
        }

        /// <summary>
        /// Deletes the selected rows.
        /// </summary>
        private void DeleteRows()
        {
            int from = Math.Min(this.CurrentCell.Row, this.SelectionCell.Row);
            int to = Math.Max(this.CurrentCell.Row, this.SelectionCell.Row);
            for (int i = to; i >= from; i--)
            {
                this.DeleteItem(i, false);
            }

            this.UpdateGridContent();

            int maxRow = this.Rows > 0 ? this.Rows - 1 : 0;
            if (this.CurrentCell.Row > maxRow)
            {
                this.CurrentCell = new CellRef(maxRow, this.CurrentCell.Column);
            }

            if (this.SelectionCell.Row > maxRow)
            {
                this.SelectionCell = new CellRef(maxRow, this.SelectionCell.Column);
            }
        }

        /// <summary>
        /// Enumerate the items in the specified cell range. This is used to update the SelectedItems property.
        /// </summary>
        /// <param name="cell0">The cell 0.</param>
        /// <param name="cell1">The cell 1.</param>
        /// <returns>
        /// The items enumeration.
        /// </returns>
        private IEnumerable EnumerateItems(CellRef cell0, CellRef cell1)
        {
            var list = this.ItemsSource;
            if (list != null)
            {
                int index0 = this.ItemsInRows ? cell0.Row : cell0.Column;
                int index1 = this.ItemsInRows ? cell1.Row : cell1.Column;
                int min = Math.Min(index0, index1);
                int max = Math.Max(index0, index1);
                for (int index = min; index <= max; index++)
                {
                    if (index >= 0 && index < list.Count)
                    {
                        yield return list[index];
                    }
                }
            }
        }

        /// <summary>
        /// Finds the next column that contains an empty cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        /// <param name="deltaColumn">The delta column.</param>
        /// <returns>
        /// The new column.
        /// </returns>
        private int FindNextColumn(int row, int column, int deltaColumn)
        {
            while (column >= 0 && column < this.Columns - 1)
            {
                var v = this.GetCellValue(new CellRef(row, column));
                if (v == null || string.IsNullOrEmpty(v.ToString()))
                {
                    break;
                }

                column += deltaColumn;
            }

            return column;
        }

        /// <summary>
        /// Finds the next row that contains an empty cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        /// <param name="deltaRow">The delta row.</param>
        /// <returns>
        /// The new row.
        /// </returns>
        private int FindNextRow(int row, int column, int deltaRow)
        {
            while (row >= 0 && row < this.Rows)
            {
                var v = this.GetCellValue(new CellRef(row, column));
                if (v == null || string.IsNullOrEmpty(v.ToString()))
                {
                    break;
                }

                row += deltaRow;
            }

            if (row >= this.Rows && this.Rows > 0)
            {
                row = this.Rows - 1;
            }

            return row;
        }

        /// <summary>
        /// Gets a cell reference from the specified display control.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The cell reference.
        /// </returns>
        private CellRef GetCellRefFromUIElement(UIElement element)
        {
            int row = Grid.GetRow(element);
            int column = Grid.GetColumn(element);
            return new CellRef(row, column);
        }

        /// <summary>
        /// Gets the column element for the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns>
        /// The column element.
        /// </returns>
        private FrameworkElement GetColumnElement(int column)
        {
            FrameworkElement headerElement;
            if (this.columnHeaderMap.TryGetValue(column, out headerElement))
            {
                return headerElement;
            }

            throw new InvalidOperationException("Invalid header column: " + column);
        }

        /// <summary>
        /// Gets the column header for the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns>
        /// The column header.
        /// </returns>
        private object GetColumnHeader(int column)
        {
            if (this.ItemsInRows)
            {
                if (column < this.PropertyDefinitions.Count)
                {
                    return this.PropertyDefinitions[column].Header ?? CellRef.ToColumnName(column);
                }
            }

            return CellRef.ToColumnName(column);
        }

        /// <summary>
        /// Gets the column width for the specified column.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>
        /// The column width.
        /// </returns>
        private GridLength GetColumnWidth(int i)
        {
            if (i < this.ColumnDefinitions.Count)
            {
                var cd = this.ColumnDefinitions[i] as ColumnDefinition;
                if (cd != null)
                {
                    if (cd.Width.Value < 0)
                    {
                        return this.DefaultColumnWidth;
                    }

                    return cd.Width;
                }
            }

            return this.DefaultColumnWidth;
        }

        /// <summary>
        /// Gets the format string for the specified cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The format string.
        /// </returns>
        private string GetFormatString(CellRef cell)
        {
            var pd = this.GetPropertyDefinition(cell);
            return pd != null ? pd.FormatString : null;
        }

        /// <summary>
        /// Gets the item for the specified cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The item.
        /// </returns>
        private object GetItem(CellRef cell)
        {
            return this.Operator.GetItem(cell);
        }

        /// <summary>
        /// Gets the item index for the specified cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>
        /// The get item index.
        /// </returns>
        private int GetItemIndex(CellRef cell)
        {
            if (this.WrapItems)
            {
                return this.ItemsInRows ? (cell.Row * this.Columns) + cell.Column : (cell.Column * this.Rows) + cell.Row;
            }

            return this.ItemsInRows ? cell.Row : cell.Column;
        }

        /// <summary>
        /// Gets the column/row definition for the specified cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <returns>
        /// The column/row definition.
        /// </returns>
        private PropertyDefinition GetPropertyDefinition(CellRef cell)
        {
            int index = this.ItemsInRows ? cell.Column : cell.Row;

            if (index < this.PropertyDefinitions.Count)
            {
                return this.PropertyDefinitions[index];
            }

            return null;
        }

        /// <summary>
        /// Gets the row header.
        /// </summary>
        /// <param name="j">The j.</param>
        /// <returns>
        /// The get row header.
        /// </returns>
        private object GetRowHeader(int j)
        {
            if (this.ItemsInColumns)
            {
                return j < this.RowDefinitions.Count ? this.RowDefinitions[j].Header : CellRef.ToRowName(j);
            }

            return CellRef.ToRowName(j);
        }

        /// <summary>
        /// Handles the button down event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        private void HandleButtonDown(MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(this.sheetGrid);
            var cellRef = this.GetCell(pos);

            if (cellRef.Column == -1 || cellRef.Row == -1)
            {
                return;
            }

            if (cellRef.Row >= this.Rows && (!this.CanInsertRows || !this.EasyInsert))
            {
                return;
            }

            if (cellRef.Column > this.Columns && (!this.CanInsertColumns || !this.EasyInsert))
            {
                return;
            }

            if (this.autoFillSelection.Visibility == Visibility.Visible)
            {
                this.AutoFillCell = cellRef;
                Mouse.OverrideCursor = this.autoFillBox.Cursor;
                this.autoFillToolTip.IsOpen = true;
            }
            else
            {
                bool shift = (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None;

                if (!shift)
                {
                    this.CurrentCell = cellRef;
                }

                this.SelectionCell = cellRef;
                this.ScrollIntoView(cellRef);
                Mouse.OverrideCursor = this.sheetGrid.Cursor;
            }

            this.CaptureMouse();
            this.isCapturing = true;
        }

        /// <summary>
        /// Inserts columns at the selected column.
        /// </summary>
        private void InsertColumns()
        {
            if (this.IsIListIList())
            {
                int from = Math.Min(this.CurrentCell.Column, this.SelectionCell.Column);
                int to = Math.Max(this.CurrentCell.Column, this.SelectionCell.Column);
                for (int i = from; i <= to; i++)
                {
                    this.InsertColumnHeader(i);
                }
            }

            if (this.ItemsInColumns)
            {
                int from = Math.Min(this.CurrentCell.Column, this.SelectionCell.Column);
                int to = Math.Max(this.CurrentCell.Column, this.SelectionCell.Column);
                for (int i = 0; i < to - from + 1; i++)
                {
                    this.InsertItem(from, false);
                }
            }

            this.UpdateGridContent();
        }

        /// <summary>
        /// Insert column header to ColumnHeadersSource.
        /// </summary>
        /// <param name="index">The position.</param>
        private void InsertColumnHeader(int index)
        {
            if (this.ColumnHeadersSource == null)
            {
                return;
            }

            var newItem = this.CreateColumnHeader(index);
            if (index >= 0 && index < this.ColumnHeadersSource.Count)
            {
                this.ColumnHeadersSource.Insert(index, newItem);
            }
            else
            {
                this.ColumnHeadersSource.Add(newItem);
            }
        }

        /// <summary>
        /// Inserts the display control for the specified cell.
        /// </summary>
        /// <param name="cellRef">The cell reference.</param>
        private void InsertDisplayControl(CellRef cellRef)
        {
            var e = this.CreateDisplayControl(cellRef, null, null);
            SetElementPosition(e, cellRef);
            this.sheetGrid.Children.Insert(this.cellInsertionIndex, e);
            this.cellInsertionIndex++;
            this.cellMap.Add(cellRef.GetHashCode(), e);
        }

        /// <summary>
        /// Inserts rows at the selection.
        /// </summary>
        private void InsertRows()
        {
            int from = Math.Min(this.CurrentCell.Row, this.SelectionCell.Row);
            int to = Math.Max(this.CurrentCell.Row, this.SelectionCell.Row);
            for (int i = 0; i < to - from + 1; i++)
            {
                this.InsertItem(from, false);
            }

            this.UpdateGridContent();
        }

        /// <summary>
        /// Determines whether the item source is <see cref="IList" />&gt;<see cref="IList" />&lt;.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the item source is <see cref="IList" />&gt;<see cref="IList" />&lt;; otherwise, <c>false</c>.
        /// </returns>
        private bool IsIListIList()
        {
            var list = this.ItemsSource;
            if (list == null)
            {
                return false;
            }

            var type = list.GetType();
            return TypeHelper.IsIListIList(type);
        }

        /// <summary>
        /// Creates a display control and bind it to the item source's cell element.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
        /// <returns>
        /// The <see cref="FrameworkElement" />.
        /// </returns>
        private FrameworkElement CreateDisplayControl(CellRef cell, PropertyDefinition pd)
        {
            return this.Operator.CreateDisplayControl(cell, pd);
        }

        /// <summary>
        /// Creates a edit control and bind it to the current cell element.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <param name="pd">The <see cref="PropertyDefinition" />.</param>
        /// <returns>
        /// The <see cref="FrameworkElement" />.
        /// </returns>
        private FrameworkElement CreateEditControl(CellRef cell, PropertyDefinition pd)
        {
            return this.Operator.CreateEditControl(cell, pd);
        }

        /// <summary>
        /// Sets value to items in cell.
        /// </summary>
        /// <param name="cell">The cell reference.</param>
        /// <param name="value">The value to be set.</param>
        private void SetValue(CellRef cell, object value)
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            this.Operator.SetValue(cell, value);
        }

        /// <summary>
        /// Gets type of the element in ItemsSource.
        /// </summary>
        /// <returns>
        /// The <see cref="Type" />.
        /// </returns>
        private Type GetItemsType()
        {
            return this.Operator.GetItemsType();
        }

        /// <summary>
        /// The on content collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // TODO: update only changed rows/columns
            this.Dispatcher.Invoke(new Action(this.UpdateGridContent));
        }

        /// <summary>
        /// The row grid mouse left button down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void RowGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();

            int row = this.GetCell(e.GetPosition(this.rowGrid)).Row;
            if (row >= 0)
            {
                bool shift = (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None;
                this.CurrentCell = shift ? new CellRef(this.CurrentCell.Row, 0) : new CellRef(row, 0);

                this.SelectionCell = new CellRef(row, this.Columns - 1);
            }

            this.isSelectingRows = true;
            this.rowGrid.CaptureMouse();
            e.Handled = true;
        }

        /// <summary>
        /// The row grid mouse left button up.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void RowGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.rowGrid.ReleaseMouseCapture();
            this.isSelectingRows = false;
        }

        /// <summary>
        /// The row grid mouse move.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void RowGridMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isSelectingRows)
            {
                return;
            }

            int row = this.GetCell(e.GetPosition(this.rowGrid)).Row;
            if (row >= 0)
            {
                this.SelectionCell = new CellRef(row, this.Columns - 1);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles changes in the row scroll viewer.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void RowScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.sheetScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
        }

        /// <summary>
        /// Handles scroll changes in the scroll viewers (both horizontal and vertical).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.columnScrollViewer.ScrollToHorizontalOffset(this.sheetScrollViewer.HorizontalOffset);
            this.rowScrollViewer.ScrollToVerticalOffset(this.sheetScrollViewer.VerticalOffset);
        }

        /// <summary>
        /// The select all.
        /// </summary>
        private void SelectAll()
        {
            this.CurrentCell = new CellRef(0, 0);
            this.SelectionCell = new CellRef(this.Rows - 1, this.Columns - 1);
            this.ScrollIntoView(this.CurrentCell);
        }

        /// <summary>
        /// Formats the selected cells as a string.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="encode">Determines whether to encode the elements.</param>
        /// <returns>
        /// The string.
        /// </returns>
        private string SelectionToString(string separator, bool encode = false)
        {
            return this.ToString(this.CurrentCell, this.SelectionCell, separator, encode);
        }

        /// <summary>
        /// Converts the selected cells to an array of objects.
        /// </summary>
        /// <returns>
        /// The array.
        /// </returns>
        private object[,] SelectionToArray()
        {
            return this.ToArray(this.CurrentCell, this.SelectionCell);
        }

        /// <summary>
        /// Sets the boolean value in the selected cells.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True if cells were modified.
        /// </returns>
        private bool SetCheckInSelectedCells(bool value)
        {
            bool modified = false;
            foreach (var cell in this.SelectedCells)
            {
                var currentValue = this.GetCellValue(cell);
                if (currentValue is bool)
                {
                    if (this.TrySetCellValue(cell, value))
                    {
                        modified = true;
                    }
                }
            }

            return modified;
        }

        /// <summary>
        /// Sets the data context for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="pd">The property definition.</param>
        /// <param name="item">The item.</param>
        private void SetElementDataContext(FrameworkElement element, PropertyDefinition pd, object item)
        {
            element.DataContext = pd.Descriptor != null ? item : this.ItemsSource;
        }

        /// <summary>
        /// Handles mouse left button down on the grid sheet.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SheetGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.ShowTextBoxEditControl();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Exports the whole grid sheet to a string.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="encode">The encode.</param>
        /// <returns>
        /// The sheet to string.
        /// </returns>
        private string SheetToString(string separator, bool encode = false)
        {
            return this.ToString(new CellRef(0, 0), new CellRef(this.Rows - 1, this.Columns - 1), separator, encode);
        }

        /// <summary>
        /// Handles the loaded event for the text editor.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void TextEditorLoaded(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            tb.CaretIndex = tb.Text.Length;
            tb.SelectAll();
        }

        /// <summary>
        /// Handles lost focus events for the text editor.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void TextEditorLostFocus(object sender, RoutedEventArgs e)
        {
            this.EndTextEdit();
        }

        /// <summary>
        /// Handles key down events in the TextBox editor.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void TextEditorPreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool shift = (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None;
            var textEditor = sender as TextBox;
            if (textEditor == null)
            {
                return;
            }

            var isEverythingSelected = textEditor.SelectionLength == textEditor.Text.Length && textEditor.SelectionLength > 0;

            switch (e.Key)
            {
                case Key.Left:
                    if (textEditor.CaretIndex == 0 && !isEverythingSelected)
                    {
                        this.EndTextEdit();
                        this.OnKeyDown(e);
                        e.Handled = true;
                    }

                    break;
                case Key.Right:
                    if (textEditor.CaretIndex == textEditor.Text.Length && !isEverythingSelected)
                    {
                        this.EndTextEdit();
                        this.OnKeyDown(e);
                        e.Handled = true;
                    }

                    break;
                case Key.Down:
                case Key.Up:
                    this.EndTextEdit();
                    this.OnKeyDown(e);
                    e.Handled = true;
                    break;
                case Key.Enter:
                    this.EndTextEdit();
                    if (this.InputDirection == InputDirection.Vertical)
                    {
                        this.ChangeCurrentCell(shift ? -1 : 1, 0);
                    }
                    else
                    {
                        this.ChangeCurrentCell(0, shift ? -1 : 1);
                    }

                    e.Handled = true;
                    break;
                case Key.Escape:
                    this.EndTextEdit(false);
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Exports the specified cell range to a string.
        /// </summary>
        /// <param name="cell1">The cell 1.</param>
        /// <param name="cell2">The cell 2.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="encode">Determines whether to encode the elements.</param>
        /// <returns>
        /// The to string.
        /// </returns>
        private string ToString(CellRef cell1, CellRef cell2, string separator, bool encode = false)
        {
            int rowMin = Math.Min(cell1.Row, cell2.Row);
            int columnMin = Math.Min(cell1.Column, cell2.Column);
            int rowMax = Math.Max(cell1.Row, cell2.Row);
            int columnMax = Math.Max(cell1.Column, cell2.Column);

            var sb = new StringBuilder();

            for (int i = rowMin; i <= rowMax; i++)
            {
                if (i > rowMin)
                {
                    sb.AppendLine();
                }

                for (int j = columnMin; j <= columnMax; j++)
                {
                    string cell = this.GetCellString(new CellRef(i, j));
                    if (encode)
                    {
                        cell = CsvEncodeString(cell);
                    }

                    if (j > columnMin)
                    {
                        sb.Append(separator);
                    }

                    if (cell != null)
                    {
                        sb.Append(cell);
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts the specified cell range to an array.
        /// </summary>
        /// <param name="cell1">The cell1.</param>
        /// <param name="cell2">The cell2.</param>
        /// <returns>
        /// An array.
        /// </returns>
        private object[,] ToArray(CellRef cell1, CellRef cell2)
        {
            int rowMin = Math.Min(cell1.Row, cell2.Row);
            int columnMin = Math.Min(cell1.Column, cell2.Column);
            int rowMax = Math.Max(cell1.Row, cell2.Row);
            int columnMax = Math.Max(cell1.Column, cell2.Column);

            int m = rowMax - rowMin + 1;
            int n = columnMax - columnMin + 1;
            var result = new object[m, n];

            for (int i = rowMin; i <= rowMax; i++)
            {
                for (int j = columnMin; j <= columnMax; j++)
                {
                    result[i - rowMin, j - columnMin] = this.GetCellValue(new CellRef(i, j));
                }
            }

            return result;
        }

        /// <summary>
        /// Toggles the check in the current cell.
        /// </summary>
        /// <returns>
        /// True if the cell was modified.
        /// </returns>
        private bool ToggleCheck()
        {
            bool value = true;
            var cellValue = this.GetCellValue(this.CurrentCell);
            if (cellValue is bool)
            {
                value = (bool)cellValue;
                value = !value;
            }

            return this.SetCheckInSelectedCells(value);
        }

        /// <summary>
        /// Handles mouse left button down events on the top/left selection control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void TopLeftMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            this.CurrentCell = new CellRef(0, 0);
            this.SelectionCell = new CellRef(this.Rows - 1, this.Columns - 1);
            e.Handled = true;
        }

        /// <summary>
        /// The unsubscribe notifications.
        /// </summary>
        private void UnsubscribeNotifications()
        {
            var ncc = this.subcribedCollection as INotifyCollectionChanged;
            if (ncc != null)
            {
                ncc.CollectionChanged -= this.OnItemsCollectionChanged;
            }

            this.subcribedCollection = null;
        }
    }
}
