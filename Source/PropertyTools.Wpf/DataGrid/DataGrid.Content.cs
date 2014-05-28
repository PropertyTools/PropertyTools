// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGrid.Content.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a data grid with a spreadsheet style.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Represents a data grid with a spreadsheet style.
    /// </summary>
    public partial class DataGrid
    {
        /// <summary>
        /// Clears the content of the control.
        /// </summary>
        private void ClearContent()
        {
            this.rowGrid.RowDefinitions.Clear();
            this.rowGrid.ColumnDefinitions.Clear();
            this.rowGrid.Children.Clear();
            this.columnGrid.RowDefinitions.Clear();
            this.columnGrid.ColumnDefinitions.Clear();
            this.columnGrid.Children.Clear();

            this.sheetGrid.RowDefinitions.Clear();
            this.sheetGrid.ColumnDefinitions.Clear();
            this.sheetGrid.Children.Clear();
            this.cellMap.Clear();
        }

        /// <summary>
        /// The subscribe notifications.
        /// </summary>
        private void SubscribeToNotifications()
        {
            var collection = this.ItemsSource as INotifyCollectionChanged;
            if (collection != null)
            {
                collection.CollectionChanged += this.OnItemsCollectionChanged;
            }

            this.subcribedCollection = this.ItemsSource;
        }

        /// <summary>
        /// Updates the cells of the grid.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        private void UpdateCells(int rows, int columns)
        {
            this.sheetGrid.Children.Add(this.selectionBackground);
            this.sheetGrid.Children.Add(this.currentBackground);

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
                    this.AddDisplayControl(new CellRef(i, j));
                }
            }

            this.sheetGrid.Children.Add(this.selection);
            this.sheetGrid.Children.Add(this.autoFillBox);
            this.sheetGrid.Children.Add(this.autoFillSelection);
        }

        /// <summary>
        /// Updates column widths.
        /// </summary>
        private void UpdateColumnWidths()
        {
            if (!this.IsLoaded)
            {
                return;
            }

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
        /// Updates the specified columns.
        /// </summary>
        /// <param name="columns">The column index.</param>
        private void UpdateColumns(int columns)
        {
            this.Columns = columns;
            for (int i = 0; i < columns; i++)
            {
                var w = this.GetColumnWidth(i);
                var cd1 = new System.Windows.Controls.ColumnDefinition { Width = w };
                this.sheetGrid.ColumnDefinitions.Add(cd1);

                // bind the width of the header column to the width of the sheet column
                var cd2 = new System.Windows.Controls.ColumnDefinition();
                this.columnGrid.ColumnDefinitions.Add(cd2);
                var binding = new Binding("Width") { Source = cd1, Mode = BindingMode.TwoWay };
                cd2.SetBinding(System.Windows.Controls.ColumnDefinition.WidthProperty, binding);
            }

            // Add one empty column covering the vertical scrollbar
            this.columnGrid.ColumnDefinitions.Add(
                new System.Windows.Controls.ColumnDefinition { Width = new GridLength(40) });

            // set the context menu
            this.columnGrid.ContextMenu = this.ColumnsContextMenu;

            this.columnGrid.Children.Add(this.columnSelectionBackground);
            for (int j = 0; j < columns; j++)
            {
                object header = this.GetColumnHeader(j);
                var cellref = new CellRef(this.ItemsInRows ? -1 : j, this.ItemsInRows ? j : -1);
                var pd = this.GetPropertyDefinition(cellref);

                var border = new Border
                                 {
                                     BorderBrush = this.HeaderBorderBrush,
                                     BorderThickness = new Thickness(0, 1, 1, 1),
                                     Margin = new Thickness(0, 0, j < columns - 1 ? -1 : 0, 0)
                                 };
                Grid.SetColumn(border, j);
                this.columnGrid.Children.Add(border);

                var cell = header as FrameworkElement
                           ?? new TextBlock
                                  {
                                      Text = header != null ? header.ToString() : "-",
                                      VerticalAlignment = VerticalAlignment.Center,
                                      HorizontalAlignment = this.ItemsInRows ? pd.HorizontalAlignment : HorizontalAlignment.Center,
                                      Padding = new Thickness(4, 2, 4, 2)
                                  };

                if (this.ColumnHeadersSource != null && this.ItemsInRows)
                {
                    cell.DataContext = this.ColumnHeadersSource;
                    cell.SetBinding(TextBlock.TextProperty, new Binding(string.Format("[{0}]", j)) { StringFormat = this.ColumnHeadersFormatString });
                }

                Grid.SetColumn(cell, j);
                this.columnGrid.Children.Add(cell);
            }

            for (int j = 0; j < columns; j++)
            {
                if (this.CanResizeColumns)
                {
                    var splitter = new GridSplitter
                        {
                            ResizeDirection = GridResizeDirection.Columns,
                            Background = Brushes.Transparent,
                            Width = 5,
                            RenderTransform = new TranslateTransform(3, 0),
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

            this.ClearContent();

            if (this.ItemsSource == null)
            {
                return;
            }

            if (this.AutoGenerateColumns && this.PropertyDefinitions.Count == 0)
            {
                this.GenerateColumnDefinitions();
            }

            // If PropertyType is undefined, use the type of the items in the ItemsSource
            var itemsType = this.GetItemsType();
            foreach (var pd in this.PropertyDefinitions)
            {
                if (pd.PropertyType == null)
                {
                    pd.PropertyType = itemsType;
                }
            }

            // Determine if columns or rows are defined
            this.ItemsInColumns = this.PropertyDefinitions.FirstOrDefault(pd => pd is RowDefinition) != null;

            // If only PropertyName has been defined, use the type of the items to get the descriptor.
            var itemType = TypeHelper.GetItemType(this.ItemsSource);
            foreach (var pd in this.PropertyDefinitions)
            {
                if (pd.Descriptor == null && !string.IsNullOrEmpty(pd.PropertyName))
                {
                    pd.Descriptor = TypeDescriptor.GetProperties(itemType)[pd.PropertyName];
                }
            }

            int n = this.ItemsSource.Cast<object>().Count();
            int m = this.PropertyDefinitions.Count;

            if (this.WrapItems)
            {
                n /= m;
            }

            int rows = this.ItemsInRows ? n : m;
            int columns = this.ItemsInRows ? m : n;

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
            this.UpdateSelectionVisibility();
            this.ShowEditControl();

            this.SubscribeToNotifications();
        }

        /// <summary>
        /// Updates the rows.
        /// </summary>
        /// <param name="rows">The number rows.</param>
        private void UpdateRows(int rows)
        {
            this.rowGrid.Children.Add(this.rowSelectionBackground);

            this.Rows = rows;

            for (var i = 0; i < rows; i++)
            {
                this.sheetGrid.RowDefinitions.Add(
                    new System.Windows.Controls.RowDefinition { Height = this.DefaultRowHeight });
                this.rowGrid.RowDefinitions.Add(
                    new System.Windows.Controls.RowDefinition { Height = this.DefaultRowHeight });
            }

            for (var i = 0; i < rows; i++)
            {
                var header = this.GetRowHeader(i);

                var border = new Border
                    {
                        BorderBrush = this.HeaderBorderBrush,
                        BorderThickness = new Thickness(1, 0, 1, 1),
                        Margin = new Thickness(0, 0, 0, -1)
                    };

                Grid.SetRow(border, i);
                this.rowGrid.Children.Add(border);

                var cell = header as FrameworkElement
                           ??
                           new TextBlock
                               {
                                   Text = header != null ? header.ToString() : "-",
                                   VerticalAlignment = VerticalAlignment.Center,
                                   HorizontalAlignment = HorizontalAlignment.Center,
                                   Padding = new Thickness(4, 2, 4, 2)
                               };

                if (this.ItemHeaderPropertyPath != null && this.ItemsInRows)
                {
                    cell.DataContext = this.GetItem(new CellRef(i, -1));
                    cell.SetBinding(TextBlock.TextProperty, new Binding(this.ItemHeaderPropertyPath));
                }

                if (this.RowHeadersSource != null && this.ItemsInRows)
                {
                    cell.DataContext = this.RowHeadersSource;
                    cell.SetBinding(
                        TextBlock.TextProperty,
                        new Binding(string.Format("[{0}]", i)) { StringFormat = this.RowHeadersFormatString });
                }

                Grid.SetRow(cell, i);
                this.rowGrid.Children.Add(cell);
            }

            // Add "Insert" row header
            this.AddInserterRow(rows);

            // set the context menu
            this.rowGrid.ContextMenu = this.RowsContextMenu;

            // to cover a possible scrollbar
            this.rowGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = new GridLength(20) });
        }

        /// <summary>
        /// Adds a "insert" row.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <remarks>This row is below/to the right of the data rows/columns.</remarks>
        private void AddInserterRow(int rows)
        {
            if (this.CanInsertRows && this.AddItemHeader != null)
            {
                this.sheetGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = this.DefaultRowHeight });
                this.rowGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = this.DefaultRowHeight });

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
                this.sheetGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = GridLength.Auto });
                this.rowGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = GridLength.Auto });
            }
        }
    }
}