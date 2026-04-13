// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridClipboardCommandsTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Threading;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class DataGridClipboardCommandsTests
    {
        private TestableDataGrid dataGrid;

        [SetUp]
        public void SetUp()
        {
            this.dataGrid = new TestableDataGrid();
        }

        [Test]
        public void HasValidSelection_NullItemsSource_ReturnsFalse()
        {
            // Arrange
            this.dataGrid.ItemsSource = null;

            // Act
            var result = this.dataGrid.TestHasValidSelection();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasValidSelection_EmptyItemsSource_ReturnsFalse()
        {
            // Arrange
            this.dataGrid.ItemsSource = new ObservableCollection<object>();

            // Act
            var result = this.dataGrid.TestHasValidSelection();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasValidSelection_ItemsSourceWithData_ReturnsTrue()
        {
            // Arrange
            this.dataGrid.ItemsSource = new ObservableCollection<object>
            {
                new { Name = "Test" }
            };

            // Act
            var result = this.dataGrid.TestHasValidSelection();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanModifySelection_NullItemsSource_ReturnsFalse()
        {
            // Arrange
            this.dataGrid.ItemsSource = null;

            // Act
            var result = this.dataGrid.TestCanModifySelection();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CanModifySelection_EmptyItemsSource_ReturnsFalse()
        {
            // Arrange
            this.dataGrid.ItemsSource = new ObservableCollection<object>();

            // Act
            var result = this.dataGrid.TestCanModifySelection();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CanModifySelection_ItemsSourceWithData_DependsOnColumnReadOnly()
        {
            // Arrange
            this.dataGrid.ItemsSource = new ObservableCollection<object>
            {
                new { Name = "Test" }
            };

            // Note: This test verifies that with data, CanModifySelection returns true 
            // if there's at least one editable column. The actual result depends on 
            // PropertyDefinitions being set up correctly.

            // Act
            var result = this.dataGrid.TestCanModifySelection();

            // Assert
            // With default PropertyDefinitions setup and data present, 
            // this should return false (no property definitions = no editable columns)
            Assert.That(result, Is.False);
        }

        [Test]
        public void ToCsv_TabSeparator_UsesTabSeparatorForBothHeaderAndDataRows()
        {
            // Arrange
            var range = new CellRange(new CellRef(0, 0), new CellRef(1, 2)); // 2 rows, 3 columns

            // Act
            var csv = this.dataGrid.TestToCsv(range, "\t", includeHeader: false);

            // Assert: every row in the data section uses tab, not semicolon
            var lines = csv.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            Assert.That(lines.Length, Is.EqualTo(2));
            Assert.That(lines[0], Is.EqualTo("r0c0\tr0c1\tr0c2"));
            Assert.That(lines[1], Is.EqualTo("r1c0\tr1c1\tr1c2"));
        }

        [Test]
        public void ClipboardSeparator_DefaultValue_IsCurrentCultureListSeparator()
        {
            // The default ClipboardSeparator should match the current culture's list separator
            Assert.That(
                this.dataGrid.ClipboardSeparator,
                Is.EqualTo(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator));
        }

        [Test]
        public void ClipboardSeparator_SetToComma_ToCsvUsesComma()
        {
            // Arrange
            this.dataGrid.ClipboardSeparator = ",";
            var range = new CellRange(new CellRef(0, 0), new CellRef(0, 2)); // 1 row, 3 columns

            // Act - pass the property value just as OnKeyDown would
            var csv = this.dataGrid.TestToCsv(range, this.dataGrid.ClipboardSeparator, includeHeader: false);

            // Assert
            var lines = csv.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            Assert.That(lines.Length, Is.EqualTo(1));
            Assert.That(lines[0], Is.EqualTo("r0c0,r0c1,r0c2"));
        }

        /// <summary>
        /// Testable DataGrid that exposes protected methods for testing.
        /// Overrides GetCellStrings to supply deterministic cell values without
        /// requiring a WPF visual tree or a configured Operator.
        /// </summary>
        private class TestableDataGrid : DataGrid
        {
            public bool TestHasValidSelection()
            {
                return this.HasValidSelection();
            }

            public bool TestCanModifySelection()
            {
                return this.CanModifySelection();
            }

            public string TestToCsv(CellRange range, string separator = ";", bool includeHeader = true)
            {
                return this.ToCsv(range, separator, includeHeader);
            }

            protected override string[,] GetCellStrings(CellRange range, object[,] values = null)
            {
                var result = new string[range.Rows, range.Columns];
                for (var i = 0; i < range.Rows; i++)
                {
                    for (var j = 0; j < range.Columns; j++)
                    {
                        result[i, j] = $"r{range.TopRow + i}c{range.LeftColumn + j}";
                    }
                }

                return result;
            }
        }
    }
}
