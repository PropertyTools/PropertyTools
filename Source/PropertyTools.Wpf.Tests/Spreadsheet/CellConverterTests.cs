// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellConverterTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class CellConverterTests
    {
        [Test]
        public void CanConvertFrom_String_ReturnsTrue()
        {
            var converter = new CellConverter();

            Assert.That(converter.CanConvertFrom(null, typeof(string)), Is.True);
        }

        [Test]
        public void CanConvertFrom_Int32_ReturnsFalse()
        {
            var converter = new CellConverter();

            Assert.That(converter.CanConvertFrom(null, typeof(int)), Is.False);
        }

        [Test]
        public void ConvertFrom_String_ReturnsSameText()
        {
            var converter = new CellConverter();

            var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, "42");

            Assert.That(result, Is.EqualTo("42"));
        }
    }
}
