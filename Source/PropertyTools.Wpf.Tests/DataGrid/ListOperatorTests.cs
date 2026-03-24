// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListOperatorTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Collections.ObjectModel;
    using NUnit.Framework;
    using PropertyTools.Wpf;

    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class ListOperatorTests
    {
        [Test]
        public void GetBindingPath_WithNonEmptyPropertyName_ReturnsPropertyName()
        {
            // Arrange
            var dataGrid = new DataGrid();
            var list = new ObservableCollection<TestItem>
            {
                new TestItem { Name = "Item1" }
            };
            dataGrid.ItemsSource = list;
            
            var columnDefinition = new ColumnDefinition
            {
                PropertyName = "Name"
            };
            dataGrid.ColumnDefinitions.Add(columnDefinition);
            
            var listOperator = new ListOperator(dataGrid);
            var cell = new CellRef(0, 0);

            // Act
            var bindingPath = listOperator.GetBindingPath(cell);

            // Assert
            Assert.That(bindingPath, Is.EqualTo("Name"));
        }

        [Test]
        public void GetBindingPath_WithEmptyPropertyName_ReturnsIndexPath()
        {
            // Arrange
            var dataGrid = new DataGrid();
            var list = new ObservableCollection<string> { "Oslo", "Reykjavik", "New York" };
            dataGrid.ItemsSource = list;
            
            var columnDefinition = new ColumnDefinition
            {
                PropertyName = string.Empty
            };
            dataGrid.ColumnDefinitions.Add(columnDefinition);
            
            var listOperator = new ListOperator(dataGrid);
            var cell = new CellRef(1, 0);

            // Act
            var bindingPath = listOperator.GetBindingPath(cell);

            // Assert
            Assert.That(bindingPath, Is.EqualTo("[1]"));
        }

        [Test]
        public void GetBindingPath_WithNullPropertyName_ReturnsIndexPath()
        {
            // Arrange
            var dataGrid = new DataGrid();
            var list = new ObservableCollection<string> { "Value1", "Value2" };
            dataGrid.ItemsSource = list;
            
            var columnDefinition = new ColumnDefinition
            {
                PropertyName = null
            };
            dataGrid.ColumnDefinitions.Add(columnDefinition);
            
            var listOperator = new ListOperator(dataGrid);
            var cell = new CellRef(0, 0);

            // Act
            var bindingPath = listOperator.GetBindingPath(cell);

            // Assert
            Assert.That(bindingPath, Is.EqualTo("[0]"));
        }

        [Test]
        public void GetBindingPath_EmptyPropertyName_EnablesTwoWayBinding()
        {
            // Arrange
            var dataGrid = new DataGrid();
            var list = new ObservableCollection<string> { "Initial Value" };
            dataGrid.ItemsSource = list;
            
            var columnDefinition = new ColumnDefinition
            {
                PropertyName = string.Empty,
                IsReadOnly = false
            };
            dataGrid.ColumnDefinitions.Add(columnDefinition);
            
            var factory = new DataGridControlFactory();
            var listOperator = new ListOperator(dataGrid);
            
            // Act
            var bindingPath = listOperator.GetBindingPath(new CellRef(0, 0));
            
            // Assert
            // Verify that the binding path is index-based, not empty
            Assert.That(bindingPath, Is.EqualTo("[0]"));
            Assert.That(bindingPath, Is.Not.Empty);
            // This ensures CreateBinding will use TwoWay mode instead of OneWay
        }

        private class TestItem
        {
            public string Name { get; set; }
        }
    }
}
