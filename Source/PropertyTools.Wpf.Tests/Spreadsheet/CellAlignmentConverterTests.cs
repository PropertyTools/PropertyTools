// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellAlignmentConverterTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;

    using SpreadsheetDemo.Spreadsheet.Converters;
    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    using HorizontalAlignment = System.Windows.HorizontalAlignment;

    [TestFixture]
    public class CellAlignmentConverterTests
    {
        private readonly CellAlignmentConverter converter = new CellAlignmentConverter();

        [Test]
        public void Convert_ExplicitRightAlignment_IgnoresValueType()
        {
            var result = this.converter.Convert(
                new object[] { CellHorizontalAlignment.Right, CellValue.FromText("hello") },
                typeof(HorizontalAlignment),
                null,
                null);

            Assert.That(result, Is.EqualTo(HorizontalAlignment.Right));
        }

        [Test]
        public void Convert_GeneralWithNumber_ReturnsRight()
        {
            var result = this.converter.Convert(
                new object[] { CellHorizontalAlignment.General, CellValue.FromNumber(1) },
                typeof(HorizontalAlignment),
                null,
                null);

            Assert.That(result, Is.EqualTo(HorizontalAlignment.Right));
        }

        [Test]
        public void Convert_GeneralWithBoolean_ReturnsCenter()
        {
            var result = this.converter.Convert(
                new object[] { CellHorizontalAlignment.General, CellValue.FromBoolean(true) },
                typeof(HorizontalAlignment),
                null,
                null);

            Assert.That(result, Is.EqualTo(HorizontalAlignment.Center));
        }

        [Test]
        public void Convert_GeneralWithText_ReturnsLeft()
        {
            var result = this.converter.Convert(
                new object[] { CellHorizontalAlignment.General, CellValue.FromText("hello") },
                typeof(HorizontalAlignment),
                null,
                null);

            Assert.That(result, Is.EqualTo(HorizontalAlignment.Left));
        }

        [Test]
        public void Convert_GeneralWithEmpty_ReturnsLeft()
        {
            var result = this.converter.Convert(
                new object[] { CellHorizontalAlignment.General, CellValue.Empty },
                typeof(HorizontalAlignment),
                null,
                null);

            Assert.That(result, Is.EqualTo(HorizontalAlignment.Left));
        }

        [Test]
        public void ConvertBack_Throws()
        {
            Assert.That(
                () => this.converter.ConvertBack(HorizontalAlignment.Left, new[] { typeof(CellHorizontalAlignment) }, null, null),
                Throws.TypeOf<NotSupportedException>());
        }
    }
}
