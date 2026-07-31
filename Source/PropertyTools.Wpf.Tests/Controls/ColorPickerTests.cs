// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPickerTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Windows.Media;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    /// <summary>
    /// Headless component tests for <see cref="ColorPicker" />.
    /// </summary>
    public class ColorPickerTests : WpfTestBase
    {
        [Test]
        public void SelectedColor_SetValue_RoundTrips()
        {
            // Arrange
            var colorPicker = new ColorPicker();

            // Act
            colorPicker.SelectedColor = Colors.Red;

            // Assert
            Assert.That(colorPicker.SelectedColor, Is.EqualTo(Colors.Red));
        }

        [Test]
        public void SelectedColor_DefaultValue_IsNotNull()
        {
            // Arrange / Act
            var colorPicker = new ColorPicker();

            // Assert
            Assert.That(colorPicker.SelectedColor, Is.Not.Null);
        }

        [Test]
        public void PrepareForLayout_DefaultInstance_DoesNotThrow()
        {
            // Arrange
            var colorPicker = new ColorPicker();

            // Act / Assert
            Assert.That(() => PrepareForLayout(colorPicker), Throws.Nothing);
        }
    }
}
