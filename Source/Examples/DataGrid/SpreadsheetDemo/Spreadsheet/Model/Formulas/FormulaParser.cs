// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormulaParser.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Parses formula text into a syntax tree.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parses formula text into a <see cref="Formula" />.
    /// </summary>
    /// <remarks>
    /// Recursive-descent, precedence climbing from lowest to highest: comparison
    /// (<c>= &lt;&gt; &lt; &lt;= &gt; &gt;=</c>), concatenation (<c>&amp;</c>), additive (<c>+ -</c>),
    /// multiplicative (<c>* /</c>), unary (<c>- +</c>), power (<c>^</c>, right-associative — its right
    /// operand allows a leading unary so that <c>2^-2</c> parses), percent (postfix <c>%</c>).
    /// Unary minus binds looser than <c>^</c>, matching both Excel's actual behaviour and standard
    /// mathematical convention: <c>-2^2</c> is <c>-4</c>, not <c>4</c>.
    /// </remarks>
    internal sealed class FormulaParser : IFormulaParser
    {
        /// <inheritdoc />
        public Formula Parse(string formulaText)
        {
            var tokens = FormulaLexer.Tokenize(formulaText ?? string.Empty);
            var state = new ParseState(tokens);
            var root = ParseComparison(state);
            if (state.Current.Type != TokenType.EndOfInput)
            {
                throw Error(state, $"Unexpected {state.Current} after the end of the formula.");
            }

            return new Formula(formulaText, root, state.Precedents, state.RangePrecedents);
        }

        private static FormulaNode ParseComparison(ParseState state)
        {
            var left = ParseConcat(state);
            while (TryGetBinaryOperator(state.Current.Type, out var op))
            {
                Advance(state);
                var right = ParseConcat(state);
                left = new BinaryOperatorNode(op, left, right);
            }

            return left;

            bool TryGetBinaryOperator(TokenType type, out BinaryOperator result)
            {
                switch (type)
                {
                    case TokenType.Equal: result = BinaryOperator.Equal; return true;
                    case TokenType.NotEqual: result = BinaryOperator.NotEqual; return true;
                    case TokenType.LessThan: result = BinaryOperator.LessThan; return true;
                    case TokenType.LessThanOrEqual: result = BinaryOperator.LessThanOrEqual; return true;
                    case TokenType.GreaterThan: result = BinaryOperator.GreaterThan; return true;
                    case TokenType.GreaterThanOrEqual: result = BinaryOperator.GreaterThanOrEqual; return true;
                    default: result = default; return false;
                }
            }
        }

        private static FormulaNode ParseConcat(ParseState state)
        {
            var left = ParseAdditive(state);
            while (state.Current.Type == TokenType.Ampersand)
            {
                Advance(state);
                var right = ParseAdditive(state);
                left = new BinaryOperatorNode(BinaryOperator.Concat, left, right);
            }

            return left;
        }

        private static FormulaNode ParseAdditive(ParseState state)
        {
            var left = ParseMultiplicative(state);
            while (state.Current.Type == TokenType.Plus || state.Current.Type == TokenType.Minus)
            {
                var op = state.Current.Type == TokenType.Plus ? BinaryOperator.Add : BinaryOperator.Subtract;
                Advance(state);
                var right = ParseMultiplicative(state);
                left = new BinaryOperatorNode(op, left, right);
            }

            return left;
        }

        private static FormulaNode ParseMultiplicative(ParseState state)
        {
            var left = ParseUnary(state);
            while (state.Current.Type == TokenType.Multiply || state.Current.Type == TokenType.Divide)
            {
                var op = state.Current.Type == TokenType.Multiply ? BinaryOperator.Multiply : BinaryOperator.Divide;
                Advance(state);
                var right = ParseUnary(state);
                left = new BinaryOperatorNode(op, left, right);
            }

            return left;
        }

        private static FormulaNode ParseUnary(ParseState state)
        {
            if (state.Current.Type == TokenType.Minus)
            {
                Advance(state);
                return new UnaryOperatorNode(UnaryOperator.Negate, ParseUnary(state));
            }

            if (state.Current.Type == TokenType.Plus)
            {
                Advance(state);
                return new UnaryOperatorNode(UnaryOperator.Plus, ParseUnary(state));
            }

            return ParsePower(state);
        }

        private static FormulaNode ParsePower(ParseState state)
        {
            var left = ParsePercent(state);
            if (state.Current.Type == TokenType.Power)
            {
                Advance(state);

                // Right-associative, and the right operand may itself start with a unary sign (2^-2).
                var right = ParseUnary(state);
                return new BinaryOperatorNode(BinaryOperator.Power, left, right);
            }

            return left;
        }

        private static FormulaNode ParsePercent(ParseState state)
        {
            var node = ParsePrimary(state);
            while (state.Current.Type == TokenType.Percent)
            {
                Advance(state);
                node = new UnaryOperatorNode(UnaryOperator.Percent, node);
            }

            return node;
        }

        private static FormulaNode ParsePrimary(ParseState state)
        {
            var token = state.Current;
            switch (token.Type)
            {
                case TokenType.Number:
                    Advance(state);
                    return new LiteralNode(CellValue.FromNumber(token.Number));

                case TokenType.String:
                    Advance(state);
                    return new LiteralNode(CellValue.FromText(token.Text));

                case TokenType.LeftParen:
                    Advance(state);
                    var inner = ParseComparison(state);
                    Expect(state, TokenType.RightParen, ")");
                    return inner;

                case TokenType.Word:
                    return ParseWord(state);

                default:
                    throw Error(state, $"Unexpected {token} in formula.");
            }
        }

        private static FormulaNode ParseWord(ParseState state)
        {
            var word = state.Current.Text;

            if (string.Equals(word, "TRUE", StringComparison.OrdinalIgnoreCase))
            {
                Advance(state);
                return new LiteralNode(CellValue.FromBoolean(true));
            }

            if (string.Equals(word, "FALSE", StringComparison.OrdinalIgnoreCase))
            {
                Advance(state);
                return new LiteralNode(CellValue.FromBoolean(false));
            }

            if (state.PeekType(1) == TokenType.LeftParen)
            {
                Advance(state); // the function name
                Advance(state); // '('

                var arguments = new List<FormulaNode>();
                if (state.Current.Type != TokenType.RightParen)
                {
                    arguments.Add(ParseComparison(state));
                    while (state.Current.Type == TokenType.Comma)
                    {
                        Advance(state);
                        arguments.Add(ParseComparison(state));
                    }
                }

                Expect(state, TokenType.RightParen, ")");
                return new FunctionNode(word, arguments);
            }

            if (CellAddress.TryParse(word, out var address))
            {
                Advance(state);

                if (state.Current.Type == TokenType.Colon)
                {
                    Advance(state);
                    if (state.Current.Type != TokenType.Word || !CellAddress.TryParse(state.Current.Text, out var address2))
                    {
                        throw Error(state, "Expected a cell reference after ':'.");
                    }

                    Advance(state);
                    var range = new CellRange(address, address2);
                    state.RangePrecedentsList.Add(range);
                    return new RangeNode(range);
                }

                state.PrecedentsList.Add(address);
                return new ReferenceNode(address);
            }

            throw Error(state, $"'{word}' is not a recognized function or cell reference.");
        }

        private static void Advance(ParseState state)
        {
            state.Advance();
        }

        private static void Expect(ParseState state, TokenType type, string symbol)
        {
            if (state.Current.Type != type)
            {
                throw Error(state, $"Expected '{symbol}' but found {state.Current}.");
            }

            Advance(state);
        }

        private static FormulaSyntaxException Error(ParseState state, string message)
        {
            return new FormulaSyntaxException(message) { Position = state.Current.Position };
        }

        /// <summary>
        /// Mutable state for a single <see cref="Parse" /> call — kept local to each call so that
        /// <see cref="FormulaParser" /> itself has no per-call state and is safe to share/reuse.
        /// </summary>
        private sealed class ParseState
        {
            private readonly List<Token> tokens;
            private int position;

            public ParseState(List<Token> tokens)
            {
                this.tokens = tokens;
            }

            public List<CellAddress> PrecedentsList { get; } = new List<CellAddress>();

            public List<CellRange> RangePrecedentsList { get; } = new List<CellRange>();

            public IReadOnlyList<CellAddress> Precedents => this.PrecedentsList;

            public IReadOnlyList<CellRange> RangePrecedents => this.RangePrecedentsList;

            public Token Current => this.tokens[this.position];

            public TokenType PeekType(int offset)
            {
                var index = this.position + offset;
                return index < this.tokens.Count ? this.tokens[index].Type : TokenType.EndOfInput;
            }

            public void Advance()
            {
                if (this.position < this.tokens.Count - 1)
                {
                    this.position++;
                }
            }
        }
    }
}
