// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvSerializerTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Globalization;
    using System.IO;
    using System.Text;

    using SpreadsheetDemo.Spreadsheet.Model;
    using SpreadsheetDemo.Spreadsheet.Model.Serialization;

    using NUnit.Framework;

    [TestFixture]
    public class CsvSerializerTests
    {
        private static string SaveToString(Sheet sheet)
        {
            using (var stream = new MemoryStream())
            {
                CsvSerializer.Save(sheet, stream);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static Sheet LoadFromString(string csv)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv)))
            {
                return CsvSerializer.Load(stream);
            }
        }

        [Test]
        public void Save_SimpleValues_WritesCommaSeparatedRows()
        {
            var sheet = new Sheet("Sheet1", 5, 5) { Culture = CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "a");
            sheet.SetCellText(new CellAddress(0, 1), "b");
            sheet.SetCellText(new CellAddress(1, 0), "1");
            sheet.SetCellText(new CellAddress(1, 1), "2");

            var csv = SaveToString(sheet);

            Assert.That(csv, Is.EqualTo("a,b\r\n1,2\r\n"));
        }

        [Test]
        public void Save_OnlyWritesTheUsedRange()
        {
            var sheet = new Sheet("Sheet1", 20, 20);
            sheet.SetCellText(new CellAddress(0, 0), "x");

            var csv = SaveToString(sheet);

            Assert.That(csv, Is.EqualTo("x\r\n"));
        }

        [Test]
        public void Save_EmptySheet_WritesNothing()
        {
            var sheet = new Sheet("Sheet1", 5, 5);

            Assert.That(SaveToString(sheet), Is.Empty);
        }

        [Test]
        public void Save_FieldWithCommaOrQuote_IsQuotedAndEscaped()
        {
            var sheet = new Sheet("Sheet1", 1, 1);
            sheet.SetCellText(new CellAddress(0, 0), "'a,\"b\"");

            Assert.That(SaveToString(sheet), Is.EqualTo("\"a,\"\"b\"\"\"\r\n"));
        }

        [Test]
        public void Load_SimpleValues_ParsesNumbersAndText()
        {
            var sheet = LoadFromString("a,b\r\n1,2\r\n");
            sheet.Culture = CultureInfo.InvariantCulture;

            Assert.That(sheet.RowCount, Is.EqualTo(2));
            Assert.That(sheet.ColumnCount, Is.EqualTo(2));
            Assert.That(sheet.GetValue(new CellAddress(0, 0)), Is.EqualTo(CellValue.FromText("a")));
            Assert.That(sheet.GetValue(new CellAddress(1, 0)), Is.EqualTo(CellValue.FromNumber(1)));
        }

        [Test]
        public void Load_QuotedFieldWithEmbeddedCommaAndQuote_ParsesRawText()
        {
            var sheet = LoadFromString("\"a,\"\"b\"\"\"\r\n");

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).Text, Is.EqualTo("a,\"b\""));
        }

        [Test]
        public void Load_RaggedRows_SizesToTheWidestRow()
        {
            var sheet = LoadFromString("a,b,c\r\nd\r\n");

            Assert.That(sheet.ColumnCount, Is.EqualTo(3));
            Assert.That(sheet.RowCount, Is.EqualTo(2));
        }

        [Test]
        public void Load_EmptyInput_ProducesOneByOneEmptySheet()
        {
            var sheet = LoadFromString(string.Empty);

            Assert.That(sheet.RowCount, Is.EqualTo(1));
            Assert.That(sheet.ColumnCount, Is.EqualTo(1));
            Assert.That(sheet.GetCell(new CellAddress(0, 0)).IsEmpty, Is.True);
        }

        [Test]
        public void RoundTrip_ValuesSurviveSaveAndLoad()
        {
            var sheet = new Sheet("Sheet1", 3, 3) { Culture = CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "42");
            sheet.SetCellText(new CellAddress(0, 1), "hello, world");
            sheet.SetCellText(new CellAddress(1, 0), "TRUE");

            var csv = SaveToString(sheet);
            var loaded = LoadFromString(csv);
            loaded.Culture = CultureInfo.InvariantCulture;

            Assert.That(loaded.GetValue(new CellAddress(0, 0)), Is.EqualTo(CellValue.FromNumber(42)));
            Assert.That(loaded.GetValue(new CellAddress(0, 1)), Is.EqualTo(CellValue.FromText("hello, world")));
            Assert.That(loaded.GetValue(new CellAddress(1, 0)), Is.EqualTo(CellValue.FromBoolean(true)));
        }

        [Test]
        public void Save_NullSheet_Throws()
        {
            using (var stream = new MemoryStream())
            {
                Assert.That(() => CsvSerializer.Save(null, stream), Throws.ArgumentNullException);
            }
        }
    }
}
