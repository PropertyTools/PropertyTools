// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorDefinitionExtensions.cs" company="PropertyTools">
//   Copyright (c) 2025 PropertyTools contributors
// </copyright>
// <summary>
//   Extensions class to configure ISelectorDefinition instance.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using PropertyTools.DataAnnotations;
using PropertyTools.Wpf.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PropertyTools.Wpf.Extensions
{
    /// <summary>
    /// Extensions class to configure <seealso cref="ISelectorDefinition"/> instance
    /// </summary>
    public static class SelectorDefinitionExtensions
    {
        /// <summary>
        /// Initializes the <paramref name="selectorDefinition"/> from <seealso cref="PropertyItem"/>
        /// </summary>
        public static T ConfigureSelectorDefinition<T>(this T selectorDefinition, PropertyItem property)
            where T : ISelectorDefinition
        {
            selectorDefinition.ItemsSource = property.ItemsSource; // May be NULL
            selectorDefinition.ItemsSourcePropertyName = property.ItemsSourceDescriptor?.Name; // May be NULL
            selectorDefinition.DisplayMemberPath = property.DisplayMemberPath;
            selectorDefinition.SelectedValuePath = property.SelectedValuePath;

            return selectorDefinition;
        }

        /// <summary>
        /// Initializes the <paramref name="selectorDefinition"/> from <seealso cref="SelectorCellDefinition"/>
        /// </summary>
        public static T ConfigureSelectorDefinition<T>(this T selectorDefinition, SelectorCellDefinition cellDefinition)
            where T : ISelectorDefinition
        {
            selectorDefinition.ItemsSource = cellDefinition.ItemsSource; // May be NULL
            selectorDefinition.ItemsSourcePropertyName = cellDefinition.ItemsSourcePropertyName; // May be NULL
            selectorDefinition.DisplayMemberPath = cellDefinition.DisplayMemberPath;
            selectorDefinition.SelectedValuePath = cellDefinition.SelectedValuePath;
            selectorDefinition.DisplayTextForNullItem = cellDefinition.DisplayTextForNullItem;

            return selectorDefinition;
        }

        /// <summary>
        /// Initializes the <paramref name="selectorDefinition"/> from <seealso cref="IColumnSelectorDefinition"/>
        /// </summary>
        public static T ConfigureSelectorDefinition<T>(this T selectorDefinition, IColumnSelectorDefinition column)
            where T : ISelectorDefinition
        {
            selectorDefinition.ItemsSource = column.ItemsSource; // May be NULL
            selectorDefinition.ItemsSourcePropertyName = column.ItemsSourceProperty_DataGridItem; // May be NULL
            selectorDefinition.DisplayMemberPath = column.DisplayMemberPath;
            selectorDefinition.SelectedValuePath = column.SelectedValuePath;
            selectorDefinition.DisplayTextForNullItem = column.DisplayTextForNullItem;

            return selectorDefinition;
        }

        /// <summary>
        /// Initialized the <paramref name="selectorDefinition"/> with a list of <see cref="ItemsControlItem"/>(s) <para/>
        /// which is built from <paramref name="enumValues"/> collection according to <see cref="IPropertyItem.EnumMetadata"/>'s configuration
        /// </summary>
        /// <typeparam name="T">Any class that implements <see cref="ISelectorDefinition"/> interface </typeparam>
        /// <param name="enumPI">Enumeration property item</param>
        /// <param name="selectorDefinition">The selectorDefinition instance</param>
        /// <param name="enumValues"></param>
        /// <returns>The configured <paramref name="selectorDefinition"/> instance </returns>
        public static T ConfigureSelectorDefinitionForEnum<T>(this T selectorDefinition, IPropertyItem enumPI,
                IEnumerable<object> enumValues)
            where T : ISelectorDefinition
        {
            if (enumPI is PropertyItem enumProperty)
            {
                selectorDefinition.ConfigureSelectorDefinition(enumProperty);
            }

            if (selectorDefinition.ItemsSource == null && selectorDefinition.ItemsSourcePropertyName == null)
            {
                selectorDefinition.ItemsSource = enumValues.Select(x =>
                {
                    return BuildItemsControlItem(enumPI.EnumMetadata, x);
                }).ToList();

                selectorDefinition.DisplayMemberPath = nameof(ItemsControlItem.Text);
                selectorDefinition.SelectedValuePath = nameof(ItemsControlItem.Value);
            }

            return selectorDefinition;
        }

        public static ItemsControlItem BuildItemsControlItem(EnumPropertyMetadata enumPropertyMetadata, object value)
        {
            string displayText;
            if (value == null) // in case it is NULL in Nullable<EnumType>
            {
                displayText = enumPropertyMetadata?.EnumDisplayNull ?? "-";
            }
            else
            {
                displayText = enumPropertyMetadata?.EnumDisplayNames?.TryGetValue((Enum)value, out string enumMemberDisplayText) == true
                            ? enumMemberDisplayText
                    : value.ToString();
            }

            return new ItemsControlItem
            {
                Value = value,
                Text = displayText
            };
        }

        public class ItemsControlItem : IEquatable<ItemsControlItem>
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override int GetHashCode()
            {
                return Value?.GetHashCode() ?? 0;
            }

            public bool Equals(ItemsControlItem other)
            {
                return Object.Equals(this.Value, other.Value);
            }
        }
    }
}
