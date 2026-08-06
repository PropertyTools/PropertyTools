// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Creates the display and edit controls for spreadsheet cells.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using SpreadsheetDemo.Spreadsheet.Converters;
    using SpreadsheetDemo.Spreadsheet.Model;

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
    /// <remarks>
    /// All bindings here use an explicit <c>Source</c> (see <see cref="CreateCellBinding" />) rather
    /// than the element's inherited <see cref="FrameworkElement.DataContext" />: after
    /// <c>ControlFactory.CreateDisplayControl</c>/<c>CreateEditControl</c> returns, <c>DataGrid</c>
    /// unconditionally sets <c>element.DataContext = cd.BindingSource</c> (the whole row collection,
    /// not the individual cell), which would silently break a binding that depended on a per-cell
    /// <c>DataContext</c> rebind.
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

            textBlock.SetBinding(TextBlock.TextProperty, CreateCellBinding(d, nameof(Cell.DisplayText)));

            var alignmentBinding = new MultiBinding { Converter = CellAlignmentConverter.Instance, Mode = BindingMode.OneWay };
            alignmentBinding.Bindings.Add(CreateCellBinding(d, $"{nameof(Cell.Style)}.{nameof(CellStyle.HorizontalAlignment)}"));
            alignmentBinding.Bindings.Add(CreateCellBinding(d, nameof(Cell.Value)));
            textBlock.SetBinding(FrameworkElement.HorizontalAlignmentProperty, alignmentBinding);

            textBlock.SetBinding(
                TextBlock.FontWeightProperty,
                CreateCellBinding(d, $"{nameof(Cell.Style)}.{nameof(CellStyle.Bold)}", BoolToFontWeightConverter.Instance));

            textBlock.SetBinding(
                TextBlock.FontStyleProperty,
                CreateCellBinding(d, $"{nameof(Cell.Style)}.{nameof(CellStyle.Italic)}", BoolToFontStyleConverter.Instance));

            textBlock.SetBinding(
                TextBlock.ForegroundProperty,
                CreateCellBinding(d, $"{nameof(Cell.Value)}.{nameof(CellValue.IsError)}", ErrorForegroundConverter.Instance));

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

            var textBinding = CreateCellBinding(d, nameof(Cell.Text));
            textBinding.Mode = BindingMode.TwoWay;
            textBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            textBinding.NotifyOnSourceUpdated = true;
            textBinding.ValidatesOnExceptions = true;
            textBinding.ValidatesOnDataErrors = true;
            textBox.SetBinding(TextBox.TextProperty, textBinding);

            var alignmentBinding = new MultiBinding { Converter = CellAlignmentConverter.Instance, Mode = BindingMode.OneWay };
            alignmentBinding.Bindings.Add(CreateCellBinding(d, $"{nameof(Cell.Style)}.{nameof(CellStyle.HorizontalAlignment)}"));
            alignmentBinding.Bindings.Add(CreateCellBinding(d, nameof(Cell.Value)));
            textBox.SetBinding(TextBox.HorizontalContentAlignmentProperty, alignmentBinding);

            return textBox;
        }

        /// <summary>
        /// Creates a <see cref="Binding" /> to a property of the <see cref="Cell" /> at <paramref name="d" />,
        /// using an explicit <see cref="Binding.Source" /> (see the class remarks for why this is required
        /// instead of a <see cref="FrameworkElement.DataContext" />-relative binding).
        /// </summary>
        private static Binding CreateCellBinding(CellDefinition d, string cellPropertyPath, IValueConverter converter = null)
        {
            return new Binding(d.BindingPath + "." + cellPropertyPath)
            {
                Source = d.BindingSource,
                Mode = BindingMode.OneWay,
                Converter = converter
            };
        }
    }
}
