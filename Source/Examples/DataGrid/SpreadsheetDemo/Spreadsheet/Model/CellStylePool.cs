// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellStylePool.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interns CellStyle instances so that cells with identical formatting share one object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Interns <see cref="CellStyle" /> instances so that cells with identical formatting share one
    /// object, the way a spreadsheet file shares style/XF records between cells.
    /// </summary>
    internal static class CellStylePool
    {
        /// <summary>
        /// The pool of interned styles, keyed on themselves via <see cref="CellStyle.Equals(CellStyle)" />.
        /// </summary>
        private static readonly ConcurrentDictionary<CellStyle, CellStyle> Styles =
            new ConcurrentDictionary<CellStyle, CellStyle>();

        /// <summary>
        /// Initializes static members of the <see cref="CellStylePool" /> class.
        /// </summary>
        static CellStylePool()
        {
            Styles[CellStyle.Default] = CellStyle.Default;
        }

        /// <summary>
        /// Returns the canonical, shared instance equal to <paramref name="style" />, adding it to the
        /// pool if this is the first time it has been seen.
        /// </summary>
        public static CellStyle Intern(CellStyle style)
        {
            return Styles.GetOrAdd(style, style);
        }
    }
}
