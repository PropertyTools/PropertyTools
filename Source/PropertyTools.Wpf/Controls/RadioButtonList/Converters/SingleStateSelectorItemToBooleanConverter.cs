// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleStateSelectorItemToBooleanConverter.cs" company="PropertyTools">
//   Copyright (c) 2025 PropertyTools contributors
// </copyright>
// <summary>
//  single-select control item to selected state converter (for example, radiobutton list)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using PropertyTools.Wpf.Common;

    /// <summary>
    /// single-select control item to selected state converter (for example, radiobutton list).
    /// If source type is a simple type  (string, int, decimal, boolean, DateTime, etc) then <see cref="SelectorDefinition.SelectedValuePath"/> must be null. <para/>
    /// If source type is a class then <see cref="SelectorDefinition.SelectedValuePath"/> must be a property of source class.
    /// </summary>
    /// <remarks>
    ///  Object to Boolean converter
    /// Usage 'Converter={StaticResource SingleStateSelectorItemToBooleanConverter}, ConverterParameter={x:Static value...}' <para/>    
    /// </remarks>
    [ValueConversion(typeof(object), typeof(bool))]
    public class SingleStateSelectorItemToBooleanConverter : IValueConverter
    {
        public ISelectorDefinition SelectorDefinition { get; set; }

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

            if (parameter == null)
            {
                return DependencyProperty.UnsetValue;
            }

            var isTargetTypeNullable = Nullable.GetUnderlyingType(targetType) != null;
            if (value == null && !isTargetTypeNullable)// null for non-Nullable type
            {
                return DependencyProperty.UnsetValue;
            }

            if (string.IsNullOrEmpty(SelectorDefinition.SelectedValuePath))
            {
                return object.Equals(value, parameter);
            }
            else if (ReflectionExtensions.TryGetFieldOrPropertyValue(parameter, SelectorDefinition.SelectedValuePath, out object objTargetValue))
            {
                if (value == null)
                {
                    return objTargetValue == null;
                }
                return value.Equals(objTargetValue);
            }

            return DependencyProperty.UnsetValue;
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
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                bool boolValue = System.Convert.ToBoolean(value, culture);
                if (boolValue)
                {
                    if (string.IsNullOrEmpty(SelectorDefinition.SelectedValuePath))
                    {
                        return parameter;
                    }
                    else if (ReflectionExtensions.TryGetFieldOrPropertyValue(parameter, SelectorDefinition.SelectedValuePath, out object objTargetValue))
                    {
                        return objTargetValue;
                    }
                }
            }
            catch (ArgumentException)
            {
            }
            catch (FormatException)
            {
            }

            return Binding.DoNothing;
        }
    }
}
