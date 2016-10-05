// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellDefinition.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines the content of a cell.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Defines the content of a cell.
    /// </summary>
    public abstract class CellDefinition
    {
        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>
        /// The horizontal alignment.
        /// </value>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the binding path.
        /// </summary>
        /// <value>
        /// The binding path.
        /// </value>
        public string BindingPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>
        /// The format string.
        /// </value>
        public string FormatString { get; set; }

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        /// <value>
        /// The converter.
        /// </value>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets the converter parameter.
        /// </summary>
        /// <value>
        /// The converter parameter.
        /// </value>
        public object ConverterParameter { get; set; }

        /// <summary>
        /// Gets or sets the converter culture.
        /// </summary>
        /// <value>
        /// The converter culture.
        /// </value>
        public CultureInfo ConverterCulture { get; set; }

        /// <summary>
        /// Gets or sets the name of a property that determines the state of the cell.
        /// </summary>
        /// <value>
        /// The name of the related property.
        /// </value>
        public string IsEnabledByProperty { get; set; }

        /// <summary>
        /// Gets or sets the value that enables the cell.
        /// </summary>
        /// <remarks>This property is used if the <see cref="IsEnabledByProperty"/> property is set.</remarks>
        public object IsEnabledByValue { get; set; }

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>
        /// The background.
        /// </value>
        public Brush Background { get; set; }
    }
}