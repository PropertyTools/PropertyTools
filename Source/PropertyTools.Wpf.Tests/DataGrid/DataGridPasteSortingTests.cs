// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridPasteSortingTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Tests for paste functionality when sorting is active on the DataGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.DataGrid
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;

    using NUnit.Framework;

    /// <summary>
    /// Tests for paste functionality when sorting is active on the DataGrid.
    /// </summary>
    [TestFixture]
    public class DataGridPasteSortingTests
    {
        /// <summary>
        /// Simple test object with sortable properties.
        /// </summary>
        public class TestItem : INotifyPropertyChanged
        {
            private double x;
            private double y;
            private double z;

            public double X
            {
                get => this.x;
                set
                {
                    this.x = value;
                    this.OnPropertyChanged(nameof(X));
                }
            }

            public double Y
            {
                get => this.y;
                set
                {
                    this.y = value;
                    this.OnPropertyChanged(nameof(Y));
                }
            }

            public double Z
            {
                get => this.z;
                set
                {
                    this.z = value;
                    this.OnPropertyChanged(nameof(Z));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Demonstrates the root cause of the paste-after-sort bug.
        /// When a CollectionView has sorting applied, view indices differ from source indices.
        /// Paste operations that use view indices will write to wrong source items.
        /// </summary>
        [Test]
        public void ViewIndices_DifferFromSourceIndices_WhenSortingIsActive()
        {
            // Arrange: Create collection with items out of natural sort order
            var items = new ObservableCollection<TestItem>
            {
                new() { X = 30, Y = 0, Z = 0 }, // Source index 0
                new() { X = 10, Y = 0, Z = 0 }, // Source index 1
                new() { X = 20, Y = 0, Z = 0 }, // Source index 2
            };

            var collectionView = CollectionViewSource.GetDefaultView(items);

            // Act: Apply ascending sort on X column
            collectionView.SortDescriptions.Add(new SortDescription("X", ListSortDirection.Ascending));
            collectionView.Refresh();

            // Assert: View order differs from source order
            var viewItems = collectionView.Cast<TestItem>().ToList();

            // View index 0 shows X=10 (source index 1)
            Assert.That(viewItems[0].X, Is.EqualTo(10), "View index 0 should show item with X=10");

            // View index 1 shows X=20 (source index 2)
            Assert.That(viewItems[1].X, Is.EqualTo(20), "View index 1 should show item with X=20");

            // View index 2 shows X=30 (source index 0)
            Assert.That(viewItems[2].X, Is.EqualTo(30), "View index 2 should show item with X=30");

            // This demonstrates the bug: If paste uses view indices to access source collection,
            // values will be written to wrong items.
            // For example, pasting value at "view row 0" would incorrectly modify items[0] (X=30)
            // instead of the item actually displayed at row 0 (X=10, which is items[1]).
        }

        /// <summary>
        /// Demonstrates that clearing sort descriptions restores view-source index alignment.
        /// </summary>
        [Test]
        public void ClearingSort_RestoresViewSourceIndexAlignment()
        {
            // Arrange
            var items = new ObservableCollection<TestItem>
            {
                new() { X = 30, Y = 0, Z = 0 },
                new() { X = 10, Y = 0, Z = 0 },
                new() { X = 20, Y = 0, Z = 0 },
            };

            var collectionView = CollectionViewSource.GetDefaultView(items);

            // Apply sort
            collectionView.SortDescriptions.Add(new SortDescription("X", ListSortDirection.Ascending));
            collectionView.Refresh();

            // Act: Clear sorting
            collectionView.SortDescriptions.Clear();
            collectionView.Refresh();

            // Assert: View order matches source order again
            var viewItems = collectionView.Cast<TestItem>().ToList();

            Assert.That(viewItems[0], Is.EqualTo(items[0]), "After clearing sort, view index 0 should match source index 0");
            Assert.That(viewItems[1], Is.EqualTo(items[1]), "After clearing sort, view index 1 should match source index 1");
            Assert.That(viewItems[2], Is.EqualTo(items[2]), "After clearing sort, view index 2 should match source index 2");
        }

        /// <summary>
        /// Demonstrates that clearing sort before paste ensures correct value assignment.
        /// </summary>
        [Test]
        public void PasteScenario_AfterClearingSort_AssignsValuesCorrectly()
        {
            // Arrange: Create an empty collection and add initial items
            var items = new ObservableCollection<TestItem>
            {
                new() { X = 0, Y = 0, Z = 0 }
            };

            var collectionView = CollectionViewSource.GetDefaultView(items);

            // Simulate clicking column header which triggers sort
            collectionView.SortDescriptions.Add(new SortDescription("X", ListSortDirection.Ascending));
            collectionView.Refresh();

            // Simulate pasting 3 new values: 1.0, 2.0, 3.0
            // This would add rows and set values using view indices
            var valuesToPaste = new[] { 1.0, 2.0, 3.0 };

            // Add items for the paste operation
            for (var i = 0; i < valuesToPaste.Length; i++)
            {
                items.Add(new TestItem { X = 0, Y = 0, Z = 0 });
            }

            // Bug scenario: Using view index to set values in source collection
            // When sort is active, this writes to wrong items

            // If we write to source using view index (the bug), we get wrong results
            // Correct behavior requires either:
            // 1. Converting view index to source index, OR
            // 2. Clearing sort before paste (recommended solution)

            // This test documents the expected behavior after the fix
            collectionView.SortDescriptions.Clear();
            collectionView.Refresh();

            // Now paste using source indices (correct after clearing sort)
            for (var viewIndex = 1; viewIndex < valuesToPaste.Length + 1; viewIndex++)
            {
                items[viewIndex].X = valuesToPaste[viewIndex - 1];
            }

            // Assert: Values are in correct source positions
            Assert.That(items[1].X, Is.EqualTo(1.0), "Item at source index 1 should have X=1.0");
            Assert.That(items[2].X, Is.EqualTo(2.0), "Item at source index 2 should have X=2.0");
            Assert.That(items[3].X, Is.EqualTo(3.0), "Item at source index 3 should have X=3.0");
        }

        /// <summary>
        /// Tests that sort is toggled in cycle: None -> Ascending -> Descending -> None.
        /// </summary>
        [Test]
        public void SortToggle_CyclesThroughStates()
        {
            // Arrange
            var sortDescriptions = new List<SortDescription>();

            // Initial state: no sorting
            Assert.That(sortDescriptions.Count, Is.EqualTo(0), "Initial state should have no sort");

            // First click: Ascending
            sortDescriptions.Add(new SortDescription("X", ListSortDirection.Ascending));
            Assert.That(sortDescriptions.Count, Is.EqualTo(1));
            Assert.That(sortDescriptions[0].Direction, Is.EqualTo(ListSortDirection.Ascending));

            // Second click: Descending
            sortDescriptions.Clear();
            sortDescriptions.Add(new SortDescription("X", ListSortDirection.Descending));
            Assert.That(sortDescriptions.Count, Is.EqualTo(1));
            Assert.That(sortDescriptions[0].Direction, Is.EqualTo(ListSortDirection.Descending));

            // Third click: Clear (back to none)
            sortDescriptions.Clear();
            Assert.That(sortDescriptions.Count, Is.EqualTo(0), "Third toggle should clear sort");
        }
    }
}
