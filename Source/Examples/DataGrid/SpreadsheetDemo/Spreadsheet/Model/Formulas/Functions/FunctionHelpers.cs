// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionHelpers.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Shared argument coercion and error-propagation helpers for built-in functions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions
{
    using System.Collections.Generic;

    /// <summary>
    /// Shared argument coercion and error-propagation helpers for built-in functions.
    /// </summary>
    internal static class FunctionHelpers
    {
        /// <summary>
        /// Tries to coerce a value to a number for use inside a function. Returns <c>false</c> and
        /// sets <paramref name="error" /> if the value is itself an error or cannot be coerced.
        /// </summary>
        public static bool TryGetNumber(CellValue value, out double number, out CellValue error)
        {
            if (value.IsError)
            {
                error = value;
                number = 0;
                return false;
            }

            if (value.TryGetNumber(out number))
            {
                error = CellValue.Empty;
                return true;
            }

            error = CellValue.FromError(CellError.Value);
            return false;
        }

        /// <summary>
        /// Tries to coerce a value to text for use inside a function. Returns <c>false</c> and sets
        /// <paramref name="error" /> if the value is itself an error.
        /// </summary>
        public static bool TryGetText(CellValue value, out string text, out CellValue error)
        {
            if (value.IsError)
            {
                error = value;
                text = null;
                return false;
            }

            value.TryGetText(out text);
            error = CellValue.Empty;
            return true;
        }

        /// <summary>
        /// Tries to coerce a value to a boolean for use inside a function: booleans pass through,
        /// numbers are non-zero, empty is false, and the text "TRUE"/"FALSE" (any case) converts.
        /// Returns <c>false</c> and sets <paramref name="error" /> for anything else, or for an error value.
        /// </summary>
        public static bool TryGetBoolean(CellValue value, out bool boolean, out CellValue error)
        {
            if (value.IsError)
            {
                error = value;
                boolean = false;
                return false;
            }

            switch (value.Type)
            {
                case CellValueType.Boolean:
                    boolean = value.AsBoolean();
                    error = CellValue.Empty;
                    return true;
                case CellValueType.Number:
                    boolean = value.AsNumber() != 0;
                    error = CellValue.Empty;
                    return true;
                case CellValueType.Empty:
                    boolean = false;
                    error = CellValue.Empty;
                    return true;
                case CellValueType.Text:
                    var text = value.AsText();
                    if (string.Equals(text, "TRUE", System.StringComparison.OrdinalIgnoreCase))
                    {
                        boolean = true;
                        error = CellValue.Empty;
                        return true;
                    }

                    if (string.Equals(text, "FALSE", System.StringComparison.OrdinalIgnoreCase))
                    {
                        boolean = false;
                        error = CellValue.Empty;
                        return true;
                    }

                    boolean = false;
                    error = CellValue.FromError(CellError.Value);
                    return false;
                default:
                    boolean = false;
                    error = CellValue.FromError(CellError.Value);
                    return false;
            }
        }

        /// <summary>
        /// Reduces a sequence of argument values (scalars and expanded ranges) to their numbers,
        /// silently skipping empty cells and text, and propagating the first error encountered.
        /// </summary>
        /// <remarks>
        /// This does not replicate Excel's asymmetry where a range's text is ignored but a directly
        /// supplied text argument produces #VALUE! — both are simply skipped here, which covers the
        /// common case with a single, easy-to-explain rule.
        /// </remarks>
        public static bool TryGetNumbers(IEnumerable<CellValue> values, out List<double> numbers, out CellValue error)
        {
            numbers = new List<double>();
            foreach (var value in values)
            {
                if (value.IsError)
                {
                    error = value;
                    return false;
                }

                switch (value.Type)
                {
                    case CellValueType.Empty:
                    case CellValueType.Text:
                        continue;
                    case CellValueType.Number:
                    case CellValueType.Boolean:
                    case CellValueType.DateTime:
                    case CellValueType.Duration:
                        numbers.Add(value.AsNumber());
                        continue;
                }
            }

            error = CellValue.Empty;
            return true;
        }
    }
}
