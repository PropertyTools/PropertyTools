// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormulaEvaluatorTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet.Model;
    using SpreadsheetDemo.Spreadsheet.Model.Formulas;

    using NUnit.Framework;

    [TestFixture]
    public class FormulaEvaluatorTests
    {
        private Sheet sheet;

        [SetUp]
        public void SetUp()
        {
            this.sheet = new Sheet("Sheet1", 20, 20) { Culture = CultureInfo.InvariantCulture };
        }

        private CellValue Eval(string formulaText)
        {
            this.sheet.SetCellText(new CellAddress(19, 19), "=" + formulaText);
            return this.sheet.GetValue(new CellAddress(19, 19));
        }

        [TestCase("1+2", 3.0)]
        [TestCase("5-2", 3.0)]
        [TestCase("3*4", 12.0)]
        [TestCase("10/4", 2.5)]
        [TestCase("2^10", 1024.0)]
        [TestCase("-2^2", -4.0)]
        [TestCase("2^-2", 0.25)]
        [TestCase("50%", 0.5)]
        [TestCase("-50%", -0.5)]
        [TestCase("(1+2)*3", 9.0)]
        [TestCase("1+2*3", 7.0)]
        public void Evaluate_ArithmeticExpression_ReturnsExpectedNumber(string formulaText, double expected)
        {
            var value = this.Eval(formulaText);

            Assert.That(value.Type, Is.EqualTo(CellValueType.Number));
            Assert.That(value.AsNumber(), Is.EqualTo(expected).Within(1e-9));
        }

        [Test]
        public void Evaluate_DivideByZero_ReturnsDivideByZeroError()
        {
            var value = this.Eval("1/0");

            Assert.That(value.IsError, Is.True);
            Assert.That(value.Error, Is.EqualTo(CellError.DivideByZero));
        }

        [Test]
        public void Evaluate_Concatenation_ReturnsJoinedText()
        {
            var value = this.Eval("\"a\"&\"b\"&1");

            Assert.That(value.Type, Is.EqualTo(CellValueType.Text));
            Assert.That(value.AsText(), Is.EqualTo("ab1"));
        }

        [TestCase("1=1", true)]
        [TestCase("1=2", false)]
        [TestCase("1<>2", true)]
        [TestCase("1<2", true)]
        [TestCase("2<=2", true)]
        [TestCase("3>2", true)]
        [TestCase("2>=3", false)]
        [TestCase("\"a\"<\"b\"", true)]
        public void Evaluate_Comparison_ReturnsExpectedBoolean(string formulaText, bool expected)
        {
            var value = this.Eval(formulaText);

            Assert.That(value.Type, Is.EqualTo(CellValueType.Boolean));
            Assert.That(value.AsBoolean(), Is.EqualTo(expected));
        }

        [Test]
        public void Evaluate_ReferenceToNumberCell_ReturnsItsValue()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "42");

            var value = this.Eval("A1*2");

            Assert.That(value.AsNumber(), Is.EqualTo(84));
        }

        [Test]
        public void Evaluate_ReferenceToEmptyCell_TreatsItAsZero()
        {
            var value = this.Eval("A1+1");

            Assert.That(value.AsNumber(), Is.EqualTo(1));
        }

        [Test]
        public void Evaluate_ErrorPropagatesThroughArithmetic()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "=1/0");

            var value = this.Eval("A1+1");

            Assert.That(value.IsError, Is.True);
            Assert.That(value.Error, Is.EqualTo(CellError.DivideByZero));
        }

        [Test]
        public void Evaluate_LeftErrorTakesPrecedenceOverRightError()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "=1/0"); // #DIV/0!
            this.sheet.SetCellText(new CellAddress(0, 1), "=BOGUS_NAME_NEVER_REGISTERED()"); // parses fine, unknown function -> #NAME?

            var value = this.Eval("A1+B1");

            Assert.That(value.Error, Is.EqualTo(CellError.DivideByZero));
        }

        [Test]
        public void Evaluate_TextThatCannotConvertToNumber_ReturnsValueError()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "Hello");

            var value = this.Eval("A1+1");

            Assert.That(value.IsError, Is.True);
            Assert.That(value.Error, Is.EqualTo(CellError.Value));
        }

        [Test]
        public void Evaluate_UnknownFunction_ReturnsNameError()
        {
            var value = this.Eval("NOTAREALFUNCTION(1)");

            Assert.That(value.IsError, Is.True);
            Assert.That(value.Error, Is.EqualTo(CellError.Name));
        }

        [Test]
        public void Evaluate_WrongArgumentCount_ReturnsValueError()
        {
            var value = this.Eval("ABS(1,2)");

            Assert.That(value.IsError, Is.True);
            Assert.That(value.Error, Is.EqualTo(CellError.Value));
        }

        [Test]
        public void Evaluate_SumOverRange_SumsAllCells()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1");
            this.sheet.SetCellText(new CellAddress(1, 0), "2");
            this.sheet.SetCellText(new CellAddress(2, 0), "3");

            var value = this.Eval("SUM(A1:A3)");

            Assert.That(value.AsNumber(), Is.EqualTo(6));
        }

        [Test]
        public void Evaluate_BareRangeOutsideFunction_ReturnsTopLeftCellValue()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "7");
            this.sheet.SetCellText(new CellAddress(0, 1), "8");

            var value = this.Eval("A1:B1");

            Assert.That(value.AsNumber(), Is.EqualTo(7));
        }

        [Test]
        public void Evaluate_If_OnlyEvaluatesTakenBranch()
        {
            // The false branch divides by zero; IF must not evaluate it when the condition is true.
            var value = this.Eval("IF(TRUE, 1, 1/0)");

            Assert.That(value.AsNumber(), Is.EqualTo(1));
        }

        [Test]
        public void Evaluate_IfError_OnlyEvaluatesFallbackWhenFirstArgumentErrors()
        {
            var value = this.Eval("IFERROR(1/0, 99)");

            Assert.That(value.AsNumber(), Is.EqualTo(99));
        }

        [Test]
        public void Evaluate_IfErrorWithNoError_ReturnsFirstArgument()
        {
            var value = this.Eval("IFERROR(5, 99)");

            Assert.That(value.AsNumber(), Is.EqualTo(5));
        }
    }
}
