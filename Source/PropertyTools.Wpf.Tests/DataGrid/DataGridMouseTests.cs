// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridMouseTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    /// <summary>
    /// Tests for <see cref="DataGrid" /> mouse handling.
    /// Covers the bug where <c>OnMouseLeftButtonDown</c> threw
    /// <see cref="System.InvalidOperationException" /> ("This Visual is not connected to a
    /// PresentationSource") when the control was not connected to a <see cref="PresentationSource" />
    /// (for example when a click handler navigates away/opens a window and the DataGrid is
    /// disconnected from the visual tree while the click is being processed) #510.
    /// </summary>
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class DataGridMouseTests
    {
        [Test]
        public void OnMouseLeftButtonDown_NotConnectedToPresentationSource_DoesNotThrow()
        {
            // Arrange
            var dataGrid = new DataGrid { ItemsSource = new ObservableCollection<object> { "a", "b" } };
            dataGrid.ApplyTemplate();

            Assert.That(PresentationSource.FromVisual(dataGrid), Is.Null);

            var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = UIElement.MouseLeftButtonDownEvent
            };

            // Act / Assert
            Assert.That(() => dataGrid.RaiseEvent(args), Throws.Nothing);
        }
    }
}
