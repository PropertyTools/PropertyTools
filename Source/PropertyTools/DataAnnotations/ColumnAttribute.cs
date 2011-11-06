// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
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
        #region Constants and Fields

        /// <summary>
        /// The type id.
        /// </summary>
        private readonly object typeId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute"/> class.
        /// </summary>
        /// <param name="columnIndex">
        /// The column index.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="formatString">
        /// The format string.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="alignment">
        /// The alignment.
        /// </param>
        public ColumnAttribute(
            int columnIndex, 
            string propertyName, 
            string header = null, 
            string formatString = null, 
            string width = "Auto", 
            char alignment = 'C')
        {
            this.ColumnIndex = columnIndex;
            this.PropertyName = propertyName;
            this.Header = header;
            this.FormatString = formatString;
            this.Width = width;
            this.Alignment = alignment;
            this.typeId = Guid.NewGuid();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the alignment (L, R, C or S).
        /// </summary>
        /// <value>The alignment.</value>
        public char Alignment { get; set; }

        /// <summary>
        ///   Gets or sets the index of the column.
        /// </summary>
        /// <value>The index of the column.</value>
        public int ColumnIndex { get; set; }

        /// <summary>
        ///   Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        public string FormatString { get; set; }

        /// <summary>
        ///   Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header { get; set; }

        /// <summary>
        ///   Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }

        /// <summary>
        ///   When implemented in a derived class, gets a unique identifier for this <see cref = "T:System.Attribute" />.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref = "T:System.Object" /> that is a unique identifier for the attribute.</returns>
        public override object TypeId
        {
            get
            {
                return this.typeId;
            }
        }

        /// <summary>
        ///   Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public string Width { get; set; }

        #endregion
    }
}