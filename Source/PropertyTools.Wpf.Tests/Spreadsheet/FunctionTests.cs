// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class FunctionTests
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

        // ----- Math -----

        [Test]
        public void Sum_MixOfScalarsAndRange_AddsAll()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1");
            this.sheet.SetCellText(new CellAddress(1, 0), "2");

            Assert.That(this.Eval("SUM(A1:A2,10)").AsNumber(), Is.EqualTo(13));
        }

        [Test]
        public void Sum_NoArguments_ReturnsZero()
        {
            Assert.That(this.Eval("SUM()").AsNumber(), Is.EqualTo(0));
        }

        [Test]
        public void Sum_RangeWithText_SkipsText()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1");
            this.sheet.SetCellText(new CellAddress(1, 0), "hello");
            this.sheet.SetCellText(new CellAddress(2, 0), "2");

            Assert.That(this.Eval("SUM(A1:A3)").AsNumber(), Is.EqualTo(3));
        }

        [TestCase("ABS(-5)", 5.0)]
        [TestCase("ABS(5)", 5.0)]
        [TestCase("SQRT(9)", 3.0)]
        [TestCase("POWER(2,10)", 1024.0)]
        [TestCase("MOD(7,3)", 1.0)]
        [TestCase("MOD(-7,3)", 2.0)]
        [TestCase("INT(3.7)", 3.0)]
        [TestCase("INT(-3.7)", -4.0)]
        [TestCase("ROUND(3.14159,2)", 3.14)]
        [TestCase("ROUNDUP(3.1,0)", 4.0)]
        [TestCase("ROUNDDOWN(3.9,0)", 3.0)]
        public void MathFunction_KnownInput_ReturnsExpectedValue(string formulaText, double expected)
        {
            Assert.That(this.Eval(formulaText).AsNumber(), Is.EqualTo(expected).Within(1e-9));
        }

        [Test]
        public void Sqrt_NegativeNumber_ReturnsNumberError()
        {
            Assert.That(this.Eval("SQRT(-1)").Error, Is.EqualTo(CellError.Number));
        }

        [Test]
        public void Mod_DivisorZero_ReturnsDivideByZeroError()
        {
            Assert.That(this.Eval("MOD(1,0)").Error, Is.EqualTo(CellError.DivideByZero));
        }

        [TestCase("SIN(0)", 0.0)]
        [TestCase("COS(0)", 1.0)]
        [TestCase("TAN(0)", 0.0)]
        [TestCase("ASIN(1)", System.Math.PI / 2)]
        [TestCase("ACOS(1)", 0.0)]
        [TestCase("ATAN(0)", 0.0)]
        [TestCase("ATAN2(1,1)", System.Math.PI / 4)]
        [TestCase("LN(1)", 0.0)]
        [TestCase("LOG10(100)", 2.0)]
        [TestCase("LOG(8,2)", 3.0)]
        [TestCase("LOG(100)", 2.0)]
        [TestCase("EXP(0)", 1.0)]
        [TestCase("PI()", System.Math.PI)]
        public void ScientificFunction_KnownInput_ReturnsExpectedValue(string formulaText, double expected)
        {
            Assert.That(this.Eval(formulaText).AsNumber(), Is.EqualTo(expected).Within(1e-9));
        }

        [Test]
        public void Ln_NonPositiveNumber_ReturnsNumberError()
        {
            Assert.That(this.Eval("LN(0)").Error, Is.EqualTo(CellError.Number));
        }

        [Test]
        public void Atan2_BothArgumentsZero_ReturnsDivideByZeroError()
        {
            Assert.That(this.Eval("ATAN2(0,0)").Error, Is.EqualTo(CellError.DivideByZero));
        }

        // ----- Statistical -----

        [Test]
        public void Average_ThreeNumbers_ReturnsMean()
        {
            Assert.That(this.Eval("AVERAGE(1,2,3)").AsNumber(), Is.EqualTo(2));
        }

        [Test]
        public void Average_NoNumericValues_ReturnsDivideByZeroError()
        {
            Assert.That(this.Eval("AVERAGE(\"a\")").Error, Is.EqualTo(CellError.DivideByZero));
        }

        [Test]
        public void Sum_RangeOfDurations_AddsTheirDayFractions()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1:30:00");
            this.sheet.SetCellText(new CellAddress(1, 0), "0:30:00");

            var sum = this.Eval("SUM(A1:A2)");

            Assert.That(sum.AsNumber(), Is.EqualTo(TimeSpan.FromHours(2).TotalDays).Within(1e-9));
        }

        [Test]
        public void Count_RangeIncludingADuration_CountsIt()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1:30:00");
            this.sheet.SetCellText(new CellAddress(1, 0), "hello");
            this.sheet.SetCellText(new CellAddress(2, 0), "5");

            Assert.That(this.Eval("COUNT(A1:A3)").AsNumber(), Is.EqualTo(2));
        }

        [Test]
        public void MinMax_RangeOfNumbers_ReturnsExtremes()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "3");
            this.sheet.SetCellText(new CellAddress(1, 0), "1");
            this.sheet.SetCellText(new CellAddress(2, 0), "2");

            Assert.That(this.Eval("MIN(A1:A3)").AsNumber(), Is.EqualTo(1));
            Assert.That(this.Eval("MAX(A1:A3)").AsNumber(), Is.EqualTo(3));
        }

        [Test]
        public void Count_RangeWithMixedTypes_CountsOnlyNumbers()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1");
            this.sheet.SetCellText(new CellAddress(1, 0), "hello");
            this.sheet.SetCellText(new CellAddress(2, 0), "TRUE");

            Assert.That(this.Eval("COUNT(A1:A3)").AsNumber(), Is.EqualTo(1));
        }

        [Test]
        public void CountA_RangeWithMixedTypes_CountsAllNonEmpty()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1");
            this.sheet.SetCellText(new CellAddress(1, 0), "hello");
            this.sheet.SetCellText(new CellAddress(2, 0), string.Empty);

            Assert.That(this.Eval("COUNTA(A1:A3)").AsNumber(), Is.EqualTo(2));
        }

        // ----- Logical -----

        [Test]
        public void And_AllTrue_ReturnsTrue()
        {
            Assert.That(this.Eval("AND(TRUE,1=1,2>1)").AsBoolean(), Is.True);
        }

        [Test]
        public void And_OneFalse_ReturnsFalse()
        {
            Assert.That(this.Eval("AND(TRUE,FALSE)").AsBoolean(), Is.False);
        }

        [Test]
        public void Or_OneTrue_ReturnsTrue()
        {
            Assert.That(this.Eval("OR(FALSE,FALSE,TRUE)").AsBoolean(), Is.True);
        }

        [Test]
        public void Not_True_ReturnsFalse()
        {
            Assert.That(this.Eval("NOT(TRUE)").AsBoolean(), Is.False);
        }

        [Test]
        public void If_MissingElseBranch_ReturnsFalseWhenConditionFalse()
        {
            Assert.That(this.Eval("IF(FALSE,1)").AsBoolean(), Is.False);
        }

        // ----- Text -----

        [Test]
        public void Concat_MixedTypes_ConvertsAndJoins()
        {
            Assert.That(this.Eval("CONCAT(\"n=\",1,TRUE)").AsText(), Is.EqualTo("n=1TRUE"));
        }

        [Test]
        public void Len_ReturnsCharacterCount()
        {
            Assert.That(this.Eval("LEN(\"hello\")").AsNumber(), Is.EqualTo(5));
        }

        [Test]
        public void UpperLower_ConvertCase()
        {
            Assert.That(this.Eval("UPPER(\"abc\")").AsText(), Is.EqualTo("ABC"));
            Assert.That(this.Eval("LOWER(\"ABC\")").AsText(), Is.EqualTo("abc"));
        }

        [Test]
        public void Trim_CollapsesInternalSpacesAndRemovesEnds()
        {
            Assert.That(this.Eval("TRIM(\"  a   b  \")").AsText(), Is.EqualTo("a b"));
        }

        [Test]
        public void LeftRight_DefaultCount_ReturnsOneCharacter()
        {
            Assert.That(this.Eval("LEFT(\"hello\")").AsText(), Is.EqualTo("h"));
            Assert.That(this.Eval("RIGHT(\"hello\")").AsText(), Is.EqualTo("o"));
        }

        [Test]
        public void LeftRight_ExplicitCount_ReturnsThatManyCharacters()
        {
            Assert.That(this.Eval("LEFT(\"hello\",2)").AsText(), Is.EqualTo("he"));
            Assert.That(this.Eval("RIGHT(\"hello\",2)").AsText(), Is.EqualTo("lo"));
        }

        [Test]
        public void Left_CountLongerThanText_ReturnsWholeText()
        {
            Assert.That(this.Eval("LEFT(\"hi\",10)").AsText(), Is.EqualTo("hi"));
        }

        [Test]
        public void Mid_ExtractsSubstring()
        {
            Assert.That(this.Eval("MID(\"hello world\",7,5)").AsText(), Is.EqualTo("world"));
        }

        [Test]
        public void Mid_StartBeyondLength_ReturnsEmptyText()
        {
            // CellValue.FromText("") collapses to CellValue.Empty by design (see CellValueTests),
            // so an empty MID result comes back as Empty rather than a zero-length Text value.
            Assert.That(this.Eval("MID(\"hi\",10,2)").IsEmpty, Is.True);
        }

        [Test]
        public void Find_TextPresent_ReturnsOneBasedPosition()
        {
            Assert.That(this.Eval("FIND(\"lo\",\"hello\")").AsNumber(), Is.EqualTo(4));
        }

        [Test]
        public void Find_TextAbsent_ReturnsValueError()
        {
            Assert.That(this.Eval("FIND(\"z\",\"hello\")").Error, Is.EqualTo(CellError.Value));
        }

        [Test]
        public void Find_WithStartNum_SearchesFromThatPosition()
        {
            Assert.That(this.Eval("FIND(\"l\",\"hello\",4)").AsNumber(), Is.EqualTo(4));
        }

        [Test]
        public void Substitute_AllOccurrences_ReplacesEveryMatch()
        {
            Assert.That(this.Eval("SUBSTITUTE(\"a-b-c\",\"-\",\"+\")").AsText(), Is.EqualTo("a+b+c"));
        }

        [Test]
        public void Substitute_WithInstanceNumber_ReplacesOnlyThatOccurrence()
        {
            Assert.That(this.Eval("SUBSTITUTE(\"a-b-c\",\"-\",\"+\",2)").AsText(), Is.EqualTo("a-b+c"));
        }

        [Test]
        public void Rept_RepeatsTextGivenNumberOfTimes()
        {
            Assert.That(this.Eval("REPT(\"ab\",3)").AsText(), Is.EqualTo("ababab"));
        }

        [Test]
        public void Proper_CapitalizesEachWord()
        {
            Assert.That(this.Eval("PROPER(\"hello WORLD\")").AsText(), Is.EqualTo("Hello World"));
        }

        // ----- Date -----

        [Test]
        public void Date_ThreeComponents_ReturnsCorrectDate()
        {
            var value = this.Eval("DATE(2026,8,5)");

            Assert.That(value.Type, Is.EqualTo(CellValueType.DateTime));
            Assert.That(value.AsDateTime(), Is.EqualTo(new System.DateTime(2026, 8, 5)));
        }

        [Test]
        public void YearMonthDay_OfDateCell_ExtractComponents()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "=DATE(2026,8,5)");

            Assert.That(this.Eval("YEAR(A1)").AsNumber(), Is.EqualTo(2026));
            Assert.That(this.Eval("MONTH(A1)").AsNumber(), Is.EqualTo(8));
            Assert.That(this.Eval("DAY(A1)").AsNumber(), Is.EqualTo(5));
        }

        // ----- Information -----

        [Test]
        public void IsBlank_EmptyCell_ReturnsTrue()
        {
            Assert.That(this.Eval("ISBLANK(A1)").AsBoolean(), Is.True);
        }

        [Test]
        public void IsNumber_NumberCell_ReturnsTrue()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1");

            Assert.That(this.Eval("ISNUMBER(A1)").AsBoolean(), Is.True);
        }

        [Test]
        public void IsText_TextCell_ReturnsTrue()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "hi");

            Assert.That(this.Eval("ISTEXT(A1)").AsBoolean(), Is.True);
        }

        [Test]
        public void IsError_ErrorCell_ReturnsTrue()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "=1/0");

            Assert.That(this.Eval("ISERROR(A1)").AsBoolean(), Is.True);
        }
    }
}
