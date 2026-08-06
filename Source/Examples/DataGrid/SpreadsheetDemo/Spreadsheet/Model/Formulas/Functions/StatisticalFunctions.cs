// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatisticalFunctions.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Built-in statistical functions: AVERAGE, MIN, MAX, COUNT, COUNTA.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions
{
    /// <summary>
    /// Built-in statistical functions: <c>AVERAGE</c>, <c>MIN</c>, <c>MAX</c>, <c>COUNT</c>, <c>COUNTA</c>.
    /// </summary>
    internal static class StatisticalFunctions
    {
        /// <summary>
        /// Registers the functions in this group.
        /// </summary>
        public static void RegisterAll(FunctionRegistry registry)
        {
            registry.Register("AVERAGE", 1, -1, context =>
            {
                if (!FunctionHelpers.TryGetNumbers(context.GetAllArgumentValues(), out var numbers, out var error))
                {
                    return error;
                }

                if (numbers.Count == 0)
                {
                    return CellValue.FromError(CellError.DivideByZero);
                }

                var sum = 0.0;
                foreach (var number in numbers)
                {
                    sum += number;
                }

                return CellValue.FromNumber(sum / numbers.Count);
            });

            registry.Register("MIN", 1, -1, context =>
            {
                if (!FunctionHelpers.TryGetNumbers(context.GetAllArgumentValues(), out var numbers, out var error))
                {
                    return error;
                }

                if (numbers.Count == 0)
                {
                    return CellValue.FromNumber(0);
                }

                var min = numbers[0];
                foreach (var number in numbers)
                {
                    if (number < min)
                    {
                        min = number;
                    }
                }

                return CellValue.FromNumber(min);
            });

            registry.Register("MAX", 1, -1, context =>
            {
                if (!FunctionHelpers.TryGetNumbers(context.GetAllArgumentValues(), out var numbers, out var error))
                {
                    return error;
                }

                if (numbers.Count == 0)
                {
                    return CellValue.FromNumber(0);
                }

                var max = numbers[0];
                foreach (var number in numbers)
                {
                    if (number > max)
                    {
                        max = number;
                    }
                }

                return CellValue.FromNumber(max);
            });

            registry.Register("COUNT", 0, -1, context =>
            {
                var count = 0;
                foreach (var value in context.GetAllArgumentValues())
                {
                    if (value.Type == CellValueType.Number || value.Type == CellValueType.DateTime || value.Type == CellValueType.Duration)
                    {
                        count++;
                    }
                }

                return CellValue.FromNumber(count);
            });

            registry.Register("COUNTA", 0, -1, context =>
            {
                var count = 0;
                foreach (var value in context.GetAllArgumentValues())
                {
                    if (!value.IsEmpty)
                    {
                        count++;
                    }
                }

                return CellValue.FromNumber(count);
            });
        }
    }
}
