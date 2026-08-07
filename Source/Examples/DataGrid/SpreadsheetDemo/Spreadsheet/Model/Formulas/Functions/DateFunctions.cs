// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateFunctions.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Built-in date functions: TODAY, NOW, DATE, YEAR, MONTH, DAY.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions
{
    using System;

    /// <summary>
    /// Built-in date functions: <c>TODAY</c>, <c>NOW</c>, <c>DATE</c>, <c>YEAR</c>, <c>MONTH</c>, <c>DAY</c>.
    /// </summary>
    internal static class DateFunctions
    {
        /// <summary>
        /// Registers the functions in this group.
        /// </summary>
        public static void RegisterAll(FunctionRegistry registry)
        {
            registry.Register("TODAY", 0, 0, context => CellValue.FromDateTime(DateTime.Today));
            registry.Register("NOW", 0, 0, context => CellValue.FromDateTime(DateTime.Now));

            registry.Register("DATE", 3, 3, context =>
            {
                if (!FunctionHelpers.TryGetNumber(context.GetArgument(0), out var year, out var error))
                {
                    return error;
                }

                if (!FunctionHelpers.TryGetNumber(context.GetArgument(1), out var month, out error))
                {
                    return error;
                }

                if (!FunctionHelpers.TryGetNumber(context.GetArgument(2), out var day, out error))
                {
                    return error;
                }

                try
                {
                    var date = new DateTime((int)year, 1, 1).AddMonths((int)month - 1).AddDays((int)day - 1);
                    return CellValue.FromDateTime(date);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return CellValue.FromError(CellError.Number);
                }
            });

            registry.Register("YEAR", 1, 1, context => DatePart(context, date => date.Year));
            registry.Register("MONTH", 1, 1, context => DatePart(context, date => date.Month));
            registry.Register("DAY", 1, 1, context => DatePart(context, date => date.Day));
        }

        private static CellValue DatePart(FunctionCallContext context, Func<DateTime, int> selector)
        {
            var value = context.GetArgument(0);
            if (value.IsError)
            {
                return value;
            }

            DateTime date;
            if (value.Type == CellValueType.DateTime)
            {
                date = value.AsDateTime();
            }
            else if (value.TryGetNumber(out var oaDate))
            {
                try
                {
                    date = DateTime.FromOADate(oaDate);
                }
                catch (ArgumentException)
                {
                    return CellValue.FromError(CellError.Number);
                }
            }
            else
            {
                return CellValue.FromError(CellError.Value);
            }

            return CellValue.FromNumber(selector(date));
        }
    }
}
