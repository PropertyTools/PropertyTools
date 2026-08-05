// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextFunctions.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Built-in text functions: CONCAT, LEN, LEFT, RIGHT, MID, UPPER, LOWER, TRIM.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo.Spreadsheet.Model.Formulas.Functions
{
    using System;
    using System.Text;

    /// <summary>
    /// Built-in text functions: <c>CONCAT</c>, <c>LEN</c>, <c>LEFT</c>, <c>RIGHT</c>, <c>MID</c>,
    /// <c>UPPER</c>, <c>LOWER</c>, <c>TRIM</c>.
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
