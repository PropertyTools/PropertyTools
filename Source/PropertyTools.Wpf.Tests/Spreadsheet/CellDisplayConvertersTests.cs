// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellDisplayConvertersTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Windows;
    using System.Windows.Media;

    using SpreadsheetDemo.Spreadsheet.Converters;

    using NUnit.Framework;

    [TestFixture]
    public class BoolToFontWeightConverterTests
    {
        private readonly BoolToFontWeightConverter converter = new BoolToFontWeightConverter();

        [Test]
        public void Convert_True_ReturnsBold()
        {
            Assert.That(this.converter.Convert(true, typeof(FontWeight), null, null), Is.EqualTo(FontWeights.Bold));
        }

        [Test]
        public void Convert_False_ReturnsNormal()
        {
            Assert.That(this.converter.Convert(false, typeof(FontWeight), null, null), Is.EqualTo(FontWeights.Normal));
        }
    }

    [TestFixture]
    public class BoolToFontStyleConverterTests
    {
        private readonly BoolToFontStyleConverter converter = new BoolToFontStyleConverter();

        [Test]
        public void Convert_True_ReturnsItalic()
        {
            Assert.That(this.converter.Convert(true, typeof(FontStyle), null, null), Is.EqualTo(FontStyles.Italic));
        }

        [Test]
        public void Convert_False_ReturnsNormal()
        {
            Assert.That(this.converter.Convert(false, typeof(FontStyle), null, null), Is.EqualTo(FontStyles.Normal));
        }
    }

    [TestFixture]
    public class ErrorForegroundConverterTests
    {
        private readonly ErrorForegroundConverter converter = new ErrorForegroundConverter();

        [Test]
        public void Convert_True_ReturnsRed()
        {
            Assert.That(this.converter.Convert(true, typeof(Brush), null, null), Is.EqualTo(Brushes.Red));
        }

        [Test]
        public void Convert_False_ReturnsControlTextBrush()
        {
            Assert.That(this.converter.Convert(false, typeof(Brush), null, null), Is.EqualTo(SystemColors.ControlTextBrush));
        }
    }
}
