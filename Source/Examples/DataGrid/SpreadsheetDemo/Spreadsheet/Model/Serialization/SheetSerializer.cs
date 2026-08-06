// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SheetSerializer.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Saves and loads a Sheet as JSON.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Saves and loads a <see cref="Sheet" /> as JSON. Only materialized cells that are non-empty or
    /// carry non-default formatting are written; everything else is implied by <see cref="SheetDto.Rows" />/
    /// <see cref="SheetDto.Columns" />. Cell content is stored as raw edit text (e.g. <c>"=A1+1"</c>),
    /// so formulas are re-parsed and recalculated on load rather than trusting a stored value.
    /// </summary>
    public static class SheetSerializer
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        /// <summary>
        /// Writes <paramref name="sheet" /> to <paramref name="stream" /> as JSON.
        /// </summary>
        public static void Save(Sheet sheet, Stream stream)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet));
            }

            var dto = new SheetDto
            {
                Rows = sheet.RowCount,
                Columns = sheet.ColumnCount,
                Cells = new List<CellDto>()
            };

            for (var row = 0; row < sheet.RowCount; row++)
            {
                for (var column = 0; column < sheet.ColumnCount; column++)
                {
                    var address = new CellAddress(row, column);
                    if (!sheet.TryGetCell(address, out var cell))
                    {
                        continue;
                    }

                    if (cell.IsEmpty && ReferenceEquals(cell.Style, CellStyle.Default))
                    {
                        continue;
                    }

                    dto.Cells.Add(new CellDto
                    {
                        Address = address.ToString(),
                        Text = cell.Text,
                        Bold = cell.Style.Bold,
                        Italic = cell.Style.Italic,
                        Align = cell.Style.HorizontalAlignment.ToString(),
                        Format = cell.Style.FormatString
                    });
                }
            }

            JsonSerializer.Serialize(stream, dto, Options);
        }

        /// <summary>
        /// Reads a new <see cref="Sheet" /> from <paramref name="stream" />.
        /// </summary>
        /// <exception cref="InvalidDataException">The stream does not contain a valid sheet.</exception>
        public static Sheet Load(Stream stream)
        {
            SheetDto dto;
            try
            {
                dto = JsonSerializer.Deserialize<SheetDto>(stream, Options);
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException("The file is not a valid spreadsheet.", ex);
            }

            if (dto == null || dto.Rows <= 0 || dto.Columns <= 0)
            {
                throw new InvalidDataException("The file is not a valid spreadsheet.");
            }

            var sheet = new Sheet("Sheet1", dto.Rows, dto.Columns);

            if (dto.Cells != null)
            {
                foreach (var cellDto in dto.Cells)
                {
                    if (cellDto.Address == null || !CellAddress.TryParse(cellDto.Address, out var address))
                    {
                        continue;
                    }

                    sheet.SetCellText(address, cellDto.Text);

                    if (!Enum.TryParse(cellDto.Align, out CellHorizontalAlignment alignment))
                    {
                        alignment = CellHorizontalAlignment.General;
                    }

                    sheet.SetCellStyle(
                        address,
                        CellStyle.Default
                            .WithBold(cellDto.Bold)
                            .WithItalic(cellDto.Italic)
                            .WithHorizontalAlignment(alignment)
                            .WithFormat(cellDto.Format));
                }
            }

            return sheet;
        }

        /// <summary>
        /// The on-disk shape of a sheet.
        /// </summary>
        private sealed class SheetDto
        {
            [JsonPropertyName("rows")]
            public int Rows { get; set; }

            [JsonPropertyName("columns")]
            public int Columns { get; set; }

            [JsonPropertyName("cells")]
            public List<CellDto> Cells { get; set; }
        }

        /// <summary>
        /// The on-disk shape of one non-default cell.
        /// </summary>
        private sealed class CellDto
        {
            [JsonPropertyName("address")]
            public string Address { get; set; }

            [JsonPropertyName("text")]
            public string Text { get; set; }

            [JsonPropertyName("bold")]
            public bool Bold { get; set; }

            [JsonPropertyName("italic")]
            public bool Italic { get; set; }

            [JsonPropertyName("align")]
            public string Align { get; set; }

            [JsonPropertyName("format")]
            public string Format { get; set; }
        }
    }
}
