// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvSerializer.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Reads and writes Sheet content as RFC 4180 CSV, for interchange with other spreadsheet tools.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Reads and writes <see cref="Sheet" /> content as RFC 4180 CSV, for interchange with other
    /// spreadsheet tools. Unlike <see cref="SheetSerializer" />, this only round-trips displayed
    /// values as text: formulas are exported as their computed result and re-imported as literal
    /// text, and styles are not preserved.
    /// </summary>
    public static class CsvSerializer
    {
        /// <summary>
        /// Writes the sheet's used range (the bounding box of its non-empty cells) as CSV.
        /// </summary>
        public static void Save(Sheet sheet, Stream stream, char separator = ',')
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var usedRows = 0;
            var usedColumns = 0;
            for (var row = 0; row < sheet.RowCount; row++)
            {
                for (var column = 0; column < sheet.ColumnCount; column++)
                {
                    if (sheet.TryGetCell(new CellAddress(row, column), out var cell) && !cell.IsEmpty)
                    {
                        usedRows = Math.Max(usedRows, row + 1);
                        usedColumns = Math.Max(usedColumns, column + 1);
                    }
                }
            }

            using (var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, leaveOpen: true))
            {
                for (var row = 0; row < usedRows; row++)
                {
                    for (var column = 0; column < usedColumns; column++)
                    {
                        if (column > 0)
                        {
                            writer.Write(separator);
                        }

                        writer.Write(EncodeField(sheet.GetCell(new CellAddress(row, column)).DisplayText, separator));
                    }

                    writer.Write("\r\n");
                }
            }
        }

        /// <summary>
        /// Reads CSV text into a new <see cref="Sheet" /> sized to fit the data, parsing each field
        /// as cell input the same way the grid editor or formula bar would (so numbers, dates and
        /// booleans are recognized, not just stored as text).
        /// </summary>
        public static Sheet Load(Stream stream, string sheetName = "Sheet1", char separator = ',')
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            string text;
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            var rows = Parse(text, separator);

            var rowCount = rows.Count;
            var columnCount = 0;
            foreach (var row in rows)
            {
                columnCount = Math.Max(columnCount, row.Count);
            }

            var sheet = new Sheet(sheetName, Math.Max(rowCount, 1), Math.Max(columnCount, 1));

            for (var row = 0; row < rows.Count; row++)
            {
                for (var column = 0; column < rows[row].Count; column++)
                {
                    var fieldText = rows[row][column];
                    if (!string.IsNullOrEmpty(fieldText))
                    {
                        sheet.SetCellText(new CellAddress(row, column), fieldText);
                    }
                }
            }

            return sheet;
        }

        /// <summary>
        /// Parses RFC 4180 CSV text (quoted fields, embedded separators/newlines, <c>""</c>-escaped
        /// quotes) into rows of raw field strings.
        /// </summary>
        private static List<List<string>> Parse(string text, char separator)
        {
            var rows = new List<List<string>>();
            var row = new List<string>();
            var field = new StringBuilder();
            var inQuotes = false;
            var rowHasContent = false;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < text.Length && text[i + 1] == '"')
                        {
                            field.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        field.Append(c);
                    }

                    continue;
                }

                if (c == '"')
                {
                    inQuotes = true;
                    rowHasContent = true;
                }
                else if (c == separator)
                {
                    row.Add(field.ToString());
                    field.Clear();
                    rowHasContent = true;
                }
                else if (c == '\r')
                {
                    // Handled by the following '\n', or treated as a line end on its own below.
                }
                else if (c == '\n')
                {
                    row.Add(field.ToString());
                    field.Clear();
                    rows.Add(row);
                    row = new List<string>();
                    rowHasContent = false;
                }
                else
                {
                    field.Append(c);
                    rowHasContent = true;
                }
            }

            if (rowHasContent || field.Length > 0 || row.Count > 0)
            {
                row.Add(field.ToString());
                rows.Add(row);
            }

            return rows;
        }

        /// <summary>
        /// Quotes <paramref name="value" /> if it contains the separator, a quote, or a line break,
        /// doubling any embedded quotes.
        /// </summary>
        private static string EncodeField(string value, char separator)
        {
            if (value.IndexOf(separator) < 0 && value.IndexOf('"') < 0 && value.IndexOf('\r') < 0 && value.IndexOf('\n') < 0)
            {
                return value;
            }

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
