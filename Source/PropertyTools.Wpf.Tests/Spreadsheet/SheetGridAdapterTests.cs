// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SheetGridAdapterTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Collections;

    using SpreadsheetDemo.Spreadsheet;
    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class SheetGridAdapterTests
    {
        [Test]
        public void Count_MatchesSheetRowCount()
        {
            var sheet = new Sheet("Sheet1", 10, 5);

            var adapter = new SheetGridAdapter(sheet);

            Assert.That(adapter.Count, Is.EqualTo(10));
        }

        [Test]
        public void Indexer_SameRowTwice_ReturnsSameRowAdapterInstance()
        {
            var sheet = new Sheet("Sheet1", 10, 5);
            var adapter = new SheetGridAdapter(sheet);

            Assert.That(adapter[3], Is.SameAs(adapter[3]));
        }

        [Test]
        public void Indexer_ReturnsRowAdapterForCorrectRow()
        {
            var sheet = new Sheet("Sheet1", 10, 5);
            var adapter = new SheetGridAdapter(sheet);

            Assert.That(adapter[3].Row, Is.EqualTo(3));
            Assert.That(adapter[3].Sheet, Is.SameAs(sheet));
        }

        [Test]
        public void NonGenericIList_Contains_FindsRowFromThisSheet()
        {
            var sheet = new Sheet("Sheet1", 10, 5);
            var adapter = new SheetGridAdapter(sheet);
            var list = (IList)adapter;

            Assert.That(list.Contains(adapter[2]), Is.True);
        }

        [Test]
        public void NonGenericIList_IsFixedSizeAndReadOnly()
        {
            var adapter = new SheetGridAdapter(new Sheet("Sheet1", 10, 5));
            var list = (IList)adapter;

            Assert.That(list.IsFixedSize, Is.True);
            Assert.That(list.IsReadOnly, Is.True);
        }

        [Test]
        public void Add_Throws()
        {
            var adapter = new SheetGridAdapter(new Sheet("Sheet1", 10, 5));

            Assert.That(() => adapter.Add(null), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void Constructor_NullSheet_Throws()
        {
            Assert.That(() => new SheetGridAdapter(null), Throws.TypeOf<ArgumentNullException>());
        }
    }
}
