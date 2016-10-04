// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGridControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies a control factory for the DataGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class CellDefinition
    {
        public HorizontalAlignment HorizontalAlignment { get; set; }

        public string BindingPath { get; set; }

        public UpdateSourceTrigger Trigger { get; set; }

        public bool IsReadOnly { get; set; }

        public string FormatString { get; set; }

        public IValueConverter Converter { get; set; }

        public object ConverterParameter { get; set; }

        public CultureInfo ConverterCulture { get; set; }
    }

    public class ColorCellDefinition : CellDefinition { }

    public class CheckCellDefinition : CellDefinition { }

    public class TextCellDefinition : CellDefinition
    {
        public int MaxLength { get; set; }
    }

    public class SelectCellDefinition : CellDefinition
    {
        public bool IsEditable { get; set; }

        public IEnumerable ItemsSource { get; set; }

        public string ItemsSourceProperty { get; set; }

        public string SelectedValuePath { get; set; }
    }

    public class TemplateCellDefinition : CellDefinition
    {
        public DataTemplate DisplayTemplate { get; set; }
        public DataTemplate EditingTemplate { get; set; }
    }

    /// <summary>
    /// Specifies a control factory for the DataGrid.
    /// </summary>
    public interface IDataGridControlFactory
    {
        /// <summary>
        /// Creates the display control with data binding.
        /// </summary>
        /// <param name="cellDefinition">The cell definition.</param>
        /// <returns>
        /// The control.
        /// </returns>
        FrameworkElement CreateDisplayControl(CellDefinition cellDefinition);

        /// <summary>
        /// Creates the edit control with data binding.
        /// </summary>
        /// <param name="cellDefinition">The cell definition.</param>
        /// <returns>
        /// The control.
        /// </returns>
        FrameworkElement CreateEditControl(CellDefinition cellDefinition);
    }
}