// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorItemsToBooleanConverter.cs" company="PropertyTools">
//   Copyright (c) 2025 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a converter for multi-select control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using PropertyTools.Wpf.Common;
    using PropertyTools.Wpf.Extensions;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// enum flag control item to selected state converter (for example, checkbox list)
    /// </summary>
    [ValueConversion(typeof(IList), typeof(bool))]
    public class EnumFlagSelectorItemsToBooleanConverter : IValueConverter
    {
        private readonly IList _target;
        private readonly ISelectorDefinition _selectorDefinition;
        private readonly EnumPropertyMetadata _enumPropertyMetadata;

        public EnumFlagSelectorItemsToBooleanConverter(IList target, ISelectorDefinition selectorDefinition,
            EnumPropertyMetadata enumPropertyMetadata)
        {
            _target = target;
            _selectorDefinition = selectorDefinition;
            _enumPropertyMetadata = enumPropertyMetadata;
        }


        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null && parameter == null)
            {
                return true;
            }

            if (value == null || parameter == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (value != _target)
            {
                return DependencyProperty.UnsetValue;
            }

            return (value as IList).Contains(parameter);

            if (!string.IsNullOrEmpty(_selectorDefinition.SelectedValuePath)
                && ReflectionExtensions.TryGetFieldOrPropertyValue(parameter, _selectorDefinition.SelectedValuePath, out object objTargetValue))
            {
                return (value as IList).Contains(objTargetValue);
            }
            else
            {
                return (value as IList).Contains(parameter);
            }
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    bool boolValue = System.Convert.ToBoolean(value, culture);
                    if (parameter != null)
                    {
                        object objTargetValue = null;

                        if (!string.IsNullOrEmpty(_selectorDefinition.SelectedValuePath)
                            && ReflectionExtensions.TryGetFieldOrPropertyValue(parameter, _selectorDefinition.SelectedValuePath, out objTargetValue)
                            ) 
                        {
                            
                        }
                        
                        bool? isZeroFlag = objTargetValue != null && ((Enum)objTargetValue).IsZeroFlag();

                        if (boolValue)
                        {
                            // set flag

                            if (isZeroFlag == true)
                            {
                                // zero is set -> remove all 
                                _target.Clear();
                            }                        
                            else if (!_target.Contains(parameter))
                            {
                                _target.Add(parameter);
                            }
                        }
                        else
                        {
                            // unset flag
                            if (isZeroFlag == true)
                            {
                                // we can not unset zero - do nothing
                            }                           
                            else if (_target.Contains(parameter))
                            {
                                _target.Remove(parameter);
                            }
                        }
                    }
                }
                catch (ArgumentException)
                {
                }
                catch (FormatException)
                {
                }
            }

            return _target;
        }
    }
}
