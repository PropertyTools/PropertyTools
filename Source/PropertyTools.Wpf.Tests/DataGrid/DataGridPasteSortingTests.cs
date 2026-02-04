// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridPasteSortingTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class DataGridPasteSortingTests
    {
        /// <summary>
        /// Test class representing a simple data item with a numeric property that can be sorted.
        /// </summary>
        public class TestDataItem : INotifyPropertyChanged
        {
            private double value;

            public event PropertyChangedEventHandler PropertyChanged;

            public double Value
            {
                get => this.value;
                set
                {
                    if (this.value != value)
                    {
                        this.value = value;
                        this.OnPropertyChanged(nameof(Value));
                    }
                }
            }

            protected virtual void OnPropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Testable DataGrid that exposes protected methods for testing.
        /// </summary>
        private class TestableDataGrid : DataGrid
        {
            public CellRange TestSetValues(object[,] values, CellRange range)
            {
                return this.SetValues(values, range);
            }

            public new ICollectionView CollectionView => base.CollectionView;
        }

        [Test]
        public void SetValues_WithSortingActive_SetsCorrectItemsInSourceCollection()
        {
            // Arrange
            var dataGrid = new TestableDataGrid();
            var items = new ObservableCollection<TestDataItem>
            {
                new TestDataItem { Value = 30 },
                new TestDataItem { Value = 10 },
                new TestDataItem { Value = 20 }
            };

            dataGrid.ItemsSource = items;
            dataGrid.ItemsInRows = true;
            dataGrid.CanInsert = true;
            
            // Force the DataGrid to initialize
            dataGrid.UpdateLayout();
            dataGrid.ApplyTemplate();

            // Apply ascending sort on the Value property
            var view = CollectionViewSource.GetDefaultView(items);
            view.SortDescriptions.Add(new SortDescription(nameof(TestDataItem.Value), ListSortDirection.Ascending));
            view.Refresh();

            // Verify sort is active - view order should be: 10, 20, 30
            var viewItems = new System.Collections.Generic.List<TestDataItem>();
            foreach (TestDataItem item in view)
            {
                viewItems.Add(item);
            }
            Assert.That(viewItems[0].Value, Is.EqualTo(10), "First item in view should be 10");
            Assert.That(viewItems[1].Value, Is.EqualTo(20), "Second item in view should be 20");
            Assert.That(viewItems[2].Value, Is.EqualTo(30), "Third item in view should be 30");

            // Values to paste: we want to set view row 0 (which is the item with Value=10) to 99
            var valuesToPaste = new object[,] { { 99.0 } };
            var range = new CellRange(new CellRef(0, 0), new CellRef(0, 0)); // Paste to view row 0

            // Act
            dataGrid.TestSetValues(valuesToPaste, range);

            // Assert
            // The item that was at view position 0 (the one with original Value=10) should now have Value=99
            // Find that item in the source collection
            var targetItem = viewItems[0]; // This is the item that was at view index 0
            Assert.That(targetItem.Value, Is.EqualTo(99), "Item at view row 0 should have been updated to 99");

            // Verify the source collection still has the same 3 items
            Assert.That(items.Count, Is.EqualTo(3), "Should still have 3 items");

            // Verify other items were not modified
            Assert.That(items[0].Value, Is.EqualTo(30), "First item in source (Value=30) should not be modified");
            Assert.That(items[2].Value, Is.EqualTo(20), "Third item in source (Value=20) should not be modified");
        }

        [Test]
        public void SetValues_WithSortingActive_AddsNewItemsAtEnd()
        {
            // Arrange
            var dataGrid = new TestableDataGrid();
            var items = new ObservableCollection<TestDataItem>
            {
                new TestDataItem { Value = 30 },
                new TestDataItem { Value = 10 },
                new TestDataItem { Value = 20 }
            };

            dataGrid.ItemsSource = items;
            dataGrid.ItemsInRows = true;
            dataGrid.CanInsert = true;
            dataGrid.CreateItem = () => new TestDataItem();
            
            // Force the DataGrid to initialize
            dataGrid.UpdateLayout();
            dataGrid.ApplyTemplate();

            // Apply ascending sort on the Value property
            var view = CollectionViewSource.GetDefaultView(items);
            view.SortDescriptions.Add(new SortDescription(nameof(TestDataItem.Value), ListSortDirection.Ascending));
            view.Refresh();

            // Values to paste: 5 new values (we have 3 rows, so 2 new rows will be added)
            var valuesToPaste = new object[,] 
            { 
                { 100.0 },
                { 200.0 },
                { 300.0 },
                { 400.0 },
                { 500.0 }
            };
            var range = new CellRange(new CellRef(0, 0), new CellRef(4, 0)); // Paste to rows 0-4

            // Act
            dataGrid.TestSetValues(valuesToPaste, range);

            // Assert
            // Should have 5 items now
            Assert.That(items.Count, Is.EqualTo(5), "Should have 5 items after paste");

            // All items should have been updated with the pasted values
            // Since we pasted to view rows 0-4, we need to check that the correct items got the correct values
            view.Refresh();
            
            var viewItems = new System.Collections.Generic.List<TestDataItem>();
            foreach (TestDataItem item in view)
            {
                viewItems.Add(item);
            }

            // The values should have been set based on the view order at the time of paste
            // Since the operation adds items at the end and then sets values, 
            // we should verify that all 5 values were set
            var allValues = new System.Collections.Generic.List<double>();
            foreach (var item in items)
            {
                allValues.Add(item.Value);
            }

            // The pasted values should all be present in the source collection
            Assert.That(allValues, Does.Contain(100.0), "Should contain pasted value 100");
            Assert.That(allValues, Does.Contain(200.0), "Should contain pasted value 200");
            Assert.That(allValues, Does.Contain(300.0), "Should contain pasted value 300");
            Assert.That(allValues, Does.Contain(400.0), "Should contain pasted value 400");
            Assert.That(allValues, Does.Contain(500.0), "Should contain pasted value 500");
        }

        [Test]
        public void SetValues_WithoutSorting_WorksAsExpected()
        {
            // Arrange
            var dataGrid = new TestableDataGrid();
            var items = new ObservableCollection<TestDataItem>
            {
                new TestDataItem { Value = 30 },
                new TestDataItem { Value = 10 },
                new TestDataItem { Value = 20 }
            };

            dataGrid.ItemsSource = items;
            dataGrid.ItemsInRows = true;
            dataGrid.CanInsert = true;
            
            // Force the DataGrid to initialize
            dataGrid.UpdateLayout();
            dataGrid.ApplyTemplate();

            // No sorting applied - view order is the same as source order

            // Values to paste
            var valuesToPaste = new object[,] { { 99.0 } };
            var range = new CellRange(new CellRef(0, 0), new CellRef(0, 0)); // Paste to row 0

            // Act
            dataGrid.TestSetValues(valuesToPaste, range);

            // Assert
            // First item should have been updated
            Assert.That(items[0].Value, Is.EqualTo(99), "First item should be updated to 99");
            Assert.That(items[1].Value, Is.EqualTo(10), "Second item should not be modified");
            Assert.That(items[2].Value, Is.EqualTo(20), "Third item should not be modified");
        }
    }
}
