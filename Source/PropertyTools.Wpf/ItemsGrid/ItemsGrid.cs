// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsGrid.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.ItemsGrid
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
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    /// <summary>
    /// Represents a datagrid with a spreadsheet style.
    /// </summary>
    [ContentProperty("Content")]
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
        ///   The auto fill box.
        /// </summary>
        private const string PartAutoFillBox = "PART_AutoFillBox";

        /// <summary>
        ///   The auto fill selection.
        /// </summary>
        private const string PartAutoFillSelection = "PART_AutoFillSelection";

        /// <summary>
        ///   The column grid.
        /// </summary>
        private const string PartColumnGrid = "PART_ColumnGrid";

        /// <summary>
        ///   The column scroller.
        /// </summary>
        private const string PartColumnScroller = "PART_ColumnScroller";

        /// <summary>
        ///   The column selection background.
        /// </summary>
        private const string PartColumnSelectionBackground = "PART_ColumnSelectionBackground";

        /// <summary>
        ///   The current background.
        /// </summary>
        private const string PartCurrentBackground = "PART_CurrentBackground";

        /// <summary>
        ///   The grid.
        /// </summary>
        private const string PartGrid = "PART_Grid";

        /// <summary>
        ///   The row grid.
        /// </summary>
        private const string PartRowGrid = "PART_RowGrid";

        /// <summary>
        ///   The row scroller.
        /// </summary>
        private const string PartRowScroller = "PART_RowScroller";

        /// <summary>
        ///   The row selection background.
        /// </summary>
        private const string PartRowSelectionBackground = "PART_RowSelectionBackground";

        /// <summary>
        ///   The selection.
        /// </summary>
        private const string PartSelection = "PART_Selection";

        /// <summary>
        ///   The selection background.
        /// </summary>
        private const string PartSelectionBackground = "PART_SelectionBackground";

        /// <summary>
        ///   The sheet grid.
        /// </summary>
        private const string PartSheetGrid = "PART_SheetGrid";

        /// <summary>
        ///   The sheet scroller.
        /// </summary>
        private const string PartSheetScroller = "PART_SheetScroller";

        /// <summary>
        ///   The top left cell.
        /// </summary>
        private const string PartTopLeft = "PART_TopLeft";

        /// <summary>
        ///   The cell map.
        /// </summary>
        private readonly Dictionary<int, UIElement> cellMap = new Dictionary<int, UIElement>();

        /// <summary>
        ///   The auto fill box.
        /// </summary>
        private Border autoFillBox;

        /// <summary>
        ///   The auto fill cell.
        /// </summary>
        private CellRef autoFillCell;

        /// <summary>
        ///   The auto fill selection.
        /// </summary>
        private Border autoFillSelection;

        /// <summary>
        ///   The auto fill tool tip.
        /// </summary>
        private ToolTip autoFillToolTip;

        /// <summary>
        ///   The auto filler.
        /// </summary>
        private AutoFiller autoFiller;

        /// <summary>
        ///   The cell insertion index.
        /// </summary>
        private int cellInsertionIndex;

        /// <summary>
        ///   The column grid.
        /// </summary>
        private Grid columnGrid;

        /// <summary>
        ///   The column scroller.
        /// </summary>
        private ScrollViewer columnScroller;

        /// <summary>
        ///   The column selection background.
        /// </summary>
        private Border columnSelectionBackground;

        /// <summary>
        ///   The current background.
        /// </summary>
        private Border currentBackground;

        /// <summary>
        /// The current editor.
        /// </summary>
        private FrameworkElement currentEditor;

        /// <summary>
        ///   The editing cells.
        /// </summary>
        private IEnumerable<CellRef> editingCells;

        /// <summary>
        ///   The end pressed.
        /// </summary>
        private bool endPressed;

        /// <summary>
        ///   The is capturing.
        /// </summary>
        private bool isCapturing;

        /// <summary>
        ///   The is selecting columns.
        /// </summary>
        private bool isSelectingColumns;

        /// <summary>
        ///   The is selecting rows.
        /// </summary>
        private bool isSelectingRows;

        /// <summary>
        ///   The row grid.
        /// </summary>
        private Grid rowGrid;

        /// <summary>
        ///   The row scroller.
        /// </summary>
        private ScrollViewer rowScroller;

        /// <summary>
        ///   The row selection background.
        /// </summary>
        private Border rowSelectionBackground;

        /// <summary>
        ///   The selection.
        /// </summary>
        private Border selection;

        /// <summary>
        ///   The selection background.
        /// </summary>
        private Border selectionBackground;

        /// <summary>
        ///   The sheet grid.
        /// </summary>
        private Grid sheetGrid;

        /// <summary>
        ///   The sheet scroller.
        /// </summary>
        private ScrollViewer sheetScroller;

        /// <summary>
        ///   The subcribed content.
        /// </summary>
        private object subcribedContent;

        /// <summary>
        ///   The topleft.
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
        }

        #endregion

        #region Public Methods

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
        /// The auto size column.
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
            var list = this.Content as IList;
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
        /// <param name="position">
        /// The position.
        /// </param>
        /// <returns>
        /// The cell ref.
        /// </returns>
        public CellRef GetCell(Point position)
        {
            double w = 0;
            int column = -1;
            int row = -1;
            for (int j = 0; j < this.sheetGrid.ColumnDefinitions.Count; j++)
            {
                double aw = this.sheetGrid.ColumnDefinitions[j].ActualWidth;
                if (position.X < w + aw)
                {
                    column = j;
                    break;
                }

                w += aw;
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
        /// The cell ref.
        /// </param>
        /// <returns>
        /// </returns>
        public UIElement GetCellElement(CellRef cellRef)
        {
            UIElement e;
            return this.cellMap.TryGetValue(cellRef.GetHashCode(), out e) ? e : null;
        }

        /// <summary>
        /// Gets the cell string at the specified cell.
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

            var formatString = this.GetFormatString(cell, value);
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
                    return pd.Descriptor.GetValue(item);
                }
            }

            return null;
        }

        /// <summary>
        /// The get position.
        /// </summary>
        /// <param name="cellRef">
        /// The cell ref.
        /// </param>
        /// <returns>
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
        /// The get visible cells.
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
        /// The hide editor.
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
            }

            this.Focus();
        }

        /// <summary>
        /// Inserts an item.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="updateGrid">
        /// The update grid.
        /// </param>
        /// <returns>
        /// The insert item.
        /// </returns>
        public bool InsertItem(int index, bool updateGrid = true)
        {
            var list = this.Content as IList;
            if (list == null)
            {
                return false;
            }

            var listType = list.GetType();
            var itemType = GetListItemType(listType);

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
        /// The move current cell.
        /// </summary>
        /// <param name="deltaRows">
        /// The delta rows.
        /// </param>
        /// <param name="deltaColumns">
        /// The delta columns.
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
        /// The on apply template.
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
            this.OnSelectedCellsChanged();

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, this.CopyExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, this.CutExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, this.PasteExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, this.DeleteExecute));
        }

        /// <summary>
        /// The paste.
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
        /// The scroll into view.
        /// </summary>
        /// <param name="cellRef">
        /// The cell ref.
        /// </param>
        public void ScrollIntoView(CellRef cellRef)
        {
            var pos0 = this.GetPosition(cellRef);
            var pos1 = this.GetPosition(new CellRef(cellRef.Row + 1, cellRef.Column + 1));

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
        /// The show editor.
        /// </summary>
        /// <returns>
        /// The show editor.
        /// </returns>
        public bool ShowEditor()
        {
            this.HideEditor();
            var d = this.GetPropertyDefinition(this.CurrentCell);
            if (d == null)
            {
                return false;
            }

            var item = this.GetItem(this.CurrentCell);
            this.currentEditor = this.ControlFactory.CreateEditControl(d);
            this.currentEditor.DataContext = item;
            if (this.currentEditor == null)
            {
                return false;
            }

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
            this.currentEditor.Focus();
            return true;
        }

        /// <summary>
        /// The show text box editor.
        /// </summary>
        public void ShowTextBoxEditor()
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
        /// The to csv.
        /// </summary>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The to csv.
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
        /// The try set cell value.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The try set cell value.
        /// </returns>
        public virtual bool TrySetCellValue(CellRef cell, object value)
        {
            var items = this.Content as IEnumerable;
            if (items != null)
            {
                var current = this.GetItem(cell);

                var field = this.GetPropertyDefinition(cell);
                if (field == null)
                {
                    return false;
                }

                object convertedValue;
                if (current != null && !field.Descriptor.IsReadOnly
                    && TryConvert(value, field.Descriptor.PropertyType, out convertedValue))
                {
                    field.Descriptor.SetValue(current, convertedValue);
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create element.
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
        /// <returns>
        /// </returns>
        protected virtual UIElement CreateElement(CellRef cell, PropertyDefinition pd, object item)
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

            if (item == null)
            {
                item = this.GetItem(cell);
            }

            if (pd != null && item != null)
            {
                element = this.ControlFactory.CreateDisplayControl(pd);
                element.DataContext = item;

                element.VerticalAlignment = VerticalAlignment.Center;
                element.HorizontalAlignment = pd.HorizontalAlignment;
            }

            return element;
        }

        /// <summary>
        /// The create instance.
        /// </summary>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <returns>
        /// The create instance.
        /// </returns>
        protected virtual object CreateInstance(Type itemType)
        {
            object newItem;
            newItem = Activator.CreateInstance(itemType);
            return newItem;
        }

        /// <summary>
        /// The generate column definitions.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        protected virtual void GeneratePropertyDefinitions(IEnumerable items)
        {
            if (items == null)
            {
                return;
            }

            var itemType = TypeHelper.FindBiggestCommonType(items);

            // itemType=items.AsQueryable().ElementType; 

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(itemType))
            {
                if (!descriptor.IsBrowsable)
                {
                    continue;
                }

                this.PropertyDefinitions.Add(
                    new PropertyDefinition(descriptor)
                        {
                            HorizontalAlignment = this.DefaultHorizontalAlignment,
                            Width = this.DefaultColumnWidth
                        });
            }
        }

        /// <summary>
        /// Gets the index of the specified item in the Content enumerable.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The get index of item.
        /// </returns>
        protected int GetIndexOfItem(object item)
        {
            var list = this.Content as IEnumerable;
            if (list != null)
            {
                int i = 0;
                foreach (var listItem in list)
                {
                    if (listItem == item)
                    {
                        return i;
                    }

                    i++;
                }
            }

            return -1;
        }

        /// <summary>
        /// Handles KeyDown events on the grid.
        /// </summary>
        /// <param name="e">
        /// The event args.
        /// </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift);
            bool alt = Keyboard.IsKeyDown(Key.LeftAlt);

            int row = shift ? this.SelectionCell.Row : this.CurrentCell.Row;
            int column = shift ? this.SelectionCell.Column : this.CurrentCell.Column;

            switch (e.Key)
            {
                case Key.Up:
                    if (row > 0)
                    {
                        row--;
                    }

                    if (this.endPressed)
                    {
                        this.FindNext(ref row, ref column, -1, 0);
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
                        this.FindNext(ref row, ref column, 1, 0);
                    }

                    if (control)
                    {
                        row = this.Rows - 1;
                    }

                    break;
                case Key.Enter:
                    this.MoveCurrentCell(shift ? -1 : 1, 0);
                    e.Handled = true;
                    return;
                case Key.Left:
                    if (column > 0)
                    {
                        column--;
                    }

                    if (this.endPressed)
                    {
                        this.FindNext(ref row, ref column, 0, -1);
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
                        this.FindNext(ref row, ref column, 0, 1);
                    }

                    if (control)
                    {
                        column = this.Columns - 1;
                    }

                    break;
                case Key.End:
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
                    this.ShowTextBoxEditor();
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
        /// The on mouse left button down.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.Focus();
            base.OnMouseLeftButtonDown(e);
            this.HandleButtonDown(e);
            e.Handled = true;
        }

        /// <summary>
        /// The on mouse left button up.
        /// </summary>
        /// <param name="e">
        /// The e.
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
        /// The on mouse move.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!this.isCapturing)
            {
                return;
            }

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
                object result;
                if (this.autoFiller.TryExtrapolate(
                    cellRef, this.CurrentCell, this.SelectionCell, this.AutoFillCell, out result))
                {
                    var fmt = this.GetFormatString(cellRef, result);
                    this.autoFillToolTip.Content = FormatValue(result, fmt);
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
        /// The on preview mouse wheel.
        /// </summary>
        /// <param name="e">
        /// The e.
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
        /// The on text input.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);
            if (e.Text == "\r")
            {
                return;
            }

            var textEditor = this.currentEditor as TextBox;
            if (textEditor != null)
            {
                this.ShowTextBoxEditor();
                textEditor.Text = e.Text;
                textEditor.CaretIndex = textEditor.Text.Length;
            }
        }

        /// <summary>
        /// The update all cells.
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
        /// The update cell content.
        /// </summary>
        /// <param name="cellRef">
        /// The cell ref.
        /// </param>
        protected void UpdateCellContent(CellRef cellRef)
        {
            var c = this.GetCellElement(cellRef);
            var value = this.GetCellValue(cellRef);

            if (c != null)
            {
                this.sheetGrid.Children.Remove(c);
                this.cellInsertionIndex--;
                this.cellMap.Remove(cellRef.GetHashCode());
            }

            this.InsertCellElement(cellRef, value, true);
        }

        /// <summary>
        /// The clamp.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <returns>
        /// The clamp.
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
        /// The csv encode string.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// The csv encode string.
        /// </returns>
        private static string CsvEncodeString(string cell)
        {
            cell = cell.Replace("\"", "\"\"");
            if (cell.Contains(";") || cell.Contains("\""))
            {
                cell = "\"" + cell + "\"";
            }

            return cell;
        }

        /// <summary>
        /// The format value.
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
            else
            {
                if (!formatString.Contains("{0"))
                {
                    formatString = "{0:" + formatString + "}";
                }

                return string.Format(formatString, value);
            }
        }

        /// <summary>
        /// The get list item type.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// </returns>
        private static Type GetListItemType(IEnumerable list)
        {
            if (list == null)
            {
                return null;
            }

            foreach (var item in list)
            {
                if (item != null)
                {
                    return item.GetType();
                }
            }

            return null;
        }

        /// <summary>
        /// The get list item type.
        /// </summary>
        /// <param name="listType">
        /// The list type.
        /// </param>
        /// <returns>
        /// </returns>
        private static Type GetListItemType(Type listType)
        {
            // http://stackoverflow.com/questions/1043755/c-generic-list-t-how-to-get-the-type-of-t
            foreach (var interfaceType in listType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    var args = interfaceType.GetGenericArguments();
                    if (args.Length > 0)
                    {
                        return args[0];
                    }
                }
            }

            return null;
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
        /// The text to array.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
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
        /// Convert a HorizontalAlignment to a TextAlignment.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        private static TextAlignment ToTextAlignment(HorizontalAlignment a)
        {
            switch (a)
            {
                case HorizontalAlignment.Left:
                    return TextAlignment.Left;
                case HorizontalAlignment.Center:
                    return TextAlignment.Center;
                case HorizontalAlignment.Right:
                    return TextAlignment.Right;
                default:
                    return TextAlignment.Left;
            }
        }

        /// <summary>
        /// The try convert.
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
        /// The try convert.
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
                if (converter != null && value != null && converter.CanConvertFrom(value.GetType()))
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
        /// The add cell element.
        /// </summary>
        /// <param name="cellRef">
        /// The cell ref.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        private void AddCellElement(CellRef cellRef, object value)
        {
            this.InsertCellElement(cellRef, value, false);
        }

        /// <summary>
        /// The add item cell mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
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
        /// The e.
        /// </param>
        private void AutoFillBoxMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Show the autofill selection border
            this.autoFillSelection.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// The build context menus.
        /// </summary>
        private void BuildContextMenus()
        {
            var rowsMenu = new ContextMenu();
            if (this.CanInsertRows)
            {
                rowsMenu.Items.Add(new MenuItem { Header = "Insert", Command = new DelegateCommand(this.InsertRows) });
            }

            if (this.CanDeleteRows)
            {
                rowsMenu.Items.Add(new MenuItem { Header = "Delete", Command = new DelegateCommand(this.DeleteRows) });
            }

            if (rowsMenu.Items.Count > 0)
            {
                this.rowGrid.ContextMenu = rowsMenu;
            }

            var columnsMenu = new ContextMenu();
            if (this.CanInsertColumns)
            {
                columnsMenu.Items.Add(new MenuItem { Header = "Insert" });
            }

            if (this.CanDeleteColumns)
            {
                columnsMenu.Items.Add(new MenuItem { Header = "Delete" });
            }

            if (columnsMenu.Items.Count > 0)
            {
                this.columnGrid.ContextMenu = columnsMenu;
            }
        }

        /// <summary>
        /// The cell checked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CellChecked(object sender, RoutedEventArgs e)
        {
            var chkbox = sender as CheckBox;
            if (chkbox == null)
            {
                return;
            }

            int row = Grid.GetRow(chkbox);
            int column = Grid.GetColumn(chkbox);

            this.CurrentCell = new CellRef(row, column);
            this.SelectionCell = new CellRef(row, column);

            // Binding was not used here, so update the value of the cell
            this.TrySetCellValue(this.CurrentCell, chkbox.IsChecked);
            this.UpdateCellContent(this.CurrentCell);
        }

        /// <summary>
        /// The column grid loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
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
        /// The e.
        /// </param>
        private void ColumnGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            int column = this.GetCell(e.GetPosition(this.columnGrid)).Column;
            if (column >= 0)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    this.CurrentCell = new CellRef(0, this.CurrentCell.Column);
                }
                else
                {
                    this.CurrentCell = new CellRef(0, column);
                }

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
        /// The e.
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
        /// The e.
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
        /// The e.
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
        /// The e.
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
        /// The e.
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
        /// The e.
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
        /// The e.
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
        /// The e.
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
        /// The delete execute.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DeleteExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Delete();
        }

        /// <summary>
        /// The delete rows.
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
        /// Enumerate the items in the specified cell range.
        ///   This is used to updated the SelectedItems property.
        /// </summary>
        /// <param name="cell0">
        /// The cell 0.
        /// </param>
        /// <param name="cell1">
        /// The cell 1.
        /// </param>
        private IEnumerable EnumerateItems(CellRef cell0, CellRef cell1)
        {
            var list = this.Content as IList;
            if (list != null)
            {
                int index0 = !this.ItemsInColumns ? cell0.Row : cell0.Column;
                int index1 = !this.ItemsInColumns ? cell1.Row : cell1.Column;
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
        /// The find next.
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
        /// <param name="deltaColumn">
        /// The delta column.
        /// </param>
        private void FindNext(ref int row, ref int column, int deltaRow, int deltaColumn)
        {
            while (row >= 0 && row < this.Rows && column >= 0 && column < this.Columns - 1)
            {
                var v = this.GetCellValue(new CellRef(row, column));
                if (v == null || string.IsNullOrEmpty(v.ToString()))
                {
                    break;
                }

                row += deltaRow;
                column += deltaColumn;
            }
        }

        /// <summary>
        /// The get cell ref from ui element.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <returns>
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
        /// </returns>
        private FrameworkElement GetColumnElement(int column)
        {
            return this.columnGrid.Children[1 + 3 * column + 1] as FrameworkElement;
        }

        /// <summary>
        /// The get column header.
        /// </summary>
        /// <param name="j">
        /// The j.
        /// </param>
        /// <returns>
        /// The get column header.
        /// </returns>
        private object GetColumnHeader(int j)
        {
            var text = CellRef.ToColumnName(j);

            if (!this.ItemsInColumns)
            {
                if (j < this.PropertyDefinitions.Count)
                {
                    return this.PropertyDefinitions[j].Header ?? text;
                }
            }

            return text;
        }

        /// <summary>
        /// The get column width.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        /// <returns>
        /// </returns>
        private GridLength GetColumnWidth(int i)
        {
            if (i < this.PropertyDefinitions.Count)
            {
                if (this.PropertyDefinitions[i].Width.Value < 0)
                {
                    return this.DefaultColumnWidth;
                }

                return this.PropertyDefinitions[i].Width;
            }

            return this.DefaultColumnWidth;
        }

        /// <summary>
        /// The get format string.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The get format string.
        /// </returns>
        private string GetFormatString(CellRef cell, object value)
        {
            int i = this.GetPropertyIndex(cell);
            if (i < this.PropertyDefinitions.Count)
            {
                return this.PropertyDefinitions[i].FormatString;
            }

            return null;
        }

        /// <summary>
        /// The get item.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// The get item.
        /// </returns>
        private object GetItem(CellRef cell)
        {
            var list = this.Content as IList;
            if (list != null)
            {
                int index = this.GetItemIndex(cell);
                if (index >= 0 && index < list.Count)
                {
                    return list[index];
                }
            }

            var items = this.Content as IEnumerable;
            if (items != null)
            {
                int i = 0;
                foreach (var item in items)
                {
                    if ((!this.ItemsInColumns && i == cell.Row) || (!this.ItemsInColumns && i == cell.Column))
                    {
                        return item;
                    }

                    i++;
                }
            }

            return null;
        }

        /// <summary>
        /// The get item index.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// The get item index.
        /// </returns>
        private int GetItemIndex(CellRef cell)
        {
            return !this.ItemsInColumns ? cell.Row : cell.Column;
        }

        /// <summary>
        /// The get property definition.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// </returns>
        private PropertyDefinition GetPropertyDefinition(CellRef cell)
        {
            int fieldIndex = this.GetPropertyIndex(cell);

            if (fieldIndex < this.PropertyDefinitions.Count)
            {
                return this.PropertyDefinitions[fieldIndex];
            }

            return null;
        }

        /// <summary>
        /// The get field index.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// The get field index.
        /// </returns>
        private int GetPropertyIndex(CellRef cell)
        {
            return !this.ItemsInColumns ? cell.Column : cell.Row;
        }

        /// <summary>
        /// The get row header.
        /// </summary>
        /// <param name="j">
        /// The j.
        /// </param>
        /// <returns>
        /// The get row header.
        /// </returns>
        private object GetRowHeader(int j)
        {
            var text = CellRef.ToRowName(j);

            if (this.ItemsInColumns)
            {
                if (j < this.PropertyDefinitions.Count)
                {
                    return this.PropertyDefinitions[j].Header;
                }
            }

            if (this.RowHeaders != null && j < this.RowHeaders.Count)
            {
                return this.RowHeaders[j];
            }

            return text;
        }

        /// <summary>
        /// The handle button down.
        /// </summary>
        /// <param name="e">
        /// The e.
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
                bool shift = Keyboard.IsKeyDown(Key.LeftShift);

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
        /// The insert cell element.
        /// </summary>
        /// <param name="cellRef">
        /// The cell ref.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="insert">
        /// The insert.
        /// </param>
        private void InsertCellElement(CellRef cellRef, object value, bool insert)
        {
            // if (value == null)
            // return;
            var e = this.CreateElement(cellRef, null, null);
            SetElementPosition(e, cellRef);
            if (insert)
            {
                this.sheetGrid.Children.Insert(this.cellInsertionIndex, e);
                this.cellInsertionIndex++;
            }
            else
            {
                this.sheetGrid.Children.Add(e);
            }

            this.cellMap.Add(cellRef.GetHashCode(), e);
        }

        /// <summary>
        /// The insert rows.
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
        /// The e.
        /// </param>
        private void OnContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
        /// The e.
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
        /// The e.
        /// </param>
        private void RowGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();

            int row = this.GetCell(e.GetPosition(this.rowGrid)).Row;
            if (row >= 0)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    this.CurrentCell = new CellRef(this.CurrentCell.Row, 0);
                }
                else
                {
                    this.CurrentCell = new CellRef(row, 0);
                }

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
        /// The e.
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
        /// The e.
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
        /// The e.
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
        /// The e.
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
        /// The selection to string.
        /// </summary>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <param name="encode">
        /// The encode.
        /// </param>
        /// <returns>
        /// The selection to string.
        /// </returns>
        private string SelectionToString(string separator, bool encode = false)
        {
            return this.ToString(this.CurrentCell, this.SelectionCell, separator, encode);
        }

        /// <summary>
        /// The set check in selected cells.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The set check in selected cells.
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
        /// The sheet mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SheetMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.ShowTextBoxEditor();
                e.Handled = true;
            }
        }

        /// <summary>
        /// The sheet to string.
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
        /// The subscribe notifications.
        /// </summary>
        private void SubscribeToNotifications()
        {
            var ncc = this.Content as INotifyCollectionChanged;
            if (ncc != null)
            {
                ncc.CollectionChanged += this.OnContentCollectionChanged;
            }

            this.subcribedContent = this.Content;
        }

        /// <summary>
        /// The text editor lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextEditorLostFocus(object sender, RoutedEventArgs e)
        {
            this.EndTextEdit();
        }

        /// <summary>
        /// The text editor was loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void TextEditorLoaded(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.CaretIndex = tb.Text.Length;
            tb.SelectAll();
        }

        /// <summary>
        /// Handles keydown events in the TextBox editor.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TextEditorPreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool shift = Keyboard.IsKeyDown(Key.LeftShift);
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
        /// The to string.
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
        /// The encode.
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
        /// The toggle check.
        /// </summary>
        /// <returns>
        /// The toggle check.
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
        /// The e.
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
            var ncc = this.subcribedContent as INotifyCollectionChanged;
            if (ncc != null)
            {
                ncc.CollectionChanged -= this.OnContentCollectionChanged;
            }

            this.subcribedContent = null;
        }

        /// <summary>
        /// The update sheet.
        /// </summary>
        /// <param name="rows">
        /// The rows.
        /// </param>
        /// <param name="columns">
        /// The columns.
        /// </param>
        private void UpdateCells(int rows, int columns)
        {
            // int rank = cells.Rank;
            // int rows = cells.GetUpperBound(0) + 1;
            // int columns = rank > 1 ? cells.GetUpperBound(1) + 1 : 1;
            this.sheetGrid.Children.Clear();
            this.sheetGrid.Children.Add(this.selectionBackground);
            this.sheetGrid.Children.Add(this.currentBackground);
            this.cellMap.Clear();

            // todo: UI virtualize grid lines (both rows and columns)

            // Add row lines to the sheet
            for (int i = 1; i <= rows; i++)
            {
                var border = new Border
                    {
                        BorderBrush = this.GridLineBrush,
                        BorderThickness = new Thickness(0, 1, 0, 0)
                    };

                if (i < rows && this.AlternatingRowsBackground != null && i % 2 == 1)
                {
                    border.Background = this.AlternatingRowsBackground;
                }

                Grid.SetColumn(border, 0);
                if (columns > 0)
                {
                    Grid.SetColumnSpan(border, columns);
                }

                Grid.SetRow(border, i);
                this.sheetGrid.Children.Add(border);
            }

            if (rows > 0)
            {
                // Add column lines to the sheet
                for (int i = 0; i < columns; i++)
                {
                    if (i == 0 && columns > 1)
                    {
                        continue;
                    }

                    var border = new Border
                        {
                            BorderBrush = this.GridLineBrush,
                            BorderThickness = new Thickness(i > 0 ? 1 : 0, 0, i == columns - 1 ? 1 : 0, 0)
                        };

                    Grid.SetRow(border, 0);
                    Grid.SetRowSpan(border, rows);
                    Grid.SetColumn(border, i);
                    this.sheetGrid.Children.Add(border);
                }
            }

            this.cellInsertionIndex = this.sheetGrid.Children.Count;

            // Add all cells to the sheet
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var cell = new CellRef(i, j);
                    var value = this.GetCellValue(cell);
                    this.AddCellElement(cell, value);
                }
            }

            this.sheetGrid.Children.Add(this.selection);
            this.sheetGrid.Children.Add(this.autoFillBox);
            this.sheetGrid.Children.Add(this.autoFillSelection);
        }

        /// <summary>
        /// The update column widths.
        /// </summary>
        private void UpdateColumnWidths()
        {
            this.sheetGrid.UpdateLayout();

            for (int i = 0; i < this.Columns; i++)
            {
                if (this.GetColumnWidth(i) == GridLength.Auto || this.AutoSizeColumns)
                {
                    this.AutoSizeColumn(i);
                }
            }

            this.sheetGrid.UpdateLayout();

            for (int j = 0; j < this.sheetGrid.ColumnDefinitions.Count; j++)
            {
                this.columnGrid.ColumnDefinitions[j].Width =
                    new GridLength(this.sheetGrid.ColumnDefinitions[j].ActualWidth);
            }
        }

        /// <summary>
        /// The update columns.
        /// </summary>
        /// <param name="columns">
        /// The columns.
        /// </param>
        private void UpdateColumns(int columns)
        {
            this.Columns = columns;
            this.rowGrid.ColumnDefinitions.Clear();
            this.sheetGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < columns; i++)
            {
                var w = this.GetColumnWidth(i);
                this.sheetGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = w });

                // the width of the header column will be updated later
                this.columnGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            }

            // Add one empty column covering the vertical scrollbar
            this.columnGrid.ColumnDefinitions.Add(
                new System.Windows.Controls.ColumnDefinition { Width = new GridLength(40) });

            this.columnGrid.Children.Clear();
            this.columnGrid.Children.Add(this.columnSelectionBackground);
            for (int j = 0; j < columns; j++)
            {
                object header = this.GetColumnHeader(j);
                var cellref = new CellRef(!this.ItemsInColumns ? -1 : j, !this.ItemsInColumns ? j : -1);
                var pd = this.GetPropertyDefinition(cellref);

                var border = new Border
                    {
                        BorderBrush = this.HeaderBorderBrush,
                        BorderThickness = new Thickness(0, 1, 1, 1),
                        Margin = new Thickness(0, 0, j < columns - 1 ? -1 : 0, 0)
                    };
                Grid.SetColumn(border, j);
                this.columnGrid.Children.Add(border);
                var cell = new TextBlock
                    {
                        Text = header != null ? header.ToString() : "-",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = pd.HorizontalAlignment,
                        Padding = new Thickness(4, 2, 4, 2)
                    };

                Grid.SetColumn(cell, j);
                this.columnGrid.Children.Add(cell);

                if (this.CanResizeColumns)
                {
                    var splitter = new GridSplitter
                        {
                            ResizeDirection = GridResizeDirection.Columns,
                            Background = Brushes.Transparent,
                            Width = 4,
                            Focusable = false,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            HorizontalAlignment = HorizontalAlignment.Right
                        };
                    splitter.MouseDoubleClick += this.ColumnSplitterDoubleClick;
                    splitter.DragStarted += this.ColumnSplitterChangeStarted;
                    splitter.DragDelta += this.ColumnSplitterChangeDelta;
                    splitter.DragCompleted += this.ColumnSplitterChangeCompleted;
                    Grid.SetColumn(splitter, j);
                    this.columnGrid.Children.Add(splitter);
                }
            }
        }

        /// <summary>
        /// Updates all the UIElements of the grid (both cells, headers, row and column lines).
        /// </summary>
        private void UpdateGridContent()
        {
            this.UnsubscribeNotifications();

            if (this.sheetGrid == null)
            {
                // return if the template has not yet been applied
                return;
            }

            int rows = -1;
            int columns = -1;

            if (this.AutoGenerateColumns && this.PropertyDefinitions.Count == 0)
            {
                this.GeneratePropertyDefinitions(this.Content as IEnumerable);
            }

            var items = this.Content as IEnumerable;
            if (items != null)
            {
                int n = items.Cast<object>().Count();
                int m = this.PropertyDefinitions.Count;

                rows = !this.ItemsInColumns ? n : m;
                columns = !this.ItemsInColumns ? m : n;
            }

            var visibility = rows >= 0 ? Visibility.Visible : Visibility.Hidden;

            // Hide the row/column headers if the content is empty
            this.rowScroller.Visibility =
                this.columnScroller.Visibility = this.sheetScroller.Visibility = this.topleft.Visibility = visibility;

            if (rows < 0)
            {
                return;
            }

            this.UpdateRows(rows);
            this.UpdateColumns(columns);
            this.UpdateCells(rows, columns);

            this.UpdateColumnWidths();

            this.SubscribeToNotifications();
            this.BuildContextMenus();
        }

        /// <summary>
        /// The update rows.
        /// </summary>
        /// <param name="rows">
        /// The rows.
        /// </param>
        private void UpdateRows(int rows)
        {
            this.rowGrid.RowDefinitions.Clear();
            this.sheetGrid.RowDefinitions.Clear();
            this.rowGrid.Children.Clear();
            this.rowGrid.Children.Add(this.rowSelectionBackground);

            this.Rows = rows;

            for (int i = 0; i < rows; i++)
            {
                this.sheetGrid.RowDefinitions.Add(new RowDefinition { Height = this.DefaultRowHeight });
                this.rowGrid.RowDefinitions.Add(new RowDefinition { Height = this.DefaultRowHeight });
            }

            for (int i = 0; i < rows; i++)
            {
                object header = this.GetRowHeader(i);

                var border = new Border
                    {
                        BorderBrush = this.HeaderBorderBrush,
                        BorderThickness = new Thickness(1, 0, 1, 1),
                        Margin = new Thickness(0, 0, 0, -1)
                    };

                Grid.SetRow(border, i);
                this.rowGrid.Children.Add(border);

                var cell = header as FrameworkElement;
                if (cell == null)
                {
                    cell = new TextBlock
                        {
                            Text = header != null ? header.ToString() : "-",
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Padding = new Thickness(4, 2, 4, 2)
                        };
                }

                if (this.ItemHeaderPropertyPath != null && !this.ItemsInColumns)
                {
                    cell.DataContext = this.GetItem(new CellRef(i, -1));
                    cell.SetBinding(TextBlock.TextProperty, new Binding(this.ItemHeaderPropertyPath));
                }

                Grid.SetRow(cell, i);
                this.rowGrid.Children.Add(cell);
            }

            // Add "Insert" row header
            if (this.CanInsertRows && this.AddItemHeader != null)
            {
                this.sheetGrid.RowDefinitions.Add(new RowDefinition { Height = this.DefaultRowHeight });
                this.rowGrid.RowDefinitions.Add(new RowDefinition { Height = this.DefaultRowHeight });

                var cell = new TextBlock
                    {
                        Text = this.AddItemHeader,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                var border = new Border
                    {
                        Background = Brushes.Transparent,
                        BorderBrush = this.HeaderBorderBrush,
                        BorderThickness = new Thickness(1, 0, 1, 1),
                        Margin = new Thickness(0, 0, 0, 0)
                    };

                border.MouseLeftButtonDown += this.AddItemCellMouseLeftButtonDown;
                Grid.SetRow(border, rows);

                cell.Padding = new Thickness(4, 2, 4, 2);
                Grid.SetRow(cell, rows);
                this.rowGrid.Children.Add(cell);
                this.rowGrid.Children.Add(border);
            }
            else
            {
                this.sheetGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                this.rowGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // to cover a posisble scrollbar
            this.rowGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
        }

        #endregion
    }
}