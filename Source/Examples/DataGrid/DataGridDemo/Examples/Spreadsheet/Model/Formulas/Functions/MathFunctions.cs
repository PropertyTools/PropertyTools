// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathFunctions.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Built-in arithmetic functions: SUM, ABS, ROUND, ROUNDUP, ROUNDDOWN, SQRT, POWER, MOD, INT,
//   SIN, COS, TAN.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo.Spreadsheet.Model.Formulas.Functions
{
    using System;

    /// <summary>
    /// Built-in arithmetic functions: <c>SUM</c>, <c>ABS</c>, <c>ROUND</c>, <c>ROUNDUP</c>,
    /// <c>ROUNDDOWN</c>, <c>SQRT</c>, <c>POWER</c>, <c>MOD</c>, <c>INT</c>, <c>SIN</c>, <c>COS</c>,
    /// <c>TAN</c>.
    /// </summary>
    internal static class MathFunctions
    {
        /// <summary>
        /// Registers the functions in this group.
        /// </summary>
        public static void RegisterAll(FunctionRegistry registry)
        {
            registry.Register("SUM", 0, -1, context =>
            {
                if (!FunctionHelpers.TryGetNumbers(context.GetAllArgumentValues(), out var numbers, out var error))
                {
                    return error;
                }

                var sum = 0.0;
                foreach (var number in numbers)
                {
                    sum += number;
                }

                return CellValue.FromNumber(sum);
            });

            registry.Register("ABS", 1, 1, context =>
            {
                if (!FunctionHelpers.TryGetNumber(context.GetArgument(0), out var n, out var error))
                {
                    return error;
                }

                return CellValue.FromNumber(Math.Abs(n));
            });

            registry.Register("SQRT", 1, 1, context =>
            {
                if (!FunctionHelpers.TryGetNumber(context.GetArgument(0), out var n, out var error))
                {
                    return error;
                }

                return n < 0 ? CellValue.FromError(CellError.Number) : CellValue.FromNumber(Math.Sqrt(n));
            });

            registry.Register("POWER", 2, 2, context =>
            {
                if (!FunctionHelpers.TryGetNumber(context.GetArgument(0), out var b, out var error))
                {
                    return error;
                }

                if (!FunctionHelpers.TryGetNumber(context.GetArgument(1), out var e, out error))
                {
                    return error;
                }

                return CellValue.FromNumber(Math.Pow(b, e));
            });

            registry.Register("MOD", 2, 2, context =>
            {
                if (!FunctionHelpers.TryGetNumber(context.GetArgument(0), out var x, out var error))
                {
                    return error;
                }

                if (!FunctionHelpers.TryGetNumber(context.GetArgument(1), out var y, out error))
                {
                    return error;
                }

                if (y == 0)
                {
                    return CellValue.FromError(CellError.DivideByZero);
                }

                return CellValue.FromNumber(x - (y * Math.Floor(x / y)));
            });

            registry.Register("INT", 1, 1, context =>
            {
                if (!FunctionHelpers.TryGetNumber(context.GetArgument(0), out var n, out var error))
                {
                    return error;
                }

                return CellValue.FromNumber(Math.Floor(n));
            });

            registry.Register("ROUND", 2, 2, context => Round(context, RoundMode.Nearest));
            registry.Register("ROUNDUP", 2, 2, context => Round(context, RoundMode.Up));
            registry.Register("ROUNDDOWN", 2, 2, context => Round(context, RoundMode.Down));

            registry.Register("SIN", 1, 1, context => Trig(context, Math.Sin));
            registry.Register("COS", 1, 1, context => Trig(context, Math.Cos));
            registry.Register("TAN", 1, 1, context => Trig(context, Math.Tan));
        }

        private static CellValue Trig(FunctionCallContext context, Func<double, double> function)
        {
            if (!FunctionHelpers.TryGetNumber(context.GetArgument(0), out var n, out var error))
            {
                return error;
            }

            return CellValue.FromNumber(function(n));
        }

        private enum RoundMode
        {
            Nearest,
            Up,
            Down
        }

        private static CellValue Round(FunctionCallContext context, RoundMode mode)
        {
            if (!FunctionHelpers.TryGetNumber(context.GetArgument(0), out var number, out var error))
            {
                return error;
            }

            if (!FunctionHelpers.TryGetNumber(context.GetArgument(1), out var digitsValue, out error))
            {
                return error;
            }

            var factor = Math.Pow(10, (int)digitsValue);
            var scaled = number * factor;

            double roundedScaled;
            switch (mode)
            {
                case RoundMode.Up:
                    roundedScaled = scaled >= 0 ? Math.Ceiling(scaled) : Math.Floor(scaled);
                    break;
                case RoundMode.Down:
                    roundedScaled = scaled >= 0 ? Math.Floor(scaled) : Math.Ceiling(scaled);
                    break;
                default:
                    roundedScaled = Math.Round(scaled, MidpointRounding.AwayFromZero);
                    break;
            }

            return CellValue.FromNumber(roundedScaled / factor);
        }
    }
}
