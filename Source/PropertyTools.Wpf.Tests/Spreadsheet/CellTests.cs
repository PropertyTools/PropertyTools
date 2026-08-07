// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Collections.Generic;
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class CellTests
    {
        private static Sheet CreateSheet()
        {
            return new Sheet("Sheet1", 10, 10) { Culture = CultureInfo.InvariantCulture };
        }

        [Test]
        public void Text_Get_ReturnsEditTextForCurrentContent()
        {
            var sheet = CreateSheet();
            var cell = sheet.GetCell(new CellAddress(0, 0));

            cell.Text = "42";

            Assert.That(cell.Text, Is.EqualTo("42"));
        }

        [Test]
        public void Text_Set_RoutesThroughSheet()
        {
            var sheet = CreateSheet();
            var address = new CellAddress(0, 0);
            var cell = sheet.GetCell(address);

            cell.Text = "Hello";

            Assert.That(sheet.GetValue(address).AsText(), Is.EqualTo("Hello"));
        }

        [Test]
        public void DisplayText_NumberWithFormat_AppliesStyleFormat()
        {
            var sheet = CreateSheet();
            var address = new CellAddress(0, 0);
            var cell = sheet.GetCell(address);
            cell.Text = "3.14159";

            sheet.SetStyle(new CellRange(address), s => s.WithFormat("0.0"));

            Assert.That(cell.DisplayText, Is.EqualTo("3.1"));
        }

        [Test]
        public void IsEmpty_NewCell_ReturnsTrue()
        {
            var sheet = CreateSheet();

            var cell = sheet.GetCell(new CellAddress(0, 0));

            Assert.That(cell.IsEmpty, Is.True);
        }

        [Test]
        public void ToString_ReturnsDisplayText()
        {
            var sheet = CreateSheet();
            var cell = sheet.GetCell(new CellAddress(0, 0));
            cell.Text = "42";

            Assert.That(cell.ToString(), Is.EqualTo(cell.DisplayText));
        }

        [Test]
        public void PropertyChanged_SettingText_RaisesContentValueAndDisplayText()
        {
            var sheet = CreateSheet();
            var cell = sheet.GetCell(new CellAddress(0, 0));
            var raised = new List<string>();
            cell.PropertyChanged += (s, e) => raised.Add(e.PropertyName);

            cell.Text = "42";

            Assert.That(raised, Does.Contain(nameof(Cell.Content)));
            Assert.That(raised, Does.Contain(nameof(Cell.Value)));
            Assert.That(raised, Does.Contain(nameof(Cell.DisplayText)));
            Assert.That(raised, Does.Contain(nameof(Cell.Text)));
        }

        [Test]
        public void PropertyChanged_SettingSameNumericValueTwice_DoesNotRaiseValueTwice()
        {
            var sheet = CreateSheet();
            var cell = sheet.GetCell(new CellAddress(0, 0));
            cell.Text = "42";

            var valueChangedCount = 0;
            cell.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Cell.Value))
                {
                    valueChangedCount++;
                }
            };

            cell.Text = "42";

            Assert.That(valueChangedCount, Is.EqualTo(0));
        }

        [Test]
        public void PropertyChanged_ChangingStyle_RaisesStyleAndDisplayText()
        {
            var sheet = CreateSheet();
            var address = new CellAddress(0, 0);
            var cell = sheet.GetCell(address);
            var raised = new List<string>();
            cell.PropertyChanged += (s, e) => raised.Add(e.PropertyName);

            sheet.SetStyle(new CellRange(address), s => s.WithBold(true));

            Assert.That(raised, Does.Contain(nameof(Cell.Style)));
            Assert.That(raised, Does.Contain(nameof(Cell.DisplayText)));
        }
    }
}
