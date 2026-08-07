// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SheetRowAdapterTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Collections;
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet;
    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class SheetRowAdapterTests
    {
        private static Sheet CreateSheet()
        {
            return new Sheet("Sheet1", 10, 10) { Culture = CultureInfo.InvariantCulture };
        }

        [Test]
        public void Indexer_Get_ReturnsCellAtRowAndColumn()
        {
            var sheet = CreateSheet();
            var row = new SheetRowAdapter(sheet, 2);

            var cell = row[3];

            Assert.That(cell.Address, Is.EqualTo(new CellAddress(2, 3)));
        }

        [Test]
        public void Indexer_SetString_ParsesAndSetsCellText()
        {
            var sheet = CreateSheet();
            var row = new SheetRowAdapter(sheet, 0);

            ((IList)row)[0] = "42";

            Assert.That(sheet.GetValue(new CellAddress(0, 0)), Is.EqualTo(CellValue.FromNumber(42)));
        }

        [Test]
        public void Indexer_SetNull_ClearsCell()
        {
            var sheet = CreateSheet();
            var row = new SheetRowAdapter(sheet, 0);
            ((IList)row)[0] = "42";

            ((IList)row)[0] = null;

            Assert.That(sheet.GetValue(new CellAddress(0, 0)).IsEmpty, Is.True);
        }

        [Test]
        public void Indexer_SetCell_CopiesContentWithoutSharingIdentity()
        {
            var sheet = CreateSheet();
            var row = new SheetRowAdapter(sheet, 0);
            ((IList)row)[0] = "42";
            var source = sheet.GetCell(new CellAddress(0, 0));

            row[1] = source;

            var target = sheet.GetCell(new CellAddress(0, 1));
            Assert.That(target, Is.Not.SameAs(source));
            Assert.That(target.Value, Is.EqualTo(source.Value));
        }

        [Test]
        public void Count_MatchesSheetColumnCount()
        {
            var sheet = CreateSheet();
            var row = new SheetRowAdapter(sheet, 0);

            Assert.That(row.Count, Is.EqualTo(10));
        }

        [Test]
        public void IndexOf_CellFromSameRow_ReturnsColumn()
        {
            var sheet = CreateSheet();
            var row = new SheetRowAdapter(sheet, 4);
            var cell = sheet.GetCell(new CellAddress(4, 7));

            Assert.That(row.IndexOf(cell), Is.EqualTo(7));
        }

        [Test]
        public void IndexOf_CellFromDifferentRow_ReturnsMinusOne()
        {
            var sheet = CreateSheet();
            var row = new SheetRowAdapter(sheet, 4);
            var cell = sheet.GetCell(new CellAddress(5, 7));

            Assert.That(row.IndexOf(cell), Is.EqualTo(-1));
        }

        [Test]
        public void Add_Throws()
        {
            var row = new SheetRowAdapter(CreateSheet(), 0);

            Assert.That(() => row.Add(null), Throws.TypeOf<NotSupportedException>());
        }
    }
}
