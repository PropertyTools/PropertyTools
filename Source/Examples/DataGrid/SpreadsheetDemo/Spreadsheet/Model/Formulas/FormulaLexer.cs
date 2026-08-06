// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormulaLexer.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Splits formula text into tokens.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Splits formula text into tokens for <see cref="FormulaParser" />.
    /// </summary>
    internal static class FormulaLexer
    {
        /// <summary>
        /// Tokenizes the given formula text.
        /// </summary>
        /// <param name="text">The formula text (without the leading '=').</param>
        /// <returns>The tokens, ending with a single <see cref="TokenType.EndOfInput" /> token.</returns>
        /// <exception cref="FormulaSyntaxException">The text contains an invalid character or an unterminated string.</exception>
        public static List<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();
            var i = 0;
            var length = text.Length;

            while (i < length)
            {
                var c = text[i];

                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                var start = i;

                if (char.IsDigit(c) || (c == '.' && i + 1 < length && char.IsDigit(text[i + 1])))
                {
                    i = ReadNumber(text, i);
                    tokens.Add(new Token(TokenType.Number, text.Substring(start, i - start), ParseNumber(text, start, i), start));
                    continue;
                }

                if (char.IsLetter(c) || c == '$' || c == '_')
                {
                    i = ReadWord(text, i);
                    tokens.Add(new Token(TokenType.Word, text.Substring(start, i - start), 0, start));
                    continue;
                }

                if (c == '"')
                {
                    i = ReadString(text, i, out var value);
                    tokens.Add(new Token(TokenType.String, value, 0, start));
                    continue;
                }

                switch (c)
                {
                    case '+':
                        tokens.Add(new Token(TokenType.Plus, "+", 0, start));
                        i++;
                        break;
                    case '-':
                        tokens.Add(new Token(TokenType.Minus, "-", 0, start));
                        i++;
                        break;
                    case '*':
                        tokens.Add(new Token(TokenType.Multiply, "*", 0, start));
                        i++;
                        break;
                    case '/':
                        tokens.Add(new Token(TokenType.Divide, "/", 0, start));
                        i++;
                        break;
                    case '^':
                        tokens.Add(new Token(TokenType.Power, "^", 0, start));
                        i++;
                        break;
                    case '%':
                        tokens.Add(new Token(TokenType.Percent, "%", 0, start));
                        i++;
                        break;
                    case '&':
                        tokens.Add(new Token(TokenType.Ampersand, "&", 0, start));
                        i++;
                        break;
                    case '(':
                        tokens.Add(new Token(TokenType.LeftParen, "(", 0, start));
                        i++;
                        break;
                    case ')':
                        tokens.Add(new Token(TokenType.RightParen, ")", 0, start));
                        i++;
                        break;
                    case ',':
                        tokens.Add(new Token(TokenType.Comma, ",", 0, start));
                        i++;
                        break;
                    case ':':
                        tokens.Add(new Token(TokenType.Colon, ":", 0, start));
                        i++;
                        break;
                    case '=':
                        tokens.Add(new Token(TokenType.Equal, "=", 0, start));
                        i++;
                        break;
                    case '<':
                        if (i + 1 < length && text[i + 1] == '>')
                        {
                            tokens.Add(new Token(TokenType.NotEqual, "<>", 0, start));
                            i += 2;
                        }
                        else if (i + 1 < length && text[i + 1] == '=')
                        {
                            tokens.Add(new Token(TokenType.LessThanOrEqual, "<=", 0, start));
                            i += 2;
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.LessThan, "<", 0, start));
                            i++;
                        }

                        break;
                    case '>':
                        if (i + 1 < length && text[i + 1] == '=')
                        {
                            tokens.Add(new Token(TokenType.GreaterThanOrEqual, ">=", 0, start));
                            i += 2;
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.GreaterThan, ">", 0, start));
                            i++;
                        }

                        break;
                    default:
                        throw new FormulaSyntaxException($"Unexpected character '{c}' in formula.") { Position = start };
                }
            }

            tokens.Add(new Token(TokenType.EndOfInput, string.Empty, 0, length));
            return tokens;
        }

        private static int ReadNumber(string text, int i)
        {
            var length = text.Length;
            while (i < length && char.IsDigit(text[i]))
            {
                i++;
            }

            if (i < length && text[i] == '.')
            {
                i++;
                while (i < length && char.IsDigit(text[i]))
                {
                    i++;
                }
            }

            if (i < length && (text[i] == 'e' || text[i] == 'E'))
            {
                var mark = i;
                i++;
                if (i < length && (text[i] == '+' || text[i] == '-'))
                {
                    i++;
                }

                if (i < length && char.IsDigit(text[i]))
                {
                    while (i < length && char.IsDigit(text[i]))
                    {
                        i++;
                    }
                }
                else
                {
                    // Not actually an exponent (e.g. a word starting with "1e" as a reference-like token
                    // never occurs, but be defensive): back off.
                    i = mark;
                }
            }

            return i;
        }

        private static double ParseNumber(string text, int start, int end)
        {
            return double.Parse(text.Substring(start, end - start), NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        private static int ReadWord(string text, int i)
        {
            var length = text.Length;
            while (i < length && (char.IsLetterOrDigit(text[i]) || text[i] == '$' || text[i] == '_'))
            {
                i++;
            }

            return i;
        }

        private static int ReadString(string text, int i, out string value)
        {
            var length = text.Length;
            var start = i;
            i++; // skip opening quote
            var sb = new StringBuilder();
            while (true)
            {
                if (i >= length)
                {
                    throw new FormulaSyntaxException("Unterminated string literal.") { Position = start };
                }

                var c = text[i];
                if (c == '"')
                {
                    if (i + 1 < length && text[i + 1] == '"')
                    {
                        sb.Append('"');
                        i += 2;
                        continue;
                    }

                    i++;
                    break;
                }

                sb.Append(c);
                i++;
            }

            value = sb.ToString();
            return i;
        }
    }
}
