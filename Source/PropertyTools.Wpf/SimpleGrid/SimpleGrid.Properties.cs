using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PropertyTools.Wpf
{
    public partial class SimpleGrid
    {
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(SimpleGrid),
                                        new UIPropertyMetadata(null, ContentChanged));

        public static readonly DependencyProperty CurrentCellProperty =
            DependencyProperty.Register("CurrentCell", typeof(CellRef), typeof(SimpleGrid),
                                        new FrameworkPropertyMetadata(new CellRef(0, 0),
                                                                      FrameworkPropertyMetadataOptions.
                                                                          BindsTwoWayByDefault, CurrentCellChanged,
                                                                      CoerceCurrentCell));

        public static readonly DependencyProperty SelectionCellProperty =
            DependencyProperty.Register("SelectionCell", typeof(CellRef), typeof(SimpleGrid),
                                        new UIPropertyMetadata(new CellRef(0, 0), SelectionCellChanged,
                                                               CoerceSelectionCell));

        //public static readonly DependencyProperty CurrentItemProperty =
        //    DependencyProperty.Register("CurrentItem", typeof(object), typeof(SimpleGrid),
        //                                new FrameworkPropertyMetadata(null,
        //                                                              FrameworkPropertyMetadataOptions.
        //                                                                  BindsTwoWayByDefault, CurrentItemChanged));

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(SimpleGrid),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty AllowExpandProperty =
            DependencyProperty.Register("AllowExpand", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(true));

        public static readonly DependencyProperty ColumnHeadersProperty =
            DependencyProperty.Register("ColumnHeaders", typeof(StringCollection), typeof(SimpleGrid),
                                        new UIPropertyMetadata(null, ContentChanged));

        public static readonly DependencyProperty FormatStringsProperty =
            DependencyProperty.Register("FormatStrings", typeof(StringCollection), typeof(SimpleGrid),
                                        new UIPropertyMetadata(null, ContentChanged));

        public static readonly DependencyProperty RowHeadersProperty =
            DependencyProperty.Register("RowHeaders", typeof(StringCollection), typeof(SimpleGrid),
                                        new UIPropertyMetadata(null, ContentChanged));

        public static readonly DependencyProperty DataFieldsProperty =
            DependencyProperty.Register("DataFields", typeof(StringCollection), typeof(SimpleGrid),
                                        new UIPropertyMetadata(null, ContentChanged));

        public static readonly DependencyProperty ItemsInColumnsProperty =
            DependencyProperty.Register("ItemsInColumns", typeof(bool), typeof(SimpleGrid),
                                        new UIPropertyMetadata(false, ContentChanged));

        public static readonly DependencyProperty RowHeaderWidthProperty =
            DependencyProperty.Register("RowHeaderWidth", typeof(GridLength), typeof(SimpleGrid),
                                        new UIPropertyMetadata(new GridLength(40)));

        public static readonly DependencyProperty HeaderBorderBrushProperty =
            DependencyProperty.Register("HeaderBorderBrush", typeof(Brush), typeof(SimpleGrid),
                                        new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(177, 181, 186))));

        public static readonly DependencyProperty GridLineBrushProperty =
            DependencyProperty.Register("GridLineBrush", typeof(Brush), typeof(SimpleGrid),
                                        new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(218, 220, 221))));

        public static readonly DependencyProperty ColumnAlignmentsProperty =
            DependencyProperty.Register("ColumnAlignments", typeof(Collection<HorizontalAlignment>),
                                        typeof(SimpleGrid), new UIPropertyMetadata(null, ContentChanged));

        public static readonly DependencyProperty ColumnWidthsProperty =
            DependencyProperty.Register("ColumnWidths", typeof(Collection<GridLength>), typeof(SimpleGrid),
                                        new UIPropertyMetadata(null, ContentChanged));

        public static readonly DependencyProperty AutoSizeColumnsProperty =
            DependencyProperty.Register("AutoSizeColumns", typeof(bool), typeof(SimpleGrid),
                                        new UIPropertyMetadata(false));

        public static readonly DependencyProperty CanInsertProperty =
            DependencyProperty.Register("CanInsert", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(true));

        public static readonly DependencyProperty CanDeleteProperty =
            DependencyProperty.Register("CanDelete", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(true));

        public static readonly DependencyProperty AlternatingRowsBackgroundProperty =
            DependencyProperty.Register("AlternatingRowsBackground", typeof(Brush), typeof(SimpleGrid),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty DefaultColumnWidthProperty =
            DependencyProperty.Register("DefaultColumnWidth", typeof(GridLength), typeof(SimpleGrid),
                                        new UIPropertyMetadata(new GridLength(100)));

        public HorizontalAlignment DefaultHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(DefaultHorizontalAlignmentProperty); }
            set { SetValue(DefaultHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty DefaultHorizontalAlignmentProperty =
            DependencyProperty.Register("DefaultHorizontalAlignment", typeof(HorizontalAlignment), typeof(SimpleGrid), new UIPropertyMetadata(System.Windows.HorizontalAlignment.Center));

        public static readonly DependencyProperty DefaultRowHeightProperty =
            DependencyProperty.Register("DefaultRowHeight", typeof(GridLength), typeof(SimpleGrid),
                                        new UIPropertyMetadata(new GridLength(20)));

        public static readonly DependencyProperty CanResizeColumnsProperty =
            DependencyProperty.Register("CanResizeColumns", typeof(bool), typeof(SimpleGrid),
                                        new UIPropertyMetadata(true));

        public static readonly DependencyProperty AddItemHeaderProperty =
            DependencyProperty.Register("AddItemHeader", typeof(string), typeof(SimpleGrid),
                                        new UIPropertyMetadata("*"));

        //public object CurrentItem
        //{
        //    get { return GetValue(CurrentItemProperty); }
        //    set { SetValue(CurrentItemProperty, value); }
        //}

        public IEnumerable SelectedItems
        {
            get { return (IEnumerable)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public bool AllowExpand
        {
            get { return (bool)GetValue(AllowExpandProperty); }
            set { SetValue(AllowExpandProperty, value); }
        }

        public Brush HeaderBorderBrush
        {
            get { return (Brush)GetValue(HeaderBorderBrushProperty); }
            set { SetValue(HeaderBorderBrushProperty, value); }
        }

        public Brush GridLineBrush
        {
            get { return (Brush)GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }


        [TypeConverter(typeof(ColumnAlignmentCollectionConverter))]
        public Collection<HorizontalAlignment> ColumnAlignments
        {
            get { return (Collection<HorizontalAlignment>)GetValue(ColumnAlignmentsProperty); }
            set { SetValue(ColumnAlignmentsProperty, value); }
        }

        public GridLength RowHeaderWidth
        {
            get { return (GridLength)GetValue(RowHeaderWidthProperty); }
            set { SetValue(RowHeaderWidthProperty, value); }
        }

        public GridLength DefaultColumnWidth
        {
            get { return (GridLength)GetValue(DefaultColumnWidthProperty); }
            set { SetValue(DefaultColumnWidthProperty, value); }
        }

        public Brush AlternatingRowsBackground
        {
            get { return (Brush)GetValue(AlternatingRowsBackgroundProperty); }
            set { SetValue(AlternatingRowsBackgroundProperty, value); }
        }

        public CellRef AutoFillCell
        {
            get { return autoFillCell; }
            set
            {
                autoFillCell = (CellRef)CoerceSelectionCell(this, value);
                OnSelectedCellsChanged();
            }
        }
       
        [TypeConverter(typeof(GridLengthCollectionConverter))]
        public Collection<GridLength> ColumnWidths
        {
            get { return (Collection<GridLength>)GetValue(ColumnWidthsProperty); }
            set { SetValue(ColumnWidthsProperty, value); }
        }

        [TypeConverter(typeof(StringCollectionConverter))]
        public StringCollection ColumnHeaders
        {
            get { return (StringCollection)GetValue(ColumnHeadersProperty); }
            set { SetValue(ColumnHeadersProperty, value); }
        }

        [TypeConverter(typeof(StringCollectionConverter))]
        public StringCollection FormatStrings
        {
            get { return (StringCollection)GetValue(FormatStringsProperty); }
            set { SetValue(FormatStringsProperty, value); }
        }

        [TypeConverter(typeof(StringCollectionConverter))]
        public StringCollection RowHeaders
        {
            get { return (StringCollection)GetValue(RowHeadersProperty); }
            set { SetValue(RowHeadersProperty, value); }
        }

        public bool ItemsInColumns
        {
            get { return (bool)GetValue(ItemsInColumnsProperty); }
            set { SetValue(ItemsInColumnsProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the content of the grid.
        /// </summary>
        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the current cell.
        /// </summary>
        public CellRef CurrentCell
        {
            get { return (CellRef)GetValue(CurrentCellProperty); }
            set { SetValue(CurrentCellProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the cell defining the selection area.
        ///   The selection area is defined by the CurrentCell and SelectionCell.
        /// </summary>
        public CellRef SelectionCell
        {
            get { return (CellRef)GetValue(SelectionCellProperty); }
            set { SetValue(SelectionCellProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public int Rows { get; set; }

        /// <summary>
        ///   Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public int Columns { get; set; }

        [TypeConverter(typeof(StringCollectionConverter))]
        public StringCollection DataFields
        {
            get { return (StringCollection)GetValue(DataFieldsProperty); }
            set { SetValue(DataFieldsProperty, value); }
        }

        public IEnumerable<CellRef> SelectedCells
        {
            get
            {
                int rowMin = Math.Min(CurrentCell.Row, SelectionCell.Row);
                int columnMin = Math.Min(CurrentCell.Column, SelectionCell.Column);
                int rowMax = Math.Max(CurrentCell.Row, SelectionCell.Row);
                int columnMax = Math.Max(CurrentCell.Column, SelectionCell.Column);

                for (int i = rowMin; i <= rowMax; i++)
                {
                    for (int j = columnMin; j <= columnMax; j++)
                    {
                        yield return new CellRef(i, j);
                    }
                }
            }
        }

        public bool AutoSizeColumns
        {
            get { return (bool)GetValue(AutoSizeColumnsProperty); }
            set { SetValue(AutoSizeColumnsProperty, value); }
        }

        public bool CanInsert
        {
            get { return (bool)GetValue(CanInsertProperty); }
            set { SetValue(CanInsertProperty, value); }
        }

        public bool CanDelete
        {
            get { return (bool)GetValue(CanDeleteProperty); }
            set { SetValue(CanDeleteProperty, value); }
        }

        protected virtual bool CanInsertRows
        {
            get { return !ItemsInColumns && CanInsert && Content is IList && !(Content is Array); }
        }

        protected virtual bool CanDeleteRows
        {
            get { return CanDelete && !ItemsInColumns && Content is IList && !(Content is Array); }
        }

        protected virtual bool CanInsertColumns
        {
            get { return ItemsInColumns && CanInsert && Content is IList && !(Content is Array); }
        }

        protected virtual bool CanDeleteColumns
        {
            get { return CanDelete && ItemsInColumns && Content is IList && !(Content is Array); }
        }

        public GridLength DefaultRowHeight
        {
            get { return (GridLength)GetValue(DefaultRowHeightProperty); }
            set { SetValue(DefaultRowHeightProperty, value); }
        }

        protected StringCollection ActualDataFields { get; set; }
        protected StringCollection ActualColumnHeaders { get; set; }

        public bool CanResizeColumns
        {
            get { return (bool)GetValue(CanResizeColumnsProperty); }
            set { SetValue(CanResizeColumnsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header used for the add item row/column. Default is "*".
        /// </summary>
        /// <value>The add item header.</value>
        public string AddItemHeader
        {
            get { return (string)GetValue(AddItemHeaderProperty); }
            set { SetValue(AddItemHeaderProperty, value); }
        }

        [Browsable(false)]
        public Collection<TypeDefinition> TypeDefinitions
        {
            get { return typeDefinitions; }
        }

        private readonly Collection<TypeDefinition> typeDefinitions = new Collection<TypeDefinition>();


        //private static void CurrentItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((SimpleGrid)d).OnCurrentItemChanged(e);
        //}

        //protected virtual void OnCurrentItemChanged(DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        //{
        //    int index = GetIndexOfItem(CurrentItem);
        //    if (index < 0)
        //    {
        //        return;
        //    }

        //    if (ItemsInColumns)
        //    {
        //        if (index < Rows)
        //        {
        //            CurrentCell = new CellRef(index, CurrentCell.Column);
        //        }
        //    }
        //    else
        //    {
        //        if (index < Columns)
        //        {
        //            CurrentCell = new CellRef(CurrentCell.Row, index);
        //        }
        //    }
        //}

        private static object CoerceCurrentCell(DependencyObject d, object basevalue)
        {
            var cr = (CellRef)basevalue;
            int row = cr.Row;
            int column = cr.Column;
            var sg = (SimpleGrid)d;
            row = Clamp(row, 0, sg.Rows - 1);
            column = Clamp(column, 0, sg.Columns - 1);

            // row = Clamp(row, 0, sg.Rows - 1 + (sg.CanInsertRows ? 1 : 0));
            // column = Clamp(column, 0, sg.Columns - 1 + (sg.CanInsertColumns ? 1 : 0));
            return new CellRef(row, column);
        }

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


        private static void ContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleGrid)d).OnCellsChanged();
        }

        private void OnCellsChanged()
        {
            UpdateGridContent();
        }

        private static void CurrentCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleGrid)d).OnCurrentCellChanged();
        }

        private static void SelectionCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleGrid)d).OnSelectionCellChanged();
        }

        private void OnCurrentCellChanged()
        {
            OnSelectedCellsChanged();
           // CurrentItem = GetItem(CurrentCell);

            this.ScrollIntoView(CurrentCell);

            // if (CurrentCell.Row >= Rows || CurrentCell.Column >= Columns)
            // InsertItem(-1);
        }

        private void OnSelectionCellChanged()
        {
            OnSelectedCellsChanged();
            this.ScrollIntoView(SelectionCell);
        }

        private void OnSelectedCellsChanged()
        {
            if (selection == null)
            {
                return;
            }
            {
                int row = Math.Min(CurrentCell.Row, SelectionCell.Row);
                int column = Math.Min(CurrentCell.Column, SelectionCell.Column);
                int rowspan = Math.Abs(CurrentCell.Row - SelectionCell.Row) + 1;
                int columnspan = Math.Abs(CurrentCell.Column - SelectionCell.Column) + 1;

                Grid.SetRow(selection, row);
                Grid.SetColumn(selection, column);
                Grid.SetColumnSpan(selection, columnspan);
                Grid.SetRowSpan(selection, rowspan);

                Grid.SetRow(selectionBackground, row);
                Grid.SetColumn(selectionBackground, column);
                Grid.SetColumnSpan(selectionBackground, columnspan);
                Grid.SetRowSpan(selectionBackground, rowspan);

                Grid.SetColumn(columnSelectionBackground, column);
                Grid.SetColumnSpan(columnSelectionBackground, columnspan);
                Grid.SetRow(rowSelectionBackground, row);
                Grid.SetRowSpan(rowSelectionBackground, rowspan);

                Grid.SetRow(currentBackground, CurrentCell.Row);
                Grid.SetColumn(currentBackground, CurrentCell.Column);

                Grid.SetColumn(autoFillBox, column + columnspan - 1);
                Grid.SetRow(autoFillBox, row + rowspan - 1);

                bool allSelected = rowspan == Rows && columnspan == Columns;
                topleft.Background = allSelected ? rowSelectionBackground.Background : rowGrid.Background;
            }
            {
                int row = Math.Min(CurrentCell.Row, AutoFillCell.Row);
                int column = Math.Min(CurrentCell.Column, AutoFillCell.Column);
                int rowspan = Math.Abs(CurrentCell.Row - AutoFillCell.Row) + 1;
                int columnspan = Math.Abs(CurrentCell.Column - AutoFillCell.Column) + 1;

                Grid.SetColumn(autoFillSelection, column);
                Grid.SetRow(autoFillSelection, row);
                Grid.SetColumnSpan(autoFillSelection, columnspan);
                Grid.SetRowSpan(autoFillSelection, rowspan);
            }
            
            // Debug.WriteLine("OnSelectedCellsChanged\n"+Environment.StackTrace+"\n");

            SelectedItems = EnumerateItems(CurrentCell, SelectionCell);

            if (!ShowEditor())
            {
                HideEditor();
            }

        }
        
        private Collection<ColumnDefinition> columnDefinitions = new Collection<ColumnDefinition>();

        public Collection<ColumnDefinition> ColumnDefinitions
        {
            get { return columnDefinitions; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this control is using UI virtualizing.
        /// When true, only the UIElements of the visible cells will be created.
        /// </summary>
        public bool IsVirtualizing
        {
            get { return (bool)GetValue(IsVirtualizingProperty); }
            set { SetValue(IsVirtualizingProperty, value); }
        }

        public static readonly DependencyProperty IsVirtualizingProperty =
            DependencyProperty.Register("IsVirtualizing", typeof(bool), typeof(SimpleGrid), new UIPropertyMetadata(false));
    }
}