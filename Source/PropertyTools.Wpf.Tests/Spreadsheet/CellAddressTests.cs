// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellAddressTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Collections.Generic;

    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class CellAddressTests
    {
        [TestCase(0, "A")]
        [TestCase(1, "B")]
        [TestCase(25, "Z")]
        [TestCase(26, "AA")]
        [TestCase(27, "AB")]
        [TestCase(51, "AZ")]
        [TestCase(701, "ZZ")]
        [TestCase(702, "AAA")]
        public void ToColumnName_KnownIndex_ReturnsExpectedName(int column, string expected)
        {
            Assert.That(CellAddress.ToColumnName(column), Is.EqualTo(expected));
        }

        [TestCase("A", 0)]
        [TestCase("B", 1)]
        [TestCase("Z", 25)]
        [TestCase("AA", 26)]
        [TestCase("aa", 26)]
        [TestCase("ZZ", 701)]
        [TestCase("AAA", 702)]
        public void ParseColumnName_KnownName_ReturnsExpectedIndex(string name, int expected)
        {
            Assert.That(CellAddress.ParseColumnName(name), Is.EqualTo(expected));
        }

        [Test]
        public void ParseColumnName_Empty_Throws()
        {
            Assert.That(() => CellAddress.ParseColumnName(string.Empty), Throws.TypeOf<FormatException>());
        }

        [TestCase("A1", 0, 0)]
        [TestCase("B12", 11, 1)]
        [TestCase("AA10", 9, 26)]
        [TestCase("$A$1", 0, 0)]
        [TestCase("a1", 0, 0)]
        public void TryParse_ValidAddress_ReturnsExpectedRowAndColumn(string input, int expectedRow, int expectedColumn)
        {
            var result = CellAddress.TryParse(input, out var address);

            Assert.That(result, Is.True);
            Assert.That(address.Row, Is.EqualTo(expectedRow));
            Assert.That(address.Column, Is.EqualTo(expectedColumn));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("1A")]
        [TestCase("A0")]
        [TestCase("A")]
        [TestCase("1")]
        [TestCase("A1B")]
        [TestCase("A1 ")]
        public void TryParse_InvalidAddress_ReturnsFalse(string input)
        {
            var result = CellAddress.TryParse(input, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void ToString_A1_ReturnsA1()
        {
            var address = new CellAddress(0, 0);

            Assert.That(address.ToString(), Is.EqualTo("A1"));
        }

        [Test]
        public void ToString_RoundTripsWithTryParse()
        {
            var address = new CellAddress(41, 27);

            var text = address.ToString();
            var parsed = CellAddress.TryParse(text, out var result);

            Assert.That(parsed, Is.True);
            Assert.That(result, Is.EqualTo(address));
        }

        [Test]
        public void Equals_SameRowAndColumn_ReturnsTrue()
        {
            var a = new CellAddress(1, 2);
            var b = new CellAddress(1, 2);

            Assert.That(a.Equals(b), Is.True);
            Assert.That(a == b, Is.True);
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void Equals_DifferentColumn_ReturnsFalse()
        {
            var a = new CellAddress(1, 2);
            var b = new CellAddress(1, 3);

            Assert.That(a.Equals(b), Is.False);
            Assert.That(a != b, Is.True);
        }

        [Test]
        public void CompareTo_SortsInRowMajorOrder()
        {
            var addresses = new List<CellAddress>
            {
                new CellAddress(1, 0),
                new CellAddress(0, 1),
                new CellAddress(0, 0),
            };

            addresses.Sort();

            Assert.That(addresses[0], Is.EqualTo(new CellAddress(0, 0)));
            Assert.That(addresses[1], Is.EqualTo(new CellAddress(0, 1)));
            Assert.That(addresses[2], Is.EqualTo(new CellAddress(1, 0)));
        }

        [Test]
        public void Constructor_NegativeRow_Throws()
        {
            Assert.That(() => new CellAddress(-1, 0), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
