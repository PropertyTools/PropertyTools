// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellStyle.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents the immutable formatting of a cell.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System;

    /// <summary>
    /// Represents the immutable formatting of a cell: alignment, bold, italic and number format.
    /// </summary>
    /// <remarks>
    /// Instances are interned (see <see cref="CellStylePool" />), so cells that share the same
    /// formatting share the same <see cref="CellStyle" /> object.
    /// </remarks>
    public sealed class CellStyle : IEquatable<CellStyle>
    {
        /// <summary>
        /// Gets the default style: general alignment, not bold, not italic, no explicit format string.
        /// </summary>
        public static readonly CellStyle Default = new CellStyle(CellHorizontalAlignment.General, false, false, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="CellStyle" /> class.
        /// </summary>
        private CellStyle(CellHorizontalAlignment horizontalAlignment, bool bold, bool italic, string formatString)
        {
            this.HorizontalAlignment = horizontalAlignment;
            this.Bold = bold;
            this.Italic = italic;
            this.FormatString = formatString;
        }

        /// <summary>
        /// Gets the horizontal alignment.
        /// </summary>
        public CellHorizontalAlignment HorizontalAlignment { get; }

        /// <summary>
        /// Gets a value indicating whether the cell text is bold.
        /// </summary>
        public bool Bold { get; }

        /// <summary>
        /// Gets a value indicating whether the cell text is italic.
        /// </summary>
        public bool Italic { get; }

        /// <summary>
        /// Gets the .NET format string applied to numbers and dates, or <c>null</c> for the general format.
        /// </summary>
        public string FormatString { get; }

        /// <summary>
        /// Returns a style equal to this one except for the horizontal alignment.
        /// </summary>
        public CellStyle WithHorizontalAlignment(CellHorizontalAlignment alignment)
        {
            return alignment == this.HorizontalAlignment
                ? this
                : CellStylePool.Intern(new CellStyle(alignment, this.Bold, this.Italic, this.FormatString));
        }

        /// <summary>
        /// Returns a style equal to this one except for the bold flag.
        /// </summary>
        public CellStyle WithBold(bool bold)
        {
            return bold == this.Bold
                ? this
                : CellStylePool.Intern(new CellStyle(this.HorizontalAlignment, bold, this.Italic, this.FormatString));
        }

        /// <summary>
        /// Returns a style equal to this one except for the italic flag.
        /// </summary>
        public CellStyle WithItalic(bool italic)
        {
            return italic == this.Italic
                ? this
                : CellStylePool.Intern(new CellStyle(this.HorizontalAlignment, this.Bold, italic, this.FormatString));
        }

        /// <summary>
        /// Returns a style equal to this one except for the format string.
        /// </summary>
        public CellStyle WithFormat(string formatString)
        {
            return string.Equals(formatString, this.FormatString, StringComparison.Ordinal)
                ? this
                : CellStylePool.Intern(new CellStyle(this.HorizontalAlignment, this.Bold, this.Italic, formatString));
        }

        /// <inheritdoc />
        public bool Equals(CellStyle other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.HorizontalAlignment == other.HorizontalAlignment
                && this.Bold == other.Bold
                && this.Italic == other.Italic
                && string.Equals(this.FormatString, other.FormatString, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return this.Equals(obj as CellStyle);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)this.HorizontalAlignment;
                hash = (hash * 397) ^ this.Bold.GetHashCode();
                hash = (hash * 397) ^ this.Italic.GetHashCode();
                hash = (hash * 397) ^ (this.FormatString?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
