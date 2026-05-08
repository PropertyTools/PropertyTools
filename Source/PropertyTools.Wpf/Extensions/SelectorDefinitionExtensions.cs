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
            selectorDefinition.ItemsSourceProperty = property.ItemsSourceDescriptor?.Name; // May be NULL
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
            selectorDefinition.ItemsSourceProperty = cellDefinition.ItemsSourceProperty; // May be NULL
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
            selectorDefinition.ItemsSourceProperty = column.ItemsSourceProperty_DataGridItem; // May be NULL
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

            if (selectorDefinition.ItemsSource == null && selectorDefinition.ItemsSourceProperty == null)
            {
                selectorDefinition.ItemsSource = enumValues.Select(x =>
                {
                    string displayText;
                    if (x == null) // in case it is NULL in Nullable<EnumType>
                    {
                        displayText = enumPI.EnumMetadata?.EnumDisplayNull ?? "-";
                    }
                    else
                    {
                        displayText = enumPI.EnumMetadata?.EnumDisplayNames?.TryGetValue((Enum)x, out string enumMemberDisplayText) == true
                            ? enumMemberDisplayText
                            : x.ToString();
                    }

                    return new ItemsControlItem
                    {
                        Value = x,
                        Text = displayText
                    };
                }).ToList();

                selectorDefinition.DisplayMemberPath = nameof(ItemsControlItem.Text);
                selectorDefinition.SelectedValuePath = nameof(ItemsControlItem.Value);
            }

            return selectorDefinition;
        }

        public class ItemsControlItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
        }
    }
}
