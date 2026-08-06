// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormulaLexerTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using SpreadsheetDemo.Spreadsheet.Model.Formulas;

    using NUnit.Framework;

    [TestFixture]
    public class FormulaLexerTests
    {
        [Test]
        public void Tokenize_Number_ReturnsNumberToken()
        {
            var tokens = FormulaLexer.Tokenize("42.5");

            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Number));
            Assert.That(tokens[0].Number, Is.EqualTo(42.5));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EndOfInput));
        }

        [Test]
        public void Tokenize_NumberWithExponent_ParsesCorrectly()
        {
            var tokens = FormulaLexer.Tokenize("1.5e2");

            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Number));
            Assert.That(tokens[0].Number, Is.EqualTo(150));
        }

        [Test]
        public void Tokenize_StringLiteral_DecodesEscapedQuotes()
        {
            var tokens = FormulaLexer.Tokenize("\"He said \"\"hi\"\"\"");

            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.String));
            Assert.That(tokens[0].Text, Is.EqualTo("He said \"hi\""));
        }

        [Test]
        public void Tokenize_UnterminatedString_Throws()
        {
            Assert.That(() => FormulaLexer.Tokenize("\"abc"), Throws.TypeOf<FormulaSyntaxException>());
        }

        [Test]
        public void Tokenize_CellReference_ReturnsWordToken()
        {
            var tokens = FormulaLexer.Tokenize("A1");

            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Word));
            Assert.That(tokens[0].Text, Is.EqualTo("A1"));
        }

        [Test]
        public void Tokenize_FunctionName_ReturnsWordThenLeftParen()
        {
            var tokens = FormulaLexer.Tokenize("SUM(");

            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Word));
            Assert.That(tokens[0].Text, Is.EqualTo("SUM"));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.LeftParen));
        }

        // NUnit requires public test methods, but TokenType is internal, so the expected type is
        // passed as an int (TestCase arguments must be compile-time constants; the cast is one).
        [TestCase("<>", (int)TokenType.NotEqual)]
        [TestCase("<=", (int)TokenType.LessThanOrEqual)]
        [TestCase(">=", (int)TokenType.GreaterThanOrEqual)]
        [TestCase("<", (int)TokenType.LessThan)]
        [TestCase(">", (int)TokenType.GreaterThan)]
        [TestCase("=", (int)TokenType.Equal)]
        public void Tokenize_ComparisonOperator_ReturnsExpectedTokenType(string input, int expectedType)
        {
            var tokens = FormulaLexer.Tokenize(input);

            Assert.That((int)tokens[0].Type, Is.EqualTo(expectedType));
        }

        [Test]
        public void Tokenize_ArithmeticOperators_ReturnsOneTokenEach()
        {
            var tokens = FormulaLexer.Tokenize("+-*/^%&");

            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Plus));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Minus));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Multiply));
            Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Divide));
            Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Power));
            Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Percent));
            Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Ampersand));
            Assert.That(tokens[7].Type, Is.EqualTo(TokenType.EndOfInput));
        }

        [Test]
        public void Tokenize_RangeReference_ReturnsWordColonWord()
        {
            var tokens = FormulaLexer.Tokenize("A1:B10");

            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Word));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Colon));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Word));
        }

        [Test]
        public void Tokenize_WhitespaceBetweenTokens_IsSkipped()
        {
            var tokens = FormulaLexer.Tokenize("  1   +   2  ");

            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Number));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Plus));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Number));
            Assert.That(tokens[3].Type, Is.EqualTo(TokenType.EndOfInput));
        }

        [Test]
        public void Tokenize_InvalidCharacter_Throws()
        {
            Assert.That(() => FormulaLexer.Tokenize("1 @ 2"), Throws.TypeOf<FormulaSyntaxException>());
        }

        [Test]
        public void Tokenize_EmptyString_ReturnsOnlyEndOfInput()
        {
            var tokens = FormulaLexer.Tokenize(string.Empty);

            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.EndOfInput));
        }

        [Test]
        public void Tokenize_DollarAnchoredReference_ReturnsSingleWordToken()
        {
            var tokens = FormulaLexer.Tokenize("$A$1");

            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Word));
            Assert.That(tokens[0].Text, Is.EqualTo("$A$1"));
        }
    }
}
