// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellFormatterTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet.Model;
    using SpreadsheetDemo.Spreadsheet.Model.Formulas;

    using NUnit.Framework;

    [TestFixture]
    public class CellFormatterTests
    {
        private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

        [Test]
        public void ToEditText_EmptyContent_ReturnsEmptyString()
        {
            Assert.That(CellFormatter.ToEditText(CellContent.Empty, Invariant), Is.EqualTo(string.Empty));
        }

        [Test]
        public void ToEditText_Number_ReturnsInvariantNumberText()
        {
            var content = CellContent.FromValue(CellValue.FromNumber(3.5));

            Assert.That(CellFormatter.ToEditText(content, Invariant), Is.EqualTo("3.5"));
        }

        [Test]
        public void ToEditText_Boolean_ReturnsTrueOrFalse()
        {
            var content = CellContent.FromValue(CellValue.FromBoolean(true));

            Assert.That(CellFormatter.ToEditText(content, Invariant), Is.EqualTo("TRUE"));
        }

        [Test]
        public void ToEditText_TextThatLooksLikeANumber_IsPrefixedWithApostrophe()
        {
            var content = CellContent.FromValue(CellValue.FromText("123"));

            Assert.That(CellFormatter.ToEditText(content, Invariant), Is.EqualTo("'123"));
        }

        [Test]
        public void ToEditText_PlainText_IsNotPrefixed()
        {
            var content = CellContent.FromValue(CellValue.FromText("Hello"));

            Assert.That(CellFormatter.ToEditText(content, Invariant), Is.EqualTo("Hello"));
        }

        [Test]
        public void ToEditText_Formula_ReturnsLeadingEqualsPlusText()
        {
            var formula = new Formula("A1+1", null, Array.Empty<CellAddress>(), Array.Empty<CellRange>());
            var content = CellContent.FromFormula(formula);

            Assert.That(CellFormatter.ToEditText(content, Invariant), Is.EqualTo("=A1+1"));
        }

        [Test]
        public void ToEditText_ThenParse_RoundTripsForNumber()
        {
            var content = CellContent.FromValue(CellValue.FromNumber(42));

            var text = CellFormatter.ToEditText(content, Invariant);
            var parsed = CellInputParser.Parse(text, null, Invariant);

            Assert.That(parsed.Value, Is.EqualTo(content.Value));
        }

        [Test]
        public void ToEditText_ThenParse_RoundTripsForTextThatLooksNumeric()
        {
            var content = CellContent.FromValue(CellValue.FromText("2026"));

            var text = CellFormatter.ToEditText(content, Invariant);
            var parsed = CellInputParser.Parse(text, null, Invariant);

            Assert.That(parsed.Value.Type, Is.EqualTo(CellValueType.Text));
            Assert.That(parsed.Value.AsText(), Is.EqualTo("2026"));
        }

        [Test]
        public void ToDisplayText_Empty_ReturnsEmptyString()
        {
            Assert.That(CellFormatter.ToDisplayText(CellValue.Empty, null, Invariant), Is.EqualTo(string.Empty));
        }

        [Test]
        public void ToDisplayText_Error_ReturnsErrorText()
        {
            var value = CellValue.FromError(CellError.DivideByZero);

            Assert.That(CellFormatter.ToDisplayText(value, null, Invariant), Is.EqualTo("#DIV/0!"));
        }

        [Test]
        public void ToDisplayText_NumberWithoutFormat_ReturnsInvariantNumberText()
        {
            var value = CellValue.FromNumber(1234.5);

            Assert.That(CellFormatter.ToDisplayText(value, null, Invariant), Is.EqualTo("1234.5"));
        }

        [Test]
        public void ToDisplayText_NumberWithFormatString_AppliesFormat()
        {
            var value = CellValue.FromNumber(3.14159);
            var style = CellStyle.Default.WithFormat("0.00");

            Assert.That(CellFormatter.ToDisplayText(value, style, Invariant), Is.EqualTo("3.14"));
        }

        [Test]
        public void ToDisplayText_NumberWithInvalidFormatString_FallsBackToGeneral()
        {
            var value = CellValue.FromNumber(3);

            // A standard format specifier with a precision beyond what .NET accepts throws FormatException.
            var style = CellStyle.Default.WithFormat("F99999999999");

            Assert.That(CellFormatter.ToDisplayText(value, style, Invariant), Is.EqualTo("3"));
        }

        [Test]
        public void ToDisplayText_Boolean_ReturnsTrueOrFalse()
        {
            Assert.That(CellFormatter.ToDisplayText(CellValue.FromBoolean(false), null, Invariant), Is.EqualTo("FALSE"));
        }

        [Test]
        public void ToEditText_Duration_ReturnsUnpaddedHoursMinutesSeconds()
        {
            var content = CellContent.FromValue(CellValue.FromDuration(new TimeSpan(1, 5, 3)));

            Assert.That(CellFormatter.ToEditText(content, Invariant), Is.EqualTo("1:05:03"));
        }

        [Test]
        public void ToEditText_ThenParse_RoundTripsForDuration()
        {
            var content = CellContent.FromValue(CellValue.FromDuration(TimeSpan.FromHours(30)));

            var text = CellFormatter.ToEditText(content, Invariant);
            var reparsed = CellInputParser.Parse(text, null, Invariant);

            Assert.That(reparsed.Value, Is.EqualTo(content.Value));
        }

        [Test]
        public void ToDisplayText_DurationWithoutFormat_ReturnsUnpaddedHoursMinutesSeconds()
        {
            var value = CellValue.FromDuration(new TimeSpan(1, 5, 3));

            Assert.That(CellFormatter.ToDisplayText(value, null, Invariant), Is.EqualTo("1:05:03"));
        }

        [Test]
        public void ToDisplayText_DurationOverTwentyFourHours_ShowsTotalHoursNotWrapped()
        {
            var value = CellValue.FromDuration(TimeSpan.FromHours(30));

            Assert.That(CellFormatter.ToDisplayText(value, null, Invariant), Is.EqualTo("30:00:00"));
        }

        [TestCase("hh:mm:ss", "01:05:03")]
        [TestCase("m:ss", "65:03")]
        [TestCase("mm:ss", "65:03")]
        public void ToDisplayText_DurationWithFormatString_AppliesFormat(string format, string expected)
        {
            var value = CellValue.FromDuration(new TimeSpan(1, 5, 3));
            var style = CellStyle.Default.WithFormat(format);

            Assert.That(CellFormatter.ToDisplayText(value, style, Invariant), Is.EqualTo(expected));
        }

        [Test]
        public void ToDisplayText_NegativeDuration_IsPrefixedWithMinus()
        {
            var value = CellValue.FromDuration(-(TimeSpan.FromMinutes(1) + TimeSpan.FromSeconds(15)));

            Assert.That(CellFormatter.ToDisplayText(value, null, Invariant), Is.EqualTo("-0:01:15"));
        }
    }
}
