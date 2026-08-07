// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormulaParserTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using SpreadsheetDemo.Spreadsheet.Model;
    using SpreadsheetDemo.Spreadsheet.Model.Formulas;

    using NUnit.Framework;

    [TestFixture]
    public class FormulaParserTests
    {
        private readonly FormulaParser parser = new FormulaParser();

        [Test]
        public void Parse_SimpleAddition_ProducesBinaryOperatorNode()
        {
            var formula = this.parser.Parse("1+2");

            Assert.That(formula.Root, Is.TypeOf<BinaryOperatorNode>());
            var node = (BinaryOperatorNode)formula.Root;
            Assert.That(node.Operator, Is.EqualTo(BinaryOperator.Add));
            Assert.That(((LiteralNode)node.Left).Value, Is.EqualTo(CellValue.FromNumber(1)));
            Assert.That(((LiteralNode)node.Right).Value, Is.EqualTo(CellValue.FromNumber(2)));
        }

        [Test]
        public void Parse_MultiplyBeforeAdd_MultiplicationIsInnerNode()
        {
            // 1+2*3 should parse as 1+(2*3)
            var formula = this.parser.Parse("1+2*3");

            var add = (BinaryOperatorNode)formula.Root;
            Assert.That(add.Operator, Is.EqualTo(BinaryOperator.Add));
            Assert.That(add.Right, Is.TypeOf<BinaryOperatorNode>());
            Assert.That(((BinaryOperatorNode)add.Right).Operator, Is.EqualTo(BinaryOperator.Multiply));
        }

        [Test]
        public void Parse_Parentheses_OverridePrecedence()
        {
            // (1+2)*3 should parse as a Multiply whose left is the Add.
            var formula = this.parser.Parse("(1+2)*3");

            var multiply = (BinaryOperatorNode)formula.Root;
            Assert.That(multiply.Operator, Is.EqualTo(BinaryOperator.Multiply));
            Assert.That(multiply.Left, Is.TypeOf<BinaryOperatorNode>());
            Assert.That(((BinaryOperatorNode)multiply.Left).Operator, Is.EqualTo(BinaryOperator.Add));
        }

        [Test]
        public void Parse_AdditiveOperators_AreLeftAssociative()
        {
            // 10-2-3 should parse as (10-2)-3, not 10-(2-3)
            var formula = this.parser.Parse("10-2-3");

            var outer = (BinaryOperatorNode)formula.Root;
            Assert.That(outer.Operator, Is.EqualTo(BinaryOperator.Subtract));
            Assert.That(outer.Left, Is.TypeOf<BinaryOperatorNode>());
            Assert.That(((LiteralNode)outer.Right).Value, Is.EqualTo(CellValue.FromNumber(3)));
        }

        [Test]
        public void Parse_Power_IsRightAssociative()
        {
            // 2^3^2 should parse as 2^(3^2)
            var formula = this.parser.Parse("2^3^2");

            var outer = (BinaryOperatorNode)formula.Root;
            Assert.That(outer.Operator, Is.EqualTo(BinaryOperator.Power));
            Assert.That(((LiteralNode)outer.Left).Value, Is.EqualTo(CellValue.FromNumber(2)));
            Assert.That(outer.Right, Is.TypeOf<BinaryOperatorNode>());
        }

        [Test]
        public void Parse_UnaryMinusBeforePower_BindsLooserThanPower()
        {
            // -2^2 parses as -(2^2), matching Excel's actual behaviour (evaluates to -4).
            var formula = this.parser.Parse("-2^2");

            Assert.That(formula.Root, Is.TypeOf<UnaryOperatorNode>());
            var unary = (UnaryOperatorNode)formula.Root;
            Assert.That(unary.Operator, Is.EqualTo(UnaryOperator.Negate));
            Assert.That(unary.Operand, Is.TypeOf<BinaryOperatorNode>());
            Assert.That(((BinaryOperatorNode)unary.Operand).Operator, Is.EqualTo(BinaryOperator.Power));
        }

        [Test]
        public void Parse_PowerWithNegativeExponent_ParsesUnaryOnRightSide()
        {
            // 2^-2 must parse successfully (unary is allowed on the right side of ^).
            var formula = this.parser.Parse("2^-2");

            var power = (BinaryOperatorNode)formula.Root;
            Assert.That(power.Operator, Is.EqualTo(BinaryOperator.Power));
            Assert.That(power.Right, Is.TypeOf<UnaryOperatorNode>());
        }

        [Test]
        public void Parse_Percent_IsPostfixOnPrimary()
        {
            var formula = this.parser.Parse("50%");

            Assert.That(formula.Root, Is.TypeOf<UnaryOperatorNode>());
            var unary = (UnaryOperatorNode)formula.Root;
            Assert.That(unary.Operator, Is.EqualTo(UnaryOperator.Percent));
        }

        [Test]
        public void Parse_CellReference_ProducesReferenceNode()
        {
            var formula = this.parser.Parse("A1");

            Assert.That(formula.Root, Is.TypeOf<ReferenceNode>());
            Assert.That(((ReferenceNode)formula.Root).Address, Is.EqualTo(new CellAddress(0, 0)));
            Assert.That(formula.Precedents, Is.EqualTo(new[] { new CellAddress(0, 0) }));
        }

        [Test]
        public void Parse_RangeReference_ProducesRangeNodeAndRangePrecedent()
        {
            var formula = this.parser.Parse("A1:B2");

            Assert.That(formula.Root, Is.TypeOf<RangeNode>());
            var range = ((RangeNode)formula.Root).Range;
            Assert.That(range.TopLeft, Is.EqualTo(new CellAddress(0, 0)));
            Assert.That(range.BottomRight, Is.EqualTo(new CellAddress(1, 1)));
            Assert.That(formula.RangePrecedents, Is.EqualTo(new[] { range }));
            Assert.That(formula.Precedents, Is.Empty);
        }

        [Test]
        public void Parse_FunctionCall_ProducesFunctionNodeWithArguments()
        {
            var formula = this.parser.Parse("SUM(A1:A2,5)");

            Assert.That(formula.Root, Is.TypeOf<FunctionNode>());
            var function = (FunctionNode)formula.Root;
            Assert.That(function.Name, Is.EqualTo("SUM"));
            Assert.That(function.Arguments.Count, Is.EqualTo(2));
            Assert.That(function.Arguments[0], Is.TypeOf<RangeNode>());
            Assert.That(function.Arguments[1], Is.TypeOf<LiteralNode>());
        }

        [Test]
        public void Parse_NestedFunctionCalls_ParsesInnerFunctionAsArgument()
        {
            var formula = this.parser.Parse("SUM(A1,MAX(B1,B2))");

            var outer = (FunctionNode)formula.Root;
            Assert.That(outer.Arguments[1], Is.TypeOf<FunctionNode>());
            Assert.That(((FunctionNode)outer.Arguments[1]).Name, Is.EqualTo("MAX"));
        }

        [Test]
        public void Parse_TrueFalseLiterals_ProduceBooleanLiteralNodes()
        {
            var trueFormula = this.parser.Parse("true");
            var falseFormula = this.parser.Parse("FALSE");

            Assert.That(((LiteralNode)trueFormula.Root).Value, Is.EqualTo(CellValue.FromBoolean(true)));
            Assert.That(((LiteralNode)falseFormula.Root).Value, Is.EqualTo(CellValue.FromBoolean(false)));
        }

        [Test]
        public void Parse_StringLiteral_ProducesTextLiteralNode()
        {
            var formula = this.parser.Parse("\"hello\"");

            Assert.That(((LiteralNode)formula.Root).Value, Is.EqualTo(CellValue.FromText("hello")));
        }

        [Test]
        public void Parse_ComparisonAndConcat_RespectPrecedenceOverArithmetic()
        {
            // 1+1=2&"x" should parse as (1+1)=(2&"x")
            var formula = this.parser.Parse("1+1=2&\"x\"");

            Assert.That(formula.Root, Is.TypeOf<BinaryOperatorNode>());
            var equal = (BinaryOperatorNode)formula.Root;
            Assert.That(equal.Operator, Is.EqualTo(BinaryOperator.Equal));
            Assert.That(((BinaryOperatorNode)equal.Left).Operator, Is.EqualTo(BinaryOperator.Add));
            Assert.That(((BinaryOperatorNode)equal.Right).Operator, Is.EqualTo(BinaryOperator.Concat));
        }

        [Test]
        public void Parse_UnknownName_ThrowsWithPosition()
        {
            var ex = Assert.Throws<FormulaSyntaxException>(() => this.parser.Parse("BOGUS+1"));
            Assert.That(ex.Position, Is.EqualTo(0));
        }

        [Test]
        public void Parse_UnmatchedParenthesis_Throws()
        {
            Assert.That(() => this.parser.Parse("(1+2"), Throws.TypeOf<FormulaSyntaxException>());
        }

        [Test]
        public void Parse_TrailingGarbage_Throws()
        {
            Assert.That(() => this.parser.Parse("1+2)"), Throws.TypeOf<FormulaSyntaxException>());
        }

        [Test]
        public void Parse_MissingOperand_Throws()
        {
            Assert.That(() => this.parser.Parse("1+"), Throws.TypeOf<FormulaSyntaxException>());
        }

        [Test]
        public void Parse_ColonWithoutSecondReference_Throws()
        {
            Assert.That(() => this.parser.Parse("A1:1"), Throws.TypeOf<FormulaSyntaxException>());
        }
    }
}
