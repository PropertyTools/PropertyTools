// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellStyleTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class CellStyleTests
    {
        [Test]
        public void Default_IsGeneralAlignmentNotBoldNotItalicNoFormat()
        {
            var style = CellStyle.Default;

            Assert.That(style.HorizontalAlignment, Is.EqualTo(CellHorizontalAlignment.General));
            Assert.That(style.Bold, Is.False);
            Assert.That(style.Italic, Is.False);
            Assert.That(style.FormatString, Is.Null);
        }

        [Test]
        public void WithBold_SameValue_ReturnsSameInstance()
        {
            var style = CellStyle.Default;

            var result = style.WithBold(false);

            Assert.That(result, Is.SameAs(style));
        }

        [Test]
        public void WithBold_DifferentValue_ReturnsBoldStyle()
        {
            var style = CellStyle.Default.WithBold(true);

            Assert.That(style.Bold, Is.True);
            Assert.That(style.HorizontalAlignment, Is.EqualTo(CellHorizontalAlignment.General));
        }

        [Test]
        public void WithBold_TwoEqualStyles_AreInternedToSameInstance()
        {
            var a = CellStyle.Default.WithBold(true);
            var b = CellStyle.Default.WithBold(true);

            Assert.That(a, Is.SameAs(b));
        }

        [Test]
        public void WithHorizontalAlignment_ThenWithBold_CombinesBothChanges()
        {
            var style = CellStyle.Default.WithHorizontalAlignment(CellHorizontalAlignment.Right).WithBold(true);

            Assert.That(style.HorizontalAlignment, Is.EqualTo(CellHorizontalAlignment.Right));
            Assert.That(style.Bold, Is.True);
        }

        [Test]
        public void WithFormat_DifferentFormat_ReturnsStyleWithFormat()
        {
            var style = CellStyle.Default.WithFormat("0.00");

            Assert.That(style.FormatString, Is.EqualTo("0.00"));
        }

        [Test]
        public void WithFormat_SameFormat_ReturnsSameInstance()
        {
            var style = CellStyle.Default.WithFormat("0.00");

            var result = style.WithFormat("0.00");

            Assert.That(result, Is.SameAs(style));
        }

        [Test]
        public void Equals_SameProperties_ReturnsTrue()
        {
            var a = CellStyle.Default.WithBold(true).WithItalic(true);
            var b = CellStyle.Default.WithItalic(true).WithBold(true);

            Assert.That(a.Equals(b), Is.True);
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void Equals_Null_ReturnsFalse()
        {
            Assert.That(CellStyle.Default.Equals(null), Is.False);
        }
    }
}
