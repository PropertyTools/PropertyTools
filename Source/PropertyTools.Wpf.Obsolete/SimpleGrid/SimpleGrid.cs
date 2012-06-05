// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleGrid.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
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
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Represents a datagrid with a spreadsheet style.
    /// </summary>
    [ContentProperty("Content")]
    [TemplatePart(Name = PART_Grid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_SheetScroller, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PART_SheetGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_ColumnScroller, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PART_ColumnGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_RowScroller, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PART_RowGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_SelectionBackground, Type = typeof(Border))]
    [TemplatePart(Name = PART_RowSelectionBackground, Type = typeof(Border))]
    [TemplatePart(Name = PART_ColumnSelectionBackground, Type = typeof(Border))]
    [TemplatePart(Name = PART_CurrentBackground, Type = typeof(Border))]
    [TemplatePart(Name = PART_Selection, Type = typeof(Border))]
    [TemplatePart(Name = PART_AutoFillSelection, Type = typeof(Border))]
    [TemplatePart(Name = PART_AutoFillBox, Type = typeof(Border))]
    [TemplatePart(Name = PART_TopLeft, Type = typeof(Border))]
    [TemplatePart(Name = PART_TextEditor, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_EnumEditor, Type = typeof(ComboBox))]
    public partial class SimpleGrid : Control
    {
        #region Constants and Fields

        /// <summary>
        ///   The par t_ auto fill box.
        /// </summary>
        private const string PART_AutoFillBox = "PART_AutoFillBox";

        /// <summary>
        ///   The par t_ auto fill selection.
        /// </summary>
        private const string PART_AutoFillSelection = "PART_AutoFillSelection";

        /// <summary>
        ///   The par t_ column grid.
        /// </summary>
        private const string PART_ColumnGrid = "PART_ColumnGrid";

        /// <summary>
        ///   The par t_ column scroller.
        /// </summary>
        private const string PART_ColumnScroller = "PART_ColumnScroller";

        /// <summary>
        ///   The par t_ column selection background.
        /// </summary>
        private const string PART_ColumnSelectionBackground = "PART_ColumnSelectionBackground";

        /// <summary>
        ///   The par t_ current background.
        /// </summary>
        private const string PART_CurrentBackground = "PART_CurrentBackground";

        /// <summary>
        ///   The par t_ enum editor.
        /// </summary>
        private const string PART_EnumEditor = "PART_EnumEditor";

        /// <summary>
        ///   The par t_ grid.
        /// </summary>
        private const string PART_Grid = "PART_Grid";

        /// <summary>
        ///   The par t_ row grid.
        /// </summary>
        private const string PART_RowGrid = "PART_RowGrid";

        /// <summary>
        ///   The par t_ row scroller.
        /// </summary>
        private const string PART_RowScroller = "PART_RowScroller";

        /// <summary>
        ///   The par t_ row selection background.
        /// </summary>
        private const string PART_RowSelectionBackground = "PART_RowSelectionBackground";

        /// <summary>
        ///   The par t_ selection.
        /// </summary>
        private const string PART_Selection = "PART_Selection";

        /// <summary>
        ///   The par t_ selection background.
        /// </summary>
        private const string PART_SelectionBackground = "PART_SelectionBackground";

        /// <summary>
        ///   The par t_ sheet grid.
        /// </summary>
        private const string PART_SheetGrid = "PART_SheetGrid";

        /// <summary>
        ///   The par t_ sheet scroller.
        /// </summary>
        private const string PART_SheetScroller = "PART_SheetScroller";

        /// <summary>
        ///   The par t_ text editor.
        /// </summary>
        private const string PART_TextEditor = "PART_TextEditor";

        /// <summary>
        ///   The par t_ top left.
        /// </summary>
        private const string PART_TopLeft = "PART_TopLeft";

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
        ///   The content editor.
        /// </summary>
        private ContentControl contentEditor;

        /// <summary>
        ///   The current background.
        /// </summary>
        private Border currentBackground;

        /// <summary>
        ///   The editing cells.
        /// </summary>
        private IEnumerable<CellRef> editingCells;

        /// <summary>
        ///   The end pressed.
        /// </summary>
        private bool endPressed;

        /// <summary>
        ///   The enum editor.
        /// </summary>
        private ComboBox enumEditor;

        /// <summary>
        ///   The grid.
        /// </summary>
        private Grid grid;

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
        ///   The text editor.
        /// </summary>
        private TextBox textEditor;

        /// <summary>
        ///   The topleft.
        /// </summary>
        private Border topleft;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "SimpleGrid" /> class.
        /// </summary>
        static SimpleGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SimpleGrid), new FrameworkPropertyMetadata(typeof(SimpleGrid)));
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets a value indicating whether to use column definitions.
        /// </summary>
        protected bool UseColumnDefinitions
        {
            get
            {
                return this.ColumnDefinitions.Count > 0;
            }
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
        /// The cancel text edit.
        /// </summary>
        public void CancelTextEdit()
        {
            this.textEditor.Visibility = Visibility.Hidden;
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
        /// The delete item.
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
            if (this.Content is Array)
            {
                return false;
            }

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
        public void EndTextEdit()
        {
            if (this.textEditor.Visibility == Visibility.Hidden)
            {
                return;
            }

            foreach (var cell in this.editingCells)
            {
                this.TrySetCellValue(cell, this.textEditor.Text);
            }

            this.textEditor.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// The get cell.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <returns>
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
        /// The get cell element.
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
        /// The get cell string.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// The get cell string.
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

            if (this.IsArray())
            {
                var cells = (Array)this.Content;
                int rank = cells.Rank;
                if (cell.Row > cells.GetUpperBound(0) || (rank > 1 && cell.Column > cells.GetUpperBound(1)))
                {
                    Debug.WriteLine("GetCellValue error " + cell);
                    return null;
                }

                var value = rank > 1 ? cells.GetValue(cell.Row, cell.Column) : cells.GetValue(cell.Row);
                return value;
            }

            var item = this.GetItem(cell);
            if (item != null)
            {
                var type = item.GetType();
                var dataField = this.GetDataField(cell);
                if (dataField != null)
                {
                    var pi = type.GetProperty(dataField);
                    if (pi == null)
                    {
                        return item;
                    }

                    return pi.GetValue(item, null);
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
            this.contentEditor.Visibility = Visibility.Hidden;
            this.enumEditor.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// The insert item.
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
            if (this.Content is Array)
            {
                return false;
            }

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

            this.grid = this.Template.FindName(PART_Grid, this) as Grid;
            this.sheetScroller = this.Template.FindName(PART_SheetScroller, this) as ScrollViewer;
            this.sheetGrid = this.Template.FindName(PART_SheetGrid, this) as Grid;
            this.columnScroller = this.Template.FindName(PART_ColumnScroller, this) as ScrollViewer;
            this.columnGrid = this.Template.FindName(PART_ColumnGrid, this) as Grid;
            this.rowScroller = this.Template.FindName(PART_RowScroller, this) as ScrollViewer;
            this.rowGrid = this.Template.FindName(PART_RowGrid, this) as Grid;
            this.rowSelectionBackground = this.Template.FindName(PART_RowSelectionBackground, this) as Border;
            this.columnSelectionBackground = this.Template.FindName(PART_ColumnSelectionBackground, this) as Border;
            this.selectionBackground = this.Template.FindName(PART_SelectionBackground, this) as Border;
            this.currentBackground = this.Template.FindName(PART_CurrentBackground, this) as Border;
            this.selection = this.Template.FindName(PART_Selection, this) as Border;
            this.autoFillSelection = this.Template.FindName(PART_AutoFillSelection, this) as Border;
            this.autoFillBox = this.Template.FindName(PART_AutoFillBox, this) as Border;
            this.topleft = this.Template.FindName(PART_TopLeft, this) as Border;

            this.textEditor = this.Template.FindName(PART_TextEditor, this) as TextBox;
            this.enumEditor = this.Template.FindName(PART_EnumEditor, this) as ComboBox;
            this.contentEditor = new ContentControl();

            this.enumEditor.SelectionChanged += this.EnumEditorSelectionChanged;

            this.textEditor.PreviewKeyDown += this.TextEditorPreviewKeyDown;
            this.textEditor.LostFocus += this.TextEditorLostFocus;

            this.sheetScroller.ScrollChanged += this.ScrollViewerScrollChanged;
            this.rowScroller.ScrollChanged += this.RowScrollerChanged;
            this.columnScroller.ScrollChanged += this.ColumnScrollerChanged;
            this.sheetScroller.SizeChanged += this.ScrollViewerSizeChanged;

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

            this.autoFiller = new AutoFiller(this.GetCellValue, this.TrySetCellValue);

            this.autoFillToolTip = new ToolTip
                {
                   Placement = PlacementMode.Bottom, PlacementTarget = this.autoFillSelection 
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
        /// The show combo box editor.
        /// </summary>
        /// <returns>
        /// The show combo box editor.
        /// </returns>
        public bool ShowComboBoxEditor()
        {
            var value = this.GetCellValue(this.CurrentCell);
            var alternatives = this.GetCellAlternatives(this.CurrentCell, value);
            if (alternatives == null)
            {
                return false;
            }

            SetElementPosition(this.enumEditor, this.CurrentCell);

            // set editingCells to null to avoid setting values when enumEditor.SelectedValue is set below
            this.editingCells = null;

            this.enumEditor.ItemsSource = alternatives;
            this.enumEditor.SelectedValue = value;
            this.enumEditor.Visibility = Visibility.Visible;

            this.editingCells = this.SelectedCells.ToList();

            return true;
        }

        /// <summary>
        /// The show editor.
        /// </summary>
        /// <returns>
        /// The show editor.
        /// </returns>
        public bool ShowEditor()
        {
            var value = this.GetCellValue(this.CurrentCell);
            if (value is Enum)
            {
                return this.ShowComboBoxEditor();
            }

            if (value != null)
            {
                var type = value.GetType();
                DataTemplate template = null;

                var cd = this.GetColumnDefinition(this.CurrentCell);
                if (cd != null && cd.EditTemplate != null)
                {
                    template = cd.EditTemplate;
                    this.contentEditor.Content = this.GetItem(this.CurrentCell);
                }

                if (template == null)
                {
                    template = this.GetEditTemplate(type);
                    this.contentEditor.Content = this.GetCellValue(this.CurrentCell);
                }

                if (template != null)
                {
                    this.contentEditor.ContentTemplate = template;
                    SetElementPosition(this.contentEditor, this.CurrentCell);
                    this.contentEditor.Visibility = Visibility.Visible;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The show text box editor.
        /// </summary>
        public void ShowTextBoxEditor()
        {
            this.editingCells = this.SelectedCells.ToList();

            Grid.SetColumn(this.textEditor, this.CurrentCell.Column);
            Grid.SetRow(this.textEditor, this.CurrentCell.Row);

            this.textEditor.Text = this.GetCellString(this.CurrentCell);
            this.textEditor.TextAlignment = ToTextAlignment(this.GetHorizontalAlignment(this.CurrentCell));

            this.textEditor.Visibility = Visibility.Visible;
            this.textEditor.Focus();
            this.textEditor.CaretIndex = this.textEditor.Text.Length;
            this.textEditor.SelectAll();
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
            Array cells;

            if (this.IsArray())
            {
                cells = (Array)this.Content;
                var eltype = cells.GetType().GetElementType();
                object convertedValue;
                if (TryConvert(value, eltype, out convertedValue))
                {
                    try
                    {
                        if (cells.Rank > 1)
                        {
                            cells.SetValue(convertedValue, cell.Row, cell.Column);
                        }
                        else
                        {
                            cells.SetValue(convertedValue, cell.Row);
                        }

                        this.UpdateCellContent(cell);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                // wrong type
                return false;
            }

            var items = this.Content as IEnumerable;
            if (items != null)
            {
                var current = this.GetItem(cell);

                var field = this.GetDataField(cell);
                if (field == null)
                {
                    return false;
                }

                PropertyInfo pi = null;
                if (current != null)
                {
                    pi = current.GetType().GetProperty(field);
                }

                if (pi == null)
                {
                    var list = this.Content as IList;
                    int itemIndex = this.GetItemIndex(cell);
                    object convertedValue1;
                    var elementType = list.AsQueryable().ElementType;
                    if (TryConvert(value, elementType, out convertedValue1))
                    {
                        list[itemIndex] = convertedValue1;
                    }

                    this.UpdateCellContent(cell);
                    return true;
                }

                object convertedValue;
                if (current != null && pi.CanWrite && TryConvert(value, pi.PropertyType, out convertedValue))
                {
                    pi.SetValue(current, convertedValue, null);
                    {
                        // if (!(current is INotifyPropertyChanged))
                        this.UpdateCellContent(cell);
                    }

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
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual UIElement CreateElement(CellRef cell, object item = null)
        {
            FrameworkElement element = null;

            if (item == null)
            {
                item = this.GetItem(cell);
            }

            var cd = this.GetColumnDefinition(cell);
            if (cd != null && cd.DisplayTemplate != null)
            {
                element = new ContentControl { ContentTemplate = cd.DisplayTemplate, Content = item };

                // note: vertical/horziontal alignment must be set in the DataTemplate
            }

            if (element == null)
            {
                var value = this.GetCellValue(cell);
                var type = value != null ? value.GetType() : null;

                var template = this.GetDisplayTemplate(type);
                if (template != null)
                {
                    element = new ContentControl { ContentTemplate = template, Content = value };
                }

                var binding = this.CreateBinding(cell, value);

                if (element == null && type == typeof(bool))
                {
                    var chkbox = new CheckBox { Cursor = Cursors.Arrow };
                    if (binding != null)
                    {
                        chkbox.SetBinding(ToggleButton.IsCheckedProperty, binding);
                    }
                    else
                    {
                        chkbox.IsChecked = (bool)value;
                        chkbox.Checked += this.CellChecked;
                    }

                    element = chkbox;
                }

                if (element == null && typeof(BitmapSource).IsAssignableFrom(type))
                {
                    var chkbox = new Image { Source = (BitmapSource)value, Stretch = Stretch.None };
                    element = chkbox;
                }

                // if (element == null && typeof(Uri).IsAssignableFrom(type))
                // {
                // element = new TextBlock(new Hyperlink(new Run(value != null ? value.ToString() : null)) { NavigateUri = (Uri)value });
                // }
                if (element == null)
                {
                    var textBlock = new TextBlock { Margin = new Thickness(4, 0, 4, 0), Foreground = this.Foreground };
                    if (binding != null)
                    {
                        textBlock.SetBinding(TextBlock.TextProperty, binding);
                    }
                    else
                    {
                        var formatString = this.GetFormatString(cell, value);
                        var text = FormatValue(value, formatString);
                        textBlock.Text = text;
                    }

                    element = textBlock;
                }

                element.Tag = type;

                if (binding != null)
                {
                    element.DataContext = item;
                }

                element.VerticalAlignment = VerticalAlignment.Center;
                element.HorizontalAlignment = this.GetHorizontalAlignment(cell);
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
        protected virtual void GenerateColumnDefinitions(IEnumerable items)
        {
            if (items == null)
            {
                return;
            }

            var itemType = GetListItemType(items.GetType());

            // todo: how to find the right type?
            if (itemType == null)
            {
                itemType = GetListItemType(items);
            }

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(itemType))
            {
                if (!descriptor.IsBrowsable)
                {
                    continue;
                }

                var displayName = descriptor.DisplayName;
                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = descriptor.Name;
                }

                this.ColumnDefinitions.Add(new ColumnDefinition { DataField = descriptor.Name, Header = displayName });
            }
        }

        /// <summary>
        /// Gets the alternative values for the cell.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual IEnumerable GetCellAlternatives(CellRef cell, object value)
        {
            if (value == null)
            {
                value = this.GetCellValue(cell);
            }

            var enumValue = value as Enum;
            if (enumValue != null)
            {
                return Enum.GetValues(enumValue.GetType());
            }

            return null;
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
        /// The is cell visible.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// The is cell visible.
        /// </returns>
        protected bool IsCellVisible(CellRef cell)
        {
            if (this.IsVirtualizing)
            {
                // todo: should store topleft and bottomright visible cells
                // and check against these
            }

            return true;
        }

        /// <summary>
        /// The on key down.
        /// </summary>
        /// <param name="e">
        /// The e.
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

                    bool value = true;
                    var cvalue = this.GetCellValue(this.CurrentCell);
                    if (cvalue is bool)
                    {
                        value = (bool)cvalue;
                        value = !value;
                    }

                    if (this.SetCheckInSelectedCells(value))
                    {
                        e.Handled = true;
                    }

                    if (this.OpenCombo())
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

            this.ShowTextBoxEditor();

            this.textEditor.Text = e.Text;
            this.textEditor.CaretIndex = this.textEditor.Text.Length;
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
        /// Virtualizes the UIElements for the visible cells.
        ///   Adds elements for the visible cells not currently in the logical tree.
        ///   Removes elements for the nonvisible cells.
        /// </summary>
        protected void VirtualizeCells()
        {
            CellRef cell1, cell2;
            this.GetVisibleCells(out cell1, out cell2);
            if (cell1.Column < 0)
            {
                return;
            }

            var delete = this.cellMap.Keys.ToList();

            for (int i = cell1.Row; i <= cell2.Row; i++)
            {
                for (int j = cell1.Column; j <= cell2.Column; j++)
                {
                    var cellRef = new CellRef(i, j);
                    var c = this.GetCellElement(cellRef);
                    if (c == null)
                    {
                        // The cell is not currently in the collection - add it
                        this.UpdateCellContent(cellRef);
                    }
                    else
                    {
                        // the cell is currently in the collection - keep it (remove it from the delete keys)
                        delete.Remove(cellRef.GetHashCode());
                    }
                }
            }

            foreach (var hash in delete)
            {
                var cell = this.cellMap[hash];
                this.sheetGrid.Children.Remove(cell);
                this.cellInsertionIndex--;
                this.cellMap.Remove(hash);
            }
        }

        // protected int GetIndexOfProperty(string propertyName)
        // {
        // if (DataFields == null)
        // {
        // return -1;
        // }

        // for (int i = 0; i < DataFields.Count; i++)
        // {
        // if (DataFields[i] == propertyName)
        // {
        // return i;
        // }
        // }

        // return -1;
        // }

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
            var gs = (GridSplitter)sender;
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
        /// The create binding.
        /// </summary>
        /// <param name="cellRef">
        /// The cell ref.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        private Binding CreateBinding(CellRef cellRef, object value)
        {
            var dataField = this.GetDataField(cellRef);
            if (dataField != null)
            {
                return new Binding(dataField) { StringFormat = this.GetFormatString(cellRef, value) };
            }

            return null;
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
        /// The enum editor selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void EnumEditorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.editingCells == null)
            {
                return;
            }

            foreach (var cell in this.editingCells)
            {
                this.TrySetCellValue(cell, this.enumEditor.SelectedValue);
            }
        }

        /// <summary>
        /// The enumerate cells.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<CellRef> EnumerateCells(object item, string propertyName)
        {
            if (this.Content is IEnumerable)
            {
                int i = 0;
                foreach (var it in this.Content as IEnumerable)
                {
                    if (it == item)
                    {
                        for (int j = 0; j < this.ColumnDefinitions.Count; j++)
                        {
                            if (this.ColumnDefinitions[j].DataField == propertyName)
                            {
                                var cell = !this.ItemsInColumns ? new CellRef(i, j) : new CellRef(j, i);
                                yield return cell;
                            }
                        }
                    }

                    i++;
                }
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
            if (!this.IsArray())
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
        /// The get column definition.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// </returns>
        private ColumnDefinition GetColumnDefinition(CellRef cell)
        {
            int fieldIndex = this.GetFieldIndex(cell);
            if (fieldIndex < this.ColumnDefinitions.Count)
            {
                return this.ColumnDefinitions[fieldIndex];
            }

            return null;
        }

        /// <summary>
        /// The get column element.
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
                if (j < this.ColumnDefinitions.Count)
                {
                    return this.ColumnDefinitions[j].Header ?? text;
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
            if (i < this.ColumnDefinitions.Count)
            {
                if (this.ColumnDefinitions[i].Width.Value < 0)
                {
                    return this.DefaultColumnWidth;
                }

                return this.ColumnDefinitions[i].Width;
            }

            return this.DefaultColumnWidth;
        }

        /// <summary>
        /// The get custom template.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// </returns>
        private TypeDefinition GetCustomTemplate(Type type)
        {
            foreach (var e in this.TypeDefinitions)
            {
                var et = e.Type;
                if (et.IsAssignableFrom(type))
                {
                    return e;
                }
            }

            return null;
        }

        /// <summary>
        /// The get data field.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// The get data field.
        /// </returns>
        private string GetDataField(CellRef cell)
        {
            int fieldIndex = this.GetFieldIndex(cell);

            if (fieldIndex < this.ColumnDefinitions.Count)
            {
                return this.ColumnDefinitions[fieldIndex].DataField;
            }

            return null;
        }

        /// <summary>
        /// The get display template.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// </returns>
        private DataTemplate GetDisplayTemplate(Type type)
        {
            var e = this.GetCustomTemplate(type);
            return e != null ? e.DisplayTemplate : null;
        }

        /// <summary>
        /// The get edit template.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// </returns>
        private DataTemplate GetEditTemplate(Type type)
        {
            var e = this.GetCustomTemplate(type);
            return e != null ? e.EditTemplate : null;
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
        private int GetFieldIndex(CellRef cell)
        {
            return !this.ItemsInColumns ? cell.Column : cell.Row;
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
            if (value != null)
            {
                var ct = this.GetCustomTemplate(value.GetType());
                if (ct != null && ct.FormatString != null)
                {
                    return ct.FormatString;
                }
            }

            int i = this.GetFieldIndex(cell);
            if (i < this.ColumnDefinitions.Count)
            {
                return this.ColumnDefinitions[i].FormatString;
            }

            return null;
        }

        /// <summary>
        /// The get horizontal alignment.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// </returns>
        private HorizontalAlignment GetHorizontalAlignment(CellRef cell)
        {
            int i = this.GetFieldIndex(cell);
            if (i < this.ColumnDefinitions.Count)
            {
                return this.ColumnDefinitions[i].HorizontalAlignment;
            }

            return this.DefaultHorizontalAlignment;
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
            if (this.IsArray())
            {
                return null;
            }

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
                    if ((!this.ItemsInColumns && i == cell.Row) || (!!this.ItemsInColumns && i == cell.Column))
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
                if (j < this.ColumnDefinitions.Count)
                {
                    return this.ColumnDefinitions[j].Header;
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
            var e = this.CreateElement(cellRef, null);
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
        /// The is array.
        /// </summary>
        /// <returns>
        /// The is array.
        /// </returns>
        private bool IsArray()
        {
            var cells = this.Content as Array;
            if (cells != null && cells.Rank >= 2)
            {
                return true;
            }

            return cells != null && this.ColumnDefinitions.Count == 0;
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
            // todo: update rows
            this.UpdateGridContent();
        }

        /// <summary>
        /// Called when any item in the Content is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnContentItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (var cell in this.EnumerateCells(sender, e.PropertyName))
            {
                if (this.IsCellVisible(cell))
                {
                    this.UpdateCellContent(cell);
                }
            }
        }

        /// <summary>
        /// The open combo.
        /// </summary>
        /// <returns>
        /// The open combo.
        /// </returns>
        private bool OpenCombo()
        {
            if (this.enumEditor.Visibility == Visibility.Visible)
            {
                this.enumEditor.IsDropDownOpen = true;
                this.enumEditor.Focus();
                return true;
            }

            return false;
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

            if (this.IsVirtualizing)
            {
                this.VirtualizeCells();
            }
        }

        /// <summary>
        /// The scroll viewer size changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ScrollViewerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.IsVirtualizing)
            {
                this.VirtualizeCells();
            }
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
        private void SubscribeNotifications()
        {
            var ncc = this.Content as INotifyCollectionChanged;
            if (ncc != null)
            {
                ncc.CollectionChanged += this.OnContentCollectionChanged;
            }

            var items = this.Content as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                {
                    var npc = item as INotifyPropertyChanged;
                    if (npc != null)
                    {
                        npc.PropertyChanged += this.OnContentItemPropertyChanged;
                    }
                }
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
        /// The text editor preview key down.
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
            switch (e.Key)
            {
                case Key.Left:
                    if (this.textEditor.CaretIndex == 0)
                    {
                        this.EndTextEdit();
                        this.OnKeyDown(e);
                        e.Handled = true;
                    }

                    break;
                case Key.Right:
                    if (this.textEditor.CaretIndex == this.textEditor.Text.Length)
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
                    this.CancelTextEdit();
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
        /// The toggle check in selected cells.
        /// </summary>
        /// <returns>
        /// The toggle check in selected cells.
        /// </returns>
        private bool ToggleCheckInSelectedCells()
        {
            bool modified = false;
            foreach (var cell in this.SelectedCells)
            {
                var currentValue = this.GetCellValue(cell);
                if (currentValue is bool)
                {
                    if (this.TrySetCellValue(cell, !((bool)currentValue)))
                    {
                        modified = true;
                    }
                }
            }

            return modified;
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

            var items = this.subcribedContent as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                {
                    var npc = item as INotifyPropertyChanged;
                    if (npc != null)
                    {
                        npc.PropertyChanged -= this.OnContentItemPropertyChanged;
                    }
                }
            }

            this.subcribedContent = null;
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

                var border = new Border
                    {
                        BorderBrush = this.HeaderBorderBrush, 
                        BorderThickness = new Thickness(0, 1, 1, 1), 
                        Margin = new Thickness(0, 0, j < columns - 1 ? -1 : 0, 0)
                    };
                Grid.SetColumn(border, j);
                this.columnGrid.Children.Add(border);

                var cell = header as FrameworkElement;
                if (cell == null)
                {
                    cell = new TextBlock
                        {
                            Text = header != null ? header.ToString() : "-", 
                            VerticalAlignment = VerticalAlignment.Center, 
                            HorizontalAlignment =
                                this.GetHorizontalAlignment(
                                    new CellRef(!this.ItemsInColumns ? -1 : j, !this.ItemsInColumns ? j : -1)), 
                            Padding = new Thickness(4, 2, 4, 2)
                        };
                }

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

            var cells = this.Content as Array;
            int rows = -1;
            int columns = -1;
            int rank = cells != null ? cells.Rank : 0;

            if (this.AutoGenerateColumns && this.ColumnDefinitions.Count == 0 && rank < 2)
            {
                this.GenerateColumnDefinitions(this.Content as IEnumerable);
            }

            if (cells != null && (this.ColumnDefinitions.Count == 0 || rank >= 2))
            {
                // Content is Array, and no ColumnDefinitions are defined
                rows = cells.GetUpperBound(0) + 1;
                columns = rank > 1 ? cells.GetUpperBound(1) + 1 : 1;
            }
            else
            {
                var items = this.Content as IEnumerable;
                if (items != null)
                {
                    int n = items.Cast<object>().Count();
                    int m = this.ColumnDefinitions.Count;

                    rows = !this.ItemsInColumns ? n : m;
                    columns = !this.ItemsInColumns ? m : n;
                }
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
            this.UpdateSheet(rows, columns);

            this.UpdateColumnWidths();

            this.SubscribeNotifications();
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
                this.sheetGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = this.DefaultRowHeight });
                this.rowGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = this.DefaultRowHeight });
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

                Grid.SetRow(cell, i);
                this.rowGrid.Children.Add(cell);
            }

            // Add "Insert" row header
            if (this.CanInsertRows && this.AddItemHeader != null)
            {
                this.sheetGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = this.DefaultRowHeight });
                this.rowGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = this.DefaultRowHeight });

                var cell = new TextBlock
                    {
                        Text = this.AddItemHeader, 
                        // ToolTip = "Add row",
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
                this.sheetGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = GridLength.Auto });
                this.rowGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = GridLength.Auto });
            }

            // to cover a posisble scrollbar
            this.rowGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = new GridLength(20) });
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
        private void UpdateSheet(int rows, int columns)
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
                       BorderBrush = this.GridLineBrush, BorderThickness = new Thickness(0, 1, 0, 0) 
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

            if (this.IsVirtualizing)
            {
                this.VirtualizeCells();
            }
            else
            {
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
            }

            this.sheetGrid.Children.Add(this.textEditor);
            this.sheetGrid.Children.Add(this.selection);
            this.sheetGrid.Children.Add(this.autoFillBox);
            this.sheetGrid.Children.Add(this.autoFillSelection);
            this.sheetGrid.Children.Add(this.contentEditor);
            this.sheetGrid.Children.Add(this.enumEditor);
        }

        #endregion
    }
}