using System;
using System.Windows.Data;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Null to bool value converter
    /// </summary>
    [ValueConversion(typeof(bool), typeof(object))]
    public class NullToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the value returned when the source value is null.
        /// </summary>
        public bool NullValue { get; set; }

        public NullToBoolConverter()
        {
            NullValue = true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return NullValue;
            return !NullValue;
        }

    }
}