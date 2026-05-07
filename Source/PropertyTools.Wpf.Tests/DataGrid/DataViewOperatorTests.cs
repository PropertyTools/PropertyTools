// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataViewOperatorTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    /// <summary>
    /// Tests for DataGrid behavior when the items source is a <see cref="DataView" />.
    /// Covers the bug where pasting into a sorted DataView-backed DataGrid threw
    /// <see cref="System.InvalidOperationException" /> from <c>GetCollectionViewIndex</c>
    /// because <c>HandleAutoInsert</c> did not check whether row insertion succeeded.
    /// </summary>
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class DataViewOperatorTests
    {
        /// <summary>
        /// Creates a DataTable with sample rows for testing.
        /// </summary>
        private static DataTable CreateSampleDataTable()
        {
            var table = new DataTable();
            table.Columns.Add(new DataColumn("BoolColumn", typeof(bool)));
            table.Columns.Add(new DataColumn("StringColumn", typeof(string)));
            table.Columns.Add(new DataColumn("IntColumn", typeof(int)));
            table.Rows.Add(true, "test1", 10);
            table.Rows.Add(false, "test2", 20);
            table.Rows.Add(true, "test3", 30);
            table.Rows.Add(false, "test4", 40);
            return table;
        }

        [Test]
        public void InsertItem_DataViewItemsSource_ReturnsMinus1()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new DataGrid();
            dataGrid.ItemsSource = dataView;
            var listOperator = new ListOperator(dataGrid);

            // Act - attempting to insert at a specific position (mirrors HandleAutoInsert which
            // calls InsertItem(this.Rows)).  DataView does not support IList.Insert, so this
            // should fail and return -1.
            var result = listOperator.InsertItem(dataView.Count);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void InsertItem_DataViewItemsSource_WithSpecificIndex_ReturnsMinus1()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new DataGrid();
            dataGrid.ItemsSource = dataView;
            var listOperator = new ListOperator(dataGrid);

            // Act - attempting to insert at specific index
            var result = listOperator.InsertItem(0);

            // Assert - DataView does not support IList.Insert, so insertion should fail
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void SetValues_DataViewWithSortingPasteAtLastRow_DoesNotThrow()
        {
            // Arrange - set up a DataGrid with DataView and simulate sorting state
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new TestableDataGrid();
            dataGrid.ItemsSource = dataView;

            // Apply sort descriptions to the CollectionView (simulates clicking a column header)
            if (dataGrid.CollectionView != null)
            {
                dataGrid.CollectionView.SortDescriptions.Add(
                    new SortDescription("IntColumn", ListSortDirection.Descending));
                dataGrid.CollectionView.Refresh();
            }

            // Set up column definitions matching the DataTable
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition { PropertyName = "BoolColumn" });
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition { PropertyName = "StringColumn" });
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition { PropertyName = "IntColumn" });

            // Values to paste: 2 rows x 3 columns (starting at the last existing row)
            var values = new object[,]
            {
                { false, "test2", 20 },
                { true,  "test1", 10 },
            };

            // Range starting at the last row (row 3, 0-indexed); the second row (row 4) extends
            // beyond the DataView's row count, so insertion will be attempted and should fail gracefully.
            var range = new CellRange(new CellRef(3, 0), new CellRef(3, 2));

            // Act & Assert - should not throw InvalidOperationException
            Assert.That(
                () => dataGrid.TestSetValues(values, range),
                Throws.Nothing);
        }

        [Test]
        public void SetValues_DataViewWithSortingPasteBeyondLastRow_DoesNotThrow()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new TestableDataGrid();
            dataGrid.ItemsSource = dataView;

            if (dataGrid.CollectionView != null)
            {
                dataGrid.CollectionView.SortDescriptions.Add(
                    new SortDescription("IntColumn", ListSortDirection.Descending));
                dataGrid.CollectionView.Refresh();
            }

            dataGrid.ColumnDefinitions.Add(new ColumnDefinition { PropertyName = "BoolColumn" });
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition { PropertyName = "StringColumn" });
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition { PropertyName = "IntColumn" });

            var values = new object[,]
            {
                { false, "test2", 20 },
                { true,  "test1", 10 },
            };

            // Range starting at row 4 (the add-item row, beyond the DataView's 4 existing rows).
            // This simulates pasting when the cursor is on the add-item row.
            var range = new CellRange(new CellRef(4, 0), new CellRef(4, 2));

            // Act & Assert - should not throw InvalidOperationException
            Assert.That(
                () => dataGrid.TestSetValues(values, range),
                Throws.Nothing);
        }

        /// <summary>
        /// A testable subclass of <see cref="DataGrid" /> that exposes protected members for testing.
        /// </summary>
        private class TestableDataGrid : DataGrid
        {
            /// <summary>
            /// Exposes the protected <see cref="DataGrid.SetValues" /> method for testing.
            /// </summary>
            public CellRange TestSetValues(object[,] values, CellRange range)
            {
                return this.SetValues(values, range);
            }
        }
    }
}
