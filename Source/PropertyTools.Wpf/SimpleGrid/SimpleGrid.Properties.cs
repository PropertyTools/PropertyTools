// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleGrid.Properties.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// The simple grid.
    /// </summary>
    public partial class SimpleGrid
    {
        #region Constants and Fields

        /// <summary>
        /// The add item header property.
        /// </summary>
        public static readonly DependencyProperty AddItemHeaderProperty = DependencyProperty.Register(
            "AddItemHeader", typeof(string), typeof(SimpleGrid), new UIPropertyMetadata("*"));

        /// <summary>
        /// The alternating rows background property.
        /// </summary>
        public static readonly DependencyProperty AlternatingRowsBackgroundProperty =
            DependencyProperty.Register(
                "AlternatingRowsBackground", typeof(Brush), typeof(SimpleGrid), new UIPropertyMetadata(null));

        /// <summary>
        /// The auto generate columns property.
        /// </summary>
        public static readonly DependencyProperty AutoGenerateColumnsProperty =
            DependencyProperty.Register(
                "AutoGenerateColumns", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(true));

        /// <summary>
        /// The auto size columns property.
        /// </summary>
        public static readonly DependencyProperty AutoSizeColumnsProperty =
            DependencyProperty.Register(
                "AutoSizeColumns", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(false));

        /// <summary>
        /// The can delete property.
        /// </summary>
        public static readonly DependencyProperty CanDeleteProperty = DependencyProperty.Register(
            "CanDelete", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(true));

        /// <summary>
        /// The can insert property.
        /// </summary>
        public static readonly DependencyProperty CanInsertProperty = DependencyProperty.Register(
            "CanInsert", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(true));

        /// <summary>
        /// The can resize columns property.
        /// </summary>
        public static readonly DependencyProperty CanResizeColumnsProperty =
            DependencyProperty.Register(
                "CanResizeColumns", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(true));

        /// <summary>
        /// The content property.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(SimpleGrid), new UIPropertyMetadata(null, ContentChanged));

        /// <summary>
        /// The current cell property.
        /// </summary>
        public static readonly DependencyProperty CurrentCellProperty = DependencyProperty.Register(
            "CurrentCell", 
            typeof(CellRef), 
            typeof(SimpleGrid), 
            new FrameworkPropertyMetadata(
                new CellRef(0, 0), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                CurrentCellChanged, 
                CoerceCurrentCell));

        /// <summary>
        /// The default column width property.
        /// </summary>
        public static readonly DependencyProperty DefaultColumnWidthProperty =
            DependencyProperty.Register(
                "DefaultColumnWidth", 
                typeof(GridLength), 
                typeof(SimpleGrid), 
                new UIPropertyMetadata(new GridLength(100)));

        /// <summary>
        /// The default horizontal alignment property.
        /// </summary>
        public static readonly DependencyProperty DefaultHorizontalAlignmentProperty =
            DependencyProperty.Register(
                "DefaultHorizontalAlignment", 
                typeof(HorizontalAlignment), 
                typeof(SimpleGrid), 
                new UIPropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// The default row height property.
        /// </summary>
        public static readonly DependencyProperty DefaultRowHeightProperty =
            DependencyProperty.Register(
                "DefaultRowHeight", typeof(GridLength), typeof(SimpleGrid), new UIPropertyMetadata(new GridLength(20)));

        /// <summary>
        /// The easy insert property.
        /// </summary>
        public static readonly DependencyProperty EasyInsertProperty = DependencyProperty.Register(
            "EasyInsert", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(true));

        /// <summary>
        /// The grid line brush property.
        /// </summary>
        public static readonly DependencyProperty GridLineBrushProperty = DependencyProperty.Register(
            "GridLineBrush", 
            typeof(Brush), 
            typeof(SimpleGrid), 
            new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(218, 220, 221))));

        /// <summary>
        /// The header border brush property.
        /// </summary>
        public static readonly DependencyProperty HeaderBorderBrushProperty =
            DependencyProperty.Register(
                "HeaderBorderBrush", 
                typeof(Brush), 
                typeof(SimpleGrid), 
                new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(177, 181, 186))));

        /// <summary>
        /// The is virtualizing property.
        /// </summary>
        public static readonly DependencyProperty IsVirtualizingProperty = DependencyProperty.Register(
            "IsVirtualizing", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(false));

        /// <summary>
        /// The items in columns property.
        /// </summary>
        public static readonly DependencyProperty ItemsInColumnsProperty = DependencyProperty.Register(
            "ItemsInColumns", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(false, ContentChanged));

        /// <summary>
        /// The row header width property.
        /// </summary>
        public static readonly DependencyProperty RowHeaderWidthProperty = DependencyProperty.Register(
            "RowHeaderWidth", typeof(GridLength), typeof(SimpleGrid), new UIPropertyMetadata(new GridLength(40)));

        /// <summary>
        /// The row headers property.
        /// </summary>
        public static readonly DependencyProperty RowHeadersProperty = DependencyProperty.Register(
            "RowHeaders", typeof(List<string>), typeof(SimpleGrid), new UIPropertyMetadata(null, ContentChanged));

        /// <summary>
        /// The selected items property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            "SelectedItems", typeof(IEnumerable), typeof(SimpleGrid), new UIPropertyMetadata(null));

        /// <summary>
        /// The selection cell property.
        /// </summary>
        public static readonly DependencyProperty SelectionCellProperty = DependencyProperty.Register(
            "SelectionCell", 
            typeof(CellRef), 
            typeof(SimpleGrid), 
            new UIPropertyMetadata(new CellRef(0, 0), SelectionCellChanged, CoerceSelectionCell));

        /// <summary>
        /// The type definitions.
        /// </summary>
        private readonly Collection<TypeDefinition> typeDefinitions = new Collection<TypeDefinition>();

        /// <summary>
        /// The column definitions.
        /// </summary>
        private Collection<ColumnDefinition> columnDefinitions = new Collection<ColumnDefinition>();

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the header used for the add item row/column. Default is "*".
        /// </summary>
        /// <value>The add item header.</value>
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
        ///   Gets or sets the alternating rows background brush.
        /// </summary>
        /// <value>The alternating rows background.</value>
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
        ///   Gets or sets the auto fill cell.
        /// </summary>
        /// <value>The auto fill cell.</value>
        public CellRef AutoFillCell
        {
            get
            {
                return this.autoFillCell;
            }

            set
            {
                this.autoFillCell = (CellRef)CoerceSelectionCell(this, value);
                this.OnSelectedCellsChanged();
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to auto generate columns.
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
        ///   Gets or sets a value indicating whether auto size columns is enabled.
        /// </summary>
        /// <value><c>true</c> if [auto size columns]; otherwise, <c>false</c>.</value>
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
        ///   Gets or sets a value indicating whether this instance can delete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can delete; otherwise, <c>false</c>.
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
        ///   Gets or sets a value indicating whether this instance can insert.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can insert; otherwise, <c>false</c>.
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
        ///   Gets or sets a value indicating whether this instance can resize columns.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can resize columns; otherwise, <c>false</c>.
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
        ///   Sets the column alignments.
        /// </summary>
        /// <value>The column alignments.</value>
        [TypeConverter(typeof(ColumnAlignmentListConverter))]
        public List<HorizontalAlignment> ColumnAlignments
        {
            set
            {
                this.SetColumnDefinitions(value, (v, d) => d.HorizontalAlignment = v);
            }
        }

        /// <summary>
        ///   Gets the column definitions.
        /// </summary>
        /// <value>The column definitions.</value>
        public Collection<ColumnDefinition> ColumnDefinitions
        {
            get
            {
                return this.columnDefinitions;
            }
        }

        /// <summary>
        ///   Gets or sets the column headers.
        /// </summary>
        /// <value>The column headers.</value>
        [TypeConverter(typeof(StringListConverter))]
        public List<string> ColumnHeaders
        {
            set
            {
                this.SetColumnDefinitions(value, (v, d) => d.Header = v);
            }
        }

        /// <summary>
        ///   Gets or sets the column widths.
        /// </summary>
        /// <value>The column widths.</value>
        [TypeConverter(typeof(GridLengthListConverter))]
        public List<GridLength> ColumnWidths
        {
            set
            {
                this.SetColumnDefinitions(value, (v, d) => d.Width = v);
            }
        }

        /// <summary>
        ///   Gets or sets the content of the grid.
        /// </summary>
        public object Content
        {
            get
            {
                return this.GetValue(ContentProperty);
            }

            set
            {
                this.SetValue(ContentProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the current cell.
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
        /// Sets DataFields.
        /// </summary>
        [TypeConverter(typeof(StringListConverter))]
        public IList<string> DataFields
        {
            set
            {
                this.SetColumnDefinitions(value, (v, d) => d.DataField = v);
            }
        }

        /// <summary>
        ///   Gets or sets the default column width.
        /// </summary>
        /// <value>The default width of the column.</value>
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
        ///   Gets or sets the default horizontal alignment.
        /// </summary>
        /// <value>The default horizontal alignment.</value>
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
        ///   Gets or sets the default height of the row.
        /// </summary>
        /// <value>The default height of the row.</value>
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
        ///   Gets or sets a value indicating whether to allow easy insert mode.
        /// </summary>
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
        /// Sets FormatStrings.
        /// </summary>
        [TypeConverter(typeof(StringListConverter))]
        public IList<string> FormatStrings
        {
            set
            {
                this.SetColumnDefinitions(value, (v, d) => d.FormatString = v);
            }
        }

        /// <summary>
        ///   Gets or sets the grid line brush.
        /// </summary>
        /// <value>The grid line brush.</value>
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
        ///   Gets or sets the header border brush.
        /// </summary>
        /// <value>The header border brush.</value>
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
        ///   Gets or sets a value indicating whether this control is using UI virtualizing.
        ///   When true, only the UIElements of the visible cells will be created.
        /// </summary>
        public bool IsVirtualizing
        {
            get
            {
                return (bool)this.GetValue(IsVirtualizingProperty);
            }

            set
            {
                this.SetValue(IsVirtualizingProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to use columns for the items.
        /// </summary>
        public bool ItemsInColumns
        {
            get
            {
                return (bool)this.GetValue(ItemsInColumnsProperty);
            }

            set
            {
                this.SetValue(ItemsInColumnsProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the width of the row header.
        /// </summary>
        /// <value>The width of the row header.</value>
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
        ///   Gets or sets the row headers.
        /// </summary>
        /// <value>The row headers.</value>
        [TypeConverter(typeof(StringListConverter))]
        public List<string> RowHeaders
        {
            get
            {
                return (List<string>)this.GetValue(RowHeadersProperty);
            }

            set
            {
                this.SetValue(RowHeadersProperty, value);
            }
        }

        /// <summary>
        ///   Gets the selected cells.
        /// </summary>
        /// <value>The selected cells.</value>
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
        ///   Gets or sets the selected items.
        /// </summary>
        /// <value>The selected items.</value>
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
        ///   Gets or sets the cell defining the selection area.
        ///   The selection area is defined by the CurrentCell and SelectionCell.
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
        ///   Gets the type definitions.
        /// </summary>
        /// <value>The type definitions.</value>
        [Browsable(false)]
        public Collection<TypeDefinition> TypeDefinitions
        {
            get
            {
                return this.typeDefinitions;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets a value indicating whether this instance can delete columns.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can delete columns; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool CanDeleteColumns
        {
            get
            {
                return this.CanDelete && this.ItemsInColumns && this.Content is IList && !(this.Content is Array);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can delete rows.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can delete rows; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool CanDeleteRows
        {
            get
            {
                return this.CanDelete && !this.ItemsInColumns && this.Content is IList && !(this.Content is Array);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can insert columns.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can insert columns; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool CanInsertColumns
        {
            get
            {
                return this.ItemsInColumns && this.CanInsert && this.Content is IList && !(this.Content is Array);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can insert rows.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can insert rows; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool CanInsertRows
        {
            get
            {
                return !this.ItemsInColumns && this.CanInsert && this.Content is IList && !(this.Content is Array);
            }
        }

        /// <summary>
        ///   Gets or sets the number of columns.
        /// </summary>
        /// <value>The columns.</value>
        private int Columns { get; set; }

        /// <summary>
        ///   Gets or sets the number of rows.
        /// </summary>
        /// <value>The rows.</value>
        private int Rows { get; set; }

        #endregion

        // private static void CurrentItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        // {
        // ((SimpleGrid)d).OnCurrentItemChanged(e);
        // }

        // protected virtual void OnCurrentItemChanged(DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        // {
        // int index = GetIndexOfItem(CurrentItem);
        // if (index < 0)
        // {
        // return;
        // }

        // if (ItemsInColumns)
        // {
        // if (index < Rows)
        // {
        // CurrentCell = new CellRef(index, CurrentCell.Column);
        // }
        // }
        // else
        // {
        // if (index < Columns)
        // {
        // CurrentCell = new CellRef(CurrentCell.Row, index);
        // }
        // }
        // }
        #region Methods

        /// <summary>
        /// The coerce current cell.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="basevalue">
        /// The basevalue.
        /// </param>
        /// <returns>
        /// The coerce current cell.
        /// </returns>
        private static object CoerceCurrentCell(DependencyObject d, object basevalue)
        {
            var cr = (CellRef)basevalue;
            int row = cr.Row;
            int column = cr.Column;
            var sg = (SimpleGrid)d;
            if (sg.EasyInsert)
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
        /// The coerce selection cell.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="basevalue">
        /// The basevalue.
        /// </param>
        /// <returns>
        /// The coerce selection cell.
        /// </returns>
        private static object CoerceSelectionCell(DependencyObject d, object basevalue)
        {
            var cr = (CellRef)basevalue;
            int row = cr.Row;
            int column = cr.Column;
            var sg = (SimpleGrid)d;
            row = Clamp(row, 0, sg.Rows - 1);
            column = Clamp(column, 0, sg.Columns - 1);
            return new CellRef(row, column);
        }

        /// <summary>
        /// The content changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void ContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleGrid)d).UpdateGridContent();
        }

        /// <summary>
        /// The current cell changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void CurrentCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleGrid)d).OnCurrentCellChanged();
        }

        /// <summary>
        /// The selection cell changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void SelectionCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleGrid)d).OnSelectionCellChanged();
        }

        /// <summary>
        /// The on current cell changed.
        /// </summary>
        private void OnCurrentCellChanged()
        {
            this.OnSelectedCellsChanged();

            // CurrentItem = GetItem(CurrentCell);
            this.ScrollIntoView(this.CurrentCell);

            if (this.EasyInsert && (this.CurrentCell.Row >= this.Rows || this.CurrentCell.Column >= this.Columns))
            {
                this.InsertItem(-1);
            }
        }

        /// <summary>
        /// The on selected cells changed.
        /// </summary>
        private void OnSelectedCellsChanged()
        {
            if (this.selection == null)
            {
                return;
            }
            {
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

                Grid.SetColumn(this.autoFillBox, column + columnspan - 1);
                Grid.SetRow(this.autoFillBox, row + rowspan - 1);

                bool allSelected = rowspan == this.Rows && columnspan == this.Columns;
                this.topleft.Background = allSelected ? this.rowSelectionBackground.Background : this.rowGrid.Background;
            }
            {
                int row = Math.Min(this.CurrentCell.Row, this.AutoFillCell.Row);
                int column = Math.Min(this.CurrentCell.Column, this.AutoFillCell.Column);
                int rowspan = Math.Abs(this.CurrentCell.Row - this.AutoFillCell.Row) + 1;
                int columnspan = Math.Abs(this.CurrentCell.Column - this.AutoFillCell.Column) + 1;

                Grid.SetColumn(this.autoFillSelection, column);
                Grid.SetRow(this.autoFillSelection, row);
                Grid.SetColumnSpan(this.autoFillSelection, columnspan);
                Grid.SetRowSpan(this.autoFillSelection, rowspan);
            }

            // Debug.WriteLine("OnSelectedCellsChanged\n"+Environment.StackTrace+"\n");
            this.SelectedItems = this.EnumerateItems(this.CurrentCell, this.SelectionCell);

            if (!this.ShowEditor())
            {
                this.HideEditor();
            }
        }

        /// <summary>
        /// The on selection cell changed.
        /// </summary>
        private void OnSelectionCellChanged()
        {
            this.OnSelectedCellsChanged();
            this.ScrollIntoView(this.SelectionCell);
        }

        /// <summary>
        /// Sets the column definitions.
        /// </summary>
        /// <typeparam name="T">
        /// Definition type to change.
        /// </typeparam>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="setter">
        /// The setter.
        /// </param>
        private void SetColumnDefinitions<T>(IList<T> values, Action<T, ColumnDefinition> setter)
        {
            if (this.columnDefinitions == null)
            {
                this.columnDefinitions = new Collection<ColumnDefinition>();
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (this.columnDefinitions.Count <= i)
                {
                    this.columnDefinitions.Add(new ColumnDefinition());
                }

                setter(values[i], this.columnDefinitions[i]);
            }
        }

        #endregion
    }
}