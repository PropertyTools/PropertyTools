// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataViewOperatorTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.ComponentModel;
    using System.Data;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    /// <summary>
    /// Tests for <see cref="DataViewOperator" /> and DataGrid behavior when the items source is a <see cref="DataView" />.
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
        public void CreateOperator_DataViewItemsSource_ReturnsDataViewOperator()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new DataGrid();

            // Act
            dataGrid.ItemsSource = dataView;

            // Assert
            Assert.That(dataGrid.Operator, Is.InstanceOf<DataViewOperator>());
        }

        [Test]
        public void InsertItem_DataViewItemsSource_AppendsRow()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new DataGrid();
            dataGrid.ItemsSource = dataView;
            var initialCount = dataView.Count;
            var op = new DataViewOperator(dataGrid);

            // Act
            var result = op.InsertItem(-1);

            // Assert
            Assert.That(result, Is.GreaterThanOrEqualTo(0));
            Assert.That(dataView.Count, Is.EqualTo(initialCount + 1));
        }

        [Test]
        public void InsertItem_DataViewWithAllowNewFalse_ReturnsMinus1()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            dataView.AllowNew = false;
            var dataGrid = new DataGrid();
            dataGrid.ItemsSource = dataView;
            var op = new DataViewOperator(dataGrid);

            // Act
            var result = op.InsertItem(-1);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void CanDeleteRows_AllowDeleteTrue_ReturnsTrue()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            dataView.AllowDelete = true;
            var dataGrid = new DataGrid();
            dataGrid.CanDelete = true;
            dataGrid.ItemsSource = dataView;
            var op = new DataViewOperator(dataGrid);

            // Act
            var result = op.CanDeleteRows();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanDeleteRows_AllowDeleteFalse_ReturnsFalse()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            dataView.AllowDelete = false;
            var dataGrid = new DataGrid();
            dataGrid.CanDelete = true;
            dataGrid.ItemsSource = dataView;
            var op = new DataViewOperator(dataGrid);

            // Act
            var result = op.CanDeleteRows();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CanInsertRows_AllowNewTrue_ReturnsTrue()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            dataView.AllowNew = true;
            var dataGrid = new DataGrid();
            dataGrid.CanInsert = true;
            dataGrid.ItemsSource = dataView;
            var op = new DataViewOperator(dataGrid);

            // Act
            var result = op.CanInsertRows();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanInsertColumns_Always_ReturnsFalse()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new DataGrid();
            dataGrid.ItemsSource = dataView;
            var op = new DataViewOperator(dataGrid);

            // Act
            var result = op.CanInsertColumns();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CanDeleteColumns_Always_ReturnsFalse()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new DataGrid();
            dataGrid.ItemsSource = dataView;
            var op = new DataViewOperator(dataGrid);

            // Act
            var result = op.CanDeleteColumns();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetItem_ValidCell_ReturnsDataRowView()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new DataGrid();
            dataGrid.ItemsSource = dataView;
            var op = new DataViewOperator(dataGrid);

            // Act
            var item = op.GetItem(new CellRef(0, 0));

            // Assert
            Assert.That(item, Is.InstanceOf<DataRowView>());
        }

        [Test]
        public void GetItem_OutOfRangeCell_ReturnsNull()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new DataGrid();
            dataGrid.ItemsSource = dataView;
            var op = new DataViewOperator(dataGrid);

            // Act
            var item = op.GetItem(new CellRef(100, 0));

            // Assert
            Assert.That(item, Is.Null);
        }

        [Test]
        public void GetBindingPath_WithPropertyDefinition_ReturnsColumnName()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new DataGrid();
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition { PropertyName = "StringColumn" });
            dataGrid.ItemsSource = dataView;
            var op = new DataViewOperator(dataGrid);

            // Act
            var path = op.GetBindingPath(new CellRef(0, 0));

            // Assert
            Assert.That(path, Is.EqualTo("StringColumn"));
        }

        [Test]
        public void AutoGenerateColumns_DataViewItemsSource_GeneratesCorrectColumns()
        {
            // Arrange
            var dataView = CreateSampleDataTable().DefaultView;
            var dataGrid = new DataGrid();
            dataGrid.ItemsSource = dataView;

            // Assert
            Assert.That(dataGrid.ColumnDefinitions.Count, Is.EqualTo(3));
            Assert.That(dataGrid.ColumnDefinitions[0].PropertyName, Is.EqualTo("BoolColumn"));
            Assert.That(dataGrid.ColumnDefinitions[1].PropertyName, Is.EqualTo("StringColumn"));
            Assert.That(dataGrid.ColumnDefinitions[2].PropertyName, Is.EqualTo("IntColumn"));
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
