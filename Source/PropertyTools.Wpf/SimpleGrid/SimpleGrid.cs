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
    /// 
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
    /// Custom display/edit templates can be defined in
    ///  - ColumnDefinitions (these are only used in the defined column)
    ///  - TypeDefinitions (these are used in any cell)
    /// 
    ///  Features
    ///  - fit to width and proportional column widths (using Gridlengths)
    ///  - autofill
    ///  - copy/paste
    ///  - zoom
    ///  - insert/delete (IList only)
    ///  - autogenerate columns
    ///
    ///  todo:
    ///  - add item issues
    ///  - only update modified cells on INPC and INCC
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
        private ContentControl contentEditor;
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
        private bool endPressed;

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
            contentEditor = new ContentControl();

            enumEditor.SelectionChanged += EnumEditorSelectionChanged;
            textEditor.PreviewKeyDown += TextEditorPreviewKeyDown;
            textEditor.LostFocus += TextEditorLostFocus;

            sheetScroller.ScrollChanged += ScrollViewerScrollChanged;
            rowScroller.ScrollChanged += RowScrollerChanged;
            columnScroller.ScrollChanged += ColumnScrollerChanged;
            sheetScroller.SizeChanged += ScrollViewerSizeChanged;

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

            autoFillToolTip = new ToolTip { Placement = PlacementMode.Bottom, PlacementTarget = autoFillSelection };

            UpdateGridContent();
            OnSelectedCellsChanged();
            BuildContextMenus();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CopyExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, CutExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, DeleteExecute));
        }


        /// <summary>
        /// Gets the index of the specified item in the Content enumerable.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
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

        //protected int GetIndexOfProperty(string propertyName)
        //{
        //    if (DataFields == null)
        //    {
        //        return -1;
        //    }

        //    for (int i = 0; i < DataFields.Count; i++)
        //    {
        //        if (DataFields[i] == propertyName)
        //        {
        //            return i;
        //        }
        //    }

        //    return -1;
        //}

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
        /// Enumerate the items in the specified cell range.
        /// This is used to updated the SelectedItems property.
        /// </summary>
        private IEnumerable EnumerateItems(CellRef cell0, CellRef cell1)
        {
            if (!(Content is Array))
            {
                var list = Content as IList;
                if (list != null)
                {
                    int index0 = !ItemsInColumns ? cell0.Row : cell0.Column;
                    int index1 = !ItemsInColumns ? cell1.Row : cell1.Column;
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
            if (list != null)
            {

                int index = GetItemIndex(cell);
                if (index >= 0 && index < list.Count)
                {
                    return list[index];
                }
            }

            var items = Content as IEnumerable;
            if (items != null)
            {
                int i = 0;
                foreach (var item in items)
                {
                    if ((!ItemsInColumns && i == cell.Row) || (!!ItemsInColumns && i == cell.Column))
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

        /// <summary>
        /// Gets the alternative values for the cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected virtual IEnumerable GetCellAlternatives(CellRef cell, object value)
        {
            if (value == null)
                value = GetCellValue(cell);
            var enumValue = value as Enum;
            if (enumValue != null)
            {
                return Enum.GetValues(enumValue.GetType());
            }
            return null;
        }

        public bool ShowEditor()
        {
            var value = GetCellValue(CurrentCell);
            if (value is Enum)
                return ShowComboBoxEditor();
            if (value != null)
            {
                var type = value.GetType();
                DataTemplate template = null;

                var cd = GetColumnDefinition(CurrentCell);
                if (cd != null && cd.EditTemplate != null)
                {
                    template = cd.EditTemplate;
                    contentEditor.Content = GetItem(CurrentCell);
                }
                if (template == null)
                {
                    template = GetEditTemplate(type);
                    contentEditor.Content = GetCellValue(CurrentCell);
                }

                if (template != null)
                {
                    contentEditor.ContentTemplate = template;
                    SetElementPosition(contentEditor, CurrentCell);
                    contentEditor.Visibility = Visibility.Visible;
                    return true;
                }
            }
            return false;
        }

        public void HideEditor()
        {
            contentEditor.Visibility = Visibility.Hidden;
            enumEditor.Visibility = Visibility.Hidden;
        }

        public void ShowTextBoxEditor()
        {
            editingCells = SelectedCells.ToList();

            Grid.SetColumn(textEditor, CurrentCell.Column);
            Grid.SetRow(textEditor, CurrentCell.Row);

            textEditor.Text = GetCellString(CurrentCell);
            textEditor.TextAlignment = ToTextAlignment(GetHorizontalAlignment(CurrentCell));

            textEditor.Visibility = Visibility.Visible;
            textEditor.Focus();
            textEditor.CaretIndex = textEditor.Text.Length;
            textEditor.SelectAll();
        }

        /// <summary>
        /// Convert a HorizontalAlignment to a TextAlignment.
        /// </summary>
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

        public bool ShowComboBoxEditor()
        {
            var value = GetCellValue(CurrentCell);
            var alternatives = GetCellAlternatives(CurrentCell, value);
            if (alternatives == null)
            {
                return false;
            }

            SetElementPosition(enumEditor, CurrentCell);

            // set editingCells to null to avoid setting values when enumEditor.SelectedValue is set below
            editingCells = null;

            enumEditor.ItemsSource = alternatives;
            enumEditor.SelectedValue = value;
            enumEditor.Visibility = Visibility.Visible;

            editingCells = SelectedCells.ToList();

            return true;
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
            }
        }



        private bool SetCheckInSelectedCells(bool value)
        {
            bool modified = false;
            foreach (var cell in SelectedCells)
            {
                var currentValue = GetCellValue(cell);
                if (currentValue is bool)
                {
                    if (TrySetCellValue(cell, value))
                        modified = true;
                }
            }

            return modified;
        }

        private bool ToggleCheckInSelectedCells()
        {
            bool modified = false;
            foreach (var cell in SelectedCells)
            {
                var currentValue = GetCellValue(cell);
                if (currentValue is bool)
                {
                    if (TrySetCellValue(cell, !((bool)currentValue)))
                        modified = true;
                }
            }

            return modified;
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
            var text = SelectionToString(separator);
            Clipboard.SetText(text);
        }

        public string ToCsv(string separator = ";")
        {
            var sb = new StringBuilder();
            for (int j = 0; j < Columns; j++)
            {
                var h = GetColumnHeader(j).ToString();
                h = CsvEncodeString(h);
                if (sb.Length > 0) sb.Append(separator);
                sb.Append(h);
            }
            sb.AppendLine();
            sb.Append(SheetToString(";", true));
            return sb.ToString();
        }

        private string SelectionToString(string separator, bool encode = false)
        {
            return ToString(CurrentCell, SelectionCell, separator, encode);
        }

        private string SheetToString(string separator, bool encode = false)
        {
            return ToString(new CellRef(0, 0), new CellRef(Rows - 1, Columns - 1), separator, encode);
        }

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
                    string cell = GetCellString(new CellRef(i, j));
                    if (encode)
                        cell = CsvEncodeString(cell);
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

        private static string CsvEncodeString(string cell)
        {
            cell = cell.Replace("\"", "\"\"");
            if (cell.Contains(";") || cell.Contains("\""))
                cell = "\"" + cell + "\"";
            return cell;
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
                InsertItem(from, false);
            }

            UpdateGridContent();
        }

        private void DeleteRows()
        {
            int from = Math.Min(CurrentCell.Row, SelectionCell.Row);
            int to = Math.Max(CurrentCell.Row, SelectionCell.Row);
            for (int i = to; i >= from; i--)
            {
                DeleteItem(i, false);
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
                case Key.Left:
                    if (textEditor.CaretIndex == 0)
                    {
                        EndTextEdit();
                        OnKeyDown(e);
                        e.Handled = true;
                    }
                    break;
                case Key.Right:
                    if (textEditor.CaretIndex == textEditor.Text.Length)
                    {
                        EndTextEdit();
                        OnKeyDown(e);
                        e.Handled = true;
                    }
                    break;
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

        private Binding CreateBinding(CellRef cellRef, object value)
        {
            var dataField = GetDataField(cellRef);
            if (dataField != null)
                return new Binding(dataField) { StringFormat = GetFormatString(cellRef, value) };
            return null;
        }

        protected bool IsCellVisible(CellRef cell)
        {
            if (IsVirtualizing)
            {
                // todo: should store topleft and bottomright visible cells
                // and check against these
            }

            return true;
        }

        protected void UpdateAllCells()
        {
            foreach (var element in cellMap.Values)
            {
                var cell = GetCellRefFromUIElement(element);
                UpdateCellContent(cell);
            }

        }

        private CellRef GetCellRefFromUIElement(UIElement element)
        {
            int row = Grid.GetRow(element);
            int column = Grid.GetColumn(element);
            return new CellRef(row, column);
        }

        /// <summary>
        /// Virtualizes the UIElements for the visible cells.
        /// Adds elements for the visible cells not currently in the logical tree.
        /// Removes elements for the nonvisible cells.
        /// </summary>
        protected void VirtualizeCells()
        {
            CellRef cell1, cell2;
            GetVisibleCells(out cell1, out cell2);
            if (cell1.Column < 0)
                return;

            var delete = cellMap.Keys.ToList();

            for (int i = cell1.Row; i <= cell2.Row; i++)
            {
                for (int j = cell1.Column; j <= cell2.Column; j++)
                {
                    var cellRef = new CellRef(i, j);
                    var c = GetCellElement(cellRef);
                    if (c == null)
                    {
                        // The cell is not currently in the collection - add it
                        UpdateCellContent(cellRef);
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
                var cell = cellMap[hash];
                sheetGrid.Children.Remove(cell);
                cellInsertionIndex--;
                cellMap.Remove(hash);
            }

        }

        protected void UpdateCellContent(CellRef cellRef)
        {
            var c = GetCellElement(cellRef);
            var value = GetCellValue(cellRef);

            if (c != null)
            {
                sheetGrid.Children.Remove(c);
                cellInsertionIndex--;
                cellMap.Remove(cellRef.GetHashCode());
            }
            InsertCellElement(cellRef, value, true);
        }


        readonly Dictionary<int, UIElement> cellMap = new Dictionary<int, UIElement>();

        private int cellInsertionIndex;

        private void AddCellElement(CellRef cellRef, object value)
        {
            InsertCellElement(cellRef, value, false);
        }

        private void InsertCellElement(CellRef cellRef, object value, bool insert)
        {
            //if (value == null)
            //    return;

            var e = CreateElement(cellRef, null);
            SetElementPosition(e, cellRef);
            if (insert)
            {
                sheetGrid.Children.Insert(cellInsertionIndex, e);
                cellInsertionIndex++;
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
        }

        private object GetColumnHeader(int j)
        {
            var text = CellRef.ToColumnName(j);

            if (!ItemsInColumns)
            {
                if (j < ColumnDefinitions.Count)
                    return ColumnDefinitions[j].Header;

                if (ActualColumnHeaders != null && j < ActualColumnHeaders.Count)
                {
                    text = ActualColumnHeaders[j];
                }
            }

            return text;
        }

        private object GetRowHeader(int j)
        {

            var text = CellRef.ToRowName(j);

            if (ItemsInColumns)
            {
                if (j < ColumnDefinitions.Count)
                    return ColumnDefinitions[j].Header;

                if (ActualColumnHeaders != null && j < ActualColumnHeaders.Count)
                {
                    text = ActualColumnHeaders[j];
                }
            }

            return text;
        }

        private int GetItemIndex(CellRef cell)
        {
            return !ItemsInColumns ? cell.Row : cell.Column;
        }

        private int GetFieldIndex(CellRef cell)
        {
            return !ItemsInColumns ? cell.Column : cell.Row;
        }

        private string GetDataField(CellRef cell)
        {
            int fieldIndex = GetFieldIndex(cell);

            if (fieldIndex < ColumnDefinitions.Count)
                return ColumnDefinitions[fieldIndex].DataField;

            if (ActualDataFields != null && fieldIndex < ActualDataFields.Count)
                return ActualDataFields[fieldIndex];

            return null;
        }

        public string GetCellString(CellRef cell)
        {
            var value = GetCellValue(cell);
            if (value == null)
                return null;
            var formatString = GetFormatString(cell, value);
            return FormatValue(value, formatString);
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

            var item = GetItem(cell);
            if (item != null)
            {
                var type = item.GetType();
                var dataField = GetDataField(cell);
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
            int rows = -1;
            int columns = -1;

            // Array
            if (Content != null && Content.GetType().IsArray)
            {
                cells = (Array)Content;
                int rank = cells.Rank;
                rows = cells.GetUpperBound(0) + 1;
                columns = rank > 1 ? cells.GetUpperBound(1) + 1 : 1;
            }

            if (cells == null)
            {
                var items = Content as IEnumerable;
                if (items != null)
                {
                    int n = items.Cast<object>().Count();
                    int m = 0;
                    if (UseColumnDefinitions)
                    {
                        m = ColumnDefinitions.Count;
                    }
                    else
                    {
                        var actualDataFields = DataFields;
                        var actualColumnHeaders = ColumnHeaders;
                        if (actualDataFields == null)
                        {
                            AutoGenerateColumns(items, out actualDataFields, out actualColumnHeaders);
                        }
                        ActualDataFields = actualDataFields;
                        ActualColumnHeaders = actualColumnHeaders;

                        m = ActualDataFields.Count;
                    }

                    rows = !ItemsInColumns ? n : m;
                    columns = !ItemsInColumns ? m : n;

                }
            }

            // Hide the row/column headers if the content is empty
            rowScroller.Visibility = columnScroller.Visibility = sheetScroller.Visibility = topleft.Visibility = rows >= 0 ? Visibility.Visible : Visibility.Hidden;

            if (rows < 0)
            {
                return;
            }


            //int rank = cells.Rank;
            //int rows = cells.GetUpperBound(0) + 1;
            //int columns = rank > 1 ? cells.GetUpperBound(1) + 1 : 1;

            UpdateRows(rows);
            UpdateColumns(columns);
            UpdateSheet(rows, columns);

            UpdateColumnWidths();

            SubscribeNotifications();

        }

        protected bool UseColumnDefinitions
        {
            get { return ColumnDefinitions.Count > 0; }
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

        /// <summary>
        /// Called when any item in the Content is changed.
        /// </summary>
        private void OnContentItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (var cell in EnumerateCells(sender, e.PropertyName))
            {
                if (IsCellVisible(cell))
                    UpdateCellContent(cell);
            }
        }

        private IEnumerable<CellRef> EnumerateCells(object item, string propertyName)
        {
            if (ActualDataFields == null)
                yield break;

            if (Content is IEnumerable)
            {
                int i = 0;
                foreach (var it in Content as IEnumerable)
                {
                    if (it == item)
                    {
                        for (int j = 0; j < ActualDataFields.Count; j++)
                            if (ActualDataFields[j] == propertyName)
                            {
                                var cell = !ItemsInColumns ? new CellRef(i, j) : new CellRef(j, i);
                                yield return cell;
                            }
                    }
                    i++;
                }
            }
        }

        private void OnContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // todo: update rows
            UpdateGridContent();
        }

        public void AutoSizeAllColumns()
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

            for (int i = 0; i < Columns; i++)
            {
                if (GetColumnWidth(i) == GridLength.Auto || AutoSizeColumns)
                    AutoSizeColumn(i);
            }

            sheetGrid.UpdateLayout();

            for (int j = 0; j < sheetGrid.ColumnDefinitions.Count; j++)
            {
                columnGrid.ColumnDefinitions[j].Width = new GridLength(sheetGrid.ColumnDefinitions[j].ActualWidth);
            }
        }

        private void UpdateSheet(int rows, int columns)
        {
            //int rank = cells.Rank;
            //int rows = cells.GetUpperBound(0) + 1;
            //int columns = rank > 1 ? cells.GetUpperBound(1) + 1 : 1;

            sheetGrid.Children.Clear();
            sheetGrid.Children.Add(selectionBackground);
            sheetGrid.Children.Add(currentBackground);
            cellMap.Clear();

            // todo: UI virtualize grid lines (both rows and columns)

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
                if (columns > 0)
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

            cellInsertionIndex = sheetGrid.Children.Count;

            if (IsVirtualizing)
            {
                VirtualizeCells();
            }
            else
            {
                // Add all cells to the sheet
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        var cell = new CellRef(i, j);
                        var value = GetCellValue(cell);
                        AddCellElement(cell, value);
                    }
                }
            }

            sheetGrid.Children.Add(textEditor);
            sheetGrid.Children.Add(selection);
            sheetGrid.Children.Add(autoFillBox);
            sheetGrid.Children.Add(autoFillSelection);
            sheetGrid.Children.Add(contentEditor);
            sheetGrid.Children.Add(enumEditor);
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
                object header = GetRowHeader(i);

                var border = new Border
                {
                    BorderBrush = HeaderBorderBrush,
                    BorderThickness = new Thickness(1, 0, 1, 1),
                    Margin = new Thickness(0, 0, 0, -1)
                };

                Grid.SetRow(border, i);
                rowGrid.Children.Add(border);

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
                    //                    ToolTip = "Add row",
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

        private void UpdateColumns(int columns)
        {
            Columns = columns;
            rowGrid.ColumnDefinitions.Clear();
            sheetGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < columns; i++)
            {
                var w = GetColumnWidth(i);
                sheetGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = w });

                // the width of the header column will be updated later
                columnGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            }

            // Add one empty column covering the vertical scrollbar
            columnGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = new GridLength(40) });


            columnGrid.Children.Clear();
            columnGrid.Children.Add(columnSelectionBackground);
            for (int j = 0; j < columns; j++)
            {
                object header = GetColumnHeader(j);

                var border = new Border
                {
                    BorderBrush = HeaderBorderBrush,
                    BorderThickness = new Thickness(0, 1, 1, 1),
                    Margin = new Thickness(0, 0, j < columns - 1 ? -1 : 0, 0)
                };
                Grid.SetColumn(border, j);
                columnGrid.Children.Add(border);

                var cell = header as FrameworkElement;
                if (cell == null)
                {
                    cell = new TextBlock
                    {
                        Text = header != null ? header.ToString() : "-",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = GetHorizontalAlignment(new CellRef(!ItemsInColumns ? -1 : j, !ItemsInColumns ? j : -1)),
                        Padding = new Thickness(4, 2, 4, 2)
                    };
                }
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

        protected virtual UIElement CreateElement(CellRef cell, object item = null)
        {
            FrameworkElement element = null;

            if (item == null)
                item = GetItem(cell);

            var cd = GetColumnDefinition(cell);
            if (cd != null && cd.DisplayTemplate != null)
            {
                element = new ContentControl { ContentTemplate = cd.DisplayTemplate, Content = item };

                // note: vertical/horziontal alignment must be set in the DataTemplate
            }

            if (element == null)
            {
                var value = GetCellValue(cell);
                var type = value != null ? value.GetType() : null;

                var template = GetDisplayTemplate(type);
                if (template != null)
                {
                    element = new ContentControl { ContentTemplate = template, Content = value };
                }

                var binding = CreateBinding(cell, value);

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
                        chkbox.Checked += CellChecked;
                    }
                    element = chkbox;
                }

                if (element == null && typeof(BitmapSource).IsAssignableFrom(type))
                {
                    var chkbox = new Image
                                     {
                                         Source = (BitmapSource)value,
                                         Stretch = Stretch.None
                                     };
                    element = chkbox;
                }

                //if (element == null && typeof(Uri).IsAssignableFrom(type))
                //{
                //    element = new TextBlock(new Hyperlink(new Run(value != null ? value.ToString() : null)) { NavigateUri = (Uri)value });
                //}

                if (element == null)
                {
                    var textBlock = new TextBlock
                                        {
                                            Margin = new Thickness(4, 0, 4, 0),
                                            Foreground = this.Foreground
                                        };
                    if (binding != null)
                    {
                        textBlock.SetBinding(TextBlock.TextProperty, binding);
                    }
                    else
                    {
                        var formatString = GetFormatString(cell, value);
                        var text = FormatValue(value, formatString);
                        textBlock.Text = text;
                    }
                    element = textBlock;
                }
                element.Tag = type;

                if (binding != null)
                    element.DataContext = item;

                element.VerticalAlignment = VerticalAlignment.Center;
                element.HorizontalAlignment = GetHorizontalAlignment(cell);
            }

            return element;
        }


        private ColumnDefinition GetColumnDefinition(CellRef cell)
        {
            int fieldIndex = GetFieldIndex(cell);
            if (fieldIndex < ColumnDefinitions.Count)
                return ColumnDefinitions[fieldIndex];
            return null;
        }

        private TypeDefinition GetCustomTemplate(Type type)
        {
            foreach (var e in TypeDefinitions)
            {
                var et = e.Type;
                if (et.IsAssignableFrom(type))
                {
                    return e;
                }
            }
            return null;
        }

        private DataTemplate GetDisplayTemplate(Type type)
        {
            var e = GetCustomTemplate(type);
            return e != null ? e.DisplayTemplate : null;
        }

        private DataTemplate GetEditTemplate(Type type)
        {
            var e = GetCustomTemplate(type);
            return e != null ? e.EditTemplate : null;
        }

        private static string FormatValue(object value, string formatString)
        {
            if (String.IsNullOrEmpty(formatString))
            {
                return value != null ? value.ToString() : null;
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


        private HorizontalAlignment GetHorizontalAlignment(CellRef cell)
        {
            int i = GetFieldIndex(cell);
            if (i < ColumnDefinitions.Count)
                return ColumnDefinitions[i].HorizontalAlignment;

            if (ColumnAlignments != null && cell.Column < ColumnAlignments.Count)
                return ColumnAlignments[cell.Column];
            return DefaultHorizontalAlignment;
        }

        private string GetFormatString(CellRef cell, object value)
        {
            if (value != null)
            {
                var ct = GetCustomTemplate(value.GetType());
                if (ct != null && ct.StringFormat != null)
                    return ct.StringFormat;
            }

            int i = GetFieldIndex(cell);
            if (i < ColumnDefinitions.Count)
                return ColumnDefinitions[i].StringFormat;
            if (FormatStrings != null && cell.Column < FormatStrings.Count)
                return FormatStrings[cell.Column];
            return null;
        }

        private GridLength GetColumnWidth(int i)
        {
            if (i < ColumnDefinitions.Count)
            {
                if (ColumnDefinitions[i].Width.Value < 0)
                    return DefaultColumnWidth;
                return ColumnDefinitions[i].Width;
            }
            if (ColumnWidths != null && i < ColumnWidths.Count)
                return ColumnWidths[i];
            return DefaultColumnWidth;
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

        private static Type GetListItemType(IEnumerable list)
        {
            if (list == null)
                return null;
            foreach (var item in list)
            {
                if (item != null)
                    return item.GetType();
            }
            return null;
        }

        private static Type GetListItemType(Type listType)
        {
            // http://stackoverflow.com/questions/1043755/c-generic-list-t-how-to-get-the-type-of-t
            foreach (var interfaceType in listType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    var args = interfaceType.GetGenericArguments();
                    if (args.Length > 0)
                        return args[0];
                }
            }

            return null;
        }

        public bool DeleteItem(int index, bool updateGrid)
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

            if (updateGrid)
            {
                this.UpdateGridContent();
            }

            return true;
        }

        public bool InsertItem(int index, bool updateGrid = true)
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

            if (updateGrid)
            {
                UpdateGridContent();
            }
            return true;
        }

        protected virtual object CreateInstance(Type itemType)
        {
            object newItem;
            newItem = Activator.CreateInstance(itemType);
            return newItem;
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

            // Binding was not used here, so update the value of the cell
            TrySetCellValue(CurrentCell, chkbox.IsChecked);
            UpdateCellContent(CurrentCell);
        }

        //private object[,] ConvertItemsSourceToArray(IEnumerable items)
        //{
        //    int nItems = items.Cast<object>().Count();

        //    var actualDataFields = DataFields;
        //    var actualColumnHeaders = ColumnHeaders;
        //    if (actualDataFields == null)
        //    {
        //        AutoGenerateColumns(items, out actualDataFields, out actualColumnHeaders);
        //    }

        //    ActualDataFields = actualDataFields;
        //    ActualColumnHeaders = actualColumnHeaders;

        //    int nFields = actualDataFields != null ? actualDataFields.Count : 1;
        //    var pi = new PropertyInfo[nFields];
        //    var cells = !ItemsInColumns ? new object[nItems, nFields] : new object[nFields, nItems];
        //    int i = 0;
        //    foreach (var item in items)
        //    {
        //        var type = item.GetType();
        //        for (int j = 0; j < nFields; j++)
        //        {
        //            object value = null;
        //            if (actualDataFields == null)
        //            {
        //                value = item;
        //            }
        //            else
        //            {
        //                if (pi[j] == null || pi[j].DeclaringType != type)
        //                {
        //                    pi[j] = type.GetProperty(actualDataFields[j]);
        //                }

        //                if (pi[j] != null)
        //                {
        //                    value = pi[j].GetValue(item, null);
        //                }
        //                else
        //                {
        //                    value = item;
        //                }
        //            }

        //            if (!ItemsInColumns)
        //            {
        //                cells[i, j] = value;
        //            }
        //            else
        //            {
        //                cells[j, i] = value;
        //            }
        //        }

        //        i++;
        //    }

        //    return cells;
        //}

        protected virtual void AutoGenerateColumns(IEnumerable items, out StringCollection dataFields,
                                         out StringCollection columnHeaders)
        {
            var itemType = GetListItemType(items.GetType());
            // todo: how to find the right type?
            if (itemType == null)
                itemType = GetListItemType(items);

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

            if (IsVirtualizing)
                VirtualizeCells();
        }

        private void ScrollViewerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsVirtualizing)
                VirtualizeCells();
        }

        private void RowScrollerChanged(object sender, ScrollChangedEventArgs e)
        {
            sheetScroller.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void ColumnScrollerChanged(object sender, ScrollChangedEventArgs e)
        {
            sheetScroller.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        public void GetVisibleCells(out CellRef topLeft, out CellRef bottomRight)
        {
            double left = sheetScroller.HorizontalOffset;
            double right = left + sheetScroller.ActualWidth;
            double top = sheetScroller.VerticalOffset;
            double bottom = top + sheetScroller.ActualHeight;

            topLeft = GetCell(new Point(left, top));
            bottomRight = GetCell(new Point(right, bottom));
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

            ShowTextBoxEditor();

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
            bool alt = Keyboard.IsKeyDown(Key.LeftAlt);

            int row = shift ? SelectionCell.Row : CurrentCell.Row;
            int column = shift ? SelectionCell.Column : CurrentCell.Column;

            switch (e.Key)
            {
                case Key.Up:
                    if (row > 0)
                    {
                        row--;
                    }

                    if (endPressed)
                    {
                        FindNext(ref row, ref column, -1, 0);
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

                    if (endPressed)
                    {
                        FindNext(ref row, ref column, 1, 0);
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

                    if (endPressed)
                    {
                        FindNext(ref row, ref column, 0, -1);
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

                    if (endPressed)
                    {
                        FindNext(ref row, ref column, 0, 1);
                    }
                    if (control)
                    {
                        column = Columns - 1;
                    }

                    break;
                case Key.End:
                    endPressed = true;
                    break;
                case Key.Home:
                    column = 0;
                    row = 0;
                    break;
                case Key.Delete:
                    Delete();
                    break;
                case Key.F2:
                    ShowTextBoxEditor();
                    break;
                case Key.Space:

                    bool value = true;
                    var cvalue = GetCellValue(CurrentCell);
                    if (cvalue is bool)
                    {
                        value = (bool)cvalue;
                        value = !value;
                    }


                    if (SetCheckInSelectedCells(value))
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
                case Key.C:
                    if (control && alt)
                    {
                        Clipboard.SetText(ToCsv());
                        e.Handled = true;
                    }
                    return;
                default:
                    return;
            }

            if (e.Key != Key.End)
                endPressed = false;

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

        private void FindNext(ref int row, ref int column, int deltaRow, int deltaColumn)
        {
            while (row >= 0 && row < Rows && column >= 0 && column < Columns - 1)
            {
                var v = GetCellValue(new CellRef(row, column));
                if (v == null || String.IsNullOrEmpty(v.ToString()))
                    break;
                row += deltaRow;
                column += deltaColumn;
            }
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
                    var fmt = GetFormatString(cellRef, result);
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
            if (w > 0 && column == -1)
                column = sheetGrid.ColumnDefinitions.Count - 1;

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

            if (h > 0 && row == -1)
                row = sheetGrid.RowDefinitions.Count - 1;

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

                int fieldIndex = !ItemsInColumns ? cell.Column : cell.Row;
                if (current != null)
                {

                    var field = GetDataField(cell);
                    var pi = current.GetType().GetProperty(field);
                    if (pi == null)
                    {
                        //var list = Content as IList;
                        //int itemIndex=GetItemIndex(cell);
                        //object convertedValue;
                        //if (TryConvert(value, pi.PropertyType, out convertedValue))
                        //{
                        //    list[itemIndex]=
                        // todo: set the actual item to the value
                        return false;
                    }

                    object convertedValue;
                    if (TryConvert(value, pi.PropertyType, out convertedValue) && pi.CanWrite)
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