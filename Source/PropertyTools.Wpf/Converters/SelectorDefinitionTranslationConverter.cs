// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorDefinitionTranslationConverter.cs" company="PropertyTools">
//   Copyright (c) 2025 PropertyTools contributors
// </copyright>
// <summary>
//  Converts a object to its representation in mapping dictionary. <para/>
//  The mapping dictionary (<see cref="ISelectorDefinition.SelectedValuePath"/> -> <see cref="ISelectorDefinition.DisplayMemberPath"/>)
//  is created from <see cref="ISelectorDefinition.ItemsSource"/> items    
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using PropertyTools.Wpf.Common;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Data;

    /// <summary>
    /// Converts a object to its representation in mapping dictionary. <para/>
    /// The mapping dictionary (<see cref="ISelectorDefinition.SelectedValuePath"/> -> <see cref="ISelectorDefinition.DisplayMemberPath"/>)
    /// is created from <see cref="ISelectorDefinition.ItemsSource"/> items    
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public class SelectorDefinitionTranslationConverter : ObjectTranslationConverter, IValueConverter
    {
        public SelectorDefinitionTranslationConverter(ISelectorDefinition selectorDefinition)
            : base(ConvertItemsSourceToDictionary(selectorDefinition))
        {
            this.TryTranslateNull = selectorDefinition.DisplayTextForNullItem;
        }

        private static IReadOnlyDictionary<object, string> ConvertItemsSourceToDictionary(ISelectorDefinition selectorDefinition)
        {
            var result = new Dictionary<object, string>();

            if (selectorDefinition.ItemsSource != null
                && !string.IsNullOrEmpty(selectorDefinition.DisplayMemberPath)
                && !string.IsNullOrEmpty(selectorDefinition.SelectedValuePath)
                )
            {
                var items = selectorDefinition.ItemsSource.Cast<object>();

                foreach (var item in items)
                {
                    if (ReflectionExtensions.TryGetFieldOrPropertyValue(item, selectorDefinition.SelectedValuePath, out object key) &&
                        ReflectionExtensions.TryGetFieldOrPropertyValue(item, selectorDefinition.DisplayMemberPath, out object value)
                    )
                    {
                        result.Add(key ?? "", value.ToString());
                    }
                }
            }

            return result;
        }
    }
}