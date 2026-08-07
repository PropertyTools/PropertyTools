// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellValue.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a typed value in a spreadsheet cell.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents a typed value in a spreadsheet cell: empty, a number, text, a boolean, a date/time or an error.
    /// </summary>
    /// <remarks>
    /// This is a readonly struct so that numbers, booleans, dates and errors do not allocate.
    /// </remarks>
    public readonly struct CellValue : IEquatable<CellValue>, IComparable<CellValue>
    {
        /// <summary>
        /// The numeric payload: the number itself, 0/1 for a boolean, an OLE Automation date for a
        /// date/time value, or the underlying <see cref="CellError" /> for an error.
        /// </summary>
        private readonly double number;

        /// <summary>
        /// The text payload. Only used when <see cref="Type" /> is <see cref="CellValueType.Text" />.
        /// </summary>
        private readonly string text;

        /// <summary>
        /// The kind of value stored.
        /// </summary>
        private readonly CellValueType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="CellValue" /> struct.
        /// </summary>
        private CellValue(CellValueType type, double number, string text)
        {
            this.type = type;
            this.number = number;
            this.text = text;
        }

        /// <summary>
        /// Gets the empty value.
        /// </summary>
        public static readonly CellValue Empty = default(CellValue);

        /// <summary>
        /// Creates a number value.
        /// </summary>
        public static CellValue FromNumber(double value)
        {
            return new CellValue(CellValueType.Number, value, null);
        }

        /// <summary>
        /// Creates a text value. A <c>null</c> or empty string produces <see cref="Empty" />.
        /// </summary>
        public static CellValue FromText(string value)
        {
            return string.IsNullOrEmpty(value) ? Empty : new CellValue(CellValueType.Text, 0, value);
        }

        /// <summary>
        /// Creates a boolean value.
        /// </summary>
        public static CellValue FromBoolean(bool value)
        {
            return new CellValue(CellValueType.Boolean, value ? 1 : 0, null);
        }

        /// <summary>
        /// Creates a date/time value.
        /// </summary>
        public static CellValue FromDateTime(DateTime value)
        {
            return new CellValue(CellValueType.DateTime, value.ToOADate(), null);
        }

        /// <summary>
        /// Creates a duration (elapsed time) value.
        /// </summary>
        /// <remarks>
        /// Stored as <see cref="TimeSpan.TotalDays" /> — the same unit <see cref="FromDateTime" /> uses
        /// (an OLE Automation date is also a day count, just anchored to an epoch), so a duration added
        /// to a date shifts it by that many days, and summing durations in a formula adds correctly.
        /// </remarks>
        public static CellValue FromDuration(TimeSpan value)
        {
            return new CellValue(CellValueType.Duration, value.TotalDays, null);
        }

        /// <summary>
        /// Creates an error value.
        /// </summary>
        public static CellValue FromError(CellError error)
        {
            return new CellValue(CellValueType.Error, (double)error, null);
        }

        /// <summary>
        /// Gets the kind of value stored.
        /// </summary>
        public CellValueType Type => this.type;

        /// <summary>
        /// Gets a value indicating whether this value is empty.
        /// </summary>
        public bool IsEmpty => this.type == CellValueType.Empty;

        /// <summary>
        /// Gets a value indicating whether this value is an error.
        /// </summary>
        public bool IsError => this.type == CellValueType.Error;

        /// <summary>
        /// Gets the error, or <see cref="CellError.None" /> if this value is not an error.
        /// </summary>
        public CellError Error => this.type == CellValueType.Error ? (CellError)(int)this.number : CellError.None;

        /// <summary>
        /// Gets the number. Throws for values that are not numbers, booleans, dates or durations.
        /// </summary>
        public double AsNumber()
        {
            switch (this.type)
            {
                case CellValueType.Number:
                case CellValueType.Boolean:
                case CellValueType.DateTime:
                case CellValueType.Duration:
                    return this.number;
                default:
                    throw new InvalidOperationException($"Cannot get a number from a {this.type} value.");
            }
        }

        /// <summary>
        /// Gets the text. Throws for values that are not text.
        /// </summary>
        public string AsText()
        {
            if (this.type != CellValueType.Text)
            {
                throw new InvalidOperationException($"Cannot get text from a {this.type} value.");
            }

            return this.text;
        }

        /// <summary>
        /// Gets the boolean. Throws for values that are not booleans.
        /// </summary>
        public bool AsBoolean()
        {
            if (this.type != CellValueType.Boolean)
            {
                throw new InvalidOperationException($"Cannot get a boolean from a {this.type} value.");
            }

            return this.number != 0;
        }

        /// <summary>
        /// Gets the date/time. Throws for values that are not dates.
        /// </summary>
        public DateTime AsDateTime()
        {
            if (this.type != CellValueType.DateTime)
            {
                throw new InvalidOperationException($"Cannot get a date from a {this.type} value.");
            }

            return DateTime.FromOADate(this.number);
        }

        /// <summary>
        /// Gets the duration. Throws for values that are not durations.
        /// </summary>
        public TimeSpan AsDuration()
        {
            if (this.type != CellValueType.Duration)
            {
                throw new InvalidOperationException($"Cannot get a duration from a {this.type} value.");
            }

            return TimeSpan.FromDays(this.number);
        }

        /// <summary>
        /// Tries to coerce this value to a number, following spreadsheet conventions: booleans become
        /// 0/1, dates and durations become their day-count value, an empty cell becomes 0, and
        /// numeric-looking text is parsed using the invariant culture.
        /// </summary>
        /// <param name="value">The resulting number.</param>
        /// <returns><c>true</c> if the value could be coerced to a number.</returns>
        public bool TryGetNumber(out double value)
        {
            switch (this.type)
            {
                case CellValueType.Number:
                case CellValueType.Boolean:
                case CellValueType.DateTime:
                case CellValueType.Duration:
                    value = this.number;
                    return true;
                case CellValueType.Empty:
                    value = 0;
                    return true;
                case CellValueType.Text:
                    return double.TryParse(
                        this.text,
                        NumberStyles.Float | NumberStyles.AllowThousands,
                        CultureInfo.InvariantCulture,
                        out value);
                default:
                    value = 0;
                    return false;
            }
        }

        /// <summary>
        /// Tries to convert this value to text, following spreadsheet conventions (used by string
        /// concatenation): an empty cell becomes an empty string, numbers/booleans/dates are formatted
        /// with the invariant culture, and errors fail.
        /// </summary>
        /// <param name="value">The resulting text.</param>
        /// <returns><c>true</c> if the value could be converted to text.</returns>
        public bool TryGetText(out string value)
        {
            switch (this.type)
            {
                case CellValueType.Empty:
                    value = string.Empty;
                    return true;
                case CellValueType.Text:
                    value = this.text;
                    return true;
                case CellValueType.Number:
                    value = this.number.ToString(CultureInfo.InvariantCulture);
                    return true;
                case CellValueType.Boolean:
                    value = this.number != 0 ? "TRUE" : "FALSE";
                    return true;
                case CellValueType.DateTime:
                    value = DateTime.FromOADate(this.number).ToString(CultureInfo.InvariantCulture);
                    return true;
                case CellValueType.Duration:
                    value = TimeSpan.FromDays(this.number).ToString("c", CultureInfo.InvariantCulture);
                    return true;
                default:
                    value = null;
                    return false;
            }
        }

        /// <summary>
        /// Converts this value to a boxed CLR object: <c>null</c>, <see cref="double" />,
        /// <see cref="string" />, <see cref="bool" />, <see cref="DateTime" /> or the error text.
        /// </summary>
        /// <remarks>This boxes the value; use it only at boundaries (clipboard, serialization).</remarks>
        public object ToObject()
        {
            switch (this.type)
            {
                case CellValueType.Empty:
                    return null;
                case CellValueType.Number:
                    return this.number;
                case CellValueType.Text:
                    return this.text;
                case CellValueType.Boolean:
                    return this.number != 0;
                case CellValueType.DateTime:
                    return DateTime.FromOADate(this.number);
                case CellValueType.Duration:
                    return TimeSpan.FromDays(this.number);
                case CellValueType.Error:
                    return CellErrorText.ToDisplayText((CellError)(int)this.number);
                default:
                    return null;
            }
        }

        /// <inheritdoc />
        public bool Equals(CellValue other)
        {
            if (this.type != other.type)
            {
                return false;
            }

            switch (this.type)
            {
                case CellValueType.Empty:
                    return true;
                case CellValueType.Text:
                    return string.Equals(this.text, other.text, StringComparison.Ordinal);
                default:
                    return this.number.Equals(other.number);
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is CellValue other && this.Equals(other);
        }

        /// <summary>
        /// Compares two values using spreadsheet ordering rules: empty &lt; numbers/dates/durations
        /// &lt; text &lt; booleans when the types differ (used by formula comparison operators and
        /// sorting); otherwise a natural same-type comparison (numeric for numbers/dates/durations,
        /// case-insensitive for text).
        /// </summary>
        public int CompareTo(CellValue other)
        {
            var rank = TypeRank(this);
            var otherRank = TypeRank(other);
            if (rank != otherRank)
            {
                return rank.CompareTo(otherRank);
            }

            switch (this.type)
            {
                case CellValueType.Number:
                case CellValueType.DateTime:
                case CellValueType.Duration:
                    return this.number.CompareTo(other.number);
                case CellValueType.Boolean:
                    return this.number.CompareTo(other.number);
                case CellValueType.Text:
                    return string.Compare(this.text, other.text, StringComparison.OrdinalIgnoreCase);
                default:
                    return 0;
            }
        }

        private static int TypeRank(CellValue value)
        {
            switch (value.type)
            {
                case CellValueType.Empty:
                    return 0;
                case CellValueType.Number:
                case CellValueType.DateTime:
                case CellValueType.Duration:
                    return 1;
                case CellValueType.Text:
                    return 2;
                case CellValueType.Boolean:
                    return 3;
                default:
                    return 4;
            }
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)this.type * 397;
                return this.type == CellValueType.Text
                    ? hash ^ (this.text?.GetHashCode() ?? 0)
                    : hash ^ this.number.GetHashCode();
            }
        }

        /// <summary>
        /// Determines whether two values are equal.
        /// </summary>
        public static bool operator ==(CellValue left, CellValue right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two values are not equal.
        /// </summary>
        public static bool operator !=(CellValue left, CellValue right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a diagnostic string representation of this value (invariant culture). Use
        /// <see cref="CellFormatter" /> to format a value for display to the user.
        /// </summary>
        public override string ToString()
        {
            if (this.type == CellValueType.Error)
            {
                return CellErrorText.ToDisplayText(this.Error);
            }

            return this.TryGetText(out var s) ? s : string.Empty;
        }
    }
}
