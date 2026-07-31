// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListListColumnsSyncTests.cs" company="PropertyTools">
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
    public class ListListColumnsSyncTests
    {
        [Test]
        public void ItemsSource_InnerCollectionItemAdded_ColumnsAreUpdated()
        {
            // Arrange
            var itemsSource = new ObservableCollection<ObservableCollection<int>>
            {
                new ObservableCollection<int> { 1, 2, 3 },
                new ObservableCollection<int> { 4, 5, 6 }
            };

            var dataGrid = new DataGrid();
            dataGrid.ApplyTemplate();
            dataGrid.ItemsSource = itemsSource;

            Assert.That(dataGrid.Columns, Is.EqualTo(3));

            // Act - add a new item to the first row (i.e. add a new column)
            itemsSource[0].Add(7);

            // Assert
            Assert.That(dataGrid.Columns, Is.EqualTo(4));
        }

        [Test]
        public void ItemsSource_InnerCollectionItemRemoved_ColumnsAreUpdated()
        {
            // Arrange
            var itemsSource = new ObservableCollection<ObservableCollection<int>>
            {
                new ObservableCollection<int> { 1, 2, 3 },
                new ObservableCollection<int> { 4, 5, 6 }
            };

            var dataGrid = new DataGrid();
            dataGrid.ApplyTemplate();
            dataGrid.ItemsSource = itemsSource;

            Assert.That(dataGrid.Columns, Is.EqualTo(3));

            // Act - remove an item from the first row (i.e. remove a column)
            itemsSource[0].RemoveAt(0);

            // Assert
            Assert.That(dataGrid.Columns, Is.EqualTo(2));
        }
    }
}
