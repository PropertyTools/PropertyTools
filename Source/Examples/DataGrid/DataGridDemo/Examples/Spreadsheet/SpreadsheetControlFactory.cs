// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Creates the display and edit controls for spreadsheet cells.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo.Spreadsheet
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using DataGridDemo.Spreadsheet.Converters;
    using DataGridDemo.Spreadsheet.Model;

    using PropertyTools.Wpf;

    /// <summary>
    /// Creates the display and edit controls for spreadsheet cells: a <see cref="TextBlockEx" /> bound
    /// to <see cref="Cell.DisplayText" /> for display, and a bare <see cref="TextBox" /> bound to
    /// <see cref="Cell.Text" /> for editing.
    /// </summary>
    /// <remarks>
    /// The edit control must be an actual <see cref="TextBox" />, not a wrapped or templated control:
    /// <c>DataGrid</c> special-cases <c>currentEditControl is TextBox</c> for type-to-edit, committing
    /// on Enter and cancelling on Escape.
    /// </remarks>
    public class SpreadsheetControlFactory : DataGridControlFactory
    {
        /// <inheritdoc />
        protected override FrameworkElement CreateDisplayControlOverride(CellDefinition d)
        {
            var textBlock = new TextBlockEx
            {
                Padding = new Thickness(4, 0, 4, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            BindDataContextToCell(textBlock, d);

            textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(Cell.DisplayText)));

            var alignmentBinding = new MultiBinding { Converter = CellAlignmentConverter.Instance, Mode = BindingMode.OneWay };
            alignmentBinding.Bindings.Add(new Binding($"{nameof(Cell.Style)}.{nameof(CellStyle.HorizontalAlignment)}"));
            alignmentBinding.Bindings.Add(new Binding(nameof(Cell.Value)));
            textBlock.SetBinding(FrameworkElement.HorizontalAlignmentProperty, alignmentBinding);

            textBlock.SetBinding(
                TextBlock.FontWeightProperty,
                new Binding($"{nameof(Cell.Style)}.{nameof(CellStyle.Bold)}") { Converter = BoolToFontWeightConverter.Instance });

            textBlock.SetBinding(
                TextBlock.FontStyleProperty,
                new Binding($"{nameof(Cell.Style)}.{nameof(CellStyle.Italic)}") { Converter = BoolToFontStyleConverter.Instance });

            textBlock.SetBinding(
                TextBlock.ForegroundProperty,
                new Binding($"{nameof(Cell.Value)}.{nameof(CellValue.IsError)}") { Converter = ErrorForegroundConverter.Instance });

            return textBlock;
        }

        /// <inheritdoc />
        protected override FrameworkElement CreateEditControlOverride(CellDefinition d)
        {
            var textBox = new TextBox
            {
                BorderThickness = new Thickness(0),
                Margin = new Thickness(1, 1, 0, 0)
            };

            textBox.Loaded += (sender, args) =>
            {
                var tb = (TextBox)sender;
                tb.CaretIndex = tb.Text.Length;
                tb.SelectAll();
            };

            var textBinding = new Binding(d.BindingPath + "." + nameof(Cell.Text))
            {
                Source = d.BindingSource,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                NotifyOnSourceUpdated = true,
                ValidatesOnExceptions = true,
                ValidatesOnDataErrors = true
            };
            textBox.SetBinding(TextBox.TextProperty, textBinding);

            var alignmentBinding = new MultiBinding { Converter = CellAlignmentConverter.Instance, Mode = BindingMode.OneWay };
            alignmentBinding.Bindings.Add(
                new Binding(d.BindingPath + "." + nameof(Cell.Style) + "." + nameof(CellStyle.HorizontalAlignment))
                {
                    Source = d.BindingSource
                });
            alignmentBinding.Bindings.Add(
                new Binding(d.BindingPath + "." + nameof(Cell.Value)) { Source = d.BindingSource });
            textBox.SetBinding(TextBox.HorizontalContentAlignmentProperty, alignmentBinding);

            return textBox;
        }

        /// <summary>
        /// Rebinds <paramref name="element" />'s <see cref="FrameworkElement.DataContext" /> from the
        /// sheet adapter (the cell definition's binding source) to the individual <see cref="Cell" />,
        /// so that the element's other bindings can use plain property names.
        /// </summary>
        private static void BindDataContextToCell(FrameworkElement element, CellDefinition d)
        {
            var binding = new Binding(d.BindingPath) { Source = d.BindingSource, Mode = BindingMode.OneWay };
            element.SetBinding(FrameworkElement.DataContextProperty, binding);
        }
    }
}
