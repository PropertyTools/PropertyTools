// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SheetTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class SheetTests
    {
        private static Sheet CreateSheet()
        {
            return new Sheet("Sheet1", 10, 10) { Culture = CultureInfo.InvariantCulture };
        }

        [Test]
        public void SetCellText_Number_UpdatesCellValue()
        {
            var sheet = CreateSheet();

            sheet.SetCellText(new CellAddress(0, 0), "42");

            Assert.That(sheet.GetValue(new CellAddress(0, 0)), Is.EqualTo(CellValue.FromNumber(42)));
        }

        [Test]
        public void SetCellText_FormulaWithParserExplicitlyDisabled_EvaluatesToNameError()
        {
            var sheet = CreateSheet();
            sheet.FormulaParser = null;

            sheet.SetCellText(new CellAddress(1, 1), "=A1+1");

            var value = sheet.GetValue(new CellAddress(1, 1));
            Assert.That(value.IsError, Is.True);
            Assert.That(value.Error, Is.EqualTo(CellError.Name));
        }

        [Test]
        public void SetCellText_Formula_UsesDefaultParserAndEvaluatorByDefault()
        {
            var sheet = CreateSheet();
            sheet.SetCellText(new CellAddress(0, 0), "1");

            sheet.SetCellText(new CellAddress(1, 1), "=A1+1");

            Assert.That(sheet.GetValue(new CellAddress(1, 1)), Is.EqualTo(CellValue.FromNumber(2)));
        }

        [Test]
        public void GetValue_UnwrittenCell_ReturnsEmptyWithoutMaterializing()
        {
            var sheet = CreateSheet();

            var value = sheet.GetValue(new CellAddress(5, 5));

            Assert.That(value.IsEmpty, Is.True);
            Assert.That(sheet.TryGetCell(new CellAddress(5, 5), out _), Is.False);
        }

        [Test]
        public void GetCell_SameAddressTwice_ReturnsSameInstance()
        {
            var sheet = CreateSheet();

            var a = sheet.GetCell(new CellAddress(1, 1));
            var b = sheet.GetCell(new CellAddress(1, 1));

            Assert.That(a, Is.SameAs(b));
        }

        [Test]
        public void GetCell_AddressOutsideBounds_Throws()
        {
            var sheet = CreateSheet();

            Assert.That(() => sheet.GetCell(new CellAddress(100, 0)), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ClearContents_ClearsValueButKeepsStyle()
        {
            var sheet = CreateSheet();
            var address = new CellAddress(0, 0);
            sheet.SetCellText(address, "42");
            sheet.SetStyle(new CellRange(address), s => s.WithBold(true));

            sheet.ClearContents(new CellRange(address));

            Assert.That(sheet.GetValue(address).IsEmpty, Is.True);
            Assert.That(sheet.GetCell(address).Style.Bold, Is.True);
        }

        [Test]
        public void SetStyle_AppliesTransformToEveryCellInRange()
        {
            var sheet = CreateSheet();
            var range = new CellRange(new CellAddress(0, 0), new CellAddress(1, 1));

            sheet.SetStyle(range, s => s.WithBold(true));

            foreach (var address in range)
            {
                Assert.That(sheet.GetCell(address).Style.Bold, Is.True);
            }
        }

        [Test]
        public void CopyCell_CopiesContentAndStyle_WithoutSharingCellIdentity()
        {
            var sheet = CreateSheet();
            var sourceAddress = new CellAddress(0, 0);
            var targetAddress = new CellAddress(0, 1);
            sheet.SetCellText(sourceAddress, "42");
            sheet.SetStyle(new CellRange(sourceAddress), s => s.WithBold(true));
            var source = sheet.GetCell(sourceAddress);

            sheet.CopyCell(targetAddress, source);

            var target = sheet.GetCell(targetAddress);
            Assert.That(target, Is.Not.SameAs(source));
            Assert.That(target.Value, Is.EqualTo(source.Value));
            Assert.That(target.Style.Bold, Is.True);
        }

        [Test]
        public void SetCellText_RaisesCellChangedWithAddress()
        {
            var sheet = CreateSheet();
            var address = new CellAddress(2, 3);
            CellAddress? changedAddress = null;
            sheet.CellChanged += (s, e) => changedAddress = e.Address;

            sheet.SetCellText(address, "1");

            Assert.That(changedAddress, Is.EqualTo(address));
        }

        [Test]
        public void ClearContents_AlreadyEmptyCell_DoesNotRaiseCellChanged()
        {
            var sheet = CreateSheet();
            var address = new CellAddress(0, 0);
            var raised = false;
            sheet.CellChanged += (s, e) => raised = true;

            sheet.ClearContents(new CellRange(address));

            Assert.That(raised, Is.False);
        }

        [Test]
        public void TryGetCell_UnwrittenCell_ReturnsFalse()
        {
            var sheet = CreateSheet();

            var result = sheet.TryGetCell(new CellAddress(0, 0), out var cell);

            Assert.That(result, Is.False);
            Assert.That(cell, Is.Null);
        }

        [Test]
        public void Constructor_NonPositiveRowCount_Throws()
        {
            Assert.That(() => new Sheet("Sheet1", 0, 10), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
