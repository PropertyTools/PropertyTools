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

namespace PropertyTools.Wpf
{
    ///<summary>
    ///  The SimpleGrid is a 'DataGrid' control with a 'spreadsheet style'.
    ///  Note: it is not doing virtualization, so the performance on large collection is not good.
    ///  Note: there are still some bugs in this control 
    ///  Supported data sources (set in the Content property)
    ///  - arrays (rank 1 or 2)
    ///  - enumerables
    ///  - lists (supports add, insert and delete)
    ///
    ///  Editing of the following types is included
    ///  - bool
    ///  - enums
    ///  - strings
    ///
    ///  Features
    ///  - fit to width
    ///  - autofill
    ///  - copy/paste
    ///  - zoom
    ///  - insert/delete (IList only)
    ///  - autogenerate columns
    ///
    ///  todo:
    ///  - add item issues
    ///  - only update modified cells on INPC and INCC
    ///  - use binding instead of subscribing to events
    ///  - update Content
    ///  - custom edit/display Templates
    ///</summary>
    [ContentProperty("Content")]
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
        private const string PART_SheetScroller = "PART_SheetScroller";
        private const string PART_SheetGrid = "PART_SheetGrid";
        private const string PART_ColumnScroller = "PART_ColumnScroller";
        private const string PART_ColumnGrid = "PART_ColumnGrid";
        private const string PART_RowScroller = "PART_RowScroller";
        private const string PART_RowGrid = "PART_RowGrid";
        private const string PART_SelectionBackground = "PART_SelectionBackground";
        private const string PART_CurrentBackground = "PART_CurrentBackground";
        private const string PART_Selection = "PART_Selection";
        private const string PART_AutoFillSelection = "PART_AutoFillSelection";
        private const string PART_AutoFillBox = "PART_AutoFillBox";
        private const string PART_TopLeft = "PART_TopLeft";
        private const string PART_TextEditor = "PART_TextEditor";
        private const string PART_EnumEditor = "PART_EnumEditor";
        private const string PART_ColumnSelectionBackground = "PART_ColumnSelectionBackground";
        private const string PART_RowSelectionBackground = "PART_RowSelectionBackground";

        private Border autoFillBox;
        private CellRef autoFillCell;
        private Border autoFillSelection;
        private ToolTip autoFillToolTip;
        private AutoFiller autoFiller;
        private Grid columnGrid;
        private ScrollViewer columnScroller;
        private Border columnSelectionBackground;
        private Border currentBackground;
        private IEnumerable<CellRef> editingCells;
        private ComboBox enumEditor;
        private bool isCapturing;
        private bool isSelectingColumns;
        private bool isSelectingRows;
        private Grid rowGrid;
        private ScrollViewer rowScroller;
        private Border rowSelectionBackground;
        private Border selection;
        private Border selectionBackground;
        private Grid sheetGrid;
        private ScrollViewer sheetScroller;
        private object subcribedContent;
        private TextBox textEditor;
        private Border topleft;

