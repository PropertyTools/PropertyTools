// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogicalFunctions.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Built-in logical functions: IF, AND, OR, NOT, IFERROR.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions
{
    /// <summary>
    /// Built-in logical functions: <c>IF</c>, <c>AND</c>, <c>OR</c>, <c>NOT</c>, <c>IFERROR</c>.
    /// </summary>
    internal static class LogicalFunctions
    {
        /// <summary>
        /// Registers the functions in this group.
        /// </summary>
        public static void RegisterAll(FunctionRegistry registry)
        {
            registry.Register("IF", 2, 3, context =>
            {
                if (!FunctionHelpers.TryGetBoolean(context.GetArgument(0), out var condition, out var error))
                {
                    return error;
                }

                if (condition)
                {
                    return context.GetArgument(1);
                }

                return context.ArgumentCount > 2 ? context.GetArgument(2) : CellValue.FromBoolean(false);
            });

            registry.Register("AND", 1, -1, context =>
            {
                var any = false;
                foreach (var value in context.GetAllArgumentValues())
                {
                    if (value.IsError)
                    {
                        return value;
                    }

                    if (value.Type == CellValueType.Empty || value.Type == CellValueType.Text)
                    {
                        continue;
                    }

                    if (!FunctionHelpers.TryGetBoolean(value, out var b, out var error))
                    {
                        return error;
                    }

                    any = true;
                    if (!b)
                    {
                        return CellValue.FromBoolean(false);
                    }
                }

                return any ? CellValue.FromBoolean(true) : CellValue.FromError(CellError.Value);
            });

            registry.Register("OR", 1, -1, context =>
            {
                var any = false;
                foreach (var value in context.GetAllArgumentValues())
                {
                    if (value.IsError)
                    {
                        return value;
                    }

                    if (value.Type == CellValueType.Empty || value.Type == CellValueType.Text)
                    {
                        continue;
                    }

                    if (!FunctionHelpers.TryGetBoolean(value, out var b, out var error))
                    {
                        return error;
                    }

                    any = true;
                    if (b)
                    {
                        return CellValue.FromBoolean(true);
                    }
                }

                return any ? CellValue.FromBoolean(false) : CellValue.FromError(CellError.Value);
            });

            registry.Register("NOT", 1, 1, context =>
            {
                if (!FunctionHelpers.TryGetBoolean(context.GetArgument(0), out var b, out var error))
                {
                    return error;
                }

                return CellValue.FromBoolean(!b);
            });

            registry.Register("IFERROR", 2, 2, context =>
            {
                var value = context.GetArgument(0);
                return value.IsError ? context.GetArgument(1) : value;
            });
        }
    }
}
