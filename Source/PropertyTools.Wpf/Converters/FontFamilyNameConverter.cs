// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontFamilyNameConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts FontFamily instances to their localized display names.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media;

    /// <summary>
    /// Converts <see cref="FontFamily" /> instances to their localized display names (e.g. "宋体" instead of "SimSun" for a Chinese culture).
    /// </summary>
    /// <remarks>
    /// The name is looked up in <see cref="FontFamily.FamilyNames" /> for the specified culture and its parent cultures.
    /// If no localized name is found, the English (en-us) name is used, and finally the <see cref="FontFamily.Source" />.
    /// </remarks>
    [ValueConversion(typeof(FontFamily), typeof(string))]
    public class FontFamilyNameConverter : IValueConverter
    {
        /// <summary>
        /// The fallback language used when no name is found for the requested culture.
        /// </summary>
        private static readonly XmlLanguage FallbackLanguage = XmlLanguage.GetLanguage("en-us");

        /// <summary>
        /// Gets the localized name of the specified font family.
        /// </summary>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="culture">The culture. If <c>null</c>, <see cref="CultureInfo.CurrentUICulture" /> is used.</param>
        /// <returns>The localized name, or <c>null</c> if <paramref name="fontFamily" /> is <c>null</c>.</returns>
        public static string GetLocalizedName(FontFamily fontFamily, CultureInfo culture)
        {
            if (fontFamily == null)
            {
                return null;
            }

            var names = fontFamily.FamilyNames;
            if (names != null && names.Count > 0)
            {
                var c = culture ?? CultureInfo.CurrentUICulture;
                while (c != null && !string.IsNullOrEmpty(c.IetfLanguageTag))
                {
                    string name;
                    if (names.TryGetValue(XmlLanguage.GetLanguage(c.IetfLanguageTag), out name) && !string.IsNullOrEmpty(name))
                    {
                        return name;
                    }

                    if (c.Parent == null || c.Parent.Equals(c))
                    {
                        break;
                    }

                    c = c.Parent;
                }

                string fallbackName;
                if (names.TryGetValue(FallbackLanguage, out fallbackName) && !string.IsNullOrEmpty(fallbackName))
                {
                    return fallbackName;
                }
            }

            return fontFamily.Source ?? fontFamily.ToString();
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// The localized font family name, or the original value if it is not a <see cref="FontFamily" />.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fontFamily = value as FontFamily;
            if (fontFamily != null)
            {
                return GetLocalizedName(fontFamily, culture);
            }

            return value;
        }

        /// <summary>
        /// Converts a value. This conversion is not supported.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// <see cref="DependencyProperty.UnsetValue" />.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
