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
        protected override ToggleButton CreateControl()
        {
            return new CheckBox();
        }

        protected override IValueConverter CreateConverter()
        {
            return (this.EnumMetadata != null)
                ? (IValueConverter)new EnumFlagSelectorItemsToBooleanConverter(this.Value as IList, selectorDefinition: this, this.EnumMetadata)
                : new MultiStateSelectorItemsToBooleanConverter(this.Value as IList, selectorDefinition: this);
        }
    }
}