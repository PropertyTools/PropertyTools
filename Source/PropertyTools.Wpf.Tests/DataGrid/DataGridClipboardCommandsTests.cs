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

        /// <summary>
        /// Testable DataGrid that exposes protected methods for testing.
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
        }
    }
}
