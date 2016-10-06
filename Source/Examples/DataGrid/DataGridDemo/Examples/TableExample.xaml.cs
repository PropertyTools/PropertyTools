// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for TableExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Interaction logic for TableExample.
    /// </summary>
    public partial class TableExample
    {
        /// <summary>
        /// The shared table. This makes it possible to open two windows and verify that property changes are working!
        /// </summary>
        private static readonly Table<int, string, string> StaticTable;

        /// <summary>
        /// Initializes static members of the <see cref="TableExample" /> class.
        /// </summary>
        static TableExample()
        {
            StaticTable = new Table<int, string, string>(
               (i, j) => 0,
               i => "NR" + (i + 1));

            StaticTable.RowHeaders.Add("R1");
            StaticTable.RowHeaders.Add("R2");
            StaticTable.ColumnHeaders.Add("C1");
            StaticTable.ColumnHeaders.Add("C2");
            StaticTable[0, 0] = 11;
            StaticTable[0, 1] = 12;
            StaticTable[1, 1] = 22;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableExample" /> class.
        /// </summary>
        public TableExample()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.CreateColumnHeader = i => "NC" + (i + 1);
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>The table.</value>
        public Table<int, string, string> Table
        {
            get
            {
                return StaticTable;
            }
        }

        /// <summary>
        /// Gets the create column header function.
        /// </summary>
        /// <value>The create column header.</value>
        public Func<int, object> CreateColumnHeader { get; private set; }
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1215:InstanceReadonlyElementsMustAppearBeforeInstanceNonReadonlyElements", Justification = "Reviewed. Suppression is OK here.")]
    public class Table<T, TR, TC>
    {
        private readonly Func<int, int, T> newCell;

        private readonly Func<int, TR> newRowHeader;

        private readonly ObservableCollection<ObservableCollection<T>> cells;

        private readonly ObservableCollection<TR> rowHeaders;

        private readonly ObservableCollection<TC> columnHeaders;

        public Table(Func<int, int, T> newCell, Func<int, TR> newRowHeader)
        {
            this.newCell = newCell;
            this.newRowHeader = newRowHeader;
            this.cells = new ObservableCollection<ObservableCollection<T>>();
            this.rowHeaders = new ObservableCollection<TR>();
            this.columnHeaders = new ObservableCollection<TC>();
            this.cells.CollectionChanged += this.CellsCollectionChanged;
            this.rowHeaders.CollectionChanged += this.RowHeadersCollectionChanged;
            this.columnHeaders.CollectionChanged += this.ColumnHeadersCollectionChanged;
        }

        public Table(ObservableCollection<TR> rowHeaders, ObservableCollection<TC> columnHeaders, Func<int, int, T> newCell, Func<int, TR> newRowHeader)
        {
            this.newCell = newCell;
            this.newRowHeader = newRowHeader;
            this.cells = new ObservableCollection<ObservableCollection<T>>();
            this.rowHeaders = rowHeaders;
            this.columnHeaders = columnHeaders;
            this.cells.CollectionChanged += this.CellsCollectionChanged;
            this.rowHeaders.CollectionChanged += this.RowHeadersCollectionChanged;
            this.columnHeaders.CollectionChanged += this.ColumnHeadersCollectionChanged;
        }

        public ObservableCollection<ObservableCollection<T>> Cells
        {
            get
            {
                return this.cells;
            }
        }

        public ObservableCollection<TR> RowHeaders
        {
            get
            {
                return this.rowHeaders;
            }
        }

        public ObservableCollection<TC> ColumnHeaders
        {
            get
            {
                return this.columnHeaders;
            }
        }

        public T this[int i, int j]
        {
            get
            {
                return this.cells[i][j];
            }

            set
            {
                this.cells[i][j] = value;
            }
        }

        private void ColumnHeadersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < this.cells.Count; i++)
                    {
                        var row = this.cells[i];
                        for (int j = e.NewStartingIndex; j < e.NewStartingIndex + e.NewItems.Count; j++)
                        {
                            row.Insert(j, this.newCell(i, j));
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < this.cells.Count; i++)
                    {
                        var row = this.cells[i];
                        for (int j = 0; j < e.OldItems.Count; j++)
                        {
                            row.RemoveAt(e.OldStartingIndex);
                        }
                    }

                    break;
            }
        }

        private void RowHeadersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.cells.CollectionChanged -= this.CellsCollectionChanged;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = e.NewStartingIndex; i < e.NewStartingIndex + e.NewItems.Count; i++)
                    {
                        var row = new ObservableCollection<T>();
                        this.cells.Insert(i, row);
                        for (int j = 0; j < this.columnHeaders.Count; j++)
                        {
                            row.Add(this.newCell(i, j));
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        this.cells.RemoveAt(e.OldStartingIndex);
                    }

                    break;
            }

            this.cells.CollectionChanged += this.CellsCollectionChanged;
        }

        private void CellsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.rowHeaders.CollectionChanged -= this.RowHeadersCollectionChanged;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = e.NewStartingIndex; i < e.NewStartingIndex + e.NewItems.Count; i++)
                    {
                        this.rowHeaders.Insert(i, this.newRowHeader(i));
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        this.rowHeaders.RemoveAt(e.OldStartingIndex);
                    }

                    break;
            }

            this.rowHeaders.CollectionChanged += this.RowHeadersCollectionChanged;
        }
    }
}