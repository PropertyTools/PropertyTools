// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridControlFactoryTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
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
        public void CreateContainer_EmptyBackgroundBindingPathWithBrush_SetsBackgroundDirectly()
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
            // The result should not be wrapped in a Border
            Assert.That(result, Is.InstanceOf<TextBlock>());
            // The background should be set directly on the TextBlock
            Assert.That(result.GetValue(Control.BackgroundProperty), Is.EqualTo(brush));
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
    }
}
