// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckBoxSelector.cs" company="PropertyTools">
//   Copyright (c) 2025 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a control that shows a list of check boxes.
//   Alternative to CheckableItems  control (<see cref="PropertyGridControlFactory.CreateCheckableItems(PropertyItem) "/>).
//   But does not require <see cref="PropertyTools.DataAnnotations.CheckableItemsAttribute.IsCheckedPropertyName"/> boolean property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    /// <summary>
    /// Represents a control that shows a list of check boxes. <para/> 
    /// Alternative to CheckableItems  control (<see cref="PropertyGridControlFactory.CreateCheckableItems(PropertyItem) "/>).
    /// But does not require <see cref="PropertyTools.DataAnnotations.CheckableItemsAttribute.IsCheckedPropertyName"/> boolean property.
    /// </summary>
    /// <remarks>
    /// The target property must be an <see cref="IList"/>
    /// </remarks>
    [TemplatePart(Name = PartPanel, Type = typeof(StackPanel))]
    public class CheckBoxSelector : RadioButtonSelector
    {
        /// <summary>
        /// Updates the content.
        /// </summary>
        protected override void UpdateContent()
        {
            if (this.panel == null)
            {
                return;
            }

            this.panel.Children.Clear();


            IEnumerable itemValues = PopulateItems();
            if (itemValues == null)
            {
                return;
            }

            var converter = new SelectorItemsToBooleanConverter(this.Value as IList, selectorDefinition: this);

            foreach (var itemValue in itemValues)
            {
                object content;
                if (itemValue == null || !ReflectionExtensions.TryGetFieldOrPropertyValue(itemValue, this.DisplayMemberPath, out content))
                {
                    content = "-";
                }

                var rb = new CheckBox
                {
                    Content = content,
                    Padding = this.ItemPadding,
                };

                var isCheckedBinding = new Binding(nameof(this.Value))
                {
                    Converter = converter,
                    ConverterParameter = itemValue,
                    Source = this,
                    Mode = BindingMode.TwoWay
                };

                rb.SetBinding(ToggleButton.IsCheckedProperty, isCheckedBinding);

                rb.SetBinding(MarginProperty, new Binding(nameof(this.ItemMargin)) { Source = this });

                this.panel.Children.Add(rb);
            }
        }

    }
}
