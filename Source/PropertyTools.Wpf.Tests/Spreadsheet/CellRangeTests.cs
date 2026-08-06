// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellRangeTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Linq;

    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class CellRangeTests
    {
        [Test]
        public void Constructor_ReversedCorners_Normalizes()
        {
            var range = new CellRange(new CellAddress(5, 5), new CellAddress(1, 1));

            Assert.That(range.TopLeft, Is.EqualTo(new CellAddress(1, 1)));
            Assert.That(range.BottomRight, Is.EqualTo(new CellAddress(5, 5)));
        }

        [Test]
        public void RowCountAndColumnCount_ReturnsExpectedCounts()
        {
            var range = new CellRange(new CellAddress(0, 0), new CellAddress(2, 3));

            Assert.That(range.RowCount, Is.EqualTo(3));
            Assert.That(range.ColumnCount, Is.EqualTo(4));
        }

        [Test]
        public void IsSingleCell_SingleAddress_ReturnsTrue()
        {
            var range = new CellRange(new CellAddress(3, 3));

            Assert.That(range.IsSingleCell, Is.True);
        }

        [Test]
        public void Contains_AddressInsideRange_ReturnsTrue()
        {
            var range = new CellRange(new CellAddress(0, 0), new CellAddress(4, 4));

            Assert.That(range.Contains(new CellAddress(2, 2)), Is.True);
        }

        [Test]
        public void Contains_AddressOutsideRange_ReturnsFalse()
        {
            var range = new CellRange(new CellAddress(0, 0), new CellAddress(4, 4));

            Assert.That(range.Contains(new CellAddress(5, 0)), Is.False);
        }

        [Test]
        public void Intersects_OverlappingRanges_ReturnsTrue()
        {
            var a = new CellRange(new CellAddress(0, 0), new CellAddress(4, 4));
            var b = new CellRange(new CellAddress(3, 3), new CellAddress(6, 6));

            Assert.That(a.Intersects(b), Is.True);
            Assert.That(b.Intersects(a), Is.True);
        }

        [Test]
        public void Intersects_DisjointRanges_ReturnsFalse()
        {
            var a = new CellRange(new CellAddress(0, 0), new CellAddress(1, 1));
            var b = new CellRange(new CellAddress(5, 5), new CellAddress(6, 6));

            Assert.That(a.Intersects(b), Is.False);
        }

        [Test]
        public void GetEnumerator_2x2Range_YieldsAllCellsInRowMajorOrder()
        {
            var range = new CellRange(new CellAddress(0, 0), new CellAddress(1, 1));

            var cells = range.ToList();

            Assert.That(cells, Is.EqualTo(new[]
            {
                new CellAddress(0, 0),
                new CellAddress(0, 1),
                new CellAddress(1, 0),
                new CellAddress(1, 1),
            }));
        }

        [Test]
        public void TryParse_SingleCell_ReturnsRangeOfOneCell()
        {
            var result = CellRange.TryParse("B2", out var range);

            Assert.That(result, Is.True);
            Assert.That(range.IsSingleCell, Is.True);
            Assert.That(range.TopLeft, Is.EqualTo(new CellAddress(1, 1)));
        }

        [Test]
        public void TryParse_RangeNotation_ReturnsExpectedCorners()
        {
            var result = CellRange.TryParse("A1:B10", out var range);

            Assert.That(result, Is.True);
            Assert.That(range.TopLeft, Is.EqualTo(new CellAddress(0, 0)));
            Assert.That(range.BottomRight, Is.EqualTo(new CellAddress(9, 1)));
        }

        [Test]
        public void TryParse_ReversedRangeNotation_Normalizes()
        {
            var result = CellRange.TryParse("B10:A1", out var range);

            Assert.That(result, Is.True);
            Assert.That(range.TopLeft, Is.EqualTo(new CellAddress(0, 0)));
            Assert.That(range.BottomRight, Is.EqualTo(new CellAddress(9, 1)));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("A1:")]
        [TestCase(":B2")]
        [TestCase("A1:ZZZ")]
        public void TryParse_InvalidRange_ReturnsFalse(string input)
        {
            var result = CellRange.TryParse(input, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void ToString_SingleCell_ReturnsAddressOnly()
        {
            var range = new CellRange(new CellAddress(0, 0));

            Assert.That(range.ToString(), Is.EqualTo("A1"));
        }

        [Test]
        public void ToString_MultiCellRange_ReturnsColonSeparatedCorners()
        {
            var range = new CellRange(new CellAddress(0, 0), new CellAddress(9, 1));

            Assert.That(range.ToString(), Is.EqualTo("A1:B10"));
        }

        [Test]
        public void Equals_SameCorners_ReturnsTrue()
        {
            var a = new CellRange(new CellAddress(0, 0), new CellAddress(1, 1));
            var b = new CellRange(new CellAddress(0, 0), new CellAddress(1, 1));

            Assert.That(a.Equals(b), Is.True);
            Assert.That(a == b, Is.True);
        }
    }
}
