// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellInputParserTests.cs" company="PropertyTools">
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
    public class CellInputParserTests
    {
        private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

        [Test]
        public void Parse_Null_ReturnsEmptyContent()
        {
            var content = CellInputParser.Parse(null, null, Invariant);

            Assert.That(content.IsEmpty, Is.True);
        }

        [Test]
        public void Parse_EmptyString_ReturnsEmptyContent()
        {
            var content = CellInputParser.Parse(string.Empty, null, Invariant);

            Assert.That(content.IsEmpty, Is.True);
        }

        [Test]
        public void Parse_LeadingApostrophe_ForcesText()
        {
            var content = CellInputParser.Parse("'123", null, Invariant);

            Assert.That(content.IsFormula, Is.False);
            Assert.That(content.Value.Type, Is.EqualTo(CellValueType.Text));
            Assert.That(content.Value.AsText(), Is.EqualTo("123"));
        }

        [TestCase("TRUE", true)]
        [TestCase("true", true)]
        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        public void Parse_BooleanText_ReturnsBooleanValue(string input, bool expected)
        {
            var content = CellInputParser.Parse(input, null, Invariant);

            Assert.That(content.Value.Type, Is.EqualTo(CellValueType.Boolean));
            Assert.That(content.Value.AsBoolean(), Is.EqualTo(expected));
        }

        [Test]
        public void Parse_Integer_ReturnsNumberValue()
        {
            var content = CellInputParser.Parse("42", null, Invariant);

            Assert.That(content.Value.Type, Is.EqualTo(CellValueType.Number));
            Assert.That(content.Value.AsNumber(), Is.EqualTo(42));
        }

        [Test]
        public void Parse_NegativeDecimal_ReturnsNumberValue()
        {
            var content = CellInputParser.Parse("-3.5", null, Invariant);

            Assert.That(content.Value.AsNumber(), Is.EqualTo(-3.5));
        }

        [Test]
        public void Parse_Percent_DividesByOneHundred()
        {
            var content = CellInputParser.Parse("50%", null, Invariant);

            Assert.That(content.Value.Type, Is.EqualTo(CellValueType.Number));
            Assert.That(content.Value.AsNumber(), Is.EqualTo(0.5));
        }

        [Test]
        public void Parse_DecimalComma_NorwegianCulture_ParsesAsNumber()
        {
            var norwegian = CultureInfo.GetCultureInfo("nb-NO");

            var content = CellInputParser.Parse("3,14", null, norwegian);

            Assert.That(content.Value.Type, Is.EqualTo(CellValueType.Number));
            Assert.That(content.Value.AsNumber(), Is.EqualTo(3.14));
        }

        [Test]
        public void Parse_Date_ReturnsDateTimeValue()
        {
            var content = CellInputParser.Parse("2026-08-05", null, Invariant);

            Assert.That(content.Value.Type, Is.EqualTo(CellValueType.DateTime));
            Assert.That(content.Value.AsDateTime().Year, Is.EqualTo(2026));
            Assert.That(content.Value.AsDateTime().Month, Is.EqualTo(8));
            Assert.That(content.Value.AsDateTime().Day, Is.EqualTo(5));
        }

        [Test]
        public void Parse_HoursMinutesSeconds_ReturnsDurationValue()
        {
            var content = CellInputParser.Parse("1:30:00", null, Invariant);

            Assert.That(content.Value.Type, Is.EqualTo(CellValueType.Duration));
            Assert.That(content.Value.AsDuration(), Is.EqualTo(new TimeSpan(1, 30, 0)));
        }

        [Test]
        public void Parse_MinutesSeconds_ReturnsDurationValue()
        {
            // A single colon means minutes:seconds (not hours:minutes, unlike TimeSpan.Parse), matching
            // the "typically entered as h:mm:ss or m:ss" convention this feature was built around.
            var content = CellInputParser.Parse("5:30", null, Invariant);

            Assert.That(content.Value.Type, Is.EqualTo(CellValueType.Duration));
            Assert.That(content.Value.AsDuration(), Is.EqualTo(TimeSpan.FromMinutes(5) + TimeSpan.FromSeconds(30)));
        }

        [Test]
        public void Parse_DurationOverTwentyFourHours_DoesNotWrap()
        {
            var content = CellInputParser.Parse("30:00:00", null, Invariant);

            Assert.That(content.Value.AsDuration(), Is.EqualTo(TimeSpan.FromHours(30)));
        }

        [Test]
        public void Parse_NegativeDuration_ReturnsNegativeTimeSpan()
        {
            var content = CellInputParser.Parse("-1:15", null, Invariant);

            Assert.That(content.Value.AsDuration(), Is.EqualTo(-(TimeSpan.FromMinutes(1) + TimeSpan.FromSeconds(15))));
        }

        [Test]
        public void Parse_PlainWord_ReturnsTextValue()
        {
            var content = CellInputParser.Parse("Hello", null, Invariant);

            Assert.That(content.Value.Type, Is.EqualTo(CellValueType.Text));
            Assert.That(content.Value.AsText(), Is.EqualTo("Hello"));
        }

        [Test]
        public void Parse_FormulaWithoutParser_PreservesTextAsUnparsedFormula()
        {
            var content = CellInputParser.Parse("=A1+1", null, Invariant);

            Assert.That(content.IsFormula, Is.True);
            Assert.That(content.Formula.Text, Is.EqualTo("A1+1"));
            Assert.That(content.Formula.HasSyntaxError, Is.True);
        }

        [Test]
        public void Parse_LoneEqualsSign_IsTreatedAsText()
        {
            var content = CellInputParser.Parse("=", null, Invariant);

            Assert.That(content.IsFormula, Is.False);
            Assert.That(content.Value.AsText(), Is.EqualTo("="));
        }

        [Test]
        public void Parse_FormulaWithParser_DelegatesToParser()
        {
            var parser = new FakeFormulaParser();

            var content = CellInputParser.Parse("=SUM(A1:A2)", parser, Invariant);

            Assert.That(content.IsFormula, Is.True);
            Assert.That(content.Formula.HasSyntaxError, Is.False);
            Assert.That(parser.LastParsedText, Is.EqualTo("SUM(A1:A2)"));
        }

        [Test]
        public void Parse_FormulaSyntaxError_PreservesTextAsUnparsedFormula()
        {
            var parser = new FakeFormulaParser { ThrowOnParse = true };

            var content = CellInputParser.Parse("=1+", parser, Invariant);

            Assert.That(content.IsFormula, Is.True);
            Assert.That(content.Formula.Text, Is.EqualTo("1+"));
            Assert.That(content.Formula.HasSyntaxError, Is.True);
        }

        [Test]
        public void Parse_NullCulture_Throws()
        {
            Assert.That(() => CellInputParser.Parse("1", null, null), Throws.TypeOf<ArgumentNullException>());
        }

        /// <summary>
        /// A minimal <see cref="IFormulaParser" /> test double used until the real formula parser exists.
        /// </summary>
        private sealed class FakeFormulaParser : IFormulaParser
        {
            public bool ThrowOnParse { get; set; }

            public string LastParsedText { get; private set; }

            public Formula Parse(string formulaText)
            {
                this.LastParsedText = formulaText;

                if (this.ThrowOnParse)
                {
                    throw new FormulaSyntaxException("Test failure.");
                }

                return new Formula(formulaText, new FakeFormulaNode(), Array.Empty<CellAddress>(), Array.Empty<CellRange>());
            }
        }

        private sealed class FakeFormulaNode : FormulaNode
        {
        }
    }
}