        static SimpleGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleGrid),
                                                     new FrameworkPropertyMetadata(typeof(SimpleGrid)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            sheetScroller = Template.FindName(PART_SheetScroller, this) as ScrollViewer;
            sheetGrid = Template.FindName(PART_SheetGrid, this) as Grid;
            columnScroller = Template.FindName(PART_ColumnScroller, this) as ScrollViewer;
            columnGrid = Template.FindName(PART_ColumnGrid, this) as Grid;
            rowScroller = Template.FindName(PART_RowScroller, this) as ScrollViewer;
            rowGrid = Template.FindName(PART_RowGrid, this) as Grid;
            rowSelectionBackground = Template.FindName(PART_RowSelectionBackground, this) as Border;
            columnSelectionBackground = Template.FindName(PART_ColumnSelectionBackground, this) as Border;
            selectionBackground = Template.FindName(PART_SelectionBackground, this) as Border;
            currentBackground = Template.FindName(PART_CurrentBackground, this) as Border;
            selection = Template.FindName(PART_Selection, this) as Border;
            autoFillSelection = Template.FindName(PART_AutoFillSelection, this) as Border;
            autoFillBox = Template.FindName(PART_AutoFillBox, this) as Border;
            topleft = Template.FindName(PART_TopLeft, this) as Border;
            textEditor = Template.FindName(PART_TextEditor, this) as TextBox;
            enumEditor = Template.FindName(PART_EnumEditor, this) as ComboBox;

            enumEditor.SelectionChanged += EnumEditorSelectionChanged;
            textEditor.PreviewKeyDown += TextEditorPreviewKeyDown;
            textEditor.LostFocus += TextEditorLostFocus;
            sheetScroller.ScrollChanged += ScrollViewerScrollChanged;
            rowScroller.ScrollChanged += RowScrollerChanged;
            columnScroller.ScrollChanged += ColumnScrollerChanged;

            // todo: should also subscribe the column/row scrollers change events...
            topleft.MouseLeftButtonDown += TopleftMouseLeftButtonDown;

            autoFillBox.MouseLeftButtonDown += AutoFillBoxMouseLeftButtonDown;
            columnGrid.MouseLeftButtonDown += ColumnGridMouseLeftButtonDown;
            columnGrid.MouseMove += ColumnGridMouseMove;
            columnGrid.MouseLeftButtonUp += ColumnGridMouseLeftButtonUp;
            columnGrid.Loaded += ColumnGridLoaded;
            sheetGrid.SizeChanged += ColumnGridSizeChanged;
            rowGrid.MouseLeftButtonDown += RowGridMouseLeftButtonDown;
            rowGrid.MouseMove += RowGridMouseMove;
            rowGrid.MouseLeftButtonUp += RowGridMouseLeftButtonUp;
            Focusable = true;

            autoFiller = new AutoFiller(GetCellValue, TrySetCellValue);

            autoFillToolTip = new ToolTip();
            autoFillToolTip.Placement = PlacementMode.Bottom;
            autoFillToolTip.PlacementTarget = autoFillSelection;

            UpdateGridContent();
            OnSelectedCellsChanged();

            BuildContextMenus();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CopyExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, CutExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, DeleteExecute));
        }

        protected int GetIndexOfItem(object item)
        {
            var list = Content as IEnumerable;
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

        protected int GetIndexOfProperty(string propertyName)
        {
            if (DataFields == null)
            {
                return -1;
            }

            for (int i = 0; i < DataFields.Count; i++)
            {
                if (DataFields[i] == propertyName)
                {
                    return i;
                }
            }

            return -1;
        }

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

        private IEnumerable GetItems(CellRef currentCell, CellRef selectionCell)
        {
            if (!(Content is Array))
            {
                var list = Content as IList;
                if (list != null)
                {
                    int index0 = ItemsInRows ? currentCell.Row : currentCell.Column;
                    int index1 = ItemsInRows ? selectionCell.Row : selectionCell.Column;
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

        private object GetItem(CellRef cell)
        {
            if (Content is Array)
            {
                return null;
            }

            var list = Content as IList;
            if (list == null)
            {
                return null;
            }

            int index = ItemsInRows ? cell.Row : cell.Column;
            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }

            var items = Content as IEnumerable;
            if (items != null)
            {
                int i = 0;
                foreach (var item in items)
                {
                    if ((ItemsInRows && i == cell.Row) || (!ItemsInRows && i == cell.Column))
                    {
                        return item;
                    }

                    i++;
                }
            }

            return null;
        }

        private static void SetElementPosition(UIElement element, CellRef cell)
        {
            Grid.SetColumn(element, cell.Column);
            Grid.SetRow(element, cell.Row);
        }

        public void BeginTextEdit()
        {
            editingCells = SelectedCells.ToList();

            Grid.SetColumn(textEditor, CurrentCell.Column);
            Grid.SetRow(textEditor, CurrentCell.Row);

            var el = GetCellElement(CurrentCell) as TextBlock;
            if (el != null)
            {
                textEditor.Text = el.Text;
            }

            textEditor.Visibility = Visibility.Visible;
            textEditor.Focus();
            textEditor.CaretIndex = textEditor.Text.Length;
            textEditor.SelectAll();
        }

        public bool BeginComboEdit()
        {
            var value = GetCellValue(CurrentCell) as Enum;
            if (value == null)
            {
                return false;
            }

            Grid.SetColumn(enumEditor, CurrentCell.Column);
            Grid.SetRow(enumEditor, CurrentCell.Row);

            // nullify editingCells to avoid setting values when enumEditor.SelectedValue is set below
            editingCells = null;

            var values = Enum.GetValues(value.GetType());
            enumEditor.ItemsSource = values;
            enumEditor.SelectedValue = value;

            editingCells = SelectedCells.ToList();

            enumEditor.Visibility = Visibility.Visible;


            // enumEditor.Focus();
            return true;
        }

        public void HideComboEdit()
        {
            enumEditor.Visibility = Visibility.Hidden;
        }

        private bool ToggleCheck()
        {
            bool result = false;
            foreach (var cell in SelectedCells)
            {
                var chk = GetCellElement(cell) as CheckBox;
                if (chk != null)
                {
                    chk.IsChecked = !chk.IsChecked;
                    result = true;
                }
            }

            return result;
        }

        public void EndTextEdit()
        {
            if (textEditor.Visibility == Visibility.Hidden)
            {
                return;
            }

            foreach (var cell in editingCells)
            {
                TrySetCellValue(cell, textEditor.Text);
                // UpdateCellContent(cell);
            }

            textEditor.Visibility = Visibility.Hidden;
        }

        public void CancelTextEdit()
        {
            textEditor.Visibility = Visibility.Hidden;
        }

        private void BuildContextMenus()
        {
            var rowsMenu = new ContextMenu();
            if (CanInsertRows)
            {
                rowsMenu.Items.Add(new MenuItem { Header = "Insert", Command = new DelegateCommand(InsertRows) });
            }

            if (CanDeleteRows)
            {
                rowsMenu.Items.Add(new MenuItem { Header = "Delete", Command = new DelegateCommand(DeleteRows) });
            }

            if (rowsMenu.Items.Count > 0)
            {
                rowGrid.ContextMenu = rowsMenu;
            }

            var columnsMenu = new ContextMenu();
            if (CanInsertColumns)
            {
                columnsMenu.Items.Add(new MenuItem { Header = "Insert" });
            }

            if (CanDeleteColumns)
            {
                columnsMenu.Items.Add(new MenuItem { Header = "Delete" });
            }

            if (columnsMenu.Items.Count > 0)
            {
                columnGrid.ContextMenu = columnsMenu;
            }
        }

        public void Paste()
        {
            if (!Clipboard.ContainsText())
            {
                return;
            }

            string text = Clipboard.GetText().Trim();
            var textArray = TextToArray(text);

            int rowMin = Math.Min(CurrentCell.Row, SelectionCell.Row);
            int columnMin = Math.Min(CurrentCell.Column, SelectionCell.Column);
            int rowMax = Math.Max(CurrentCell.Row, SelectionCell.Row);
            int columnMax = Math.Max(CurrentCell.Column, SelectionCell.Column);

            int rows = textArray.GetUpperBound(0) + 1;
            int columns = textArray.GetUpperBound(1) + 1;

            for (int i = rowMin; i <= rowMax || i < rowMin + rows; i++)
            {
                if (i >= Rows)
                {
                    if (!InsertItem(-1))
                    {
                        break;
                    }
                }

                for (int j = columnMin; j <= columnMax || j < columnMin + columns; j++)
                {
                    string value =
                        textArray[(i - rowMin) % rows, (j - columnMin) % columns];
                    TrySetCellValue(new CellRef(i, j), value);
                }
            }

            CurrentCell = new CellRef(rowMin, columnMin);
            SelectionCell = new CellRef(Math.Max(rowMax, rowMin + rows - 1),
                                        Math.Max(columnMax, columnMin + columns - 1));
        }

        public void Copy()
        {
            Copy("\t");
        }

        public void Copy(string separator)
        {
            int rowMin = Math.Min(CurrentCell.Row, SelectionCell.Row);
            int columnMin = Math.Min(CurrentCell.Column, SelectionCell.Column);
            int rowMax = Math.Max(CurrentCell.Row, SelectionCell.Row);
            int columnMax = Math.Max(CurrentCell.Column, SelectionCell.Column);

            var sb = new StringBuilder();

            // include column headers if full columns are selected?
            //if (rowMin == 0 && rowMax == Rows - 1)
            //{
            //    for (int j = columnMin; j <= columnMax; j++)
            //    {
            //        string cell = GetColumnHeader(j);
            //        if (j > columnMin)
            //        {
            //            sb.Append(separator);
            //        }

            //        if (cell != null)
            //        {
            //            sb.Append(cell);
            //        }
            //    }

            //    sb.AppendLine();
            //}

            for (int i = rowMin; i <= rowMax; i++)
            {
                if (i > rowMin)
                {
                    sb.AppendLine();
                }

                for (int j = columnMin; j <= columnMax; j++)
                {
                    string cell = GetText(new CellRef(i, j));
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

            Clipboard.SetText(sb.ToString());
        }

        private void Delete()
        {
            foreach (var cell in SelectedCells)
            {
                TrySetCellValue(cell, null);
            }
        }

        private void InsertRows()
        {
            int from = Math.Min(CurrentCell.Row, SelectionCell.Row);
            int to = Math.Max(CurrentCell.Row, SelectionCell.Row);
            for (int i = 0; i < to - from + 1; i++)
            {
                InsertItem(from);
            }

            UpdateGridContent();
        }

        private void DeleteRows()
        {
            int from = Math.Min(CurrentCell.Row, SelectionCell.Row);
            int to = Math.Max(CurrentCell.Row, SelectionCell.Row);
            for (int i = to; i >= from; i--)
            {
                DeleteItem(i);
            }

            UpdateGridContent();

            int maxRow = Rows > 0 ? Rows - 1 : 0;
            if (CurrentCell.Row > maxRow)
            {
                CurrentCell = new CellRef(maxRow, CurrentCell.Column);
            }

            if (SelectionCell.Row > maxRow)
            {
                SelectionCell = new CellRef(maxRow, SelectionCell.Column);
            }
        }

        private void EnumEditorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editingCells == null)
            {
                return;
            }

            foreach (var cell in editingCells)
            {
                TrySetCellValue(cell, enumEditor.SelectedValue);
                UpdateCellContent(cell);
            }
        }

        private void ColumnGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateColumnWidths();
        }

        private void ColumnGridLoaded(object sender, RoutedEventArgs e)
        {
            UpdateColumnWidths();
        }

        private void AutoFillBoxMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Show the autofill selection border
            autoFillSelection.Visibility = Visibility.Visible;
        }

        private void TextEditorLostFocus(object sender, RoutedEventArgs e)
        {
            EndTextEdit();
        }

        private void TextEditorPreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool shift = Keyboard.IsKeyDown(Key.LeftShift);
            switch (e.Key)
            {
                case Key.Down:
                case Key.Up:
                    EndTextEdit();
                    OnKeyDown(e);
                    e.Handled = true;
                    break;
                case Key.Enter:
                    EndTextEdit();
                    MoveCurrentCell(shift ? -1 : 1, 0);
                    e.Handled = true;
                    break;
                case Key.Escape:
                    CancelTextEdit();
                    e.Handled = true;
                    break;
            }
        }

        private void DeleteExecute(object sender, ExecutedRoutedEventArgs e)
        {
            Delete();
        }

        private void RowGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int row = GetCell(e.GetPosition(rowGrid)).Row;
            if (row >= 0)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    CurrentCell = new CellRef(CurrentCell.Row, 0);
                }
                else
                {
                    CurrentCell = new CellRef(row, 0);
                }

                SelectionCell = new CellRef(row, Columns - 1);
                e.Handled = true;
            }

            isSelectingRows = true;
            rowGrid.CaptureMouse();
        }

        private void RowGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rowGrid.ReleaseMouseCapture();
            isSelectingRows = false;
        }

        private void RowGridMouseMove(object sender, MouseEventArgs e)
        {
            if (!isSelectingRows)
            {
                return;
            }

            int row = GetCell(e.GetPosition(rowGrid)).Row;
            if (row >= 0)
            {
                SelectionCell = new CellRef(row, Columns - 1);
                e.Handled = true;
            }
        }

        private void ColumnGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int column = GetCell(e.GetPosition(columnGrid)).Column;
            if (column >= 0)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    CurrentCell = new CellRef(0, CurrentCell.Column);
                }
                else
                {
                    CurrentCell = new CellRef(0, column);
                }

                SelectionCell = new CellRef(Rows - 1, column);
                e.Handled = true;
            }

            isSelectingColumns = true;
        }

        private void ColumnGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            columnGrid.ReleaseMouseCapture();
            isSelectingColumns = false;
        }

        private void ColumnGridMouseMove(object sender, MouseEventArgs e)
        {
            if (!isSelectingColumns)
            {
                return;
            }

            int column = GetCell(e.GetPosition(columnGrid)).Column;
            if (column >= 0)
            {
                SelectionCell = new CellRef(Rows - 1, column);
                e.Handled = true;
            }
        }

        private void TopleftMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CurrentCell = new CellRef(0, 0);
            SelectionCell = new CellRef(Rows - 1, Columns - 1);
            e.Handled = true;
        }

        private void CutExecute(object sender, ExecutedRoutedEventArgs e)
        {
            Copy();
            Delete();
        }

        private void PasteExecute(object sender, ExecutedRoutedEventArgs e)
        {
            Paste();
        }

        private void CopyExecute(object sender, ExecutedRoutedEventArgs e)
        {
            Copy();
        }

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

        private Binding CreateBinding(CellRef cellRef)
        {
            int index = cellRef.Column;
            return new Binding(ActualDataFields[index]) { StringFormat = GetFormatString(cellRef) };
        }

        public void UpdateCellContent(CellRef cellRef)
        {
            var c = GetCellElement(cellRef);
            var value = GetCellValue(cellRef);

            if (c != null)
            {
                sheetGrid.Children.Remove(c);
                cellMap.Remove(cellRef.GetHashCode());
            }
            InsertCellElement(cellRef, value, true);
        }

        private string GetFormatString(CellRef cellRef)
        {
            return FormatStrings != null && cellRef.Column < FormatStrings.Count
                       ? FormatStrings[cellRef.Column]
                       : null;
        }

        Dictionary<int, UIElement> cellMap = new Dictionary<int, UIElement>();

        private void AddCellElement(CellRef cellRef, object value)
        {
            InsertCellElement(cellRef, value, false);
        }

        private void InsertCellElement(CellRef cellRef, object value, bool insert)
        {
            if (value == null)
                return;

            var e = CreateElement(value, GetFormatString(cellRef), GetAlignment(cellRef));
            SetElementPosition(e, cellRef);
            if (insert)
            {
                int index = sheetGrid.Children.IndexOf(textEditor);
                sheetGrid.Children.Insert(index, e);
            }
            else
            {
                sheetGrid.Children.Add(e);
            }

            cellMap.Add(cellRef.GetHashCode(), e);
        }

        public UIElement GetCellElement(CellRef cellRef)
        {
            UIElement e;
            return cellMap.TryGetValue(cellRef.GetHashCode(), out e) ? e : null;

            //foreach (UIElement c in sheetGrid.Children)
            //{
            //    if (c is Border)
            //    {
            //        continue;
            //    }

            //    if (Grid.GetColumn(c) == cellRef.Column && Grid.GetRow(c) == cellRef.Row)
            //    {
            //        return c;
            //    }
            //}


            //// todo: create cell if not found?
            //return null;
        }

        private string GetColumnHeader(int j)
        {
            var text = CellRef.ToColumnName(j);
            if (ActualColumnHeaders != null && j < ActualColumnHeaders.Count)
            {
                text = ActualColumnHeaders[j];
            }

            return text;
        }

        private string GetText(CellRef cellRef)
        {
            var c = GetCellElement(cellRef) as TextBlock;
            if (c != null)
            {
                return c.Text;
            }

            return null;
        }

        /// <summary>
        ///   Gets the cell value from the Content property for the specified cell.
        /// </summary>
        /// <param name = "cell">The cell reference.</param>
        /// <returns></returns>
        public object GetCellValue(CellRef cell)
        {
            if (cell.Column < 0 || cell.Column >= Columns || cell.Row < 0 || cell.Row >= Rows)
            {
                return null;
            }

            if (Content.GetType().IsArray)
            {
                var cells = (Array)Content;
                int rank = cells.Rank;
                var value = rank > 1 ? cells.GetValue(cell.Row, cell.Column) : cells.GetValue(cell.Row);
                return value;
            }

            var items = Content as IEnumerable;
            if (items != null)
            {
                int itemIndex = ItemsInRows ? cell.Row : cell.Column;
                int fieldIndex = ItemsInRows ? cell.Column : cell.Row;
                int i = 0;
                foreach (var item in items)
                {
                    if (i == itemIndex)
                    {
                        var type = item.GetType();
                        if (ActualDataFields != null && fieldIndex < ActualDataFields.Count)
                        {
                            var pi = type.GetProperty(ActualDataFields[fieldIndex]);
                            if (pi == null)
                            {
                                return item;
                            }

                            return pi.GetValue(item, null);
                        }

                        return item;
                    }

                    i++;
                }
            }

            return null;
        }

        /// <summary>
        ///   Updates all the UIElements of the grid (both cells, headers, row and column lines).
        /// </summary>
        private void UpdateGridContent()
        {
            UnsubscribeNotifications();

            if (sheetGrid == null)
            {
                // return if the template has not yet been applied
                return;
            }

            Array cells = null;

            if (Content != null && Content.GetType().IsArray)
            {
                cells = (Array)Content;
            }

            if (cells == null)
            {
                var items = Content as IEnumerable;
                if (items != null)
                {
                    cells = ConvertItemsSourceToArray(items);
                }
            }

            // Hide the row/column headers if the content is empty
            rowScroller.Visibility = columnScroller.Visibility = sheetScroller.Visibility = topleft.Visibility = cells != null ? Visibility.Visible : Visibility.Hidden;

            if (cells == null)
            {
                return;
            }


            int rank = cells.Rank;
            int rows = cells.GetUpperBound(0) + 1;
            int columns = rank > 1 ? cells.GetUpperBound(1) + 1 : 1;

            UpdateRows(rows);
            UpdateColumns(columns);
            UpdateSheet(cells);

            UpdateColumnWidths();

            SubscribeNotifications();

        }

        private void SubscribeNotifications()
        {
            var ncc = Content as INotifyCollectionChanged;
            if (ncc != null)
            {
                ncc.CollectionChanged += OnContentCollectionChanged;
            }

            var items = Content as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                {
                    var npc = item as INotifyPropertyChanged;
                    if (npc != null)
                    {
                        npc.PropertyChanged += OnContentItemPropertyChanged;
                    }
                }
            }

            subcribedContent = Content;
        }

        private void UnsubscribeNotifications()
        {
            var ncc = subcribedContent as INotifyCollectionChanged;
            if (ncc != null)
            {
                ncc.CollectionChanged -= OnContentCollectionChanged;
            }

            var items = subcribedContent as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                {
                    var npc = item as INotifyPropertyChanged;
                    if (npc != null)
                    {
                        npc.PropertyChanged -= OnContentItemPropertyChanged;
                    }
                }
            }

            subcribedContent = null;
        }

        private void OnContentItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // todo: find cell and update
            UpdateGridContent();
        }

        private void OnContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // todo: update rows
            UpdateGridContent();
        }

        private void AutoSizeAllColumns()
        {
            sheetGrid.UpdateLayout();
            for (int i = 0; i < Columns; i++)
            {
                AutoSizeColumn(i);
            }
        }

        private void UpdateColumnWidths()
        {
            sheetGrid.UpdateLayout();

            if (AutoSizeColumns)
            {
                AutoSizeAllColumns();
            }

            for (int j = 0; j < sheetGrid.ColumnDefinitions.Count; j++)
            {
                columnGrid.ColumnDefinitions[j].Width = new GridLength(sheetGrid.ColumnDefinitions[j].ActualWidth);
            }
        }

        private void UpdateSheet(Array cells)
        {
            int rank = cells.Rank;
            int rows = cells.GetUpperBound(0) + 1;
            int columns = rank > 1 ? cells.GetUpperBound(1) + 1 : 1;

            sheetGrid.Children.Clear();
            sheetGrid.Children.Add(selectionBackground);
            sheetGrid.Children.Add(currentBackground);
            cellMap.Clear();

            // Add row lines to the sheet
            for (int i = 1; i <= rows; i++)
            {
                var border = new Border
                                 {
                                     BorderBrush = GridLineBrush,
                                     BorderThickness = new Thickness(0, 1, 0, 0)
                                 };

                if (i < rows && AlternatingRowsBackground != null && i % 2 == 1)
                {
                    border.Background = AlternatingRowsBackground;
                }

                Grid.SetColumn(border, 0);
                Grid.SetColumnSpan(border, columns);
                Grid.SetRow(border, i);
                sheetGrid.Children.Add(border);
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
                                         BorderBrush = GridLineBrush,
                                         BorderThickness = new Thickness(i > 0 ? 1 : 0, 0, i == columns - 1 ? 1 : 0, 0)
                                     };

                    Grid.SetRow(border, 0);
                    Grid.SetRowSpan(border, rows);
                    Grid.SetColumn(border, i);
                    sheetGrid.Children.Add(border);
                }
            }

            // Add content elements to the sheet
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var value = rank > 1 ? cells.GetValue(i, j) : cells.GetValue(i);
                    AddCellElement(new CellRef(i, j), value);
                }
            }

            sheetGrid.Children.Add(textEditor);
            sheetGrid.Children.Add(enumEditor);
            sheetGrid.Children.Add(selection);
            sheetGrid.Children.Add(autoFillBox);
            sheetGrid.Children.Add(autoFillSelection);
        }

        private HorizontalAlignment GetAlignment(CellRef cellref)
        {
            if (ColumnAlignments != null && cellref.Column < ColumnAlignments.Count)
            {
                return ColumnAlignments[cellref.Column];
            }

            return HorizontalAlignment.Center;
        }

        protected virtual UIElement CreateElement(object value, string formatString, HorizontalAlignment alignment)
        {
            FrameworkElement element = null;
            var type = value.GetType();

            if (type == typeof(bool))
            {
                var chkbox = new CheckBox
                                 {
                                     IsChecked = (bool)value,
                                     Cursor = Cursors.Arrow,
                                     VerticalAlignment = VerticalAlignment.Center,
                                     HorizontalAlignment = alignment
                                 };
                chkbox.Checked += CellChecked;
                element = chkbox;
            }

            if (typeof(BitmapSource).IsAssignableFrom(type))
            {
                var chkbox = new Image
                                 {
                                     Source = (BitmapSource)value,
                                     Stretch = Stretch.None,
                                     VerticalAlignment = VerticalAlignment.Center,
                                     HorizontalAlignment = alignment
                                 };
                element = chkbox;
            }

            if (element == null)
            {
                element = new TextBlock
                              {
                                  Text = FormatValue(value, formatString),
                                  Margin = new Thickness(4, 0, 4, 0),
                                  Foreground = Foreground,
                                  VerticalAlignment = VerticalAlignment.Center,
                                  HorizontalAlignment = alignment
                              };
            }

            element.Tag = type;
            return element;
        }

        private static string FormatValue(object value, string formatString)
        {
            if (String.IsNullOrEmpty(formatString))
            {
                return value.ToString();
            }
            else
            {
                if (!formatString.Contains("{0"))
                {
                    formatString = "{0:" + formatString + "}";
                }

                return String.Format(formatString, value);
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            if (control)
            {
                double s = 1 + e.Delta * 0.0004;
                var tg = new TransformGroup();
                if (LayoutTransform != null)
                {
                    tg.Children.Add(LayoutTransform);
                }

                tg.Children.Add(new ScaleTransform(s, s));
                LayoutTransform = tg;
                e.Handled = true;
            }
        }

        private void UpdateColumns(int columns)
        {
            Columns = columns;
            rowGrid.ColumnDefinitions.Clear();
            sheetGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < columns; i++)
            {
                var w = ColumnWidths != null && i < ColumnWidths.Count ? ColumnWidths[i] : DefaultColumnWidth;
                sheetGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = w });
                columnGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Add one empty column covering the vertical scrollbar
            columnGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });


            columnGrid.Children.Clear();
            columnGrid.Children.Add(columnSelectionBackground);
            for (int j = 0; j < columns; j++)
            {
                string text = GetColumnHeader(j);

                var border = new Border
                                 {
                                     BorderBrush = HeaderBorderBrush,
                                     BorderThickness = new Thickness(0, 1, 1, 1),
                                     Margin = new Thickness(0, 0, j < columns - 1 ? -1 : 0, 0)
                                 };
                Grid.SetColumn(border, j);
                columnGrid.Children.Add(border);

                var cell = new TextBlock
                               {
                                   Text = text,
                                   VerticalAlignment = VerticalAlignment.Center,
                                   HorizontalAlignment = GetAlignment(new CellRef(-1, j)),
                                   Padding = new Thickness(4, 2, 4, 2)
                               };
                Grid.SetColumn(cell, j);
                columnGrid.Children.Add(cell);

                if (CanResizeColumns)
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
                    splitter.MouseDoubleClick += ColumnSplitterDoubleClick;
                    splitter.DragStarted += ColumnSplitterChangeStarted;
                    splitter.DragDelta += ColumnSplitterChangeDelta;
                    splitter.DragCompleted += ColumnSplitterChangeCompleted;
                    Grid.SetColumn(splitter, j);
                    columnGrid.Children.Add(splitter);
                }
            }
        }

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
            var width = columnGrid.ColumnDefinitions[column].ActualWidth;
            tt.Content = string.Format("Width: {0}", width); // device-independent units

            tt.PlacementTarget = columnGrid;
            tt.Placement = PlacementMode.Relative;
            var p = Mouse.GetPosition(columnGrid);
            tt.HorizontalOffset = p.X;
            tt.VerticalOffset = gs.ActualHeight + 4;
        }

        private void ColumnSplitterChangeStarted(object sender, DragStartedEventArgs dragStartedEventArgs)
        {
            var gs = (GridSplitter)sender;
            ColumnSplitterChangeDelta(sender, null);
        }

        private void ColumnSplitterChangeCompleted(object sender, DragCompletedEventArgs dragCompletedEventArgs)
        {
            var gs = (GridSplitter)sender;
            var tt = gs.ToolTip as ToolTip;
            if (tt != null)
            {
                tt.IsOpen = false;
                gs.ToolTip = null;
            }

            for (int i = 0; i < sheetGrid.ColumnDefinitions.Count; i++)
            {
                sheetGrid.ColumnDefinitions[i].Width = columnGrid.ColumnDefinitions[i].Width;
            }
        }

        private static Type GetListItemType(Type listType)
        {
            // http://stackoverflow.com/questions/1043755/c-generic-list-t-how-to-get-the-type-of-t
            foreach (var interfaceType in listType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    return listType.GetGenericArguments()[0];
                }
            }

            return null;
        }

        public bool DeleteItem(int index)
        {
            if (Content is Array)
            {
                return false;
            }

            var list = Content as IList;
            if (list == null)
            {
                return false;
            }

            if (index < 0 || index >= list.Count)
            {
                return false;
            }

            list.RemoveAt(index);
            return true;
        }

        public bool InsertItem(int index)
        {
            if (Content is Array)
            {
                return false;
            }

            var list = Content as IList;
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
                    newItem = CreateInstance(itemType);
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

            UpdateGridContent();
            return true;
        }

        protected virtual object CreateInstance(Type itemType)
        {
            object newItem;
            newItem = Activator.CreateInstance(itemType);
            return newItem;
        }

        private void UpdateRows(int rows)
        {
            rowGrid.RowDefinitions.Clear();
            sheetGrid.RowDefinitions.Clear();
            rowGrid.Children.Clear();
            rowGrid.Children.Add(rowSelectionBackground);

            Rows = rows;

            for (int i = 0; i < rows; i++)
            {
                sheetGrid.RowDefinitions.Add(new RowDefinition { Height = DefaultRowHeight });
                rowGrid.RowDefinitions.Add(new RowDefinition { Height = DefaultRowHeight });
            }

            for (int i = 0; i < rows; i++)
            {
                string text = (i + 1).ToString();
                if (RowHeaders != null && i < RowHeaders.Count)
                {
                    text = RowHeaders[i];
                }


                var border = new Border
                                 {
                                     BorderBrush = HeaderBorderBrush,
                                     BorderThickness = new Thickness(1, 0, 1, 1),
                                     Margin = new Thickness(0, 0, 0, -1)
                                 };

                Grid.SetRow(border, i);
                rowGrid.Children.Add(border);

                var cell = new TextBlock
                {
                    Text = text,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                cell.Padding = new Thickness(4, 2, 4, 2);
                Grid.SetRow(cell, i);
                rowGrid.Children.Add(cell);
            }

            // Add "Insert" row header
            if (CanInsertRows && AddItemHeader != null)
            {
                sheetGrid.RowDefinitions.Add(new RowDefinition { Height = DefaultRowHeight });
                rowGrid.RowDefinitions.Add(new RowDefinition { Height = DefaultRowHeight });

                var cell = new TextBlock
                               {
                                   Text = AddItemHeader,
                                   ToolTip = "Add row",
                                   VerticalAlignment = VerticalAlignment.Center,
                                   HorizontalAlignment = HorizontalAlignment.Center
                               };
                var border = new Border
                                 {
                                     Background = Brushes.Transparent,
                                     BorderBrush = HeaderBorderBrush,
                                     BorderThickness = new Thickness(1, 0, 1, 1),
                                     Margin = new Thickness(0, 0, 0, 0)
                                 };

                border.MouseLeftButtonDown += addItemCell_MouseLeftButtonDown;
                Grid.SetRow(border, rows);

                cell.Padding = new Thickness(4, 2, 4, 2);
                Grid.SetRow(cell, rows);
                rowGrid.Children.Add(cell);
                rowGrid.Children.Add(border);
            }
            else
            {
                sheetGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                rowGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // to cover a posisble scrollbar
            rowGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
        }

        private void addItemCell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InsertItem(-1);
        }

        private void CellChecked(object sender, RoutedEventArgs e)
        {
            var chkbox = sender as CheckBox;
            if (chkbox == null)
            {
                return;
            }

            int row = Grid.GetRow(chkbox);
            int column = Grid.GetColumn(chkbox);

            CurrentCell = new CellRef(row, column);
            SelectionCell = new CellRef(row, column);

            TrySetCellValue(CurrentCell, chkbox.IsChecked);
            UpdateCellContent(CurrentCell);
        }

        private object[,] ConvertItemsSourceToArray(IEnumerable items)
        {
            int nItems = items.Cast<object>().Count();

            var actualDataFields = DataFields;
            var actualColumnHeaders = ColumnHeaders;
            if (actualDataFields == null)
            {
                AutoGenerateColumns(items, out actualDataFields, out actualColumnHeaders);
            }

            ActualDataFields = actualDataFields;
            ActualColumnHeaders = actualColumnHeaders;

            int nFields = actualDataFields != null ? actualDataFields.Count : 1;
            var pi = new PropertyInfo[nFields];
            var cells = ItemsInRows ? new object[nItems, nFields] : new object[nFields, nItems];
            int i = 0;
            foreach (var item in items)
            {
                var type = item.GetType();
                for (int j = 0; j < nFields; j++)
                {
                    object value = null;
                    if (actualDataFields == null)
                    {
                        value = item;
                    }
                    else
                    {
                        if (pi[j] == null || pi[j].DeclaringType != type)
                        {
                            pi[j] = type.GetProperty(actualDataFields[j]);
                        }

                        if (pi[j] != null)
                        {
                            value = pi[j].GetValue(item, null);
                        }
                        else
                        {
                            value = item;
                        }
                    }

                    if (ItemsInRows)
                    {
                        cells[i, j] = value;
                    }
                    else
                    {
                        cells[j, i] = value;
                    }
                }

                i++;
            }

            return cells;
        }

        private void AutoGenerateColumns(IEnumerable items, out StringCollection dataFields,
                                         out StringCollection columnHeaders)
        {
            var itemType = GetListItemType(items.GetType());
            dataFields = new StringCollection();
            columnHeaders = new StringCollection();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(itemType))
            {
                if (!descriptor.IsBrowsable)
                {
                    continue;
                }

                dataFields.Add(descriptor.Name);
                var displayName = descriptor.DisplayName;
                if (String.IsNullOrEmpty(displayName))
                {
                    displayName = descriptor.Name;
                }

                columnHeaders.Add(displayName);
            }
        }

        private void ColumnSplitterDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int column = Grid.GetColumn((GridSplitter)sender);
            AutoSizeColumn(column);
        }

        public void AutoSizeColumn(int column)
        {
            var h = GetColumnElement(column);
            double maxwidth = h.ActualWidth;
            for (int i = 0; i < sheetGrid.RowDefinitions.Count; i++)
            {
                var c = GetCellElement(new CellRef(i, column)) as FrameworkElement;
                if (c != null)
                {
                    maxwidth = Math.Max(maxwidth, c.ActualWidth + c.Margin.Left + c.Margin.Right);
                }
            }

            sheetGrid.ColumnDefinitions[column].Width =
                columnGrid.ColumnDefinitions[column].Width = new GridLength((int)maxwidth + 2);
        }

        private FrameworkElement GetColumnElement(int column)
        {
            return columnGrid.Children[1 + 3 * column + 1] as FrameworkElement;
        }


        private void ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            columnScroller.ScrollToHorizontalOffset(sheetScroller.HorizontalOffset);
            rowScroller.ScrollToVerticalOffset(sheetScroller.VerticalOffset);
        }

        private void RowScrollerChanged(object sender, ScrollChangedEventArgs e)
        {
            sheetScroller.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void ColumnScrollerChanged(object sender, ScrollChangedEventArgs e)
        {
            sheetScroller.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        public void ScrollIntoView(CellRef cellRef)
        {
            var pos0 = GetPosition(cellRef);
            var pos1 = GetPosition(new CellRef(cellRef.Row + 1, cellRef.Column + 1));

            double scrollBarWidth = 20;
            double scrollBarHeight = 20;

            if (pos0.X < sheetScroller.HorizontalOffset)
            {
                sheetScroller.ScrollToHorizontalOffset(pos0.X);
            }

            if (pos1.X > sheetScroller.HorizontalOffset + sheetScroller.ActualWidth - scrollBarWidth)
            {
                sheetScroller.ScrollToHorizontalOffset(Math.Max(pos1.X - sheetScroller.ActualWidth + scrollBarWidth, 0));
            }

            if (pos0.Y < sheetScroller.VerticalOffset)
            {
                sheetScroller.ScrollToVerticalOffset(pos0.Y);
            }

            if (pos1.Y > sheetScroller.VerticalOffset + sheetScroller.ActualHeight - scrollBarHeight)
            {
                sheetScroller.ScrollToVerticalOffset(Math.Max(pos1.Y - sheetScroller.ActualHeight + scrollBarHeight, 0));
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);
            if (e.Text == "\r")
            {
                return;
            }

            BeginTextEdit();

            textEditor.Text = e.Text;
            textEditor.CaretIndex = textEditor.Text.Length;
        }

        public void MoveCurrentCell(int deltaRows, int deltaColumns)
        {
            int row = CurrentCell.Row;
            int column = CurrentCell.Column;
            row += deltaRows;
            column += deltaColumns;
            if (row < 0)
            {
                row = Rows - 1;
                column--;
            }

            if (row >= Rows && !CanInsertRows)
            {
                column++;
                row = 0;
            }

            if (column < 0)
            {
                column = 0;
            }

            if (column >= Columns && !CanInsertColumns)
            {
                column = 0;
            }

            CurrentCell = new CellRef(row, column);
            SelectionCell = new CellRef(row, column);
            ScrollIntoView(CurrentCell);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift);

            int row = shift ? SelectionCell.Row : CurrentCell.Row;
            int column = shift ? SelectionCell.Column : CurrentCell.Column;

            switch (e.Key)
            {
                case Key.Up:
                    if (row > 0)
                    {
                        row--;
                    }

                    if (control)
                    {
                        row = 0;
                    }

                    break;
                case Key.Down:
                    if (row < Rows - 1 || CanInsertRows)
                    {
                        row++;
                    }

                    if (control)
                    {
                        row = Rows - 1;
                    }

                    break;
                case Key.Enter:
                    MoveCurrentCell(shift ? -1 : 1, 0);
                    e.Handled = true;
                    return;
                case Key.Left:
                    if (column > 0)
                    {
                        column--;
                    }

                    if (control)
                    {
                        column = 0;
                    }

                    break;
                case Key.Right:
                    if (column < Columns - 1 || CanInsertColumns)
                    {
                        column++;
                    }

                    if (control)
                    {
                        column = Columns - 1;
                    }

                    break;
                case Key.Home:
                    column = 0;
                    row = 0;
                    break;
                case Key.Delete:
                    Delete();
                    break;
                case Key.F2:
                    BeginTextEdit();
                    break;
                case Key.Space:
                    if (ToggleCheck())
                    {
                        e.Handled = true;
                    }

                    if (OpenCombo())
                    {
                        e.Handled = true;
                    }

                    return;
                case Key.A:
                    if (control)
                    {
                        SelectAll();
                        e.Handled = true;
                    }

                    return;
                default:
                    return;
            }

            if (shift)
            {
                SelectionCell = new CellRef(row, column);
            }
            else
            {
                CurrentCell = new CellRef(row, column);
                SelectionCell = new CellRef(row, column);
            }

            ScrollIntoView(new CellRef(row, column));

            e.Handled = true;
        }

        private bool OpenCombo()
        {
            if (enumEditor.Visibility == Visibility.Visible)
            {
                enumEditor.IsDropDownOpen = true;
                enumEditor.Focus();
                return true;
            }

            return false;
        }

        private void SelectAll()
        {
            CurrentCell = new CellRef(0, 0);
            SelectionCell = new CellRef(Rows - 1, Columns - 1);
            ScrollIntoView(CurrentCell);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            var pos = e.GetPosition(sheetGrid);
            var cellRef = GetCell(pos);

            if (cellRef.Column == -1 || cellRef.Row == -1)
            {
                return;
            }

            if (cellRef.Row >= Rows && !CanInsertRows)
            {
                return;
            }

            if (cellRef.Column > Columns && !CanInsertColumns)
            {
                return;
            }

            if (autoFillSelection.Visibility == Visibility.Visible)
            {
                AutoFillCell = cellRef;
                Mouse.OverrideCursor = autoFillBox.Cursor;
                autoFillToolTip.IsOpen = true;
            }
            else
            {
                bool shift = Keyboard.IsKeyDown(Key.LeftShift);

                if (!shift)
                {
                    CurrentCell = cellRef;
                }

                SelectionCell = cellRef;
                ScrollIntoView(cellRef);
                Mouse.OverrideCursor = sheetGrid.Cursor;
            }

            CaptureMouse();
            isCapturing = true;
            Focus();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!isCapturing)
            {
                return;
            }

            var pos = e.GetPosition(sheetGrid);
            var cellRef = GetCell(pos);

            if (cellRef.Column == -1 || cellRef.Row == -1)
            {
                return;
            }

            if (cellRef.Row >= Rows && !CanInsertRows)
            {
                return;
            }

            if (cellRef.Column > Columns && !CanInsertColumns)
            {
                return;
            }

            if (autoFillSelection.Visibility == Visibility.Visible)
            {
                AutoFillCell = cellRef;
                object result;
                if (autoFiller.TryExtrapolate(cellRef, CurrentCell, SelectionCell, AutoFillCell, out result))
                {
                    var fmt = GetFormatString(cellRef);
                    autoFillToolTip.Content = FormatValue(result, fmt);
                }

                autoFillToolTip.Placement = PlacementMode.Relative;
                var p = e.GetPosition(autoFillSelection);
                autoFillToolTip.HorizontalOffset = p.X + 8;
                autoFillToolTip.VerticalOffset = p.Y + 8;
            }
            else
            {
                SelectionCell = cellRef;
            }

            ScrollIntoView(cellRef);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            OnMouseUp(e);

            isCapturing = false;
            ReleaseMouseCapture();
            Mouse.OverrideCursor = null;

            if (autoFillSelection.Visibility == Visibility.Visible)
            {
                autoFiller.AutoFill(CurrentCell, SelectionCell, AutoFillCell);

                autoFillSelection.Visibility = Visibility.Hidden;
                autoFillToolTip.IsOpen = false;
            }
        }

        public Point GetPosition(CellRef cellRef)
        {
            double x = 0;
            double y = 0;
            for (int j = 0; j < cellRef.Column && j < sheetGrid.ColumnDefinitions.Count; j++)
            {
                x += sheetGrid.ColumnDefinitions[j].ActualWidth;
            }

            for (int i = 0; i < cellRef.Row && i < sheetGrid.RowDefinitions.Count; i++)
            {
                y += sheetGrid.RowDefinitions[i].ActualHeight;
            }

            return new Point(x, y);
        }

        public CellRef GetCell(Point position)
        {
            double w = 0;
            int column = -1;
            int row = -1;
            for (int j = 0; j < sheetGrid.ColumnDefinitions.Count; j++)
            {
                double aw = sheetGrid.ColumnDefinitions[j].ActualWidth;
                if (position.X < w + aw)
                {
                    column = j;
                    break;
                }

                w += aw;
            }

            double h = 0;
            for (int i = 0; i < sheetGrid.RowDefinitions.Count; i++)
            {
                double ah = sheetGrid.RowDefinitions[i].ActualHeight;
                if (position.Y < h + ah)
                {
                    row = i;
                    break;
                }

                h += ah;
            }

            if (column == -1 || row == -1)
            {
                return new CellRef(-1, -1);
            }

            return new CellRef(row, column);
        }

        public virtual bool TrySetCellValue(CellRef cell, object value)
        {
            Array cells;

            if (Content.GetType().IsArray)
            {
                cells = (Array)Content;
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

                        UpdateCellContent(cell);
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


            var items = Content as IEnumerable;
            if (items != null)
            {
                var current = GetItem(cell);

                int fieldIndex = ItemsInRows ? cell.Column : cell.Row;
                if (current != null && ActualDataFields != null && fieldIndex < ActualDataFields.Count)
                {
                    var field = ActualDataFields[fieldIndex];
                    var pi = current.GetType().GetProperty(field);
                    if (pi == null)
                    {
                        // todo: set the actual item to the value
                        return false;
                    }

                    object convertedValue;
                    if (TryConvert(value, pi.PropertyType, out convertedValue))
                    {
                        pi.SetValue(current, convertedValue, null);
                        if (!(current is INotifyPropertyChanged))
                        {
                            UpdateCellContent(cell);
                        }

                        return true;
                    }
                }
            }

            return false;
        }

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
    }
}