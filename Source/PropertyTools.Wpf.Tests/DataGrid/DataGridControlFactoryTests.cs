// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridControlFactoryTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class DataGridControlFactoryTests
    {
        private DataGridControlFactory factory;

        [SetUp]
        public void SetUp()
        {
            this.factory = new DataGridControlFactory();
        }

        [Test]
        public void CreateContainer_NullBackgroundBindingPath_ReturnsControlDirectly()
        {
            // Arrange
            var cellDefinition = new TextCellDefinition
            {
                BackgroundBindingPath = null
            };
            var control = new TextBlock { Text = "Test" };

            // Act
            var result = this.factory.CreateDisplayControl(cellDefinition);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<TextBlock>());
        }

        [Test]
        public void CreateContainer_EmptyBackgroundBindingPathWithBrush_TextBlock_WrapsInBorder()
        {
            // Arrange
            var brush = new SolidColorBrush(Colors.LightYellow);
            var cellDefinition = new TextCellDefinition
            {
                BackgroundBindingPath = string.Empty,
                BackgroundBindingSource = brush,
                BindingPath = "TestProperty"
            };

            // Act
            var result = this.factory.CreateDisplayControl(cellDefinition);

            // Assert
            Assert.That(result, Is.Not.Null);
            // TextBlock doesn't support Background, so should be wrapped in a Border
            Assert.That(result, Is.InstanceOf<Border>());
            var border = (Border)result;
            Assert.That(border.Background, Is.EqualTo(brush));
            Assert.That(border.Child, Is.InstanceOf<TextBlock>());
        }

        [Test]
        public void CreateContainer_BackgroundBindingPathWithProperty_CreatesBindingContainer()
        {
            // Arrange
            var cellDefinition = new TextCellDefinition
            {
                BackgroundBindingPath = "BackgroundColor",
                BackgroundBindingSource = new { BackgroundColor = Colors.LightBlue },
                BindingPath = "TestProperty"
            };

            // Act
            var result = this.factory.CreateDisplayControl(cellDefinition);

            // Assert
            Assert.That(result, Is.Not.Null);
            // When a property path is provided, a Border container should be created
            Assert.That(result, Is.InstanceOf<Border>());
        }

        [Test]
        public void SetBackgroundBinding_EmptyPathWithBrush_SetsBackgroundDirectly()
        {
            // Arrange
            var brush = new SolidColorBrush(Colors.LightBlue);
            var cellDefinition = new CheckCellDefinition
            {
                BackgroundBindingPath = string.Empty,
                BackgroundBindingSource = brush
            };
            var checkBox = new CheckBox();

            // Act - Using reflection to access protected method
            var method = typeof(DataGridControlFactory).GetMethod(
                "SetBackgroundBinding",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new[] { typeof(CellDefinition), typeof(Control) },
                null);
            method.Invoke(this.factory, new object[] { cellDefinition, checkBox });

            // Assert
            Assert.That(checkBox.Background, Is.EqualTo(brush));
        }

        [Test]
        public void SetBackgroundBinding_BorderOverload_EmptyPathWithBrush_SetsBackgroundDirectly()
        {
            // Arrange
            var brush = new SolidColorBrush(Colors.LightGreen);
            var cellDefinition = new ProgressCellDefinition
            {
                BackgroundBindingPath = string.Empty,
                BackgroundBindingSource = brush
            };
            var border = new Border();

            // Act - Using reflection to access protected method
            var method = typeof(DataGridControlFactory).GetMethod(
                "SetBackgroundBinding",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new[] { typeof(CellDefinition), typeof(Border) },
                null);
            method.Invoke(this.factory, new object[] { cellDefinition, border });

            // Assert
            Assert.That(border.Background, Is.EqualTo(brush));
        }

        [Test]
        public void CreateContainer_EmptyBackgroundBindingPathWithBrush_Control_SetsBackgroundDirectly()
        {
            // Arrange - Test with a Control (not TextBlock) to verify direct Background setting
            var brush = new SolidColorBrush(Colors.Orange);
            
            // Use reflection to test CreateContainer directly with a Control
            var method = typeof(DataGridControlFactory).GetMethod(
                "CreateContainer",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var cellDef = new CheckCellDefinition
            {
                BackgroundBindingPath = string.Empty,
                BackgroundBindingSource = brush
            };
            
            var control = new CheckBox();

            // Act
            var result = (FrameworkElement)method.Invoke(this.factory, new object[] { cellDef, control });

            // Assert
            Assert.That(result, Is.SameAs(control)); // Should return the control itself, not wrap it
            Assert.That(control.Background, Is.EqualTo(brush));
        }
    }
}
