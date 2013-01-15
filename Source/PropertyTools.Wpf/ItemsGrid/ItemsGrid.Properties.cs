// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsGrid.Properties.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
//   Properties for the ItemsGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    ///     Properties for the ItemsGrid.
    /// </summary>
    public partial class ItemsGrid
    {
        /// <summary>
        ///     The add item header property.
        /// </summary>
        public static readonly DependencyProperty AddItemHeaderProperty = DependencyProperty.Register(
            "AddItemHeader", typeof(string), typeof(ItemsGrid), new UIPropertyMetadata("*"));

        /// <summary>
        ///     The alternating rows background property.
        /// </summary>
        public static readonly DependencyProperty AlternatingRowsBackgroundProperty =
            DependencyProperty.Register(
                "AlternatingRowsBackground", typeof(Brush), typeof(ItemsGrid), new UIPropertyMetadata(null));

        /// <summary>
        ///     The auto generate columns property.
        /// </summary>
        public static readonly DependencyProperty AutoGenerateColumnsProperty =
            DependencyProperty.Register(
                "AutoGenerateColumns", typeof(bool), typeof(ItemsGrid), new UIPropertyMetadata(true));

        /// <summary>
        ///     The auto insert property.
        /// </summary>
        public static readonly DependencyProperty AutoInsertProperty = DependencyProperty.Register(
            "AutoInsert", typeof(bool), typeof(ItemsGrid), new UIPropertyMetadata(true));

        /// <summary>
        ///     The auto size columns property.
        /// </summary>
        public static readonly DependencyProperty AutoSizeColumnsProperty =
            DependencyProperty.Register(
                "AutoSizeColumns", typeof(bool), typeof(ItemsGrid), new UIPropertyMetadata(false));

        /// <summary>
        ///     The can delete property.
        /// </summary>
        public static readonly DependencyProperty CanDeleteProperty = DependencyProperty.Register(
            "CanDelete", typeof(bool), typeof(ItemsGrid), new UIPropertyMetadata(true));

        /// <summary>
        ///     The can insert property.
        /// </summary>
        public static readonly DependencyProperty CanInsertProperty = DependencyProperty.Register(
            "CanInsert", typeof(bool), typeof(ItemsGrid), new UIPropertyMetadata(true));

        /// <summary>
        ///     The can resize columns property.
        /// </summary>
        public static readonly DependencyProperty CanResizeColumnsProperty =
            DependencyProperty.Register(
                "CanResizeColumns", typeof(bool), typeof(ItemsGrid), new UIPropertyMetadata(true));

        /// <summary>
        ///     The column header height property.
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderHeightProperty =
            DependencyProperty.Register(
                "ColumnHeaderHeight", typeof(GridLength), typeof(ItemsGrid), new UIPropertyMetadata(new GridLength(20)));

        /// <summary>
        ///     The columns context menu property.
        /// </summary>
        public static readonly DependencyProperty ColumnsContextMenuProperty =
            DependencyProperty.Register(
                "ColumnsContextMenu", typeof(ContextMenu), typeof(ItemsGrid), new UIPropertyMetadata(null));

        /// <summary>
        ///     The control factory property.
        /// </summary>
        public static readonly DependencyProperty ControlFactoryProperty = DependencyProperty.Register(
            "ControlFactory",
            typeof(IItemsGridControlFactory),
            typeof(ItemsGrid),
            new UIPropertyMetadata(new ItemsGridControlFactory()));

        /// <summary>
        ///     The current cell property.
        /// </summary>
        public static readonly DependencyProperty CurrentCellProperty = DependencyProperty.Register(
            "CurrentCell",
            typeof(CellRef),
            typeof(ItemsGrid),
            new FrameworkPropertyMetadata(
                new CellRef(0, 0),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (d, e) => ((ItemsGrid)d).CurrentCellChanged(),
                CoerceCurrentCell));

        /// <summary>
        ///     The default column width property.
        /// </summary>
        public static readonly DependencyProperty DefaultColumnWidthProperty =
            DependencyProperty.Register(
                "DefaultColumnWidth",
                typeof(GridLength),
                typeof(ItemsGrid),
                new UIPropertyMetadata(new GridLength(1, GridUnitType.Star)));

        /// <summary>
        ///     The default horizontal alignment property.
        /// </summary>
        public static readonly DependencyProperty DefaultHorizontalAlignmentProperty =
            DependencyProperty.Register(
                "DefaultHorizontalAlignment",
                typeof(HorizontalAlignment),
                typeof(ItemsGrid),
                new UIPropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        ///     The default row height property.
        /// </summary>
        public static readonly DependencyProperty DefaultRowHeightProperty =
            DependencyProperty.Register(
                "DefaultRowHeight", typeof(GridLength), typeof(ItemsGrid), new UIPropertyMetadata(new GridLength(20)));

        /// <summary>
        ///     The easy insert property
        /// </summary>
        public static readonly DependencyProperty EasyInsertProperty = DependencyProperty.Register(
            "EasyInsert", typeof(bool), typeof(ItemsGrid), new UIPropertyMetadata(true));

        /// <summary>
        ///     The grid line brush property.
        /// </summary>
        public static readonly DependencyProperty GridLineBrushProperty = DependencyProperty.Register(
            "GridLineBrush",
            typeof(Brush),
            typeof(ItemsGrid),
            new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(218, 220, 221))));

        /// <summary>
        ///     The header border brush property.
        /// </summary>
        public static readonly DependencyProperty HeaderBorderBrushProperty =
            DependencyProperty.Register(
                "HeaderBorderBrush",
                typeof(Brush),
                typeof(ItemsGrid),
                new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(177, 181, 186))));

        /// <summary>
        ///     The input direction property
        /// </summary>
        public static readonly DependencyProperty InputDirectionProperty = DependencyProperty.Register(
            "InputDirection", typeof(InputDirection), typeof(ItemsGrid), new UIPropertyMetadata(InputDirection.Vertical));

        /// <summary>
        ///     The is virtualizing property.
        /// </summary>
        public static readonly DependencyProperty IsVirtualizingProperty = DependencyProperty.Register(
            "IsVirtualizing", typeof(bool), typeof(ItemsGrid), new UIPropertyMetadata(false));

        /// <summary>
        ///     The item header property path property.
        /// </summary>
        public static readonly DependencyProperty ItemHeaderPropertyPathProperty =
            DependencyProperty.Register(
                "ItemHeaderPropertyPath", typeof(string), typeof(ItemsGrid), new UIPropertyMetadata(null));

        /// <summary>
        ///     The items in columns property.
        /// </summary>
        public static readonly DependencyProperty ItemsInColumnsProperty = DependencyProperty.Register(
            "ItemsInColumns",
            typeof(bool),
            typeof(ItemsGrid),
            new UIPropertyMetadata(false, (d, e) => ((ItemsGrid)d).UpdateGridContent()));

        /// <summary>
        ///     The items source property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(IList),
            typeof(ItemsGrid),
            new UIPropertyMetadata(null, (d, e) => ((ItemsGrid)d).UpdateGridContent()));

        /// <summary>
        ///     The row header width property.
        /// </summary>
        public static readonly DependencyProperty RowHeaderWidthProperty = DependencyProperty.Register(
            "RowHeaderWidth", typeof(GridLength), typeof(ItemsGrid), new UIPropertyMetadata(new GridLength(40)));

        /// <summary>
        ///     The rows context menu property.
        /// </summary>
        public static readonly DependencyProperty RowsContextMenuProperty =
            DependencyProperty.Register(
                "RowsContextMenu", typeof(ContextMenu), typeof(ItemsGrid), new UIPropertyMetadata(null));

        /// <summary>
        ///     The selected items property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            "SelectedItems", typeof(IEnumerable), typeof(ItemsGrid), new UIPropertyMetadata(null));

        /// <summary>
        ///     The selection cell property.
        /// </summary>
        public static readonly DependencyProperty SelectionCellProperty = DependencyProperty.Register(
            "SelectionCell",
            typeof(CellRef),
            typeof(ItemsGrid),
            new UIPropertyMetadata(
                new CellRef(0, 0), (d, e) => ((ItemsGrid)d).SelectionCellChanged(), CoerceSelectionCell));

        /// <summary>
        ///     The wrap items property.
        /// </summary>
        public static readonly DependencyProperty WrapItemsProperty = DependencyProperty.Register(
            "WrapItems", typeof(bool), typeof(ItemsGrid), new UIPropertyMetadata(false));

        /// <summary>
        ///     The column definitions.
        /// </summary>
        private readonly Collection<PropertyDefinition> propertyDefinitions = new Collection<PropertyDefinition>();

        /// <summary>
        ///     Gets or sets the header used for the add item row/column. Default is "*".
        /// </summary>
        /// <value> The add item header. </value>
        public string AddItemHeader
        {
            get
            {
                return (string)this.GetValue(AddItemHeaderProperty);
            }

            set
            {
                this.SetValue(AddItemHeaderProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the alternating rows background brush.
        /// </summary>
        /// <value> The alternating rows background. </value>
        public Brush AlternatingRowsBackground
        {
            get
            {
                return (Brush)this.GetValue(AlternatingRowsBackgroundProperty);
            }

            set
            {
                this.SetValue(AlternatingRowsBackgroundProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the auto fill cell.
        /// </summary>
        /// <value> The auto fill cell. </value>
        public CellRef AutoFillCell
        {
            get
            {
                return this.autoFillCell;
            }

            set
            {
                this.autoFillCell = (CellRef)CoerceSelectionCell(this, value);
                this.SelectedCellsChanged();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to auto generate columns.
        /// </summary>
        public bool AutoGenerateColumns
        {
            get
            {
                return (bool)this.GetValue(AutoGenerateColumnsProperty);
            }

            set
            {
                this.SetValue(AutoGenerateColumnsProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to allow automatic insert mode.
        /// </summary>
        public bool AutoInsert
        {
            get
            {
                return (bool)this.GetValue(AutoInsertProperty);
            }

            set
            {
                this.SetValue(AutoInsertProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether auto size columns is enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [auto size columns]; otherwise, <c>false</c> .
        /// </value>
        public bool AutoSizeColumns
        {
            get
            {
                return (bool)this.GetValue(AutoSizeColumnsProperty);
            }

            set
            {
                this.SetValue(AutoSizeColumnsProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance can delete.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can delete; otherwise, <c>false</c> .
        /// </value>
        public bool CanDelete
        {
            get
            {
                return (bool)this.GetValue(CanDeleteProperty);
            }

            set
            {
                this.SetValue(CanDeleteProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance can insert.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can insert; otherwise, <c>false</c> .
        /// </value>
        public bool CanInsert
        {
            get
            {
                return (bool)this.GetValue(CanInsertProperty);
            }

            set
            {
                this.SetValue(CanInsertProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance can resize columns.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can resize columns; otherwise, <c>false</c> .
        /// </value>
        public bool CanResizeColumns
        {
            get
            {
                return (bool)this.GetValue(CanResizeColumnsProperty);
            }

            set
            {
                this.SetValue(CanResizeColumnsProperty, value);
            }
        }

        /// <summary>
        ///     Gets the column definitions.
        /// </summary>
        /// <value> The column definitions. </value>
        public Collection<PropertyDefinition> ColumnDefinitions
        {
            get
            {
                return this.propertyDefinitions;
            }
        }

        /// <summary>
        ///     Gets or sets the height of the column headers.
        /// </summary>
        /// <value> The height of the column header. </value>
        public GridLength ColumnHeaderHeight
        {
            get
            {
                return (GridLength)this.GetValue(ColumnHeaderHeightProperty);
            }

            set
            {
                this.SetValue(ColumnHeaderHeightProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the columns context menu.
        /// </summary>
        /// <value> The columns context menu. </value>
        public ContextMenu ColumnsContextMenu
        {
            get
            {
                return (ContextMenu)this.GetValue(ColumnsContextMenuProperty);
            }

            set
            {
                this.SetValue(ColumnsContextMenuProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets ControlFactory.
        /// </summary>
        public IItemsGridControlFactory ControlFactory
        {
            get
            {
                return (IItemsGridControlFactory)this.GetValue(ControlFactoryProperty);
            }

            set
            {
                this.SetValue(ControlFactoryProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the current cell.
        /// </summary>
        public CellRef CurrentCell
        {
            get
            {
                return (CellRef)this.GetValue(CurrentCellProperty);
            }

            set
            {
                this.SetValue(CurrentCellProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the default column width.
        /// </summary>
        /// <value> The default width of the column. </value>
        public GridLength DefaultColumnWidth
        {
            get
            {
                return (GridLength)this.GetValue(DefaultColumnWidthProperty);
            }

            set
            {
                this.SetValue(DefaultColumnWidthProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the default horizontal alignment.
        /// </summary>
        /// <value> The default horizontal alignment. </value>
        public HorizontalAlignment DefaultHorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)this.GetValue(DefaultHorizontalAlignmentProperty);
            }

            set
            {
                this.SetValue(DefaultHorizontalAlignmentProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the default height of the row.
        /// </summary>
        /// <value> The default height of the row. </value>
        public GridLength DefaultRowHeight
        {
            get
            {
                return (GridLength)this.GetValue(DefaultRowHeightProperty);
            }

            set
            {
                this.SetValue(DefaultRowHeightProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether easy insert (press enter/down/right to add new rows/columns) is enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [easy insert]; otherwise, <c>false</c>.
        /// </value>
        public bool EasyInsert
        {
            get
            {
                return (bool)this.GetValue(EasyInsertProperty);
            }

            set
            {
                this.SetValue(EasyInsertProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the grid line brush.
        /// </summary>
        /// <value> The grid line brush. </value>
        public Brush GridLineBrush
        {
            get
            {
                return (Brush)this.GetValue(GridLineBrushProperty);
            }

            set
            {
                this.SetValue(GridLineBrushProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the header border brush.
        /// </summary>
        /// <value> The header border brush. </value>
        public Brush HeaderBorderBrush
        {
            get
            {
                return (Brush)this.GetValue(HeaderBorderBrushProperty);
            }

            set
            {
                this.SetValue(HeaderBorderBrushProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the input direction (the moving direction of the current cell when Enter is pressed).
        /// </summary>
        /// <value>The input direction.</value>
        public InputDirection InputDirection
        {
            get
            {
                return (InputDirection)this.GetValue(InputDirectionProperty);
            }

            set
            {
                this.SetValue(InputDirectionProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets ItemHeaderPropertyPath.
        /// </summary>
        public string ItemHeaderPropertyPath
        {
            get
            {
                return (string)this.GetValue(ItemHeaderPropertyPathProperty);
            }

            set
            {
                this.SetValue(ItemHeaderPropertyPathProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the items source.
        /// </summary>
        /// <value> The items source. </value>
        public IList ItemsSource
        {
            get
            {
                return (IList)this.GetValue(ItemsSourceProperty);
            }

            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        ///     Gets the row definitions.
        /// </summary>
        /// <value> The row definitions. </value>
        public Collection<PropertyDefinition> RowDefinitions
        {
            get
            {
                return this.propertyDefinitions;
            }
        }

        /// <summary>
        ///     Gets or sets the width of the row headers.
        /// </summary>
        /// <value> The width of the row header. </value>
        public GridLength RowHeaderWidth
        {
            get
            {
                return (GridLength)this.GetValue(RowHeaderWidthProperty);
            }

            set
            {
                this.SetValue(RowHeaderWidthProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the rows context menu.
        /// </summary>
        /// <value> The rows context menu. </value>
        public ContextMenu RowsContextMenu
        {
            get
            {
                return (ContextMenu)this.GetValue(RowsContextMenuProperty);
            }

            set
            {
                this.SetValue(RowsContextMenuProperty, value);
            }
        }

        /// <summary>
        ///     Gets the selected cells.
        /// </summary>
        /// <value> The selected cells. </value>
        public IEnumerable<CellRef> SelectedCells
        {
            get
            {
                int rowMin = Math.Min(this.CurrentCell.Row, this.SelectionCell.Row);
                int columnMin = Math.Min(this.CurrentCell.Column, this.SelectionCell.Column);
                int rowMax = Math.Max(this.CurrentCell.Row, this.SelectionCell.Row);
                int columnMax = Math.Max(this.CurrentCell.Column, this.SelectionCell.Column);

                for (int i = rowMin; i <= rowMax; i++)
                {
                    for (int j = columnMin; j <= columnMax; j++)
                    {
                        yield return new CellRef(i, j);
                    }
                }
            }
        }

        /// <summary>
        ///     Gets or sets the selected items.
        /// </summary>
        /// <value> The selected items. </value>
        public IEnumerable SelectedItems
        {
            get
            {
                return (IEnumerable)this.GetValue(SelectedItemsProperty);
            }

            set
            {
                this.SetValue(SelectedItemsProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the cell defining the selection area. The selection area is defined by the CurrentCell and SelectionCell.
        /// </summary>
        public CellRef SelectionCell
        {
            get
            {
                return (CellRef)this.GetValue(SelectionCellProperty);
            }

            set
            {
                this.SetValue(SelectionCellProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to wrap items in the defined columns.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [wrap items]; otherwise, <c>false</c> .
        /// </value>
        public bool WrapItems
        {
            get
            {
                return (bool)this.GetValue(WrapItemsProperty);
            }

            set
            {
                this.SetValue(WrapItemsProperty, value);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance can delete columns.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can delete columns; otherwise, <c>false</c> .
        /// </value>
        protected virtual bool CanDeleteColumns
        {
            get
            {
                var list = this.ItemsSource;
                return this.CanDelete && this.ItemsInColumns && list != null && !list.IsFixedSize;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance can delete rows.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can delete rows; otherwise, <c>false</c> .
        /// </value>
        protected virtual bool CanDeleteRows
        {
            get
            {
                var list = this.ItemsSource;
                return this.CanDelete && this.ItemsInRows && list != null && !list.IsFixedSize;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance can insert columns.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can insert columns; otherwise, <c>false</c> .
        /// </value>
        protected virtual bool CanInsertColumns
        {
            get
            {
                var list = this.ItemsSource;
                return this.ItemsInColumns && this.CanInsert && list != null && !list.IsFixedSize;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance can insert rows.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can insert rows; otherwise, <c>false</c> .
        /// </value>
        private bool CanInsertRows
        {
            get
            {
                var list = this.ItemsSource;
                return this.ItemsInRows && this.CanInsert && list != null && !list.IsFixedSize;
            }
        }

        /// <summary>
        ///     Gets or sets the number of columns.
        /// </summary>
        /// <value> The columns. </value>
        private int Columns { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to use columns for the items.
        /// </summary>
        private bool ItemsInColumns { get; set; }

        /// <summary>
        ///     Gets a value indicating whether to use rows for the items.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the items are in rows; otherwise, <c>false</c> .
        /// </value>
        private bool ItemsInRows
        {
            get
            {
                return !this.ItemsInColumns;
            }
        }

        /// <summary>
        ///     Gets the column definitions.
        /// </summary>
        /// <value> The column definitions. </value>
        private Collection<PropertyDefinition> PropertyDefinitions
        {
            get
            {
                return this.propertyDefinitions;
            }
        }

        /// <summary>
        ///     Gets or sets the number of rows.
        /// </summary>
        /// <value> The rows. </value>
        private int Rows { get; set; }

        /// <summary>
        ///     Coerces the current cell.
        /// </summary>
        /// <param name="d">
        ///     The d.
        /// </param>
        /// <param name="basevalue">
        ///     The base value.
        /// </param>
        /// <returns>
        ///     The coerced current cell.
        /// </returns>
        private static object CoerceCurrentCell(DependencyObject d, object basevalue)
        {
            var cr = (CellRef)basevalue;
            int row = cr.Row;
            int column = cr.Column;
            var sg = (ItemsGrid)d;
            if (sg.AutoInsert)
            {
                row = Clamp(row, 0, sg.Rows - 1 + (sg.CanInsertRows ? 1 : 0));
                column = Clamp(column, 0, sg.Columns - 1 + (sg.CanInsertColumns ? 1 : 0));
            }
            else
            {
                row = Clamp(row, 0, sg.Rows - 1);
                column = Clamp(column, 0, sg.Columns - 1);
            }

            return new CellRef(row, column);
        }

        /// <summary>
        ///     Coerces the selection cell.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="basevalue">
        ///     The base value.
        /// </param>
        /// <returns>
        ///     The coerced selection cell.
        /// </returns>
        private static object CoerceSelectionCell(DependencyObject sender, object basevalue)
        {
            var cr = (CellRef)basevalue;
            int row = cr.Row;
            int column = cr.Column;
            var sg = (ItemsGrid)sender;
            row = Clamp(row, 0, sg.Rows - 1);
            column = Clamp(column, 0, sg.Columns - 1);
            return new CellRef(row, column);
        }

        /// <summary>
        ///     The current cell changed.
        /// </summary>
        private void CurrentCellChanged()
        {
            this.SelectedCellsChanged();

            this.ScrollIntoView(this.CurrentCell);

            if (this.AutoInsert && (this.CurrentCell.Row >= this.Rows || this.CurrentCell.Column >= this.Columns))
            {
                this.InsertItem(-1);
            }
        }

        /// <summary>
        ///     The selected cells changed, update the UI elements showing the selection.
        /// </summary>
        private void SelectedCellsChanged()
        {
            if (this.selection == null)
            {
                return;
            }

            int row = Math.Min(this.CurrentCell.Row, this.SelectionCell.Row);
            int column = Math.Min(this.CurrentCell.Column, this.SelectionCell.Column);
            int rowspan = Math.Abs(this.CurrentCell.Row - this.SelectionCell.Row) + 1;
            int columnspan = Math.Abs(this.CurrentCell.Column - this.SelectionCell.Column) + 1;

            Grid.SetRow(this.selection, row);
            Grid.SetColumn(this.selection, column);
            Grid.SetColumnSpan(this.selection, columnspan);
            Grid.SetRowSpan(this.selection, rowspan);

            Grid.SetRow(this.selectionBackground, row);
            Grid.SetColumn(this.selectionBackground, column);
            Grid.SetColumnSpan(this.selectionBackground, columnspan);
            Grid.SetRowSpan(this.selectionBackground, rowspan);

            Grid.SetColumn(this.columnSelectionBackground, column);
            Grid.SetColumnSpan(this.columnSelectionBackground, columnspan);
            Grid.SetRow(this.rowSelectionBackground, row);
            Grid.SetRowSpan(this.rowSelectionBackground, rowspan);

            Grid.SetRow(this.currentBackground, this.CurrentCell.Row);
            Grid.SetColumn(this.currentBackground, this.CurrentCell.Column);

            this.UpdateSelectionVisibility();

            Grid.SetColumn(this.autoFillBox, column + columnspan - 1);
            Grid.SetRow(this.autoFillBox, row + rowspan - 1);

            bool allSelected = rowspan == this.Rows && columnspan == this.Columns;
            this.topleft.Background = allSelected ? this.rowSelectionBackground.Background : this.rowGrid.Background;

            int r = Math.Min(this.CurrentCell.Row, this.AutoFillCell.Row);
            int c = Math.Min(this.CurrentCell.Column, this.AutoFillCell.Column);
            int rs = Math.Abs(this.CurrentCell.Row - this.AutoFillCell.Row) + 1;
            int cs = Math.Abs(this.CurrentCell.Column - this.AutoFillCell.Column) + 1;

            Grid.SetColumn(this.autoFillSelection, c);
            Grid.SetRow(this.autoFillSelection, r);
            Grid.SetColumnSpan(this.autoFillSelection, cs);
            Grid.SetRowSpan(this.autoFillSelection, rs);

            this.SelectedItems = this.EnumerateItems(this.CurrentCell, this.SelectionCell);

            if (!this.ShowEditControl())
            {
                this.HideEditor();
            }
        }

        /// <summary>
        /// Updates the selection visibility.
        /// </summary>
        private void UpdateSelectionVisibility()
        {
            this.currentBackground.Visibility = this.CurrentCell.Row < this.Rows && this.CurrentCell.Column < this.Columns
                                                    ? Visibility.Visible
                                                    : Visibility.Hidden;
            this.autoFillBox.Visibility = this.CurrentCell.Row < this.Rows && this.CurrentCell.Column < this.Columns
                                              ? Visibility.Visible
                                              : Visibility.Hidden;
            this.selectionBackground.Visibility = this.CurrentCell.Row < this.Rows && this.CurrentCell.Column < this.Columns
                                                      ? Visibility.Visible
                                                      : Visibility.Hidden;
            this.columnSelectionBackground.Visibility = this.CurrentCell.Column < this.Columns
                                                            ? Visibility.Visible
                                                            : Visibility.Hidden;
            this.rowSelectionBackground.Visibility = this.CurrentCell.Row < this.Rows ? Visibility.Visible : Visibility.Hidden;
            this.selection.Visibility = this.CurrentCell.Row < this.Rows && this.CurrentCell.Column < this.Columns
                                            ? Visibility.Visible
                                            : Visibility.Hidden;
            this.selection.Visibility = this.CurrentCell.Row < this.Rows && this.CurrentCell.Column < this.Columns
                                            ? Visibility.Visible
                                            : Visibility.Hidden;
        }

        /// <summary>
        ///     The selection cell changed.
        /// </summary>
        private void SelectionCellChanged()
        {
            this.SelectedCellsChanged();

            this.ScrollIntoView(this.SelectionCell);
        }
    }
}