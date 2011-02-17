using System;
using System.Windows.Data;
using System.Windows.Media;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Color to Brush value converter
    /// </summary>
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            if (typeof(Brush).IsAssignableFrom(targetType))
            {
                if (value is Color)
                    return new SolidColorBrush((Color)value);
            }
            if (targetType == typeof(Color))
            {
                if (value is SolidColorBrush)
                    return ((SolidColorBrush)value).Color;
            }
            return Binding.DoNothing;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
