﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsGrid.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents a datagrid with a spreadsheet style.
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
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    /// <summary>
    /// Represents a datagrid with a spreadsheet style.
    /// </summary>
    [ContentProperty("ItemsSource")]
    [DefaultProperty("ItemsSource")]
    [TemplatePart(Name = PartGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PartSheetScroller, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PartSheetGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PartColumnScroller, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PartColumnGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PartRowScroller, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PartRowGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PartSelectionBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartRowSelectionBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartColumnSelectionBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartCurrentBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartSelection, Type = typeof(Border))]
    [TemplatePart(Name = PartAutoFillSelection, Type = typeof(Border))]
    [TemplatePart(Name = PartAutoFillBox, Type = typeof(Border))]
    [TemplatePart(Name = PartTopLeft, Type = typeof(Border))]
    public partial class ItemsGrid : Control
    {
        #region Constants and Fields

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
        /// The column scroller.
        /// </summary>
        private const string PartColumnScroller = "PART_ColumnScroller";

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
        /// The row scroller.
        /// </summary>
        private const string PartRowScroller = "PART_RowScroller";

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
        /// The sheet scroller.
        /// </summary>
        private const string PartSheetScroller = "PART_SheetScroller";

        /// <summary>
        /// The top left cell.
        /// </summary>
        private const string PartTopLeft = "PART_TopLeft";

        /// <summary>
        /// The cell map.
        /// </summary>
        private readonly Dictionary<int, UIElement> cellMap = new Dictionary<int, UIElement>();

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
        /// The index in the sheetGrid where new cells can be inserted.
        /// </summary>
        /// <remarks>
        /// The selection and autofill controls should always be at the end of the sheetGrid children collection.
        /// </remarks>
        private int cellInsertionIndex;

        /// <summary>
        /// The column grid control.
        /// </summary>
        private Grid columnGrid;

        /// <summary>
        /// The column scroller control.
        /// </summary>
        private ScrollViewer columnScroller;

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
        /// The is selecting rows.
        /// </summary>
        private bool isSelectingRows;

        /// <summary>
        /// The row grid control.
        /// </summary>
        private Grid rowGrid;

        /// <summary>
        /// The row scroller control.
        /// </summary>
        private ScrollViewer rowScroller;

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
        /// The sheet grid control.
        /// </summary>
        private Grid sheetGrid;

        /// <summary>
        /// The sheet scroller control.
        /// </summary>
        private ScrollViewer sheetScroller;

        /// <summary>
        /// Reference to the collection that has subscribed to the INotifyCollectionChanged event.
        /// </summary>
        private object subcribedCollection;

        /// <summary>
        /// The topleft control.
        /// </summary>
        private Border topleft;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="ItemsGrid"/> class.
        /// </summary>
        static ItemsGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ItemsGrid), new FrameworkPropertyMetadata(typeof(ItemsGrid)));

            InsertRowsCommand = new RoutedCommand("InsertRows", typeof(ItemsGrid));
            DeleteRowsCommand = new RoutedCommand("DeleteRows", typeof(ItemsGrid));
            InsertColumnsCommand = new RoutedCommand("InsertColumns", typeof(ItemsGrid));
            DeleteColumnsCommand = new RoutedCommand("DeleteColumns", typeof(ItemsGrid));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsGrid"/> class.
        /// </summary>
        public ItemsGrid()
        {
            this.CommandBindings.Add(
                new CommandBinding(
                    InsertRowsCommand, (s, e) => this.InsertRows(), (s, e) => e.CanExecute = this.CanInsertRows));
            this.CommandBindings.Add(
                new CommandBinding(
                    DeleteRowsCommand, (s, e) => this.DeleteRows(), (s, e) => e.CanExecute = this.CanDeleteRows));
            this.CommandBindings.Add(
                new CommandBinding(
                    InsertColumnsCommand, (s, e) => this.InsertColumns(), (s, e) => e.CanExecute = this.CanInsertColumns));
            this.CommandBindings.Add(
                new CommandBinding(
                    DeleteColumnsCommand, (s, e) => this.DeleteColumns(), (s, e) => e.CanExecute = this.CanDeleteColumns));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the delete columns command.
        /// </summary>
        /// <value>
        /// The delete columns command. 
        /// </value>
        public static ICommand DeleteColumnsCommand { get; private set; }

        /// <summary>
        /// Gets the delete rows command.
        /// </summary>
        /// <value>
        /// The delete rows command. 
        /// </value>
        public static ICommand DeleteRowsCommand { get; private set; }

        /// <summary>
        /// Gets the insert columns command.
        /// </summary>
        /// <value>
        /// The insert columns command. 
        /// </value>
        public static ICommand InsertColumnsCommand { get; private set; }

        /// <summary>
        /// Gets the insert rows command.
        /// </summary>
        /// <value>
        /// The insert rows command. 
        /// </value>
        public static ICommand InsertRowsCommand { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The auto size all columns.
        /// </summary>
        public void AutoSizeAllColumns()
        {
            this.sheetGrid.UpdateLayout();
            for (int i = 0; i < this.Columns; i++)
            {
                this.AutoSizeColumn(i);
            }
        }

        /// <summary>
        /// Auto-size the specified column.
        /// </summary>
        /// <param name="column">
        /// The column. 
        /// </param>
        public void AutoSizeColumn(int column)
        {
            var h = this.GetColumnElement(column);
            double maxwidth = h.ActualWidth;
            for (int i = 0; i < this.sheetGrid.RowDefinitions.Count; i++)
            {
                var c = this.GetCellElement(new CellRef(i, column)) as FrameworkElement;
                if (c != null)
                {
                    maxwidth = Math.Max(maxwidth, c.ActualWidth + c.Margin.Left + c.Margin.Right);
                }
            }

            this.sheetGrid.ColumnDefinitions[column].Width =
                this.columnGrid.ColumnDefinitions[column].Width = new GridLength((int)maxwidth + 2);
        }

        /// <summary>
        /// The copy.
        /// </summary>
        public void Copy()
        {
            this.Copy("\t");
        }

        /// <summary>
        /// The copy.
        /// </summary>
        /// <param name="separator">
        /// The separator. 
        /// </param>
        public void Copy(string separator)
        {
            var text = this.SelectionToString(separator);
            Clipboard.SetText(text);
        }

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="index">
        /// The index. 
        /// </param>
        /// <param name="updateGrid">
        /// The update grid. 
        /// </param>
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

            list.RemoveAt(index);

            if (updateGrid)
            {
                this.UpdateGridContent();
            }

            return true;
        }

        /// <summary>
        /// The end text edit.
        /// </summary>
        /// <param name="commit">
        /// The commit. 
        /// </param>
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
        /// <returns>The cell reference.</returns>
        public CellRef GetCell(Point position, bool isInAutoFillMode = false, CellRef relativeTo = default(CellRef))
        {
            double w = 0;
            int column = -1;
            int row = -1;
            for (int j = 0; j < this.sheetGrid.ColumnDefinitions.Count; j++)
            {
                double aw0 = j - 1 >= 0 ? this.sheetGrid.ColumnDefinitions[j - 1].ActualWidth : 0;
                double aw1 = this.sheetGrid.ColumnDefinitions[j].ActualWidth;
                double aw2 = j + 1 < this.sheetGrid.ColumnDefinitions.Count ? this.sheetGrid.ColumnDefinitions[j + 1].ActualWidth : 0;
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
        /// <param name="cellRef">
        /// The cell reference. 
        /// </param>
        /// <returns>
        /// The element, or null if the cell was not found. 
        /// </returns>
        public UIElement GetCellElement(CellRef cellRef)
        {
            UIElement e;
            return this.cellMap.TryGetValue(cellRef.GetHashCode(), out e) ? e : null;
        }

        /// <summary>
        /// Gets the formatted string value for the specified cell.
        /// </summary>
        /// <param name="cell">
        /// The cell. 
        /// </param>
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
        /// <param name="cell">
        /// The cell reference. 
        /// </param>
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
        /// <param name="cellRef">
        /// The cell reference. 
        /// </param>
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
        /// <param name="topLeft">
        /// The top left. 
        /// </param>
        /// <param name="bottomRight">
        /// The bottom right. 
        /// </param>
        public void GetVisibleCells(out CellRef topLeft, out CellRef bottomRight)
        {
            double left = this.sheetScroller.HorizontalOffset;
            double right = left + this.sheetScroller.ActualWidth;
            double top = this.sheetScroller.VerticalOffset;
            double bottom = top + this.sheetScroller.ActualHeight;

            topLeft = this.GetCell(new Point(left, top));
            bottomRight = this.GetCell(new Point(right, bottom));
        }

        /// <summary>
        /// Hides the current editor control.
        /// </summary>
        public void HideEditor()
        {
            if (this.currentEditor != null)
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
        /// <param name="index">
        /// The index. 
        /// </param>
        /// <param name="updateGrid">
        /// Determines whether the grid should be updated. 
        /// </param>
        /// <returns>
        /// True if an item was inserted. 
        /// </returns>
        public bool InsertItem(int index, bool updateGrid = true)
        {
            var list = this.ItemsSource;
            if (list == null)
            {
                return false;
            }

            var itemType = TypeHelper.GetItemType(list);

            object newItem = null;
            if (itemType == typeof(string))
            {
                newItem = string.Empty;
            }

            if (itemType == typeof(double))
            {
                newItem = 0.0;
            }

            if (itemType == typeof(int))
            {
                newItem = 0;
            }

            try
            {
                if (newItem == null)
                {
                    newItem = this.CreateInstance(itemType);
                }
            }
            catch
            {
                return false;
            }

            if (index < 0)
            {
                list.Add(newItem);
            }
            else
            {
                list.Insert(index, newItem);
            }

            if (updateGrid)
            {
                this.UpdateGridContent();
            }

            return true;
        }

        /// <summary>
        /// Changes current cell with the specified delta.
        /// </summary>
        /// <param name="deltaRows">
        /// The change in rows. 
        /// </param>
        /// <param name="deltaColumns">
        /// The change in columns. 
        /// </param>
        public void MoveCurrentCell(int deltaRows, int deltaColumns)
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

            if (row >= this.Rows && !this.CanInsertRows)
            {
                column++;
                row = 0;
            }

            if (column < 0)
            {
                column = 0;
            }

            if (column >= this.Columns && !this.CanInsertColumns)
            {
                column = 0;
            }

            this.CurrentCell = new CellRef(row, column);
            this.SelectionCell = new CellRef(row, column);
            this.ScrollIntoView(this.CurrentCell);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/> .
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.sheetScroller = this.Template.FindName(PartSheetScroller, this) as ScrollViewer;
            this.sheetGrid = this.Template.FindName(PartSheetGrid, this) as Grid;
            this.columnScroller = this.Template.FindName(PartColumnScroller, this) as ScrollViewer;
            this.columnGrid = this.Template.FindName(PartColumnGrid, this) as Grid;
            this.rowScroller = this.Template.FindName(PartRowScroller, this) as ScrollViewer;
            this.rowGrid = this.Template.FindName(PartRowGrid, this) as Grid;
            this.rowSelectionBackground = this.Template.FindName(PartRowSelectionBackground, this) as Border;
            this.columnSelectionBackground = this.Template.FindName(PartColumnSelectionBackground, this) as Border;
            this.selectionBackground = this.Template.FindName(PartSelectionBackground, this) as Border;
            this.currentBackground = this.Template.FindName(PartCurrentBackground, this) as Border;
            this.selection = this.Template.FindName(PartSelection, this) as Border;
            this.autoFillSelection = this.Template.FindName(PartAutoFillSelection, this) as Border;
            this.autoFillBox = this.Template.FindName(PartAutoFillBox, this) as Border;
            this.topleft = this.Template.FindName(PartTopLeft, this) as Border;

            if (this.sheetScroller == null)
            {
                throw new Exception(PartSheetScroller + " not found.");
            }

            if (this.rowScroller == null)
            {
                throw new Exception(PartRowScroller + " not found.");
            }

            if (this.columnScroller == null)
            {
                throw new Exception(PartColumnScroller + " not found.");
            }

            if (this.topleft == null)
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

            this.sheetScroller.ScrollChanged += this.ScrollViewerScrollChanged;
            this.rowScroller.ScrollChanged += this.RowScrollerChanged;
            this.columnScroller.ScrollChanged += this.ColumnScrollerChanged;

            this.topleft.MouseLeftButtonDown += this.TopleftMouseLeftButtonDown;
            this.autoFillBox.MouseLeftButtonDown += this.AutoFillBoxMouseLeftButtonDown;
            this.columnGrid.MouseLeftButtonDown += this.ColumnGridMouseLeftButtonDown;
            this.columnGrid.MouseMove += this.ColumnGridMouseMove;
            this.columnGrid.MouseLeftButtonUp += this.ColumnGridMouseLeftButtonUp;
            this.rowGrid.MouseLeftButtonDown += this.RowGridMouseLeftButtonDown;
            this.rowGrid.MouseMove += this.RowGridMouseMove;
            this.rowGrid.MouseLeftButtonUp += this.RowGridMouseLeftButtonUp;

            this.columnGrid.Loaded += this.ColumnGridLoaded;

            this.sheetGrid.SizeChanged += this.ColumnGridSizeChanged;
            this.sheetGrid.MouseLeftButtonDown += this.SheetMouseLeftButtonDown;

            this.autoFiller = new AutoFiller(this.GetCellValue, this.TrySetCellValue);

            this.autoFillToolTip = new ToolTip
                {
                    Placement = PlacementMode.Bottom,
                    PlacementTarget = this.autoFillSelection
                };

            this.UpdateGridContent();
            this.SelectedCellsChanged();

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, this.CopyExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, this.CutExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, this.PasteExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, this.DeleteExecute));
        }

        /// <summary>
        /// Pastes the content from the clipboard.
        /// </summary>
        public void Paste()
        {
            if (!Clipboard.ContainsText())
            {
                return;
            }

            string text = Clipboard.GetText().Trim();
            var textArray = TextToArray(text);

            int rowMin = Math.Min(this.CurrentCell.Row, this.SelectionCell.Row);
            int columnMin = Math.Min(this.CurrentCell.Column, this.SelectionCell.Column);
            int rowMax = Math.Max(this.CurrentCell.Row, this.SelectionCell.Row);
            int columnMax = Math.Max(this.CurrentCell.Column, this.SelectionCell.Column);

            int rows = textArray.GetUpperBound(0) + 1;
            int columns = textArray.GetUpperBound(1) + 1;

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
                    string value = textArray[(i - rowMin) % rows, (j - columnMin) % columns];
                    this.TrySetCellValue(new CellRef(i, j), value);
                }
            }

            this.CurrentCell = new CellRef(rowMin, columnMin);
            this.SelectionCell = new CellRef(
                Math.Max(rowMax, rowMin + rows - 1), Math.Max(columnMax, columnMin + columns - 1));
        }

        /// <summary>
        /// Scroll the specified cell into view.
        /// </summary>
        /// <param name="cellRef">
        /// The cell reference. 
        /// </param>
        public void ScrollIntoView(CellRef cellRef)
        {
            var pos0 = this.GetPosition(cellRef);
            var pos1 = this.GetPosition(new CellRef(cellRef.Row + 1, cellRef.Column + 1));

            // todo: get correct size
            double scrollBarWidth = 20;
            double scrollBarHeight = 20;

            if (pos0.X < this.sheetScroller.HorizontalOffset)
            {
                this.sheetScroller.ScrollToHorizontalOffset(pos0.X);
            }

            if (pos1.X > this.sheetScroller.HorizontalOffset + this.sheetScroller.ActualWidth - scrollBarWidth)
            {
                this.sheetScroller.ScrollToHorizontalOffset(
                    Math.Max(pos1.X - this.sheetScroller.ActualWidth + scrollBarWidth, 0));
            }

            if (pos0.Y < this.sheetScroller.VerticalOffset)
            {
                this.sheetScroller.ScrollToVerticalOffset(pos0.Y);
            }

            if (pos1.Y > this.sheetScroller.VerticalOffset + this.sheetScroller.ActualHeight - scrollBarHeight)
            {
                this.sheetScroller.ScrollToVerticalOffset(
                    Math.Max(pos1.Y - this.sheetScroller.ActualHeight + scrollBarHeight, 0));
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
            if (pd == null)
            {
                return false;
            }

            int index = this.GetItemIndex(this.CurrentCell);
            var item = this.GetItem(index);
            this.currentEditor = this.ControlFactory.CreateEditControl(pd, index);

            if (this.currentEditor == null)
            {
                return false;
            }

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
        /// The show text box editor.
        /// </summary>
        public void ShowTextBoxEditControl()
        {
            var textEditor = this.currentEditor as TextBox;
            if (textEditor != null)
            {
                textEditor.Visibility = Visibility.Visible;
                textEditor.Focus();
                textEditor.CaretIndex = textEditor.Text.Length;
                textEditor.SelectAll();
            }
        }

        /// <summary>
        /// Exports the grid to csv.
        /// </summary>
        /// <param name="separator">
        /// The separator. 
        /// </param>
        /// <returns>
        /// The csv string. 
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
        /// <param name="cell">
        /// The cell. 
        /// </param>
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <returns>
        /// True if the value was set. 
        /// </returns>
        public virtual bool TrySetCellValue(CellRef cell, object value)
        {
            if (this.ItemsSource != null)
            {
                int index = this.GetItemIndex(cell);
                var current = this.GetItem(index);

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
                        var list = this.ItemsSource;
                        if (list != null)
                        {
                            list[index] = convertedValue;
                        }

                        if (!(list is INotifyCollectionChanged))
                        {
                            this.UpdateCellContent(cell);
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the display control for the specified cell.
        /// </summary>
        /// <param name="cell">
        /// The cell. 
        /// </param>
        /// <param name="pd">
        /// The pd. 
        /// </param>
        /// <param name="item">
        /// The item. 
        /// </param>
        /// <param name="index">
        /// The index. 
        /// </param>
        /// <returns>
        /// The display control. 
        /// </returns>
        protected virtual UIElement CreateDisplayControl(
            CellRef cell, PropertyDefinition pd = null, object item = null, int index = -1)
        {
            FrameworkElement element = null;

            if (item == null)
            {
                index = this.GetItemIndex(cell);
                item = this.GetItem(index);
            }

            if (pd == null)
            {
                pd = this.GetPropertyDefinition(cell);
            }

            if (pd != null && item != null)
            {
                element = this.ControlFactory.CreateDisplayControl(pd, index);
                this.SetElementDataContext(element, pd, item);

                element.VerticalAlignment = VerticalAlignment.Center;
                element.HorizontalAlignment = pd.HorizontalAlignment;
            }

            return element;
        }

        /// <summary>
        /// Creates a new instance of the specified type.
        /// </summary>
        /// <param name="itemType">
        /// The type. 
        /// </param>
        /// <returns>
        /// The new instance. 
        /// </returns>
        protected virtual object CreateInstance(Type itemType)
        {
            return Activator.CreateInstance(itemType);
        }

        /// <summary>
        /// Auto-generates the column definitions.
        /// </summary>
        /// <param name="items">
        /// The items. 
        /// </param>
        protected virtual void GenerateColumnDefinitions(IList items)
        {
            if (items == null)
            {
                return;
            }

            var itemType = TypeHelper.FindBiggestCommonType(items);

            var properties = TypeDescriptor.GetProperties(itemType);

            foreach (PropertyDescriptor descriptor in properties)
            {
                if (!descriptor.IsBrowsable)
                {
                    continue;
                }

                this.ColumnDefinitions.Add(
                    new ColumnDefinition
                        {
                            Descriptor = descriptor,
                            Header = descriptor.Name,
                            HorizontalAlignment = this.DefaultHorizontalAlignment,
                            Width = this.DefaultColumnWidth
                        });
            }

            var itemsType = TypeHelper.GetItemType(items);
            if (properties.Count == 0)
            {
                this.ColumnDefinitions.Add(
                    new ColumnDefinition
                        {
                            PropertyType = itemsType,
                            Header = itemsType.Name,
                            HorizontalAlignment = this.DefaultHorizontalAlignment,
                            Width = this.DefaultColumnWidth
                        });
            }
        }

        /// <summary>
        /// Handles KeyDown events on the grid.
        /// </summary>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
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
                    this.MoveCurrentCell(shift ? -1 : 1, 0);
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
                    if (row < this.Rows - 1 || this.CanInsertRows)
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
                    if (column < this.Columns - 1 || this.CanInsertColumns)
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
                    break;
                case Key.Home:
                    column = 0;
                    row = 0;
                    break;
                case Key.Delete:
                    this.Delete();
                    break;
                case Key.F2:
                    this.ShowTextBoxEditControl();
                    break;
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
        /// <param name="e">
        /// The event arguments. 
        /// </param>
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
        /// <param name="e">
        /// The event arguments. 
        /// </param>
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
        /// <param name="e">
        /// The event arguments. 
        /// </param>
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

            if (cellRef.Row >= this.Rows && !this.CanInsertRows)
            {
                return;
            }

            if (cellRef.Column > this.Columns && !this.CanInsertColumns)
            {
                return;
            }

            if (isInAutoFillMode)
            {
                this.AutoFillCell = cellRef;
                object result;
                if (this.autoFiller.TryExtrapolate(
                    cellRef, this.CurrentCell, this.SelectionCell, this.AutoFillCell, out result))
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
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            if (control)
            {
                double s = 1 + e.Delta * 0.0004;
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
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);
            if (e.Text == "\r")
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
        /// <param name="cellRef">
        /// The cell reference. 
        /// </param>
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
        /// Clamps a value between a minimum and maximum limit.
        /// </summary>
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <param name="min">
        /// The minimum. 
        /// </param>
        /// <param name="max">
        /// The maximum. 
        /// </param>
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
        /// Csv-encodes the specified string.
        /// </summary>
        /// <param name="input">
        /// The input string. 
        /// </param>
        /// <returns>
        /// The csv-encoded string. 
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
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <param name="formatString">
        /// The format string. 
        /// </param>
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
        /// The set element position.
        /// </summary>
        /// <param name="element">
        /// The element. 
        /// </param>
        /// <param name="cell">
        /// The cell. 
        /// </param>
        private static void SetElementPosition(UIElement element, CellRef cell)
        {
            Grid.SetColumn(element, cell.Column);
            Grid.SetRow(element, cell.Row);
        }

        /// <summary>
        /// Splits a string separated by \n and \t into an array.
        /// </summary>
        /// <param name="text">
        /// The text. 
        /// </param>
        /// <returns>
        /// An 2-dimensional array of strings. 
        /// </returns>
        private static string[,] TextToArray(string text)
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

            var result = new string[rows, columns];
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
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <param name="targetType">
        /// The target type. 
        /// </param>
        /// <param name="convertedValue">
        /// The converted value. 
        /// </param>
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
        /// Adds the display control for the specified cell.
        /// </summary>
        /// <param name="cellRef">
        /// The cell reference. 
        /// </param>
        private void AddDisplayControl(CellRef cellRef)
        {
            int index = this.GetItemIndex(cellRef);
            var e = this.CreateDisplayControl(cellRef, null, null, index);
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
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void AddItemCellMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            this.InsertItem(-1);
            e.Handled = true;
        }

        /// <summary>
        /// The auto fill box mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void AutoFillBoxMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Show the autofill selection border
            this.autoFillSelection.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// The column grid loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void ColumnGridLoaded(object sender, RoutedEventArgs e)
        {
            this.UpdateColumnWidths();
        }

        /// <summary>
        /// The column grid mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
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
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void ColumnGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.columnGrid.ReleaseMouseCapture();
            this.isSelectingColumns = false;
        }

        /// <summary>
        /// The column grid mouse move.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
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
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void ColumnGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateColumnWidths();
        }

        /// <summary>
        /// The column scroller changed.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void ColumnScrollerChanged(object sender, ScrollChangedEventArgs e)
        {
            this.sheetScroller.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        /// <summary>
        /// The column splitter change completed.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="dragCompletedEventArgs">
        /// The drag completed event args. 
        /// </param>
        private void ColumnSplitterChangeCompleted(object sender, DragCompletedEventArgs dragCompletedEventArgs)
        {
            var gs = (GridSplitter)sender;
            var tt = gs.ToolTip as ToolTip;
            if (tt != null)
            {
                tt.IsOpen = false;
                gs.ToolTip = null;
            }

            for (int i = 0; i < this.sheetGrid.ColumnDefinitions.Count; i++)
            {
                this.sheetGrid.ColumnDefinitions[i].Width = this.columnGrid.ColumnDefinitions[i].Width;
            }
        }

        /// <summary>
        /// The column splitter change delta.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
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
            tt.Content = string.Format("Width: {0}", width); // device-independent units

            tt.PlacementTarget = this.columnGrid;
            tt.Placement = PlacementMode.Relative;
            var p = Mouse.GetPosition(this.columnGrid);
            tt.HorizontalOffset = p.X;
            tt.VerticalOffset = gs.ActualHeight + 4;
        }

        /// <summary>
        /// The column splitter change started.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="dragStartedEventArgs">
        /// The drag started event args. 
        /// </param>
        private void ColumnSplitterChangeStarted(object sender, DragStartedEventArgs dragStartedEventArgs)
        {
            this.ColumnSplitterChangeDelta(sender, null);
        }

        /// <summary>
        /// The column splitter double click.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void ColumnSplitterDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int column = Grid.GetColumn((GridSplitter)sender);
            this.AutoSizeColumn(column);
        }

        /// <summary>
        /// The copy execute.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void CopyExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Copy();
        }

        /// <summary>
        /// The cut execute.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void CutExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Copy();
            this.Delete();
        }

        /// <summary>
        /// The delete.
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
            int from = Math.Min(this.CurrentCell.Column, this.SelectionCell.Column);
            int to = Math.Max(this.CurrentCell.Column, this.SelectionCell.Column);
            for (int i = to; i >= from; i--)
            {
                this.DeleteItem(i, false);
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
        /// The delete execute.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void DeleteExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Delete();
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
        /// <param name="cell0">
        /// The cell 0. 
        /// </param>
        /// <param name="cell1">
        /// The cell 1. 
        /// </param>
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
        /// <param name="row">
        /// The row. 
        /// </param>
        /// <param name="column">
        /// The column. 
        /// </param>
        /// <param name="deltaColumn">
        /// The delta column. 
        /// </param>
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
        /// <param name="row">
        /// The row. 
        /// </param>
        /// <param name="column">
        /// The column. 
        /// </param>
        /// <param name="deltaRow">
        /// The delta row. 
        /// </param>
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

            return row;
        }

        /// <summary>
        /// Gets a cell reference from the specified display control.
        /// </summary>
        /// <param name="element">
        /// The element. 
        /// </param>
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
        /// <param name="column">
        /// The column. 
        /// </param>
        /// <returns>
        /// The column element. 
        /// </returns>
        private FrameworkElement GetColumnElement(int column)
        {
            return this.columnGrid.Children[1 + 3 * column + 1] as FrameworkElement;
        }

        /// <summary>
        /// Gets the column header for the specified column.
        /// </summary>
        /// <param name="j">
        /// The j. 
        /// </param>
        /// <returns>
        /// The get column header. 
        /// </returns>
        private object GetColumnHeader(int j)
        {
            if (this.ItemsInRows)
            {
                if (j < this.PropertyDefinitions.Count)
                {
                    return this.PropertyDefinitions[j].Header ?? CellRef.ToColumnName(j);
                }
            }

            return CellRef.ToColumnName(j);
        }

        /// <summary>
        /// Gets the column width for the specified column.
        /// </summary>
        /// <param name="i">
        /// The column index. 
        /// </param>
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
        /// <param name="cell">
        /// The cell. 
        /// </param>
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
        /// <param name="cell">
        /// The cell. 
        /// </param>
        /// <returns>
        /// The item. 
        /// </returns>
        private object GetItem(CellRef cell)
        {
            return this.GetItem(this.GetItemIndex(cell));
        }

        /// <summary>
        /// Gets the item for the specified index in the ItemsSource collection.
        /// </summary>
        /// <param name="index">
        /// The item index. 
        /// </param>
        /// <returns>
        /// The item. 
        /// </returns>
        private object GetItem(int index)
        {
            var list = this.ItemsSource;
            if (list == null)
            {
                return null;
            }

            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }

            return null;
        }

        /// <summary>
        /// Gets the item index for the specified cell.
        /// </summary>
        /// <param name="cell">
        /// The cell. 
        /// </param>
        /// <returns>
        /// The get item index. 
        /// </returns>
        private int GetItemIndex(CellRef cell)
        {
            if (this.WrapItems)
            {
                return this.ItemsInRows ? cell.Row * this.Columns + cell.Column : cell.Column * this.Rows + cell.Row;
            }

            return this.ItemsInRows ? cell.Row : cell.Column;
        }

        /// <summary>
        /// Gets the column/row definition for the specified cell.
        /// </summary>
        /// <param name="cell">
        /// The cell. 
        /// </param>
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
        /// <param name="j">
        /// The j. 
        /// </param>
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
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void HandleButtonDown(MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(this.sheetGrid);
            var cellRef = this.GetCell(pos);

            if (cellRef.Column == -1 || cellRef.Row == -1)
            {
                return;
            }

            if (cellRef.Row >= this.Rows && !this.CanInsertRows)
            {
                return;
            }

            if (cellRef.Column > this.Columns && !this.CanInsertColumns)
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
            int from = Math.Min(this.CurrentCell.Column, this.SelectionCell.Column);
            int to = Math.Max(this.CurrentCell.Column, this.SelectionCell.Column);
            for (int i = 0; i < to - from + 1; i++)
            {
                this.InsertItem(from, false);
            }

            this.UpdateGridContent();
        }

        /// <summary>
        /// Inserts the display control for the specified cell.
        /// </summary>
        /// <param name="cellRef">
        /// The cell reference. 
        /// </param>
        private void InsertDisplayControl(CellRef cellRef)
        {
            int index = this.GetItemIndex(cellRef);
            var e = this.CreateDisplayControl(cellRef, null, null, index);
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
        /// The on content collection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // todo: update only changed rows/columns
            this.UpdateGridContent();
        }

        /// <summary>
        /// The paste execute.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void PasteExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Paste();
        }

        /// <summary>
        /// The row grid mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
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
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void RowGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.rowGrid.ReleaseMouseCapture();
            this.isSelectingRows = false;
        }

        /// <summary>
        /// The row grid mouse move.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
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
        /// The row scroller changed.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void RowScrollerChanged(object sender, ScrollChangedEventArgs e)
        {
            this.sheetScroller.ScrollToVerticalOffset(e.VerticalOffset);
        }

        /// <summary>
        /// The scroll viewer scroll changed.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.columnScroller.ScrollToHorizontalOffset(this.sheetScroller.HorizontalOffset);
            this.rowScroller.ScrollToVerticalOffset(this.sheetScroller.VerticalOffset);
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
        /// Exports the selection to a string.
        /// </summary>
        /// <param name="separator">
        /// The separator. 
        /// </param>
        /// <param name="encode">
        /// Determines whether to csv encode the elements. 
        /// </param>
        /// <returns>
        /// The string. 
        /// </returns>
        private string SelectionToString(string separator, bool encode = false)
        {
            return this.ToString(this.CurrentCell, this.SelectionCell, separator, encode);
        }

        /// <summary>
        /// Set the bool value in the selected cells.
        /// </summary>
        /// <param name="value">
        /// The value. 
        /// </param>
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
        /// The set element data context.
        /// </summary>
        /// <param name="element">
        /// The element. 
        /// </param>
        /// <param name="pd">
        /// The pd. 
        /// </param>
        /// <param name="item">
        /// The item. 
        /// </param>
        private void SetElementDataContext(FrameworkElement element, PropertyDefinition pd, object item)
        {
            element.DataContext = pd.Descriptor != null ? item : this.ItemsSource;
        }

        /// <summary>
        /// Handles mouse left button down on the grid sheet.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void SheetMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
        /// <param name="separator">
        /// The separator. 
        /// </param>
        /// <param name="encode">
        /// The encode. 
        /// </param>
        /// <returns>
        /// The sheet to string. 
        /// </returns>
        private string SheetToString(string separator, bool encode = false)
        {
            return this.ToString(new CellRef(0, 0), new CellRef(this.Rows - 1, this.Columns - 1), separator, encode);
        }

        /// <summary>
        /// The text editor was loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data. 
        /// </param>
        private void TextEditorLoaded(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            tb.CaretIndex = tb.Text.Length;
            tb.SelectAll();
        }

        /// <summary>
        /// The text editor lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void TextEditorLostFocus(object sender, RoutedEventArgs e)
        {
            this.EndTextEdit();
        }

        /// <summary>
        /// Handles keydown events in the TextBox editor.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void TextEditorPreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool shift = (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None;
            var textEditor = sender as TextBox;
            if (textEditor == null)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    if (textEditor.CaretIndex == 0)
                    {
                        this.EndTextEdit();
                        this.OnKeyDown(e);
                        e.Handled = true;
                    }

                    break;
                case Key.Right:
                    if (textEditor.CaretIndex == textEditor.Text.Length)
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
                    this.MoveCurrentCell(shift ? -1 : 1, 0);
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
        /// <param name="cell1">
        /// The cell 1. 
        /// </param>
        /// <param name="cell2">
        /// The cell 2. 
        /// </param>
        /// <param name="separator">
        /// The separator. 
        /// </param>
        /// <param name="encode">
        /// Determines whether to csv encode the elements. 
        /// </param>
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
        /// Toggles the check in the current cell.
        /// </summary>
        /// <returns>
        /// True if the cell was modified. 
        /// </returns>
        private bool ToggleCheck()
        {
            bool value = true;
            var cvalue = this.GetCellValue(this.CurrentCell);
            if (cvalue is bool)
            {
                value = (bool)cvalue;
                value = !value;
            }

            return this.SetCheckInSelectedCells(value);
        }

        /// <summary>
        /// The topleft mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void TopleftMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        #endregion
    }
}