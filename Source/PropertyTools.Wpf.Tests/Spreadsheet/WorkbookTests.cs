// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkbookTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class WorkbookTests
    {
        [Test]
        public void IsModified_NewWorkbook_IsFalse()
        {
            var workbook = new Workbook();

            Assert.That(workbook.IsModified, Is.False);
        }

        [Test]
        public void IsModified_EditingCellOnAddedSheet_BecomesTrue()
        {
            var workbook = new Workbook();
            var sheet = new Sheet("Sheet1", 10, 10) { Culture = CultureInfo.InvariantCulture };
            workbook.Sheets.Add(sheet);

            sheet.SetCellText(new CellAddress(0, 0), "42");

            Assert.That(workbook.IsModified, Is.True);
        }

        [Test]
        public void IsModified_EditingCellOnRemovedSheet_StaysFalse()
        {
            var workbook = new Workbook();
            var sheet = new Sheet("Sheet1", 10, 10) { Culture = CultureInfo.InvariantCulture };
            workbook.Sheets.Add(sheet);
            workbook.Sheets.Remove(sheet);

            sheet.SetCellText(new CellAddress(0, 0), "42");

            Assert.That(workbook.IsModified, Is.False);
        }

        [Test]
        public void MarkSaved_AfterEdit_ClearsIsModified()
        {
            var workbook = new Workbook();
            var sheet = new Sheet("Sheet1", 10, 10) { Culture = CultureInfo.InvariantCulture };
            workbook.Sheets.Add(sheet);
            sheet.SetCellText(new CellAddress(0, 0), "42");

            workbook.MarkSaved();

            Assert.That(workbook.IsModified, Is.False);
        }

        [Test]
        public void ActiveSheet_Set_RaisesPropertyChanged()
        {
            var workbook = new Workbook();
            var sheet = new Sheet("Sheet1", 10, 10);
            var raised = false;
            workbook.PropertyChanged += (s, e) => raised = e.PropertyName == nameof(Workbook.ActiveSheet);

            workbook.ActiveSheet = sheet;

            Assert.That(raised, Is.True);
            Assert.That(workbook.ActiveSheet, Is.SameAs(sheet));
        }

        [Test]
        public void FilePath_Set_RaisesPropertyChanged()
        {
            var workbook = new Workbook();
            var raised = false;
            workbook.PropertyChanged += (s, e) => raised = e.PropertyName == nameof(Workbook.FilePath);

            workbook.FilePath = "book.ptsheet";

            Assert.That(raised, Is.True);
            Assert.That(workbook.FilePath, Is.EqualTo("book.ptsheet"));
        }
    }
}
