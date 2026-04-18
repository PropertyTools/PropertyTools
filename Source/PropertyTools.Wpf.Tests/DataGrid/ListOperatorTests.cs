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
        public void GetDataContext_WithEmptyPropertyName_ReturnsItemsSource()
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
            var cell = new CellRef(0, 0);

            // Act
            var dataContext = listOperator.GetDataContext(cell);

            // Assert
            // When PropertyName is empty, DataContext should be the collection, not the item
            Assert.That(dataContext, Is.SameAs(list));
        }

        [Test]
        public void GetDataContext_WithNonEmptyPropertyName_ReturnsItem()
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
            var dataContext = listOperator.GetDataContext(cell);

            // Assert
            // When PropertyName is set, DataContext should be the item
            Assert.That(dataContext, Is.SameAs(list[0]));
        }

        private class TestItem
        {
            public string Name { get; set; }
        }
    }
}
