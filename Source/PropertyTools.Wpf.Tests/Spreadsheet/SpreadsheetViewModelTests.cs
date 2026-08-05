// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetViewModelTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using DataGridDemo.Spreadsheet;
    using DataGridDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class SpreadsheetViewModelTests
    {
        [Test]
        public void Constructor_Default_Creates100By26Sheet()
        {
            var viewModel = new SpreadsheetViewModel();

            Assert.That(viewModel.Sheet.RowCount, Is.EqualTo(100));
            Assert.That(viewModel.Sheet.ColumnCount, Is.EqualTo(26));
            Assert.That(viewModel.Rows.Count, Is.EqualTo(100));
        }

        [Test]
        public void RowHeaders_MatchOneBasedRowNumbers()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 3, 3));

            Assert.That(viewModel.RowHeaders, Is.EqualTo(new[] { "1", "2", "3" }));
        }

        [Test]
        public void ColumnHeaders_MatchColumnNames()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 3, 28));

            Assert.That(viewModel.ColumnHeaders[0], Is.EqualTo("A"));
            Assert.That(viewModel.ColumnHeaders[25], Is.EqualTo("Z"));
            Assert.That(viewModel.ColumnHeaders[26], Is.EqualTo("AA"));
            Assert.That(viewModel.ColumnHeaders[27], Is.EqualTo("AB"));
        }

        [Test]
        public void ControlFactory_IsSpreadsheetControlFactory()
        {
            var viewModel = new SpreadsheetViewModel();

            Assert.That(viewModel.ControlFactory, Is.InstanceOf<SpreadsheetControlFactory>());
        }
    }
}
