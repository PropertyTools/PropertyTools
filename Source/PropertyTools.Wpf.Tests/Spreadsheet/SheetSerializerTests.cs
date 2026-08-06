// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SheetSerializerTests.cs" company="PropertyTools">
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
    public class SheetSerializerTests
    {
        private static Sheet RoundTrip(Sheet sheet)
        {
            using (var stream = new MemoryStream())
            {
                SheetSerializer.Save(sheet, stream);
                stream.Position = 0;
                return SheetSerializer.Load(stream);
            }
        }

        [Test]
        public void RoundTrip_PreservesDimensions()
        {
            var sheet = new Sheet("Sheet1", 12, 7);

            var loaded = RoundTrip(sheet);

            Assert.That(loaded.RowCount, Is.EqualTo(12));
            Assert.That(loaded.ColumnCount, Is.EqualTo(7));
        }

        [Test]
        public void RoundTrip_PreservesLiteralValue()
        {
            var sheet = new Sheet("Sheet1", 5, 5) { Culture = CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(1, 1), "42");

            var loaded = RoundTrip(sheet);
            loaded.Culture = CultureInfo.InvariantCulture;

            Assert.That(loaded.GetValue(new CellAddress(1, 1)), Is.EqualTo(CellValue.FromNumber(42)));
        }

        [Test]
        public void RoundTrip_PreservesFormulaTextAndRecalculates()
        {
            var sheet = new Sheet("Sheet1", 5, 5) { Culture = CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "3");
            sheet.SetCellText(new CellAddress(0, 1), "=A1+1");

            var loaded = RoundTrip(sheet);

            Assert.That(loaded.GetCell(new CellAddress(0, 1)).Content.IsFormula, Is.True);
            Assert.That(loaded.GetValue(new CellAddress(0, 1)), Is.EqualTo(CellValue.FromNumber(4)));
        }

        [Test]
        public void RoundTrip_PreservesStyle()
        {
            var sheet = new Sheet("Sheet1", 5, 5) { Culture = CultureInfo.InvariantCulture };
            var address = new CellAddress(2, 2);
            sheet.SetCellText(address, "text");
            sheet.SetCellStyle(address, CellStyle.Default.WithBold(true).WithItalic(true).WithHorizontalAlignment(CellHorizontalAlignment.Right).WithFormat("0.00"));

            var loaded = RoundTrip(sheet);

            var style = loaded.GetCell(address).Style;
            Assert.That(style.Bold, Is.True);
            Assert.That(style.Italic, Is.True);
            Assert.That(style.HorizontalAlignment, Is.EqualTo(CellHorizontalAlignment.Right));
            Assert.That(style.FormatString, Is.EqualTo("0.00"));
        }

        [Test]
        public void RoundTrip_StyledButEmptyCell_PreservesStyleWithoutContent()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var address = new CellAddress(0, 0);
            sheet.SetCellStyle(address, CellStyle.Default.WithBold(true));

            var loaded = RoundTrip(sheet);

            Assert.That(loaded.GetCell(address).IsEmpty, Is.True);
            Assert.That(loaded.GetCell(address).Style.Bold, Is.True);
        }

        [Test]
        public void RoundTrip_UntouchedSheet_ProducesEmptySheetOfSameSize()
        {
            var sheet = new Sheet("Sheet1", 5, 5);

            var loaded = RoundTrip(sheet);

            Assert.That(loaded.TryGetCell(new CellAddress(0, 0), out _), Is.False);
        }

        [Test]
        public void Save_UnmaterializedDefaultCells_AreNotWritten()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(0, 0), "x");
            // Reading other cells materializes them without changing content/style.
            sheet.GetCell(new CellAddress(1, 1));

            using (var stream = new MemoryStream())
            {
                SheetSerializer.Save(sheet, stream);
                var json = Encoding.UTF8.GetString(stream.ToArray());

                Assert.That(json, Does.Contain("\"A1\""));
                Assert.That(json, Does.Not.Contain("\"B2\""));
            }
        }

        [Test]
        public void Load_MalformedJson_ThrowsInvalidDataException()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("not json")))
            {
                Assert.That(() => SheetSerializer.Load(stream), Throws.TypeOf<InvalidDataException>());
            }
        }

        [Test]
        public void Load_MissingDimensions_ThrowsInvalidDataException()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("{}")))
            {
                Assert.That(() => SheetSerializer.Load(stream), Throws.TypeOf<InvalidDataException>());
            }
        }

        [Test]
        public void Save_NullSheet_Throws()
        {
            using (var stream = new MemoryStream())
            {
                Assert.That(() => SheetSerializer.Save(null, stream), Throws.ArgumentNullException);
            }
        }
    }
}
