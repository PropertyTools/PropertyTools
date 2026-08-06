// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextFunctions.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Built-in text functions: CONCAT, LEN, LEFT, RIGHT, MID, UPPER, LOWER, TRIM, FIND, SUBSTITUTE,
//   REPT, PROPER.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Built-in text functions: <c>CONCAT</c>, <c>LEN</c>, <c>LEFT</c>, <c>RIGHT</c>, <c>MID</c>,
    /// <c>UPPER</c>, <c>LOWER</c>, <c>TRIM</c>, <c>FIND</c>, <c>SUBSTITUTE</c>, <c>REPT</c>,
    /// <c>PROPER</c>.
    /// </summary>
    internal static class TextFunctions
    {
        /// <summary>
        /// Registers the functions in this group.
        /// </summary>
        public static void RegisterAll(FunctionRegistry registry)
        {
            registry.Register("CONCAT", 1, -1, context =>
            {
                var sb = new StringBuilder();
                foreach (var value in context.GetAllArgumentValues())
                {
                    if (!FunctionHelpers.TryGetText(value, out var text, out var error))
                    {
                        return error;
                    }

                    sb.Append(text);
                }

                return CellValue.FromText(sb.ToString());
            });

            registry.Register("LEN", 1, 1, context =>
            {
                if (!FunctionHelpers.TryGetText(context.GetArgument(0), out var text, out var error))
                {
                    return error;
                }

                return CellValue.FromNumber(text.Length);
            });

            registry.Register("UPPER", 1, 1, context =>
            {
                if (!FunctionHelpers.TryGetText(context.GetArgument(0), out var text, out var error))
                {
                    return error;
                }

                return CellValue.FromText(text.ToUpperInvariant());
            });

            registry.Register("LOWER", 1, 1, context =>
            {
                if (!FunctionHelpers.TryGetText(context.GetArgument(0), out var text, out var error))
                {
                    return error;
                }

                return CellValue.FromText(text.ToLowerInvariant());
            });

            registry.Register("TRIM", 1, 1, context =>
            {
                if (!FunctionHelpers.TryGetText(context.GetArgument(0), out var text, out var error))
                {
                    return error;
                }

                // Excel's TRIM also collapses internal runs of multiple spaces down to one.
                var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return CellValue.FromText(string.Join(" ", words));
            });

            registry.Register("LEFT", 1, 2, context => Substring(context, fromStart: true));
            registry.Register("RIGHT", 1, 2, context => Substring(context, fromStart: false));

            registry.Register("MID", 3, 3, context =>
            {
                if (!FunctionHelpers.TryGetText(context.GetArgument(0), out var text, out var error))
                {
                    return error;
                }

                if (!FunctionHelpers.TryGetNumber(context.GetArgument(1), out var startNumber, out error))
                {
                    return error;
                }

                if (!FunctionHelpers.TryGetNumber(context.GetArgument(2), out var lengthNumber, out error))
                {
                    return error;
                }

                var start = (int)startNumber;
                var length = (int)lengthNumber;
                if (start < 1 || length < 0)
                {
                    return CellValue.FromError(CellError.Value);
                }

                var startIndex = start - 1;
                if (startIndex >= text.Length)
                {
                    return CellValue.FromText(string.Empty);
                }

                var actualLength = Math.Min(length, text.Length - startIndex);
                return CellValue.FromText(text.Substring(startIndex, actualLength));
            });

            registry.Register("FIND", 2, 3, context =>
            {
                if (!FunctionHelpers.TryGetText(context.GetArgument(0), out var findText, out var error))
                {
                    return error;
                }

                if (!FunctionHelpers.TryGetText(context.GetArgument(1), out var withinText, out error))
                {
                    return error;
                }

                var startNum = 1;
                if (context.ArgumentCount > 2)
                {
                    if (!FunctionHelpers.TryGetNumber(context.GetArgument(2), out var startNumber, out error))
                    {
                        return error;
                    }

                    startNum = (int)startNumber;
                }

                if (startNum < 1 || startNum > withinText.Length + 1)
                {
                    return CellValue.FromError(CellError.Value);
                }

                var index = withinText.IndexOf(findText, startNum - 1, StringComparison.Ordinal);
                return index < 0 ? CellValue.FromError(CellError.Value) : CellValue.FromNumber(index + 1);
            });

            registry.Register("SUBSTITUTE", 3, 4, context =>
            {
                if (!FunctionHelpers.TryGetText(context.GetArgument(0), out var text, out var error))
                {
                    return error;
                }

                if (!FunctionHelpers.TryGetText(context.GetArgument(1), out var oldText, out error))
                {
                    return error;
                }

                if (!FunctionHelpers.TryGetText(context.GetArgument(2), out var newText, out error))
                {
                    return error;
                }

                if (string.IsNullOrEmpty(oldText))
                {
                    return CellValue.FromText(text);
                }

                if (context.ArgumentCount <= 3)
                {
                    return CellValue.FromText(text.Replace(oldText, newText));
                }

                if (!FunctionHelpers.TryGetNumber(context.GetArgument(3), out var instanceNumber, out error))
                {
                    return error;
                }

                var instance = (int)instanceNumber;
                if (instance < 1)
                {
                    return CellValue.FromError(CellError.Value);
                }

                var searchStart = 0;
                var occurrence = 0;
                while (true)
                {
                    var index = text.IndexOf(oldText, searchStart, StringComparison.Ordinal);
                    if (index < 0)
                    {
                        return CellValue.FromText(text);
                    }

                    occurrence++;
                    if (occurrence == instance)
                    {
                        return CellValue.FromText(text.Substring(0, index) + newText + text.Substring(index + oldText.Length));
                    }

                    searchStart = index + oldText.Length;
                }
            });

            registry.Register("REPT", 2, 2, context =>
            {
                if (!FunctionHelpers.TryGetText(context.GetArgument(0), out var text, out var error))
                {
                    return error;
                }

                if (!FunctionHelpers.TryGetNumber(context.GetArgument(1), out var countNumber, out error))
                {
                    return error;
                }

                var count = (int)countNumber;
                if (count < 0)
                {
                    return CellValue.FromError(CellError.Value);
                }

                var sb = new StringBuilder();
                for (var i = 0; i < count; i++)
                {
                    sb.Append(text);
                }

                return CellValue.FromText(sb.ToString());
            });

            registry.Register("PROPER", 1, 1, context =>
            {
                if (!FunctionHelpers.TryGetText(context.GetArgument(0), out var text, out var error))
                {
                    return error;
                }

                return CellValue.FromText(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text.ToLowerInvariant()));
            });
        }

        private static CellValue Substring(FunctionCallContext context, bool fromStart)
        {
            if (!FunctionHelpers.TryGetText(context.GetArgument(0), out var text, out var error))
            {
                return error;
            }

            var count = 1;
            if (context.ArgumentCount > 1)
            {
                if (!FunctionHelpers.TryGetNumber(context.GetArgument(1), out var countNumber, out error))
                {
                    return error;
                }

                count = (int)countNumber;
            }

            if (count < 0)
            {
                return CellValue.FromError(CellError.Value);
            }

            count = Math.Min(count, text.Length);
            return CellValue.FromText(fromStart ? text.Substring(0, count) : text.Substring(text.Length - count));
        }
    }
}
