// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Lets text be assigned where a Cell is expected, so that DataGrid paste operations succeed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Lets <c>PropertyTools.Wpf.DataGrid</c>'s built-in value coercion (used when pasting from the
    /// clipboard) accept plain strings where a <see cref="Cell" /> is expected.
    /// </summary>
    /// <remarks>
    /// The grid's multi-cell edit propagation assigns whole <see cref="Cell" /> instances directly;
    /// that is already handled by an exact-type match before any <see cref="TypeConverter" /> runs.
    /// This converter exists only to unblock the clipboard-paste path, which offers plain strings.
    /// </remarks>
    public sealed class CellConverter : TypeConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                // The caller (the sheet's grid adapter) re-parses this text through
                // Sheet.SetCellText, which needs the target cell's address; this converter only has
                // to make the grid's value coercion succeed, so it passes the text through unchanged.
                return value;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
