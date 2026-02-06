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
        /// Gets or sets the binding source.
        /// </summary>
        /// <value>
        /// The binding source.
        /// </value>
        /// <remarks>This is used for the DataContext of the cell.</remarks>
        public object BindingSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the binding mode should be one-way.
        /// </summary>
        /// <value>
        /// <c>true</c> if the binding mode should be one-way; otherwise, <c>false</c>.
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
        /// Gets or sets the source for the IsEnabled binding.
        /// </summary>
        public object IsEnabledBindingSource { get; set; }

        /// <summary>
        /// Gets or sets the IsEnabled binding path.
        /// </summary>
        /// <value>
        /// The binding path.
        /// </value>
        public string IsEnabledBindingPath { get; set; }

        /// <summary>
        /// Gets or sets the IsEnabled converter parameter.
        /// </summary>
        /// <value>
        /// The converter parameter.
        /// </value>
        public object IsEnabledBindingParameter { get; set; }

        /// <summary>
        /// Gets or sets the background source.
        /// </summary>
        /// <value>
        /// The binding source.
        /// </value>
        public object BackgroundBindingSource { get; set; }

        /// <summary>
        /// Gets or sets the background binding path.
        /// </summary>
        /// <value>
        /// The binding path.
        /// </value>
        public string BackgroundBindingPath { get; set; }
    }
}