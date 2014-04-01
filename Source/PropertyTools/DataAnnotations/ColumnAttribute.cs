// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnAttribute.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Specifies a grid column.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies a grid column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// The type id.
        /// </summary>
        private readonly object typeId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute" /> class.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="propertyName">Name of the property.</param>
        public ColumnAttribute(int columnIndex, string propertyName)
        {
            this.ColumnIndex = columnIndex;
            this.PropertyName = propertyName;
            this.typeId = Guid.NewGuid();
            this.Width = "Auto";
            this.Alignment = 'C';
            this.IsReadOnly = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute" /> class.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="header">The header.</param>
        public ColumnAttribute(int columnIndex, string propertyName, string header)
            : this(columnIndex, propertyName)
        {
            this.Header = header;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute" /> class.
        /// </summary>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="header">The header.</param>
        /// <param name="formatString">The format string.</param>
        /// <param name="width">The width.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="isReadOnly">The columns is read only if set to <c>true</c>.</param>
        public ColumnAttribute(
            int columnIndex,
            string propertyName,
            string header,
            string formatString,
            string width = "Auto",
            char alignment = 'C',
            bool isReadOnly = false)
            : this(columnIndex, propertyName, header)
        {
            this.FormatString = formatString;
            this.Width = width;
            this.Alignment = alignment;
            this.IsReadOnly = isReadOnly;
        }

        /// <summary>
        /// Gets or sets the alignment (L, R, C or S).
        /// </summary>
        /// <value>The alignment.</value>
        public char Alignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column values are read only (overriding default binding mode).
        /// </summary>
        /// <value><c>true</c> if the column is read only; otherwise, <c>false</c>.</value>
        /// <remarks>If this property is set to <c>true</c>, the binding mode will be one-way also for properties with setters.</remarks>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the index of the column.
        /// </summary>
        /// <value>The index of the column.</value>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        public string FormatString { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }

#if !PCL
        /// <summary>
        /// When implemented in a derived class, gets a unique identifier for this <see cref = "T:System.Attribute" />.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An <see cref = "T:System.Object" /> that is a unique identifier for the attribute.
        /// </returns>
        public override object TypeId
        {
            get
            {
                return this.typeId;
            }
        }
#endif

        /// <summary>
        /// Gets or sets the width ("Auto", "0.5*" etc. are ok).
        /// </summary>
        /// <value>The width.</value>
        public string Width { get; set; }
    }
}