// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumValueToMultiStateSelectorItemsConverter.cs" company="PropertyTools">
//   Copyright (c) 2025 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a converter for multi-select control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    using PropertyTools.Wpf.Common;
    using PropertyTools.Wpf.Extensions;

    /// <summary>
    /// Converts an single Enum value to a list of the <see cref="SelectorDefinitionExtensions.ItemsControlItem"/>
    /// </summary>
    /// <remarks>
    /// May be use for <seealso cref="CheckBoxSelector"/>.<para/>
    /// But not for <see cref="RadioButtonSelector"/>
    /// </remarks>
    [ValueConversion(typeof(Enum), typeof(List<SelectorDefinitionExtensions.ItemsControlItem>))]
    public class EnumValueToMultiStateSelectorItemsConverter : IValueConverter
    {
        private readonly EnumPropertyMetadata _enumPropertyMetadata;

        public EnumValueToMultiStateSelectorItemsConverter(EnumPropertyMetadata enumPropertyMetadata)
        {
            _enumPropertyMetadata = enumPropertyMetadata;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = new List<SelectorDefinitionExtensions.ItemsControlItem>();

            if (value != null)
            {
                Enum enumValue = (Enum)value;

                if (_enumPropertyMetadata.Flags)
                {
                    var enumDefaultValue = ReflectionExtensions.GetEnumDefaultValue(_enumPropertyMetadata.EnumType);

                    if (enumValue.Equals(enumDefaultValue)) // use 'Equals' instead of '=='
                    {
                        // set zero flag or default (non-zero) flag
                        result.Add(
                            SelectorDefinitionExtensions.BuildItemsControlItem(_enumPropertyMetadata, enumDefaultValue)
                        );
                    }
                    else if (enumValue.IsZeroFlag())
                    {
                        if (_enumPropertyMetadata.InitializeWithDefault == true)
                        {
                            result.Add(
                                SelectorDefinitionExtensions.BuildItemsControlItem(_enumPropertyMetadata, enumDefaultValue)
                            );
                        }
                    }
                    else
                    {
                        foreach (var i in _enumPropertyMetadata.EnumDisplayNames.Keys) // only prefiltered from EnumPropertyMetadata
                        {
                            if (i.IsZeroFlag())
                            {
                                // skip zero
                            }
                            else if (enumValue.HasFlag(i))
                            {
                                result.Add(
                                    SelectorDefinitionExtensions.BuildItemsControlItem(_enumPropertyMetadata, i)
                                );
                            }
                        }
                    }
                }
                else
                {
                    // add itself
                    result.Add(SelectorDefinitionExtensions.BuildItemsControlItem(_enumPropertyMetadata, enumValue));
                }
            }
            else
            {
                // for NULL - list remains empty
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList list && list.Count > 0)
            {
                if (list.Count == 1)
                {
                    return (list[0] as SelectorDefinitionExtensions.ItemsControlItem).Value;
                }
                else if (_enumPropertyMetadata.Flags)
                {
                    Enum enumValue = (list[0] as SelectorDefinitionExtensions.ItemsControlItem).Value as Enum;
                    for (int i = 1; i < list.Count; i++)
                    {
                        Enum enumValueNext = (list[i] as SelectorDefinitionExtensions.ItemsControlItem).Value as Enum;
                        ReflectionExtensions.SetFlag(ref enumValue, enumValueNext);
                    }
                    return enumValue;
                }
                else
                {
                    // two or more values are not allowed without Flags
                }
            }

            if (_enumPropertyMetadata.IsNullableEnum)
            {
                return null;
            }
            else
            {
                // enum default value
                var enumDefaultValue = ReflectionExtensions.GetEnumDefaultValue(_enumPropertyMetadata.EnumType);

                if (!enumDefaultValue.IsZeroFlag())
                {
                    // default value is not a zero 
                    if (_enumPropertyMetadata.ResetToDefault == true)
                    {
                        return enumDefaultValue;
                    }
                    else
                    {
                        return ReflectionExtensions.GetEnumZeroNumber(_enumPropertyMetadata.EnumType);
                    }
                }
                else
                {
                    // default value is zero 
                    return enumDefaultValue;
                }
            }
        }
    }
}
