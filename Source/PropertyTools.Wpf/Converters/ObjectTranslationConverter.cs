// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectTranslationConverter.cs" company="PropertyTools">
//   Copyright (c) 2025 PropertyTools contributors
// </copyright>
// <summary>
//   Converts a value to its translation representation from a mapping dictionary (object => translation)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    /// <summary>
    /// Converts a value to its translation representation from a mapping dictionary (object => translation).<para/>    
    /// </summary>
    /// <remarks>
    /// In order to provide translation for NULL value  <para/>
    /// 1) the mapping dictionary must contain a pair with Key=<see cref="string.Empty"/>  <para/>
    /// 2) the <see cref="TryTranslateNull"/> property must be TRUE.
    /// </remarks>
    [ValueConversion(typeof(object), typeof(string))]
    public class ObjectTranslationConverter : IValueConverter
    {
        private IReadOnlyDictionary<object, string> _translationMap;

        /// <param name="translationMap">
        /// In order to provide translation for NULL value the mapping dictionary must contain a pair with Key=<see cref="string.Empty"/>.
        /// </param>
        public ObjectTranslationConverter(IReadOnlyDictionary<object, string> translationMap)
        {
            _translationMap = translationMap;
        }

        /// <summary>
        /// Indicates whether to search for pair with Key=<see cref="string.Empty"/> in mapping dictionary 
        /// when translating NULL value or not.
        /// </summary>
        public bool TryTranslateNull { get; set; }

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
            var key = value;
            if (key == null && TryTranslateNull)
            {
                key = key ?? ""; // use String.Empty as key for NULL translation pair
            }

            if (key != null && _translationMap.TryGetValue(key, out string translation))
            {
                return translation;
            }

            return value;
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
            var translation = (string)value;

            var pair = _translationMap.FirstOrDefault(x => x.Value == translation);

            if (pair.Key != null && pair.Key.Equals(""))
            {
                return null;
            }
            return pair.Key;  // KEY may be NULL
        }
    }
}