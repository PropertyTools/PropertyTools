// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Token.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a single lexical token in a formula.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas
{
    /// <summary>
    /// Represents a single lexical token in a formula.
    /// </summary>
    internal readonly struct Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Token" /> struct.
        /// </summary>
        public Token(TokenType type, string text, double number, int position)
        {
            this.Type = type;
            this.Text = text;
            this.Number = number;
            this.Position = position;
        }

        /// <summary>
        /// Gets the kind of token.
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// Gets the source text of the token (the word/number/string as written, or the operator symbol).
        /// For <see cref="TokenType.String" />, this is the decoded string content (quotes removed,
        /// <c>""</c> unescaped to <c>"</c>).
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the numeric value. Only meaningful when <see cref="Type" /> is <see cref="TokenType.Number" />.
        /// </summary>
        public double Number { get; }

        /// <summary>
        /// Gets the 0-based character position where this token starts in the source text.
        /// </summary>
        public int Position { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Type == TokenType.EndOfInput ? "<end of formula>" : $"'{this.Text}'";
        }
    }
}
