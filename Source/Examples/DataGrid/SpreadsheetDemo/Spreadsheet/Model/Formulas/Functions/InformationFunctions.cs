// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InformationFunctions.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Built-in information functions: ISBLANK, ISNUMBER, ISTEXT, ISERROR.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions
{
    /// <summary>
    /// Built-in information functions: <c>ISBLANK</c>, <c>ISNUMBER</c>, <c>ISTEXT</c>, <c>ISERROR</c>.
    /// </summary>
    internal static class InformationFunctions
    {
        /// <summary>
        /// Registers the functions in this group.
        /// </summary>
        public static void RegisterAll(FunctionRegistry registry)
        {
            registry.Register("ISBLANK", 1, 1, context => CellValue.FromBoolean(context.GetArgument(0).IsEmpty));
            registry.Register("ISNUMBER", 1, 1, context => CellValue.FromBoolean(context.GetArgument(0).Type == CellValueType.Number));
            registry.Register("ISTEXT", 1, 1, context => CellValue.FromBoolean(context.GetArgument(0).Type == CellValueType.Text));
            registry.Register("ISERROR", 1, 1, context => CellValue.FromBoolean(context.GetArgument(0).IsError));
        }
    }
}
